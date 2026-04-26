using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Ocr_api20210707;
using AlibabaCloud.SDK.Vod20170321.Models;
using AlibabaCloud.TeaUtil.Models;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using HaoKao.Common.Extensions;
using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;
using Newtonsoft.Json;
using DefaultProfile = Aliyun.Acs.Core.Profile.DefaultProfile;
using GetPlayInfoRequest = Aliyun.Acs.vod.Model.V20170321.GetPlayInfoRequest;
using GetVideoInfoRequest = Aliyun.Acs.vod.Model.V20170321.GetVideoInfoRequest;
using GetVideoPlayAuthRequest = Aliyun.Acs.vod.Model.V20170321.GetVideoPlayAuthRequest;

namespace HaoKao.CourseService.Application.Storages.AliyunStorage;

public class AliyunStorageHandler : StorageHandler<AliyunStorageConfig, SearchMediaRequest>
{
    public override Guid HandlerId => new("15B50340-6F3B-4ADE-9052-ECBAE86DE3A9");

    public override string HandlerName => "阿里云视频点播";

    public override Task DynamicBindingModel(SearchMediaRequest model)
    {
        var request = EngineContext.Current.HttpContext.Request;

        if (request.Query.ContainsKey("pageno")) model.PageNo = int.Parse(request.Query["pageno"]);

        if (request.Query.ContainsKey("pagesize")) model.PageSize = int.Parse(request.Query["pagesize"]);

        if (request.Query.ContainsKey("fields")) model.Fields = request.Query["fields"];

        if (request.Query.ContainsKey("searchtype")) model.SearchType = request.Query["searchtype"];

        if (request.Query.ContainsKey("sortby")) model.SortBy = request.Query["sortby"];

        if (request.Query.ContainsKey("match")) model.Match = request.Query["match"];

        if (request.Query.ContainsKey("scrolltoken")) model.ScrollToken = request.Query["scrolltoken"];

        return Task.CompletedTask;
    }

    public override Task<VideoModel> GetVideoAuth(string videoId)
    {
        var client = new DefaultAcsClient(
            DefaultProfile.GetProfile(
                Config.RegionId,
                Config.AlibabaCloudAccessKeyId,
                Config.AlibabaCloudAccessKeySecret
            )
        );
        var request = new GetVideoPlayAuthRequest { VideoId = videoId, };
        var response = client.GetAcsResponse(request);
        return Task.FromResult(new VideoModel
        {
            Duration = response.VideoMeta.Duration,
            PlayAuth = response.PlayAuth,
            CoverURL = response.VideoMeta.CoverURL,
            Title = response.VideoMeta.Title,
        });
    }

    public override Task<VideoInfoModel> GetVideoInfo(string videoId)
    {
        var client = new DefaultAcsClient(
            DefaultProfile.GetProfile(
                Config.RegionId,
                Config.AlibabaCloudAccessKeyId,
                Config.AlibabaCloudAccessKeySecret
            )
        );
        var request = new GetVideoInfoRequest { VideoId = videoId, };
        var response = client.GetAcsResponse(request);
        return Task.FromResult(new VideoInfoModel
        {
            Title = response.Video.Title,
            CateId = response.Video.CateId,
            CateName = response.Video.CateName,
            Tags = response.Video.Tags,
            VideoId = response.Video.VideoId,
        });
    }

    public override Task<VideoPlayInfo> GetVideoPlayInfo(string videoId)
    {
        var client = new DefaultAcsClient(
            DefaultProfile.GetProfile(
                Config.RegionId,
                Config.AlibabaCloudAccessKeyId,
                Config.AlibabaCloudAccessKeySecret
            )
        );

        var request = new GetPlayInfoRequest
        {
            VideoId = videoId,
            Formats = "mp4",
            StreamType = "video",
            Definition = "OD,SD",
        };

        var acsResponse = client.GetAcsResponse(request);
        var model = acsResponse.PlayInfoList.Select(x => new VideoPlayInfo
        {
            PlayURL = x.PlayURL,
            Size = x.Size,
            Definition = x.Definition,
            Duration = x.Duration,
            Height = x.Height,
            Width = x.Width,
        }).ToList().FirstOrDefault();
        return Task.FromResult(model);
    }

