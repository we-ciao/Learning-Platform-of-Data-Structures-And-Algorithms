using DSAA.EntityFrameworkCore;
using Learning_Platform_of_DSAA.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Learning_Platform_of_DSAA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//关闭GDPR规范
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //添加数据库上下文
            var connection = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<EntityDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(connection, b => b.MigrationsAssembly("Learning_Platform_of_DSAA")));
            services.AddMvc();
            //添加IP查询
            services.AddHttpContextAccessor();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            //手动注入
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IProblemRepository, ProblemRepository>();
            //反射加载接口实现类，集中注册服务
            AddScopedByClassName(services, "DSAA.Repository", x =>
                 x.Name != "IRepository" &&
                 x.Name != "FonourRepositoryBase"
                );
            AddScopedByClassName(services, "DSAA.Service", x =>
                 1 == 1

            );

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Policy-based authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy =>
                    policy.Requirements.Add(new RoleRequirement("Administrator")));
                options.AddPolicy("Teacher", policy =>
                    policy.Requirements.Add(new RoleRequirement("Teacher")));
                options.AddPolicy("Student", policy =>
                    policy.Requirements.Add(new RoleRequirement("Student")));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Login");
                options.LoginPath = new PathString("/Login");
                options.LogoutPath = new PathString("/Logout");
            });

            services.AddSingleton<IAuthorizationHandler, RoleHandler>();

            services.AddDistributedMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //开发环境异常处理
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //生产环境异常处理
                app.UseExceptionHandler("/Home/Error");
            }

            //使用静态文件
            app.UseStaticFiles();
            app.UseAuthentication();
            //Session
            app.UseSession();

            app.UseCookiePolicy();

            //使用Mvc，设置默认路由为系统登录
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    //template: "{controller=Home}/{action=Index}/{id?}");
                    template: "{controller=Login}/{action=Index}/{id?}");
            });



        }

        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>  
        /// <param name="assemblyName">程序集</param>
        public void AddScopedByClassName(IServiceCollection services, string assemblyName, Func<Type, bool> unclude)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            foreach (var implement in assembly.GetTypes().Where(x => x.IsGenericType == false))
            {
                if (implement.IsDefined(typeof(CompilerGeneratedAttribute), false))
                    continue;

                Type[] interfaceType1 = implement.GetInterfaces();
                var sss = implement.GetInterfaces().Where(x => x.IsGenericType == false).ToArray();
                Type[] interfaceType = implement.GetInterfaces().Where(x => x.IsGenericType == false).Where(unclude).ToArray() as Type[];

                foreach (var service in interfaceType)
                {
                    services.AddTransient(service, implement);
                }
            }
        }
    }
}
