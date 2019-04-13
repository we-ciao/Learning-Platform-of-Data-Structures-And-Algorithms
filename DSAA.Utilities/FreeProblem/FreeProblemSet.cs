using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DSAA.Utilities.FreeProblem
{
    /// <summary>
    /// Free Problem Set
    /// </summary>
    /// <remarks>
    /// http://code.google.com/p/freeproblemset/wiki/TransportFileDefinition
    /// </remarks>
    public class FreeProblemSet
    {
        #region 常量
        public const Double SupportMaxVersion = 1.2;
        #endregion

        #region 字段
        private List<FreeProblem> _problems = null;
        #endregion

        #region 属性
        /// <summary>
        /// 获取包含的FreeProblem个数
        /// </summary>
        public Int32 Count
        {
            get { return (this._problems != null ? this._problems.Count : 0); }
        }
        #endregion

        #region 索引器
        /// <summary>
        /// 获取指定索引的FreeProblem
        /// </summary>
        /// <param name="index">指定索引</param>
        /// <returns>指定索引的FreeProblem</returns>
        public FreeProblem this[Int32 index]
        {
            get { return this._problems[index]; }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化行的Free Problem Set实体
        /// </summary>
        /// <param name="xml">Xml内容</param>
        public FreeProblemSet(String xml)
        {
            if (String.IsNullOrEmpty(xml))
            {
                throw new Exception();
            }

            XDocument doc = XDocument.Parse(xml);
            XElement xe = doc.Root;

            if (xe == null || xe.Name != "fps")
            {
                throw new Exception();
            }

            XAttribute ver = xe.Attribute("version");
            Double version = 0;

            if (ver == null || !Double.TryParse(ver.Value, out version))
            {
                throw new Exception();
            }

            if (version > FreeProblemSet.SupportMaxVersion)
            {
                throw new Exception();
            }

            this._problems = new List<FreeProblem>();
            IEnumerable<XElement> items = xe.Elements("item");

            if (items == null)
            {
                return;
            }

            foreach (XElement item in items)
            {
                this._problems.Add(new FreeProblem(item));
            }
        }
        #endregion
    }
}