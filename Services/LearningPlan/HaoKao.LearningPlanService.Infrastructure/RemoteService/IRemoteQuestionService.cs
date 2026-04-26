using Girvs.AutoMapper.Mapper;
using Girvs.BusinessBasis.Dto;
using Girvs.Refit;
using HaoKao.Common.RemoteService;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.RemoteService;

[RefitService(RefitServiceNames.QuestionService)]
public interface IRemoteQuestionService : IGirvsRefit
{
    [Get("/api/App/QuestionAppService/ChaperCategorieQuestionCount/{chaperId}/{questionCategoryId}")]
    Task<int> GetChaperCategorieQuestionCount(Guid chaperId, Guid questionCategoryId);
}



