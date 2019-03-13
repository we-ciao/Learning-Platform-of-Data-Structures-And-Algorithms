using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 用户组实体类
    /// </summary>
    [Serializable]
    public class Group : Entity
    {
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "名字最长允许50个字。")]
        [DisplayName("组名")]
        public String GroupName { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public Boolean isAdmin { get; set; } = false;

        public virtual ICollection<User> Users { get; set; }

    }

}