using Girvs.Extensions;
using HaoKao.OpenPlatformService.Domain.Enumerations;

namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class RegisterUserEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<RegisterUser>
{
    public override void Configure(EntityTypeBuilder<RegisterUser> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<RegisterUser, Guid>(builder);

        builder.Property(x => x.Account).HasColumnType("varchar(30)").HasComment("用户账号");
        builder.Property(x => x.Phone).HasColumnType("varchar(11)").HasComment("手机号码");
        builder.Property(x => x.Password).HasColumnType("varchar(32)").HasComment("密码");
        builder.Property(x => x.UserGender).HasComment("用户性别");
        builder.Property(x => x.NickName).HasColumnType("varchar(30)").HasComment("用户昵称");
        builder.Property(x => x.UserState).HasComment("用户状态");
        builder.Property(x => x.EmailAddress).HasColumnType("varchar(40)").HasComment("邮箱地址");
        builder.Property(x => x.HeadImage).HasColumnType("varchar(200)").HasComment("头像");
        builder.Property(x => x.LastLoginIp).HasColumnType("varchar(30)").HasComment("最后登录IP");
        builder.Property(x => x.LastLoginTime).HasComment("最后登录时间");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.ClientId).HasComment("客户端Id");

        builder.HasIndex(x => new { x.Phone, x.Account, x.NickName, x.UserState, x.CreateTime });

        //添加临时用户账户
        builder.HasData(new RegisterUser
        {
            Id = Guid.Parse("D2EAD372-8C6A-4C52-9F17-9D1599F202F0"),
            Account = "tempuser",
            Phone = "13000000000",
            Password = "123456".ToMd5(),
            UserGender = UserGender.Unknown,
            NickName = "临时用户",
            UserState = UserState.Enable,
            EmailAddress = "13000000000@qq.com",
            LastLoginTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
    }
}