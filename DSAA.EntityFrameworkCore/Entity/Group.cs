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
        public PermissionType Permission { get; set; } = PermissionType.Student;

        public virtual ICollection<User> Users { get; set; }

    }


    /// <summary>
    /// 权限类型
    /// </summary>
    [Flags]
    public enum PermissionType : int
    {
        Student = 0x0,                       //0000 0000 0000 0000 0000 0000 0000 0000   //没有权限
        HttpJudge = 0x1,                  //0000 0000 0000 0000 0000 0000 0000 0001   //0 //独立权限 不得与其他共有
        Teacher = 0x80,                   //0000 0000 0000 0000 0000 0000 1000 0000   //7 //基本管理员权限 所有管理员必须添加
        Administrator = 0x3FFFFFFE   //0011 1111 1111 1111 1111 1111 1111 1110   //30 //-1 去除HttpJudge 超级管理员
    }

}