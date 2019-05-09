using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 题目类型种类实体类
    /// </summary>
    [Serializable]
    public class Category : Entity
    {
        /// <summary>
        /// 获取或设置题目类型种类名称
        /// </summary>
        [DisplayName("名称")]
        public String Title { get; set; }

        /// <summary>
        /// 获取或设置显示顺序
        /// </summary>
        public Int32 Order { get; set; }

        /// <summary>
        /// 分类题目
        /// </summary>
        public virtual List<ProblemCategory> Problems { get; set; }
    }
}