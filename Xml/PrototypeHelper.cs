﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaoFeng.Xml;
namespace XiaoFeng
{
    /// <summary>
    /// 扩展XML方法
    /// </summary>
    public static partial class PrototypeHelper
    {
        #region 对象转XML
        /// <summary>
        /// 对象转XML
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="t">对象</param>
        /// <param name="encode">编码</param>
        /// <param name="removeNamespaces">是否移除命名空间</param>
        /// <param name="removeXmlDeclaration">是否移除XML声明</param>
        /// <returns></returns>
        public static string ToXml<T>(this T t, string encode = "UTF-8", Boolean removeNamespaces = false, Boolean removeXmlDeclaration = false)
        {
            if (t == null) return String.Empty;
            return XmlConvert.SerializerObject(t, encode, removeNamespaces, removeXmlDeclaration);
        }
        /// <summary>
        /// 对象转XML
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="t">对象</param>
        /// <param name="encoding">编码</param>
        /// <param name="OmitXmlDeclaration">是否忽略XML声明</param>
        /// <param name="OmitEmptyNode">是否忽略空节点</param>
        /// <param name="OmitNamespace">是否忽略命名空间</param>
        /// <param name="OmitComment">是否忽略注释</param>
        /// <param name="Indented">是否格式化</param>
        /// <returns></returns>
        public static string EntityToXml<T>(this T t, Encoding encoding = null, Boolean OmitXmlDeclaration = false, Boolean OmitEmptyNode = true, Boolean OmitNamespace = true, Boolean OmitComment = false, Boolean Indented = true) => XmlSerializer.Serializer(t, encoding, OmitXmlDeclaration, OmitEmptyNode, OmitNamespace, OmitComment, Indented);
        #endregion

        #region XML转对象
        /// <summary>
        /// XML转对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="xml">XML数据</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static T XmlToObject<T>(this String xml, string encode = "UTF-8")
        {
            if (xml.IsNullOrEmpty()) return Activator.CreateInstance<T>();
            return XmlConvert.DeserializeObject<T>(xml, encode);
        }
        /// <summary>
        /// XML转对象
        /// </summary>
        /// <param name="xml">XML数据</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object XmlToObject(this String xml, Type type)
        {
            if (xml.IsNullOrEmpty()) return Activator.CreateInstance(type);
            return XmlConvert.DeserializeObject(type, xml);
        }
        /// <summary>
        /// XML转对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">XML数据</param>
        /// <returns></returns>
        public static T XmlToEntity<T>(this String xml) => XmlSerializer.Deserialize<T>(xml);
        /// <summary>
        /// XML转对象
        /// </summary>
        /// <param name="xml">XML数据</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static object XmlToEntity(this String xml, Type type = null) => XmlSerializer.Deserialize(xml, type);
        #endregion
    }
}