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
            Group teachers = new Group
            {
                GroupName = "老师",
                Permission = PermissionType.Teacher
            };
            Group students = new Group
            {
                GroupName = "学生",
                Permission = PermissionType.Student
            };
            Group HttpJudge = new Group
            {
                GroupName = "评测员",
                Permission = PermissionType.HttpJudge
            };
            context.Groups.Add(administrator);
            context.Groups.Add(teachers);
            context.Groups.Add(students);
            context.Groups.Add(HttpJudge);


            User admin = new User
            {
                NickName = "管理员姓名",
                UserName = "admin",
                PassWord = MD5Encrypt.Encrypt("admin", "12345"),
                Group = administrator
            };
            User teacher = new User
            {
                NickName = "老师",
                UserName = "teacher",
                PassWord = MD5Encrypt.Encrypt("teacher", "12345"),
                Group = teachers
            };
            User student = new User
            {
                NickName = "学生",
                UserName = "student",
                PassWord = MD5Encrypt.Encrypt("student", "12345"),
                Group = students
            };
            User judge = new User
            {
                NickName = "评测员1",
                UserName = "judger",
                PassWord = MD5Encrypt.Encrypt("judger", "12345"),
                Group = HttpJudge
            };

            context.Uesrs.Add(admin);
            context.Uesrs.Add(teacher);
            context.Uesrs.Add(student);
            context.Uesrs.Add(judge);

            Compiler compiler = new Compiler
            {
                Name = "gcc",
                isForbidden = false,
                isScript = false,
                CodeFormat = "",
                ExecutionFormat = "",
                CompilerPath = "Compilers\\mingw\\bin\\g++.exe",
                CompilerArgs = "-w -o2 <tempdir>src.cpp -o <tempdir>program.exe",
                RunnerPath = "<tempdir>program.exe",
                RunnerArgs = ""
            };
            context.Compilers.Add(compiler);


            context.SaveChanges();

        }

        public static void Initialize(IServiceProvider applicationServices)
        {
            throw new NotImplementedException();
        }
    }
}
