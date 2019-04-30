using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using DSAA.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DSAA.Service
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    public class UserAppService : ServiceBase<User>, IUserAppService
    {
        //用户管理仓储接口
        private readonly IUserRepository _userReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public UserAppService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : base(userRepository)
        {
            _userReporitory = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> CheckUser(string userName, string password)
        {
            password = MD5Encrypt.Encrypt(userName, password);
            return await _userReporitory.CheckUser(userName, password);
        }

        public string SignUp(User userModel)
        {
            if (_userReporitory.CheckUser(userModel.UserName))
            {
                return string.Format("用户名 \"{0}\"已经存在!", userModel.UserName);
            }

            userModel.PassWord = MD5Encrypt.Encrypt(userModel.UserName, userModel.PassWord);
            userModel.NickName = HtmlEncoder.HtmlEncode(userModel.NickName);

            userModel.CreateDate = DateTime.Now;
            if (_userReporitory.SignUp(userModel))
                return null;
            else
                return "注册失败!";
        }


        /// <summary>
        /// 设置当前登录用户
        /// </summary>
        public void SetCurrentUser(User user)
        {
            if (user != null)
            {
                string role = string.Empty;

                if (user.Group == null)
                    role = "Student";
                else
                    switch (user.Group.Permission)
                    {
                        case PermissionType.Administrator:
                            role = "Administrator";
                            break;
                        case PermissionType.Teacher:
                            role = "Teacher";
                            break;
                        default:
                            role = "Student";
                            break;
                    }

                var _isession = _httpContextAccessor.HttpContext.Session;
                ////记录Session
                //_isession.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                _isession.Set("UserId", System.Text.Encoding.UTF8.GetBytes(user.Id.ToString()));
                _isession.Set("UserName", System.Text.Encoding.UTF8.GetBytes(user.UserName));
                _isession.Set("UserNickName", System.Text.Encoding.UTF8.GetBytes(user.NickName));
                _isession.Set("UserRole", System.Text.Encoding.UTF8.GetBytes(role));

            }
        }

        /// <summary>
        /// 获取当前登陆的用户名
        /// </summary>
        public String GetCurrentUserName()
        {
            string UserNickName = null;
            byte[] bytes;
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("UserNickName", out bytes))
            {
                UserNickName = System.Text.Encoding.UTF8.GetString(bytes);
            }

            return UserNickName;
        }


        /// <summary>
        /// 获取当前登陆的用户实体
        /// </summary>
        public User GetCurrentUser()
        {
            int UserID = -1;
            byte[] bytes;
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("UserId", out bytes))
            {
                UserID = Convert.ToInt32(System.Text.Encoding.UTF8.GetString(bytes));
            }

            return _userReporitory.Get(UserID);
        }


    }
}