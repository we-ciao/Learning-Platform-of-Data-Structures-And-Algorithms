using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DSAA.Repository
{
    /// <summary>
    /// 编译器实体类
    /// </summary>
    [Serializable]
    public class Compiler
    {
        /// <summary>
        /// 编译器ID
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// 编译器名称
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "名字最长允许20个字。")]
        [DisplayName("名称")]
        public String Name { get; set; }

        /// <summary>
        /// 编译器参数
        /// </summary>
        [Required]
        public Boolean isForbidden { get; set; } = false;

        [Required]
        public Boolean isScript { get; set; } = false;

        public String CodeFormat { get; set; }

        public string ExecutionFormat { get; set; }

        public string CompilerPath { get; set; }

        public string CompilerArgs { get; set; }

        public string RunnerPath { get; set; }

        public string RunnerArgs { get; set; }
    }
}