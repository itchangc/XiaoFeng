﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XiaoFeng;
namespace XiaoFeng.Json
{
    /// <summary>
    /// JsonValue
    /// </summary>
    public class JsonValue : IComparable, IFormattable, IConvertible, IComparable<JsonValue>, IEquatable<JsonValue>
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public JsonValue() : this(JsonType.Null, null) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="value">值</param>
        public JsonValue(JsonType type, object value)
        {
            this.Type = type;
            this.value = value;
        }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(JsonValue[] value) : this(JsonType.Array, value.ToList<JsonValue>()) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(List<JsonValue> value) : this(JsonType.Array, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(float value) : this(JsonType.Float, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(double value) : this(JsonType.Float, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(decimal value) : this(JsonType.Float, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(long value) : this(JsonType.Number, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(ulong value) : this(JsonType.Number, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(DateTime value) : this(JsonType.DateTime, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(Guid value) : this(JsonType.Guid, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(Type value) : this(JsonType.Type, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(bool value) : this(JsonType.Bool, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(string value) : this(JsonType.String, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(object value) : this(JsonType.Object, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(int value) : this(JsonType.Number, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(uint value) : this(JsonType.Number, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(byte value) : this(JsonType.Byte, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(sbyte value) : this(JsonType.Byte, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(short value) : this(JsonType.Number, value) { }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="value">值</param>
        public JsonValue(ushort value) : this(JsonType.Number, value) { }
        #endregion

        #region 属性
        /// <summary>
        /// 序列化格式
        /// </summary>
        public JsonSerializerSetting SerializerSetting { get; set; } = new JsonSerializerSetting();
        /// <summary>
        /// Json类型
        /// </summary>
        public JsonType Type { get; set; }
        /// <summary>
        /// Json对象值
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
        public object value { get; set; }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public JsonValue this[string key]
        {
            get
            {
                if (this.Type == JsonType.Object)
                {
                    var val = this.ToDictionary();
                    if (val.TryGetValue(key, out var v))
                        return v;
                }
                return new JsonValue();
            }
            set
            {
                if (this.Type == JsonType.Object)
                {
                    var val = this.ToDictionary();
                    if (val.ContainsKey(key))
                    {
                        val[key] = value;
                        this.value = val;
                    }
                }
            }
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public JsonValue this[int index]
        {
            get
            {
                if(this.Type == JsonType.Array)
                {
                    var val = this.ToArray();
                    if (val.Length > index)
                        return val[index];
                }
                return new JsonValue();
            }
            set
            {
                if (this.Type == JsonType.Array)
                {
                    var val = this.ToArray();
                    if (val.Length > index)
                    {
                        val[index] = value;
                        this.value = val;
                    }
                }
            }
        }
        #endregion

        #region 使用JsonValue作为JsonObject 操作对象
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取JsonValue项
        /// 如果没有找到则返回空
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public JsonValue AsObjectGet(string key)
        {
            DebugTool.Assert(this.Type == JsonType.Object, "JsonValue 类型不是 Object !");
            var dict = new Dictionary<string, JsonValue>(StringComparer.OrdinalIgnoreCase);
            (this.value as Dictionary<string, JsonValue>).Each(a =>
            {
                dict.Add(a.Key, a.Value);
            });
            if (dict.TryGetValue(key, out JsonValue jsonValue))
                return jsonValue;
            else
                return null;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取JsonObject项
        /// 如果没有找到则返回空
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public Dictionary<string, JsonValue> AsObjectGetObject(string key)
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue?.AsObject();
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取JsonArray项
        /// 如果没有找到则返回空
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public List<JsonValue> AsObjectGetArray(string key)
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue?.AsArray();
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取字符串项
        /// 如果没有找到则返回空
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string AsObjectGetString(string key)
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue?.ToString();
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取时间项
        /// 如果没有找到则返回默认值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public DateTime AsObjectGetDateTime(string key, DateTime defaultValue = default(DateTime))
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue != null ? jsonValue.ToDateTime() : defaultValue;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取Guid项
        /// 如果没有找到则返回默认值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public Guid AsObjectGetGuid(string key, Guid defaultValue = default(Guid))
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue != null ? jsonValue.ToGuid() : defaultValue;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取浮点项
        /// 如果没有找到则返回默认值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public float AsObjectGetFloat(string key, float defaultValue = default(float))
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue != null ? jsonValue.ToFloat() : defaultValue;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取int项
        /// 如果没有找到则返回默认值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public long AsObjectGetLong(string key, long defaultValue = default(long))
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue != null ? jsonValue.ToLong() : defaultValue;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取Type项
        /// 如果没有找到则返回默认值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public Type AsObjectGetType(string key)
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue?.ToType();
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键获取bool项.
        /// 如果没有找到则返回默认值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public bool AsObjectGetBool(string key, bool defaultValue = default(bool))
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue != null ? jsonValue.ToBoolean() : defaultValue;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject并按键检查null项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public bool AsObjectGetIsNull(string key)
        {
            var jsonValue = this.AsObjectGet(key);
            return jsonValue != null && jsonValue.IsNull();
        }
        #endregion

        #region 基础类型
        /// <summary>
        /// 使用JsonValue作为JsonArray
        /// </summary>
        public List<JsonValue> AsArray()
        {
            DebugTool.Assert(this.Type == JsonType.Array, "JsonValue 类型不是 Array !");
            return this.value as List<JsonValue>;
        }
        /// <summary>
        /// 使用JsonValue作为JsonObject
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, JsonValue> AsObject()
        {
            DebugTool.Assert(this.Type == JsonType.Object, "JsonValue 类型不是 Object !");
            Dictionary<string, JsonValue> data = new Dictionary<string, JsonValue>(StringComparer.OrdinalIgnoreCase);
            (this.value as Dictionary<string, JsonValue>).Each(a =>
            {
                data.Add(a.Key, a.Value);
            });
            return data;
        }
        /// <summary>
        /// JsonValue 是否为空
        /// </summary>
        public bool IsNull()
        {
            return this.Type == JsonType.Null || this.value == null || this.value.IsNullOrEmpty();
        }
        #endregion

        #region 获取Value值
        /*/// <summary>
        /// 获取Value值
        /// </summary>
        /// <param name="name">key</param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                var data = this.ToDictionary();
                if (data == null) return null;
                return data.TryGetValue(name, out var val) ? val.value : null;
            }
        }*/
        /// <summary>
        /// 获取Value值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public T Value<T>(string name = "", T defaultValue = default(T))
        {
            return name.IsNullOrEmpty() ? this.ToObject<T>() : this[name].ToCast<T>(defaultValue);
        }
        #endregion

        #region 比较
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (Type == JsonType.Null || value == null) return -1;
            if (obj is JsonValue v) return this.CompareTo(v);
            if (obj is int || obj is uint || obj is long || obj is ulong ||
                obj is short || obj is ushort || obj is float || obj is double ||
                obj is decimal || obj is char || obj is byte || obj is sbyte ||
                obj is bool)
                if (Type == JsonType.Number || Type == JsonType.Float ||
                    Type == JsonType.Byte)
                {
                    var val = this.value.GetValue(obj.GetType());
                    return val == obj ? 0 : 1;
                }
                else
                    return -1;
            if ((obj is Guid && Type == JsonType.Guid) ||
                (obj is DateTime && Type == JsonType.DateTime) ||
                (obj is Type && Type == JsonType.Type) ||
                ((obj is Array || obj is ArrayList || obj is IEnumerable) && Type == JsonType.Array))
                if (Type == JsonType.Guid)
                    return obj == value ? 0 : -1;
                else
                    return -1;
            if (obj is object && Type == JsonType.Object)
                return obj == value ? 0 : -1;
            return -1;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">其它值</param>
        /// <returns></returns>
        public int CompareTo(JsonValue other)
        {
            return (this.value == other.value && this.Type == other.Type) ? 0 : -1;
        }
        #endregion

        #region 实现== !=
        /// <summary>
        /// 两类型相等
        /// </summary>
        /// <param name="a">第一个对象</param>
        /// <param name="b">第二个对象</param>
        /// <returns></returns>
        public static bool operator ==(JsonValue a, JsonValue b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.Type.Equals(b.Type) && a.value.Equals(b.value);
        }
        /// <summary>
        /// 两类型不相等
        /// </summary>
        /// <param name="a">第一个对象</param>
        /// <param name="b">第二个对象</param>
        /// <returns></returns>
        public static bool operator !=(JsonValue a,JsonValue b)
        {
            return !(a == b);
        }
        #endregion

        #region 强制转换

        #region int
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator int(JsonValue v)
        {
            return v.ToInt();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(int v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region uint
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator uint(JsonValue v)
        {
            return v.ToUInt();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(uint v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region short
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator short(JsonValue v)
        {
            return v.ToShort();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(short v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region ushort
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator ushort(JsonValue v)
        {
            return v.ToUInt16();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(ushort v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region long
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator long(JsonValue v)
        {
            return v.ToLong();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(long v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region ulong
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator ulong(JsonValue v)
        {
            return v.ToUlong();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(ulong v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region byte
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator byte(JsonValue v)
        {
            return v.ToByte();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(byte v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region sbyte
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator sbyte(JsonValue v)
        {
            return v.ToSByte();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(sbyte v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region double
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator double(JsonValue v)
        {
            return v.ToDouble();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(double v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region float
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator float(JsonValue v)
        {
            return v.ToFloat();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(float v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region decimal
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator decimal(JsonValue v)
        {
            return v.ToDecimal();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(decimal v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region bool
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator bool(JsonValue v)
        {
            return v.ToBoolean();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(bool v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region DateTime
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator DateTime(JsonValue v)
        {
            return v.ToDateTime();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(DateTime v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region Guid
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator Guid(JsonValue v)
        {
            return v.ToGuid();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(Guid v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region string
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="v">值</param>
        public static explicit operator string(JsonValue v)
        {
            return v.ToString();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="v">值</param>
        public static implicit operator JsonValue(string v)
        {
            return new JsonValue(v);
        }
        #endregion

        #region 转换类型
        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static JsonValue Parse(string value)
        {
            if (value.IsFloat())
                return new JsonValue(value.ToCast<float>());
            else if (value.IsNumberic())
                return new JsonValue(value.ToCast<long>());
            else if (value.IsDateOrTime())
                return new JsonValue(value.ToCast<DateTime>());
            else if (value.IsGUID())
                return new JsonValue(value.ToCast<Guid>());
            else if (value.IsBoolean())
                return new JsonValue(value.ToCast<Boolean>());
            else if (value.IsJson() || value.IsQuery())
                return value.JsonToObject();
            else
                return new JsonValue(value);
        }
        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="val">返回值</param>
        /// <returns></returns>
        public static bool TryParse(string value, out JsonValue val)
        {
            try
            {
                val = Parse(value);
                return true;
            }
            catch
            {
                val = null;
                return false;
            }
        }
        #endregion

        #endregion

        #region 相等
        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="other">其它值</param>
        /// <returns></returns>
        public bool Equals(JsonValue other)
        {
            return this.value.Equals(other.value) && this.Type.Equals(other.Type);
        }
        #endregion

        #region 获取类型码
        /// <summary>
        /// 获取类型码
        /// </summary>
        /// <returns></returns>
        public TypeCode GetTypeCode()
        {
            return ((IConvertible)Type).GetTypeCode();
        }
        #endregion

        #region 转类型
        /// <summary>
        /// 转类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T ToObject<T>()
        {
            return (T)this.ToObject(typeof(T));
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="target">目标类型</param>
        /// <returns></returns>
        public object ToObject(Type type, object target = null)
        {
            if ((type == typeof(JsonValue) || type == null) && target == null) return this;
            if (type == null && target != null) type = target.GetType();
            ValueTypes valueTypes = type.GetValueType();
            if (this.Type == JsonType.Array)
            {
                if (valueTypes == ValueTypes.Array)
                    return this.ParseArray(this, type, target);
                else if (valueTypes == ValueTypes.DataTable)
                    return this.ParseDataTable(this, type, target);
                else if (valueTypes == ValueTypes.List || valueTypes == ValueTypes.ArrayList)
                    return this.ParseList(this, type, target);
                else if (valueTypes == ValueTypes.Dictionary || valueTypes == ValueTypes.IDictionary)
                    return this.ParseDictionary(this, type, (IDictionary)target);
                else
                    return null;
            }
            else if (this.Type == JsonType.Bool || this.Type == JsonType.Float || this.Type == JsonType.Guid || this.Type == JsonType.Number || this.Type == JsonType.String)
                return this.value.GetValue(type);
            else if(this.Type == JsonType.DateTime)
            {
                return this.value.ToCast<DateTime>().ToString(this.SerializerSetting.DateTimeFormat);
            }
            else if (this.Type == JsonType.Null)
                return null;
            else if (this.Type == JsonType.Object)
                return ParseObject(this, type, target);
            else if (this.Type == JsonType.Type)
                return this.value;
            else
                return this.value.GetValue(type);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <returns></returns>
        public List<JsonValue> ToList()
        {
            return this.Type == JsonType.Array ? this.value as List<JsonValue> : null;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <returns></returns>
        public JsonValue[] ToArray()
        {
            return this.ToList()?.ToArray();
        }
        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public List<T> ToList<T>()
        {
            var list = new List<T>();
            this.ToList()?.Each(v =>
            {
                list.Add(v.ToCast<T>());
            });
            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T[] ToArray<T>()
        {
            return this.ToList<T>()?.ToArray();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public object GetValue(Type type)
        {
            return this.ToObject(type);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public T ToCast<T>(IFormatProvider provider = null)
        {
            return (T)this.GetValue(typeof(T));
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, JsonValue> ToDictionary()
        {
            if (Type == JsonType.Object)
            {
                var data = new Dictionary<string, JsonValue>(StringComparer.OrdinalIgnoreCase);
                (this.value as Dictionary<string, JsonValue>).Each(a =>
                {
                    data.Add(a.Key, a.Value);
                });
                return data;
            }
            return null;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <typeparam name="TKey">Key</typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <returns></returns>
        public Dictionary<TKey,TValue> ToDictionary<TKey, TValue>()
        {
            if (typeof(TKey) == typeof(string)) return this.ToDictionary<TValue>() as Dictionary<TKey,TValue>;
            Dictionary<TKey, TValue> data = new Dictionary<TKey, TValue>();
            this.ToDictionary()?.Each(a =>
            {
                data.Add(a.Key.ToCast<TKey>(), a.Value.ToCast<TValue>());
            });
            return data.Count > 0 ? data : null;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <typeparam name="TValue">Value</typeparam>
        /// <returns></returns>
        public Dictionary<string, TValue> ToDictionary<TValue>()
        {
            Dictionary<string, TValue> data = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
            this.ToDictionary()?.Each(a =>
            {
                data.Add(a.Key, a.Value.ToCast<TValue>());
            });
            return data.Count > 0 ? data : null;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public bool ToBoolean(IFormatProvider provider = null)
        {
            return this.Type == JsonType.Bool ? (bool)value : value.ToString().ToCast<float>() > 0;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public byte ToByte(IFormatProvider provider = null)
        {
            return this.Type == JsonType.Byte ? (byte)value : default(byte);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public char ToChar(IFormatProvider provider = null)
        {
            return Type == JsonType.Char ? (char)value : default(char);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public DateTime ToDateTime(IFormatProvider provider = null)
        {
            return Type == JsonType.DateTime ? (DateTime)value : Type == JsonType.String ? value.ToString().ToCast<DateTime>() : default(DateTime);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public Guid ToGuid(IFormatProvider provider = null)
        {
            return Type == JsonType.Guid ? (Guid)value : Type == JsonType.String ? value.ToString().ToCast<Guid>() : default(Guid);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public decimal ToDecimal(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<decimal>() : default(decimal);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public double ToDouble(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<double>() : default(double);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public float ToFloat(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<float>() : default(float);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public Int16 ToInt16(IFormatProvider provider = null)
        {
            return this.ToShort();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public short ToShort(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<short>() : default(short);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public Int32 ToInt32(IFormatProvider provider = null)
        {
            return this.ToInt();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public int ToInt(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<int>() : default(int);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public Int64 ToInt64(IFormatProvider provider = null)
        {
            return this.ToLong();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public long ToLong(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<long>() : default(long);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public sbyte ToSByte(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<sbyte>() : default(sbyte);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public float ToSingle(IFormatProvider provider = null)
        {
            return this.ToFloat();
        }
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (Type == JsonType.String)
                return value.ToString();
            else if (Type == JsonType.Char || Type == JsonType.Byte ||
                Type == JsonType.Float || Type == JsonType.Number ||
                Type == JsonType.Object)
                return value.ToString();
            else if (Type == JsonType.Guid)
                return value.ToString();
            else if (Type == JsonType.DateTime)
                return value.ToString();
            else if (Type == JsonType.Type)
                return value.GetType().AssemblyQualifiedName;
            else if (Type == JsonType.Array || Type == JsonType.Object)
                return String.Join(",", value);
            return string.Empty;
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider = null)
        {
            return ToString(null, provider);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <param name="conversionType">类型</param>
        /// <returns></returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Type == JsonType.Type ? value.GetValue(conversionType) : default(Type);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public Type ToType(IFormatProvider provider = null) => Type == JsonType.Type ? (Type)value : default(Type);
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public UInt16 ToUInt16(IFormatProvider provider = null)
        {
            return this.ToUShort();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public ushort ToUShort(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<ushort>() : default(ushort);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public UInt32 ToUInt32(IFormatProvider provider = null)
        {
            return this.ToUInt();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public uint ToUInt(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<uint>() : default(uint);
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public UInt64 ToUInt64(IFormatProvider provider = null)
        {
            return this.ToUlong();
        }
        /// <summary>
        /// 转类型
        /// </summary>
        /// <param name="provider">格式</param>
        /// <returns></returns>
        public ulong ToUlong(IFormatProvider provider = null)
        {
            var _ = value.ToString();
            return (Type == JsonType.String || Type == JsonType.Number || Type == JsonType.Float) ? _.ToCast<ulong>() : default(ulong);
        }
        #endregion

        #region 转换成数组
        /// <summary>
        /// 转换成数组
        /// </summary>
        /// <param name="jsonValue">json对象</param>
        /// <param name="type">模板类型</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        private Array ParseArray(JsonValue jsonValue, Type type, object target)
        {
            if (jsonValue == null) return null;
            var list = jsonValue.AsArray();
            var elmType = type?.GetElementXType();
            if (elmType == null) elmType = typeof(object);
            if (!(target is Array arr)) arr = Array.CreateInstance(elmType, list.Count);
            list.For(0, list.Count, i =>
            {
                var item = list[i];
                object val = null;
                if (item != null)
                {
                    val = item.ToObject(elmType, arr.GetValue(i));
                }
                arr.SetValue(val.GetValue(elmType), i);
                i++;
            });
            return arr;
        }
        #endregion

        #region 转换成List
        /// <summary>
        /// 转换成List
        /// </summary>
        /// <param name="jsonValue">json对象</param>
        /// <param name="type">模板类型</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        private IList ParseList(JsonValue jsonValue, Type type, object target)
        {
            if (jsonValue == null) return null;
            var vlist = jsonValue.AsArray();
            var elmType = type.GetGenericArguments().FirstOrDefault();
            if (elmType == null) elmType = typeof(object);
            // 处理一下type是IList<>的情况
            if (type.IsInterface) type = typeof(List<>).MakeGenericType(elmType);
            // 创建列表
            var list = (target ?? Activator.CreateInstance(type)) as IList;
            foreach (var item in vlist)
            {
                object val = null;
                if (item != null) val = item.ToObject(elmType, null);
                list.Add(val.GetValue(elmType));
            }
            return list;
        }
        #endregion

        #region 转换成DataTable
        /// <summary>
        /// 转换成DataTable
        /// </summary>
        /// <param name="jsonValue">jsonValue</param>
        /// <param name="type">目标类型</param>
        /// <param name="target">目标</param>
        /// <returns></returns>
        public DataTable ParseDataTable(JsonValue jsonValue, Type type, object target)
        {
            var data = new DataTable();
            if(jsonValue.Type == JsonType.Object)
            {
                jsonValue = new JsonValue(new List<JsonValue> { jsonValue });
            }
            if(jsonValue.Type == JsonType.Array)
            {
                var list = jsonValue.ToArray();
                if (list.Length == 0) return null;
                var first = list[0].ToDictionary();
                first.Keys.Each(k =>
                {
                    var dataColumn = new DataColumn(k);
                    var val = first[k];
                    if (val.Type == JsonType.Number)
                        dataColumn.DataType = typeof(long);
                    else if (val.Type == JsonType.Float)
                        dataColumn.DataType = typeof(double);
                    else if (val.Type == JsonType.Bool)
                        dataColumn.DataType = typeof(Boolean);
                    else if (val.Type == JsonType.Byte)
                        dataColumn.DataType = typeof(Byte);
                    else if (val.Type == JsonType.Char)
                        dataColumn.DataType = typeof(Char);
                    else if (val.Type == JsonType.DateTime)
                        dataColumn.DataType = typeof(DateTime);
                    else if (val.Type == JsonType.Guid)
                        dataColumn.DataType = typeof(Guid);
                    else if (val.Type == JsonType.String)
                        dataColumn.DataType = typeof(String);
                    else if (val.Type == JsonType.Type)
                        dataColumn.DataType = typeof(Type);
                    else if (val.Type == JsonType.Object || val.Type == JsonType.Null)
                        dataColumn.DataType = typeof(Object);
                    if (k.EqualsIgnoreCase("ID"))
                    {
                        if (dataColumn.DataType == typeof(long))
                        {
                            dataColumn.AutoIncrement = true;
                            dataColumn.AutoIncrementSeed = 1;
                            dataColumn.AutoIncrementStep = 1;
                        }
                        dataColumn.Unique = true;
                    }
                    data.Columns.Add(dataColumn);
                });
                list.Each(j =>
                {
                    data.Rows.Add(j.ToDictionary().Values.ToArray<object>());
                });
            }
            return data;
        }
        #endregion

        #region 转换成对象
        /// <summary>
        /// 转换成对象
        /// </summary>
        /// <param name="jsonValue">Json对象</param>
        /// <param name="type">模板类型</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns> 
        private object ParseObject(JsonValue jsonValue, Type type, object target)
        {
            ValueTypes types = type.GetValueType();
            if (type == typeof(object)) return jsonValue.value;
            Dictionary<string, JsonValue> list = jsonValue.AsObject();
            try
            {
                if (target == null) target = Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
            if (types == ValueTypes.Dictionary || types == ValueTypes.IDictionary)
            {
                return this.ParseDictionary(jsonValue, type, target as IDictionary);
            }
            else if (types == ValueTypes.DataTable)
                return this.ParseDataTable(jsonValue, type, target as DataTable);
            else if (types == ValueTypes.Class || types == ValueTypes.Struct)
            {
                if (list == null || list.Count == 0 /*|| list.FirstOrDefault().Value.Type == JsonType.Null || list.FirstOrDefault().Value.value == null*/) return null;
                type.GetMembers().Each(m =>
                {
                    if (m is FieldInfo || m is PropertyInfo)
                    {
                        if (m.IsDefined(typeof(JsonIgnoreAttribute), false)) return;
                        object val = null;
                        var name = m.Name;
                        if (m.IsDefined(typeof(JsonElement), false))
                        {
                            var element = m.GetCustomAttribute<JsonElement>();
                            if (element.Name.IsNotNullOrEmpty()) name = element.Name;
                        }
                        var _f = list.ContainsKey(name);
                        if (m is FieldInfo f)
                        {
                            val = _f ? list[name].ToObject(f.FieldType, null) : null;
                            if (val != null && _f && list[name].Type != JsonType.Type)
                                val = val.GetValue(f.FieldType);
                            f.SetValue(target, val);
                        }
                        else if (m is PropertyInfo p)
                        {
                            if (!p.CanRead || !p.CanWrite || p.IsIndexer()) return;
                            val = _f ? list[name].ToObject(p.PropertyType, null) : null;
                            if (val != null && _f && list[name].Type != JsonType.Type)
                                val = val.GetValue(p.PropertyType);
                            p.SetValue(target, val, null);
                        }
                    }
                });
            }
            return target;
        }
        #endregion

        #region 转换成字典
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <param name="jsonValue">Json对象</param>
        /// <param name="type">模板类型</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        private IDictionary ParseDictionary(JsonValue jsonValue, Type type, IDictionary target)
        {
            ValueTypes types = type.GetValueType();
            if (types != ValueTypes.Dictionary) return null;
            if (jsonValue.Type == JsonType.Array)
            {
                Dictionary<string, JsonValue> list = new Dictionary<string, JsonValue>();
                var _list = jsonValue.AsArray();
                _list.Each(a =>
                {
                    var data = a.AsObject();
                    if (!data.ContainsKey("key") || !data.ContainsKey("value")) return;
                    var key = data["key"].ToString();
                    JsonValue val = data["value"];
                    list.Add(key, val);                   
                });
                return null;
            }
            else
            {
                Dictionary<string, JsonValue> list = jsonValue.AsObject();

                var _types = type.GetGenericArguments();
                if (target == null)
                {
                    // 处理一下type是Dictionary<,>的情况
                    if (type.IsInterface) type = typeof(Dictionary<,>).MakeGenericType(_types[0], _types[1]);
                    target = Activator.CreateInstance(type) as IDictionary;
                }
                list.Each(a =>
                {
                    var key = new JsonValue(a.Key).ToObject(_types[0], null);
                    var val = a.Value.ToObject(_types[1], null);
                    target.Add(key.GetValue(_types[0]), val.GetValue(_types[1]));
                });
                return target;
            }
        }
        #endregion

        #region 重写
        /// <summary>
        /// 重写 Equals
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is JsonValue val) return this.Equals(val);
            return false;
        }
        /// <summary>
        /// 重写 GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}