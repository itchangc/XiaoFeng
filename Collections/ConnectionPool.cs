﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using XiaoFeng.Threading;
using XiaoFeng.Data;

namespace XiaoFeng.Collections
{
    /// <summary>
    /// 数据库连接池
    /// </summary>
    public class ConnectionPool : ObjectPool<DbConnection>
    {
        #region 无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ConnectionPool() {  }
        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="factory">驱动工厂</param>
        /// <param name="connectionString">连接串</param>
        public ConnectionPool(DbProviderFactory factory, string connectionString) : this()
        {
            this.Factory = factory;
            this.ConnectionString = connectionString;
        }
        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="config">连接配置</param>
        public ConnectionPool(ConnectionConfig config) : this()
        {
            this.ConnectionString = config.ConnectionString;
            this.Factory = ProviderFactory.GetDbProviderFactory(config.ProviderType);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 数据库连接
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 驱动工厂
        /// </summary>
        public DbProviderFactory Factory { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        protected override DbConnection OnCreate()
        {
            var conn = Factory?.CreateConnection();
            if (conn == null)
            {
                var msg = "连接创建失败,请检查驱动是否正常.如果本地没有驱动请从nuget上获取.";
                var ex = new Exception(msg);
                LogHelper.Error(ex);
                throw ex;
            }
            conn.ConnectionString = this.ConnectionString;
            try { conn.Open(); }
            catch (DbException ex)
            {
                LogHelper.Error(ex, "数据库连接对象创建失败.");
            }
            finally
            {
                Synchronized.Run(() =>
                {
                    if (this.Job == null)
                    {
                        //Console.ForegroundColor = ConsoleColor.Magenta;
                        LogHelper.Warn($"-- 有新请求,启动过期销毁作业[{this.Name}]. --");
                        //Console.ResetColor();
                        this.Job = new Job
                        {
                            Name = this.Name,
                            Async = true,
                            TimerType = TimerType.Interval,
                            Period = Math.Min(this.TimeOut, 60) * 1000,
                            SuccessCallBack = Work,
                            StopCallBack = job => {
                                this.Job = null;
                                //Console.ForegroundColor = ConsoleColor.Magenta;
                                LogHelper.Warn($"-- 等待时长超过连接等待时长 {Math.Min(this.TimeOut, 60)}S 限制,终止当前作业[{this.Name}]. --");
                                //Console.ResetColor();
                            }
                        };
                        this.Job.Start();
                    }
                });
            }
            return conn;
        }
        /// <summary>
        /// 借资源
        /// </summary>
        /// <returns></returns>
        public override PoolItem<DbConnection> Get()
        {
            var value = base.Get();
            if (value.Value.State == ConnectionState.Closed) value.Value.Open();
            return value;
        }
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="value">资源对象</param>
        /// <returns></returns>
        protected override bool OnGet(PoolItem<DbConnection> value)
        {
            try
            {
                if (base.OnGet(value))
                {
                    if (value.Value.State == ConnectionState.Closed) value.Value.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="value">资源对象</param>
        /// <returns></returns>
        protected override bool OnPut(PoolItem<DbConnection> value)
        {
            return base.OnPut(value);
        }
        /// <summary>
        /// 借一个连接去执行
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public T Execute<T>(Func<DbConnection, T> callback) => this.Execute((db, factory) => callback.Invoke(db));
        /// <summary>
        /// 借一个连接去执行
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public T Execute<T>(Func<DbConnection, DbProviderFactory, T> callback)
        {
            var conn = Get();
            try
            {
                return callback.Invoke(conn.Value, Factory);
            }
            finally
            {
                //if (conn.Value.State == ConnectionState.Open) conn.Value.Close();
                Put(conn);
            }
        }
        ///<inheritdoc/>
        public override void OnDispose(DbConnection value)
        {
            if (value == null) return;
            if (value.State != ConnectionState.Closed)
            {
                value.Close();
                value.Dispose();
            }
            base.OnDispose(value);
        }
        /// <summary>
        /// 关闭资源
        /// </summary>
        /// <param name="obj">资源</param>
        public override void Close(DbConnection obj)
        {
            if (obj == null) return;

            if (obj.State != ConnectionState.Closed)
            {
                //conn.Close();
                //conn.Dispose();
            }
        }
        #endregion
    }
}