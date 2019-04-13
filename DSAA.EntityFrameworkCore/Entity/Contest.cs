using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSAA.EntityFrameworkCore.Entity
{
    /// <summary>
    /// 竞赛状态
    /// </summary>
    public enum ContestStatus : byte
    {
        Pending = 0,
        Registering = 1,
        Running = 2,
        Ended = 3
    }

    /// <summary>
    /// 竞赛类型
    /// </summary>
    public enum ContestType : byte
    {
        Private = 0,
        Public = 1,
        RegisterPrivate = 2,
        RegisterPublic = 3
    }

    /// <summary>
    /// 竞赛实体类
    /// </summary>
    [Serializable]
    public class Contest : Entity
    {

        /// <summary>
        /// 竞赛标题
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// 竞赛描述
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 竞赛类型
        /// </summary>
        public ContestType ContestType { get; set; }

        /// <summary>
        /// 竞赛开始日期
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 竞赛结束日期
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 竞赛注册开始日期
        /// </summary>
        public DateTime? RegisterStartTime { get; set; }

        /// <summary>
        /// 竞赛注册结束日期
        /// </summary>
        public DateTime? RegisterEndTime { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        /// 支持语言
        /// </summary>
        public String SupportLanguage { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public Boolean IsHide { get; set; }

        /// <summary>
        /// 获取竞赛类型名称
        /// </summary>
        [NotMapped]
        public String ContestTypeString
        {
            get
            {
                switch (this.ContestType)
                {
                    case ContestType.Private:
                        return "Private";
                    case ContestType.Public:
                        return "Public";
                    case ContestType.RegisterPrivate:
                        return "Register Private";
                    case ContestType.RegisterPublic:
                        return "Register Public";
                    default:
                        return String.Empty;
                }
            }
        }

        /// <summary>
        /// 获取竞赛状态
        /// </summary>
        [NotMapped]
        public ContestStatus ContestStatus
        {
            get
            {
                if (this.ContestType == ContestType.RegisterPrivate || this.ContestType == ContestType.RegisterPublic)
                {
                    if (DateTime.Now >= this.RegisterStartTime && DateTime.Now <= this.RegisterEndTime)
                    {
                        return ContestStatus.Registering;
                    }
                }

                if (DateTime.Now < this.StartTime)
                {
                    return ContestStatus.Pending;
                }

                if (DateTime.Now >= this.StartTime && DateTime.Now <= this.EndTime)
                {
                    return ContestStatus.Running;
                }

                return ContestStatus.Ended;
            }
        }
    }
}