    public static Client CreateClientOcr(string accessKeyId, string accessKeySecret)
    {
        return new Client(new Config
        {
            // 必填，您的 AccessKey ID
            AccessKeyId = accessKeyId,
            // 必填，您的 AccessKey Secret
            AccessKeySecret = accessKeySecret,
            // Endpoint 请参考 https://api.aliyun.com/product/ocr-api
            Endpoint = "ocr-api.cn-hangzhou.aliyuncs.com",
        });
    }

    public override async Task<dynamic> SearchStorage(HttpRequest request)
    {
        // 请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_ID 和 ALIBABA_CLOUD_ACCESS_KEY_SECRET。
        // 工程代码泄露可能会导致 AccessKey 泄露，并威胁账号下所有资源的安全性。以下代码示例使用环境变量获取 AccessKey 的方式进行调用，仅供参考，建议使用更安全的 STS 方式，更多鉴权访问方式请参见：https://help.aliyun.com/document_detail/378671.html
        var client = CreateClient();
        var searchMediaRequest = new SearchMediaRequest
        {
            PageNo = 1,
            PageSize = 5,
            Fields = "Title,CoverURL,Duration",
            SearchType = "video",
            SortBy = "CreationTime:Desc",
        };

        await DynamicBindingModel(searchMediaRequest);

        searchMediaRequest.ScrollToken = JsonConvert.SerializeObject(searchMediaRequest).ToMd5();

        var searchMediaResponse = await client.SearchMediaWithOptionsAsync(searchMediaRequest, new RuntimeOptions());
        if (searchMediaResponse.StatusCode == StatusCodes.Status200OK)
        {
            return ConvertToLocalTime(searchMediaResponse);
        }

        throw new GirvsException("请求出错！");
    }

    /// <summary>
    /// 接口返回的是零区时间，需要转为本地时间
    /// </summary>
    /// <param name="searchMediaResponse"></param>
    /// <returns></returns>
    private SearchMediaResponseBody ConvertToLocalTime(SearchMediaResponse searchMediaResponse)
    {
        var result = searchMediaResponse.Body;
        foreach (var item in result.MediaList)
        {
            item.CreationTime = item.CreationTime.ConvertToLocalTime();
            item.Video.CreationTime = item.Video.CreationTime.ConvertToLocalTime();
        }

        return result;
    }

    public override async Task<dynamic> GetCategories()
    {
        var request = EngineContext.Current.HttpContext.Request;

        var client = CreateClient();
        var categoriesRequest = new GetCategoriesRequest
        {
            PageNo = request.Query.TryGetValue("pageno", out var pageNo) ? pageNo.To<long>() : 1,
            PageSize = request.Query.TryGetValue("pagesize", out var pageSize) ? pageSize.To<long>() : 100,
            SortBy = request.Query.TryGetValue("sortby", out var sortBy) ? sortBy : "CreationTime:Desc",
        };

        var categoriesResponse = await client.GetCategoriesWithOptionsAsync(categoriesRequest, new RuntimeOptions());
        if (categoriesResponse.StatusCode == StatusCodes.Status200OK)
        {
            return categoriesResponse.Body;
        }

        throw new GirvsException("请求出错！");
    }

    private AlibabaCloud.SDK.Vod20170321.Client CreateClient()
    {
        return new AlibabaCloud.SDK.Vod20170321.Client(new Config
        {
            // 必填，您的 AccessKey ID
            AccessKeyId = Config.AlibabaCloudAccessKeyId,
            // 必填，您的 AccessKey Secret
            AccessKeySecret = Config.AlibabaCloudAccessKeySecret,
            // Endpoint 请参考 https://api.aliyun.com/product/vod
            Endpoint = Config.Endpoint,
        });
    }

    public override dynamic AssumeRole()
    {
        // 构建一个阿里云客户端，用于发起请求。
        // 设置调用者（RAM用户或RAM角色）的AccessKey ID和AccessKey Secret。
        var profile = DefaultProfile.GetProfile(
            Config.OssRegionId,
            Config.RamAccessKeyID,
            Config.RamAccessKeySecret
        );
        var client = new DefaultAcsClient(profile);

        // 构建请求，设置参数。
        var request = new AssumeRoleRequest
        {
            RoleArn = Config.AssumeRoleArn,
            RoleSessionName = Config.AssumeRoleSessionName,
            DurationSeconds = Config.AssumeRoleDurationSeconds,
        };

        // 发起请求，并得到响应。
        var response = client.GetAcsResponse(request);
        return response.Credentials;
    }
}