namespace HaoKao.CourseService.Application.Storages.AliyunStorage;

public class AliyunStorageConfig : IStorageConfig
{
    public string AlibabaCloudAccessKeyId { get; set; } = string.Empty;

    public string AlibabaCloudAccessKeySecret { get; set; } = string.Empty;

    public string Endpoint { get; set; } = "vod.cn-shanghai.aliyuncs.com";

    public string RegionId { get; set; } = "cn-shanghai";

    public string OssRegionId { get; set; } = "cn-shenzhen";

    public string RamAccessKeyID { get; set; } = string.Empty;

    public string RamAccessKeySecret { get; set; } = string.Empty;

    public string AssumeRoleArn { get; set; } = string.Empty;

    public string AssumeRoleSessionName { get; set; } = string.Empty;

    public long AssumeRoleDurationSeconds { get; set; } = 0L;
}