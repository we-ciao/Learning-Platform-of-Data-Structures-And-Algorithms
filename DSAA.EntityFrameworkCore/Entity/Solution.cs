using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 提交结果
    /// </summary>
    public enum ResultType : byte
    {
        [Description("Pending")] Pending = 0,
        [Description("Rejudge Pending")] RejudgePending = 1,
        [Description("Judging")] Judging = 2,
        [Description("Compile Error")] CompileError = 3,
        [Description("Runtime Error")] RuntimeError = 4,
        [Description("Time Limit Exceeded")] TimeLimitExceeded = 5,
        [Description("Memory Limit Exceeded")] MemoryLimitExceeded = 6,
        [Description("Output Limit Exceeded")] OutputLimitExceeded = 7,
        [Description("Wrong Answer")] WrongAnswer = 8,
        [Description("Presentation Error")] PresentationError = 9,
        [Description("Accepted")] Accepted = 10,
        [Description("Judge Failed")] JudgeFailed = 255
    }

    /// <summary>
    /// 提交实体类
    /// </summary>
    [Serializable]
    public class Solution : Entity
    {
        /// <summary>
        /// 提交ID
        /// </summary>
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// 题目ID
        /// </summary>
        [Required]
        public virtual Problem Problem { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public virtual User User { get; set; }

        /// <summary>
        /// 源代码
        /// </summary>
        [Required]
        [MaxLength(6000000, ErrorMessage = "源代码最长允许60000个字。")]
        [DisplayName("源代码")]
        public String SourceCode { get; set; }

        /// <summary>
        /// 语言类型
        /// </summary>
        public virtual Compiler LanguageType { get; set; }

        /// <summary>
        /// 结果类型
        /// </summary>
        public ResultType Result { get; set; }

        /// <summary>
        /// 代码长度(Byte)
        /// </summary>
        public Int32 CodeLength { get; set; }

        /// <summary>
        /// 竞赛ID
        /// </summary>
        public virtual Contest Contest { get; set; }

        /// <summary>
        /// 竞赛题目ID
        /// </summary>
        public Int32 ContestProblemID { get; set; }

        /// <summary>
        /// 花费时间(MS)
        /// </summary>
        public Int32 TimeCost { get; set; }

        /// <summary>
        /// 占用内存(KB)
        /// </summary>
        public Int32 MemoryCost { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 评测时间
        /// </summary>
        public DateTime JudgeTime { get; set; }

        /// <summary>
        /// 获取或设提交IP
        /// </summary>
        public String SubmitIP { get; set; }

        /// <summary>
        /// 获取用来输出的结果
        /// </summary>
        public virtual String ResultString
        {
            get
            {
                switch (this.Result)
                {
                    case ResultType.Pending: return "Pending";
                    case ResultType.RejudgePending: return "Rejudge Pending";
                    case ResultType.Judging: return "Judging";
                    case ResultType.CompileError: return "Compile Error";
                    case ResultType.RuntimeError: return "Runtime Error";
                    case ResultType.TimeLimitExceeded: return "Time Limit Exceeded";
                    case ResultType.MemoryLimitExceeded: return "Memory Limit Exceeded";
                    case ResultType.OutputLimitExceeded: return "Output Limit Exceeded";
                    case ResultType.WrongAnswer: return "Wrong Answer";
                    case ResultType.PresentationError: return "Presentation Error";
                    case ResultType.Accepted: return "Accepted";
                    case ResultType.JudgeFailed: return "Judge Failed";
                    default: return String.Empty;
                }
            }
        }
    }
}