using DSAA.EntityFrameworkCore.Entity;
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



    }
}
