using Microsoft.AspNetCore.Authorization;

namespace Learning_Platform_of_DSAA.Models
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Role { get; private set; }

        public RoleRequirement(string role)
        {
            this.Role = role;
        }
    }
}
