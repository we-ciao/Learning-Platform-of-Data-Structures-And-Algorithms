using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSAA.Repository
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "名字最长允许50个字。")]
        [DisplayName("用户名")]
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DisplayName("密码")]
        public String PassWord { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required]
        [DisplayName("昵称")]
        public String NickName { get; set; }

        /// <summary>
        /// 电子邮箱地址
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "E-mail地址过长，请检查。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$", ErrorMessage = "请输入正确的Email.")]
        [DisplayName("电子邮箱")]
        public String Email { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [DisplayName("手机号")]
        public String Phone { get; set; } 
        
       /// <summary>
       /// 用户组
       /// </summary>
        public virtual ICollection<Group> Groups { get; set; }
        
        /// <summary>
        /// 题目提交数
        /// </summary>
        public Int32 SubmitCount { get; set; }

        /// <summary>
        /// 题目通过数
        /// </summary>
        public Int32 SolvedCount { get; set; }

        /// <summary>
        /// 是否锁定账户
        /// </summary>
        public Boolean IsLocked { get; set; }

        /// <summary>
        /// 注册IP地址
        /// </summary>
        public String CreateIP { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public String LastIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        /// 用户排名
        /// </summary>
        public virtual Double Rank { get; set; }

        /// <summary>
        /// 最后在线时间
        /// </summary>
        public virtual DateTime? LastOnline { get; set; }

        /// <summary>
        /// 获取AC比率
        /// </summary>
        public virtual Double Ratio
        {
            get { return 100 * (this.SubmitCount > 0 ? (Double)this.SolvedCount / (Double)this.SubmitCount : 0); }
        }
    }
}