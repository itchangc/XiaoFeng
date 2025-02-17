﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if (NETCORE && !NETSTANDARD)
using System.Text.Json;
using System.Text.Json.Serialization;
using XiaoFeng;

namespace XiaoFeng.Json
{
    /// <summary>
    /// Guid转换器
    /// </summary>
    public class JsonConverterGuid : JsonConverter<Guid>
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public JsonConverterGuid() { this.Format = "D"; }
        /// <summary>
        /// 设置格式
        /// </summary>
        /// <param name="format">格式</param>
        public JsonConverterGuid(string format)
        {
            this.Format = format;
        }
        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 读JSON
        /// </summary>
        /// <param name="reader">JsonReader</param>
        /// <param name="typeToConvert">类型</param>
        /// <param name="options">配置</param>
        /// <returns></returns>
        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString().ToCast<Guid>();
        }
        /// <summary>
        /// 写JSON
        /// </summary>
        /// <param name="writer">JsonWriter</param>
        /// <param name="value">值</param>
        /// <param name="options">配置</param>
        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
#endif