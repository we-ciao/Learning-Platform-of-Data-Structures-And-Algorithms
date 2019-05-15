using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        [MaxLength(20, ErrorMessage = "名字最长允许20个字。")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{4,16}$", ErrorMessage = "用户名为4到16位（字母，数字，下划线，减号）。")]
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public String PassWord { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [DisplayName("昵称")]
        [Required(ErrorMessage = "昵称不能为空")]
        [MaxLength(20, ErrorMessage = "昵称最长允许20个字。")]
        public String NickName { get; set; }

        /// <summary>
        /// 电子邮箱地址
        /// </summary>
        [DisplayName("电子邮箱")]
        [MaxLength(50, ErrorMessage = "E-mail地址过长，请检查。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$", ErrorMessage = "请输入正确的Email。")]
        public String Email { get; set; } = null;

        /// <summary>
        /// 手机号
        /// </summary>
        [DisplayName("手机号")]
        [RegularExpression(@"^((13[0-9])|(17[0-1,6-8])|(15[^4,\\D])|(18[0-9]))\d{8}$", ErrorMessage = "请输入正确的手机号。")]
        public String Phone { get; set; } = null;

        /// <summary>
        /// 用户组
        /// </summary>
        public virtual Group Group { get; set; }

        /// <summary>
        /// 题目提交数
        /// </summary>
        public Int32 SubmitCount { get; set; } = 0;

        /// <summary>
        /// 题目通过数
        /// </summary>
        public Int32 SolvedCount { get; set; } = 0;

        /// <summary>
        /// 是否锁定账户
        /// </summary>
        public Boolean IsLocked { get; set; } = false;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateDate { get; set; } 


        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 用户排名
        /// </summary>
        public virtual Double Rank { get; set; }

        /// <summary>
        /// 获取AC比率
        /// </summary>
        [NotMapped]
        public virtual Double Ratio
        {
            get { return 100 * (this.SubmitCount > 0 ? (Double)this.SolvedCount / (Double)this.SubmitCount : 0); }
        }
    }
}