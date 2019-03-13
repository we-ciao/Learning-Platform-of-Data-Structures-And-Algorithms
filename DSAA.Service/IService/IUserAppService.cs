using DSAA.EntityFrameworkCore.Entity;

namespace DSAA.Service.IService
{
    public interface IUserAppService
    {
        User CheckUser(string userName, string password);

    }
}
