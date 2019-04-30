using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DSAA.Service
{
    /// <summary>
    /// 编译器管理服务
    /// </summary>
    public class CompilerManager : ServiceBase<Compiler>, ICompilerAppService
    {
        //用户管理仓储接口
        private readonly ICompilerRepository _compilerReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public CompilerManager(IHttpContextAccessor httpContextAccessor, ICompilerRepository compilerReporitory, IConfiguration configuration) : base(compilerReporitory)
        {
            _compilerReporitory = compilerReporitory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
    }
}
