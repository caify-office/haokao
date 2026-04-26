using Girvs.BusinessBasis;
using HaoKao.BurialPointService.Domain.MappingModel;

namespace HaoKao.BurialPointService.Domain.Repositories;

public interface IRegisterUserRepository:IManager
{
   Task<RegisterUser> Get(Guid id);
}

