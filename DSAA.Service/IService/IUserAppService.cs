using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Threading.Tasks;

namespace DSAA.Service.IService
{
    public interface IUserAppService:IService<User>
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        string SignUp(User userModel);

        Task<User> CheckUser(string userName, string password);

        void SetCurrentUser(User user);


        /// <summary>
        /// 获取当前登陆的用户实体
        /// </summary>
        User GetCurrentUser();

        /// <summary>
        /// 获取当前登陆的用户名
        /// </summary>
        String GetCurrentUserName();


        /// <summary>
        /// 增加或修改一条用户
        /// </summary>
        string InsertOrUpdateUser(User entity);
    }
}
