// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using ZhuoFan.Wb.NotificationMessage.Infrastructure;
//
// namespace ZhuoFan.Wb.NotificationMessage.WebApi.App_Data
// {
// #if DEBUG
//
//     public class DbContextFactory : IDesignTimeDbContextFactory<NotificationMessageDbContext>
//     {
//         public NotificationMessageDbContext CreateDbContext(string[] args)
//         {
//             var optionsBuilder = new DbContextOptionsBuilder<NotificationMessageDbContext>();
//             optionsBuilder.UseMySql(
//                 "Server=192.168.51.166;database=Wb_RegisteredUserManagement;User ID=root;Password=123456;Character Set=utf8;",
//                 new MySqlServerVersion(new Version(8, 0, 25)));
//             return new NotificationMessageDbContext(optionsBuilder.Options);
//         }
//     }
// #endif
// }