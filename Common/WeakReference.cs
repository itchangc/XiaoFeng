﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoFeng
{
    /// <summary>
    /// 弱引用
    /// </summary>
    /// <typeparam name="T">目标引用对象类型</typeparam>
    public class WeakReference<T> : WeakReference
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public WeakReference() : base(null) { }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="target">目标引用对象</param>
        public WeakReference(T target) : base(target) { }
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="target">目标引用对象</param>
        /// <param name="trackResurrection"></param>
        public WeakReference(T target, bool trackResurrection) : base(target, trackResurrection) { }
        /// <summary>
        /// 目标引用对象
        /// </summary>
        public new T Target
        {
            get { return (T)base.Target; }
            set { base.Target = value; }
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="obj">弱引用对象</param>
        /// <returns></returns>
        public static implicit operator T(WeakReference<T> obj)
        {
            if (obj != null && obj.Target != null) return obj.Target;
            return default(T);
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="target">目标引用对象</param>
        /// <returns></returns>
        public static implicit operator WeakReference<T>(T target)=> new WeakReference<T>(target);
    }
}
