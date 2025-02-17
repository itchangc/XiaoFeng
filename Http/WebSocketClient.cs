﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/****************************************************************
*  Copyright © (2022) www.fayelf.com All Rights Reserved.       *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@fayelf.com                                     *
*  Site : www.fayelf.com                                        *
*  Create Time : 2022-01-17 15:59:53                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Http
{
    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="data">数据</param>
    public delegate void ReceiveMessageEventHandler(byte[] data);
    /// <summary>
    /// 错误事件
    /// </summary>
    /// <param name="msg">错误信息</param>
    public delegate void ErrorEventHandler(string msg);
    /// <summary>
    /// WebSocket客户端
    /// </summary>
    public class WebSocketClient
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public WebSocketClient() { }
        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="url">服务端网址</param>
        /// <param name="cancellationToken">Token凭证</param>
        public WebSocketClient(string url, CancellationToken cancellationToken)
        {
            this.ServerUri = new Uri(url);
            this.CancelToken = cancellationToken;
        }
        #endregion

        #region 属性
        /// <summary>
        /// Token凭证
        /// </summary>
        private CancellationToken CancelToken { get; set; }
        /// <summary>
        /// 服务端网址
        /// </summary>
        private Uri ServerUri { get; set; }
        /// <summary>
        /// 客户端对象
        /// </summary>
        private ClientWebSocket Client { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        public WebSocketState ClientState { get; set; } = WebSocketState.None;
        /// <summary>
        /// 接收数据事件
        /// </summary>
        public event ReceiveMessageEventHandler OnReceiveMessage;
        /// <summary>
        /// 错误事件
        /// </summary>
        public event ErrorEventHandler OnError;
        #endregion

        #region 方法
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            this.ClientState = WebSocketState.Connecting;
            this.Client = new ClientWebSocket();
            try
            {
                /*连接*/
                await this.Client.ConnectAsync(this.ServerUri, this.CancelToken);
                /*接收*/
                HandleMessage().ConfigureAwait(false).GetAwaiter();
            }
            catch (Exception ex)
            {
                this.ClientState = this.Client.State;
                this.OnError?.Invoke(ex.Message);
            }
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <returns></returns>
        public async Task HandleMessage()
        {
            WebSocketReceiveResult result;
            do
            {
                if (this.ClientState != this.Client.State)
                    this.ClientState = this.Client.State;
                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[1024 * 1];
                    do
                    {
                        result = await this.Client.ReceiveAsync(new ArraySegment<byte>(buffer), this.CancelToken);
                        ms.Write(buffer, 0, result.Count);
                        Array.Clear(buffer, 0, 1024);
                    } while (!result.EndOfMessage);
                    this.OnReceiveMessage?.Invoke(ms.ToArray());
                }
            } while (this.Client.State == WebSocketState.Open && !result.CloseStatus.HasValue);
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await this.Client.CloseAsync(WebSocketCloseStatus.NormalClosure, "人为关闭", this.CancelToken);
            this.ClientState = this.Client.State;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="messageType">消息类型</param>
        /// <returns></returns>
        public async Task SendAsync(string msg, WebSocketMessageType messageType = WebSocketMessageType.Text)
        {
            if (this.ClientState == WebSocketState.Open && this.Client.State == WebSocketState.Open)
            {
                try
                {
                    await this.Client.SendAsync(new ArraySegment<byte>(msg.GetBytes()), messageType, true, this.CancelToken);
                }
                catch (Exception ex)
                {
                    this.ClientState = this.Client.State;
                    this.OnError?.Invoke(ex.Message);
                }
            }
            else if (this.ClientState != WebSocketState.None)
            {
                await Task.Delay(100);
                await this.SendAsync(msg, messageType);
            }
            else
            {
                await this.ConnectAsync();
                await this.SendAsync(msg, messageType);
            }
        }
        #endregion
    }
}