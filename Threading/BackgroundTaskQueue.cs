﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaoFeng.Config;

/****************************************************************
*  Copyright © (2021) www.fayelf.com All Rights Reserved.       *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@fayelf.com                                     *
*  Site : www.fayelf.com                                        *
*  Create Time : 2021-08-19 15:53:43                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Threading
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public BackgroundTaskQueue()
        {
            
        }
        /// <summary>
        /// 设置队列名称
        /// </summary>
        /// <param name="name">队列名称</param>
        public BackgroundTaskQueue(string name) => this.QueueName = name;
        #endregion

        #region 属性
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 任务集
        /// </summary>
        private ConcurrentQueue<Func<CancellationToken, Task>> WorkItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        /// <summary>
        /// 线程同步信号
        /// </summary>
        private ManualResetEventSlim Manual = new ManualResetEventSlim(false);
        /// <summary>
        /// 取消事件
        /// </summary>
        private CancellationTokenSource CancelToken = new CancellationTokenSource();
        /// <summary>
        /// 消费运行状态
        /// </summary>
        private Boolean ConsumeState { get; set; } = false;
        #endregion

        #region 方法
        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="action">事件</param>
        public async Task AddWorkItem(Action action) => await this.AddWorkItem(c => Task.Factory.StartNew(action, c));
        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="action">事件</param>
        /// <param name="state">数据对象</param>
        public async Task AddWorkItem(Action<object> action, object state) => await this.AddWorkItem(c => Task.Factory.StartNew(action, state, c));
        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="workItem">任务</param>
        public async Task AddWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null) return;
            WorkItems.Enqueue(workItem);
            //Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]写进一个任务.");
            Manual.Set();
            Synchronized.Run(() =>
            {
                if (this.CancelToken.IsCancellationRequested || !this.ConsumeState)
                {
                    this.CancelToken = new CancellationTokenSource();
                    this.CancelToken.Token.Register(() =>
                    {
                        Synchronized.Run(() =>
                        {
                            this.ConsumeState = false;
                        });
                    });
                    this.ConsumeState = true;
                    //有新任务,重新启动任务.
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("".PadLeft(70, '='));
                    Console.WriteLine($"-- 有新任务,启动消费任务[{this.QueueName}] - {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff} --");
                    Console.WriteLine("".PadLeft(70, '='));
                    Console.ResetColor();
                    this.ConsumeTask();
                }
            });
            await Task.CompletedTask;
        }
        /// <summary>
        /// 消费任务线程
        /// </summary>
        void ConsumeTask()
        {
            var setting = Setting.Current;
            new Task(async () =>
            {
                if (this.QueueName.IsNotNullOrEmpty())
                    Thread.CurrentThread.Name = this.QueueName;
                while (!this.CancelToken.Token.IsCancellationRequested)
                {
                    if (WorkItems.TryDequeue(out var workItem))
                    {
                        //if (this.CancelToken.IsCancellationRequested) this.CancelToken = new CancellationTokenSource();
                        try
                        {
                            await workItem.Invoke(this.CancelToken.Token);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex, "执行任务队列出错.");
                        }
                    }
                    else
                    {
                        Manual.Reset();
                        Manual.Wait(TimeSpan.FromSeconds(setting.IdleSeconds <= 0 ? 0 : setting.IdleSeconds));
                        if (WorkItems.IsEmpty)
                        {
                            Synchronized.Run(() =>
                            {
                                //等待时长超过消费等待时长限制 终止当前消费任务.
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("".PadLeft(70, '='));
                                Console.WriteLine($"-- 等待时长超过消费等待时长 {setting.IdleSeconds}S 限制,终止当前消费任务[{this.QueueName}] - {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff} --");
                                Console.WriteLine("".PadLeft(70, '='));
                                Console.ResetColor();
                                this.CancelToken.Cancel();
                            });
                        }
                    }
                }
            }, this.CancelToken.Token, TaskCreationOptions.LongRunning).Start();
        }
        /// <summary>
        /// 停止消费任务
        /// </summary>
        public async Task Stop()
        {
            //取消任务
            this.CancelToken.Cancel();
            //清空队列
            this.WorkItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
            await Task.CompletedTask;
        }
        #endregion
    }
}