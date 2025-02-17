﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoFeng.Json
{
    /// <summary>
    /// Json格式设置
    /// </summary>
    public class JsonSerializerSetting
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public JsonSerializerSetting() { }
        #endregion

        #region 属性
        /// <summary>
        /// Guid格式
        /// </summary>
        public string GuidFormat { get; set; } = "D";
        /// <summary>
        /// 日期格式
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";
        /// <summary>
        /// 是否格式化
        /// </summary>
        public bool Indented { get; set; } = false;
        /// <summary>
        /// 枚举值
        /// </summary>
        public EnumValueType EnumValueType { get; set; } = 0;
        /// <summary>
        /// 解析最大深度
        /// </summary>
        public int MaxDepth { get; set; } = 28;
        /// <summary>
        /// 是否写注释
        /// </summary>
        public bool IsComment { get; set; } = false;
        /// <summary>
        /// 忽略大小写 key值统一变为小写
        /// </summary>
        public bool IgnoreCase { get; set; } = false;
        /// <summary>
        /// 忽略空节点
        /// </summary>
        public bool OmitEmptyNode { get; set; } = false;
        #endregion
    }
}