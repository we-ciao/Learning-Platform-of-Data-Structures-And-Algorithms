using DSAA.EntityFrameworkCore.Entity;
using DSAA.Utilities;
using System;
using System.Linq;

namespace DSAA.EntityFrameworkCore
{
    public static class DbInitializer
    {
        public static void Initialize(EntityDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Groups.Any())
                return;

            Group administrator = new Group
            {
                GroupName = "超级管理员",
                Permission = PermissionType.Administrator
            };

            Group techer = new Group
            {
                GroupName = "老师",
                Permission = PermissionType.Teacher
            };


            User admin = new User
            {
                NickName = "管理员姓名",
                UserName = "Admin",
                PassWord = MD5Encrypt.Encrypt("admin", "12345"),
                Group = administrator
            };


            context.Groups.Add(administrator);
            context.Groups.Add(techer);

            context.Uesrs.Add(admin);
            context.SaveChanges();

        }

        public static void Initialize(IServiceProvider applicationServices)
        {
            throw new NotImplementedException();
        }
    }
}
