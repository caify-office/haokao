// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;
// using Girvs.EntityFrameworkCore.TableRoutes;
// using ShardingCore.Core.EntityMetadatas;
// using ShardingCore.Core.VirtualRoutes;
// using ShardingCore.Core.VirtualRoutes.TableRoutes.Abstractions;
// using HaoKao.OrderService.Domain.Models;
//
// namespace HaoKao.OrderService.Infrastructure.Shardings
// {
//     public class AuditVirtualTableRoute : AbstractShardingOperatorVirtualTableRoute<AuditLog,Guid>,
//         IGirvsTableRoute
//     {
//         public override string ShardingKeyToTail(object shardingKey)
//         {
//             throw new NotImplementedException();
//         }
//
//         public override List<string> GetAllTails()
//         {
//             throw new NotImplementedException();
//         }
//
//         public override void Configure(EntityMetadataTableBuilder<AuditLog> builder)
//         {
//             throw new NotImplementedException();
//         }
//
//         public override Expression<Func<string, bool>> GetRouteToFilter(Guid shardingKey, ShardingOperatorEnum shardingOperator)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }