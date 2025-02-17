﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/****************************************************************
*  Copyright © (2021) www.fayelf.com All Rights Reserved.       *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@fayelf.com                                     *
*  Site : www.fayelf.com                                        *
*  Create Time : 2021-05-26 15:59:42                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Http
{
    /// <summary>
    /// 基础接口
    /// </summary>
    public interface IHttpBase
    {
        /// <summary>
        /// 获取或设置与此响应关联的 Cookie
        /// </summary>
        CookieContainer CookieContainer { get; set; }
        /// <summary>
        /// 指定构成 HTTP 标头的名称/值对的集合。
        /// </summary>
        Dictionary<string, string> Headers { get; set; }
        /// <summary>
        /// 获取或设置请求的方法
        /// </summary>
        HttpMethod Method { get; set; }
        /// <summary>
        /// 请求或响应内容类型
        /// </summary>
        string ContentType { get; set; }
    }
}