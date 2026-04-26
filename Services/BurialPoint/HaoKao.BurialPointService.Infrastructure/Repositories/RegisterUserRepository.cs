using HaoKao.BurialPointService.Domain.MappingModel;
using System.Threading.Tasks;

namespace HaoKao.BurialPointService.Infrastructure.Repositories;

public class RegisterUserRepository : IRegisterUserRepository
{
    public Task<RegisterUser> Get(Guid id)
    {
        var dbContext=EngineContext.Current.Resolve<BurialPointDbContext>();
        return dbContext.RegisterUser.FirstOrDefaultAsync(x => x.Id == id);
    }
}
