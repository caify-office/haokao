using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Infrastructure.Repositories;

public class PlatformPayerRepository : Repository<PlatformPayer>, IPlatformPayerRepository;