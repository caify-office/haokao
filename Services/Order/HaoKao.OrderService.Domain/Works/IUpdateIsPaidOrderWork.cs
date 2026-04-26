namespace HaoKao.OrderService.Domain.Works;

public interface IUpdateIsPaidOrderWork : IManager
{
    Task ExecuteAsync();
}