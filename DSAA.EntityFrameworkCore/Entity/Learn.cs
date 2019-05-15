using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 学习实体类
    /// </summary>
    public class Learn : Entity
    {
        /// <summary>
        /// 获取或设置名称
        /// </summary>
        [DisplayName("名称")]
        public String Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public String Info { get; set; }

        /// <summary>
        /// 源内容
        /// </summary>
        [Required]
        [DisplayName("源内容")]
        public String Content { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }


        /// <summary>
        /// 分类题目
        /// </summary>
        public virtual List<LearnCategory> Categorys { get; set; }
    }
}
