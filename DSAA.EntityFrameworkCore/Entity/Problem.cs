using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 题目实体类
    /// </summary>
    [Serializable]
    public class Problem : Entity
    {

        /// <summary>
        /// 题目标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(50, ErrorMessage = "标题最长允许50个字。")]
        [DisplayName("标题")]
        public String Title { get; set; }

        /// <summary>
        /// 题目描述
        /// </summary>
        [DisplayName("题目")]
        [Required(ErrorMessage = "题目不能为空")]
        public String Description { get; set; }

        /// <summary>
        /// 输入描述
        /// </summary>
        [DisplayName("输入描述")]
        [Required(ErrorMessage = "输入描述不能为空")]
        public String Input { get; set; }

        /// <summary>
        /// 输出描述
        /// </summary>
        [DisplayName("输出描述")]
        [Required(ErrorMessage = "输出描述不能为空")]
        public String Output { get; set; }

        /// <summary>
        /// 样例输入
        /// </summary>
        [DisplayName("样例输入")]
        [Required(ErrorMessage = "样例输入不能为空")]
        public String SampleInput { get; set; }

        /// <summary>
        /// 样例输出
        /// </summary>
        [DisplayName("样例输出")]
        [Required(ErrorMessage = "样例输出不能为空")]
        public String SampleOutput { get; set; }

        /// <summary>
        /// 题目提示
        /// </summary>
        [DisplayName("题目提示")]
        public String Hint { get; set; }

        /// <summary>
        /// 时间限制(MS)
        /// </summary>
        [Required(ErrorMessage = "编译时间不能为空")]
        [RegularExpression(@"^\+?[1-9]\d*$", ErrorMessage = "编译时间为大于0的正整数。")]
        [DisplayName("编译时间")]
        public Int32 TimeLimit { get; set; }

        /// <summary>
        /// 内存限制(KB)
        /// </summary>
        [Required(ErrorMessage = "内存限制不能为空")]
        [RegularExpression(@"^\+?[1-9]\d*$", ErrorMessage = "内存限制为大于0的正整数。")]
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
        /// 分类题目
        /// </summary>
        public virtual List<ProblemCategory> Categorys { get; set; }

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