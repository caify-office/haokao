//using System;
//using HaoKao.OrderService.Infrastructure;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace HaoKao.OrderService.WebApi.App_Data;
//#if DEBUG
    
//public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
//{
//    public OrderDbContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
//        optionsBuilder.UseMySql("Server=192.168.51.166;database=Wb_AuditLog;User ID=root;Password=123456;Character Set=utf8;",
//                                new MySqlServerVersion(new Version(8, 0, 25)));
//        return new OrderDbContext(optionsBuilder.Options);
//    }
//}
    
//#endif