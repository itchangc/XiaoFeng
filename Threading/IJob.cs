﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XiaoFeng.Threading
{
    /// <summary>
    /// 作业接口
    /// </summary>
    public interface IJob
    {
        #region 属性
        /// <summary>
        /// 当前调度
        /// </summary>
        IJobScheduler Scheduler { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        JobStatus Status { get; set; }
        /// <summary>
        /// 作业数据
        /// </summary>
        object State { get; set; }
        /// <summary>
        /// 是否是异步
        /// </summary>
        bool Async { get; set; }
        /// <summary>
        /// 已成功运行次数
        /// </summary>
        int SuccessCount { get; set; }
        /// <summary>
        /// 失败运行次数
        /// </summary>
        int FailureCount { get; set; }
        /// <summary>
        /// 运行日志
        /// </summary>
        List<string> Message { get; set; }
        /// <summary>
        /// 取消信号
        /// </summary>
        CancellationTokenSource CancelToken { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// 作业名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 运行次数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 成功回调
        /// </summary>
        Action<IJob> SuccessCallBack { get; set; }
        /// <summary>
        /// 当前任务执行完成后再进入计时队列 此方法最后一定要设置job的状态等待
        /// </summary>
        Action<IJob> CompleteCallBack { get; set; }
        /// <summary>
        /// 失败回调
        /// </summary>
        Action<IJob, Exception> FailureCallBack { get; set; }
        /// <summary>
        /// 停止作业回调
        /// </summary>
        Action<IJob> StopCallBack { get; set; }
        /// <summary>
        /// 最后一次运行时间
        /// </summary>
        DateTime? LastTime { get; set; }
        /// <summary>
        /// 下次运行时间
        /// </summary>
        DateTime? NextTime { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        DateTime? StartTime { get; set; }
        /// <summary>
        /// 最大运行次数
        /// </summary>
        int? MaxCount { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        DateTime? ExpireTime { get; set; }
        /// <summary>
        /// 运行完是否销毁
        /// </summary>
        bool IsDestroy { get; set; }
        /// <summary>
        /// 定时器类型
        /// </summary>
        TimerType TimerType { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        Time Time { get; set; }
        /// <summary>
        /// 间隔 单位毫秒
        /// </summary>
        int Period { get; set; }
        /// <summary>
        /// 是几号还是周几
        /// </summary>
        int[] DayOrWeekOrHour { get; set; }
        #endregion

        #region 启动作业
        /// <summary>
        /// 启动作业
        /// </summary>
        void Start();
        /// <summary>
        /// 启动作业
        /// </summary>
        /// <param name="scheduler">调度</param>
        void Start(IJobScheduler scheduler);
        #endregion

        #region 停止作业
        /// <summary>
        /// 停止作业
        /// </summary>
        void Stop();
        /// <summary>
        /// 停止作业
        /// </summary>
        /// <param name="scheduler">调度</param>
        void Stop(IJobScheduler scheduler);
        #endregion
    }
}