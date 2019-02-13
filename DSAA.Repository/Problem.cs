using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DSAA.Repository
{
    /// <summary>
    /// 题目实体类
    /// </summary>
    [Serializable]
    public class Problem
    {
        /// <summary>
        /// 题目ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 题目标题
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "标题最长允许50个字。")]
        [DisplayName("标题")]
        public String Title { get; set; }

        /// <summary>
        /// 题目描述
        /// </summary>
        [Required]
        [DisplayName("题目")]
        public String Description { get; set; }

        /// <summary>
        /// 输入描述
        /// </summary>
        [DisplayName("输入描述")]
        public String Input { get; set; }

        /// <summary>
        /// 输出描述
        /// </summary>
        [DisplayName("输出描述")]
        public String Output { get; set; }

        /// <summary>
        /// 样例输入
        /// </summary>
        [DisplayName("样例输入")]
        public String SampleInput { get; set; }

        /// <summary>
        /// 样例输出
        /// </summary>
        [DisplayName("样例输出")]
        public String SampleOutput { get; set; }

        /// <summary>
        /// 题目提示
        /// </summary>
        [DisplayName("题目提示")]
        public String Hint { get; set; }

        /// <summary>
        /// 时间限制(MS)
        /// </summary>
        [Required]
        [DisplayName("编译时间")]
        public Int32 TimeLimit { get; set; }

        /// <summary>
        /// 内存限制(KB)
        /// </summary>
        [Required]
        [DisplayName("内存限制")]
        public Int32 MemoryLimit { get; set; }

        /// <summary>
        /// 用户提交数
        /// </summary>
        public Int32 SubmitCount { get; set; }

        /// <summary>
        /// 用户通过数
        /// </summary>
        public Int32 SolvedCount { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public Boolean IsHide { get; set; }

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