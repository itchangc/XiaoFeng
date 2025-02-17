﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoFeng.Data.SQL
{
    #region 数据库语句类型
    /// <summary>
    /// 数据库语句类型
    /// </summary>
    public enum SQLType
    {
        /// <summary>
        /// 空
        /// </summary>
        NULL = 0,
        /// <summary>
        /// 查询
        /// </summary>
        select = 1,
        /// <summary>
        /// 插入
        /// </summary>
        insert = 2,
        /// <summary>
        /// 更新
        /// </summary>
        update = 3,
        /// <summary>
        /// 删除
        /// </summary>
        delete = 4,
        /// <summary>
        /// 联表
        /// </summary>
        join = 5,
        /// <summary>
        /// 是否存在
        /// </summary>
        exists = 6,
        /// <summary>
        /// 跳过多少条
        /// </summary>
        limit = 7,
        /// <summary>
        /// 删除表
        /// </summary>
        drop = 8,
        /// <summary>
        /// 初始化表
        /// </summary>
        truncate = 9,
        /// <summary>
        /// 自增长ID
        /// </summary>
        AutoIncrement = 10,
        /// <summary>
        /// 分组
        /// </summary>
        groupby = 11
    }
    #endregion
}
