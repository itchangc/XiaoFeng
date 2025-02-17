﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/****************************************************************
*  Copyright © (2021) www.fayelf.com All Rights Reserved.       *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@fayelf.com                                     *
*  Site : www.fayelf.com                                        *
*  Create Time : 2021-07-07 11:11:02                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Redis
{
    /// <summary>
    /// 有序集合(ZSet)
    /// </summary>
    public partial class RedisClient : Disposable
    {
        #region 有序集合(ZSet)

        #region 设置
        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public Boolean SetSortedSetMember(string key, Dictionary<object, float> values, int? dbNum = null)
        {
            if (values == null || values.Count == 0) return false;
            var list = new List<object> { key };
            values.Each(a =>
            {
                list.Add(a.Value);
                list.Add(this.GetValue(a.Key));
            });
            return this.Execute(CommandType.ZADD, dbNum, result => result.OK, list.ToArray());
        }
        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<Boolean> SetSortedSetMemberAsync(string key, Dictionary<object, float> values, int? dbNum = null)
        {
            if (values == null || values.Count == 0) return false;
            var list = new List<object> { key };
            values.Each(a =>
            {
                list.Add(a.Value);
                list.Add(this.GetValue(a.Key));
            });
            return await this.ExecuteAsync(CommandType.ZADD, dbNum, async result => await Task.FromResult(result.OK), list.ToArray());
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>有序集合的成员数</returns>
        public int GetSortedSetCount(string key, int? dbNum = null) => this.Execute(CommandType.ZCARD, dbNum, result => result.OK ? (int)result.Value : -1, key);
        /// <summary>
        /// 获取有序集合的成员数 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>有序集合的成员数</returns>
        public async Task<int> GetSortedSetCountAsync(string key, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZCARD, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key);
        /// <summary>
        /// 获取有序集合指定区间分数的成员数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>有序集合的成员数</returns>
        public int GetSortedSetCount(string key, object min, object max, int? dbNum = null) => this.Execute((min is String && max is String) ? CommandType.ZLEXCOUNT : CommandType.ZCOUNT, dbNum, result => result.OK ? (int)result.Value : -1, key, min, max);
        /// <summary>
        /// 获取有序集合指定区间分数的成员数 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>有序集合的成员数</returns>
        public async Task<int> GetSortedSetCountAsync(string key, object min, object max, int? dbNum = null) => await this.ExecuteAsync((min is String && max is String) ? CommandType.ZLEXCOUNT : CommandType.ZCOUNT, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key, min, max);
        /// <summary>
        /// 计算给定的一个或多个有序集的交集并将结果集存储在新的有序集合 destination 中
        /// </summary>
        /// <param name="destKey">存储key</param>
        /// <param name="options">计算项</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public int GetSortedSetInterStore(string destKey, SortedSetOptions options, int? dbNum = null) => this.Execute(CommandType.ZINTERSTORE, dbNum, result => result.OK ? (int)result.Value : -1, new object[] { destKey }.Concat(options.ToArgments()).ToArray());
        /// <summary>
        /// 计算给定的一个或多个有序集的交集并将结果集存储在新的有序集合 destination 中 异步
        /// </summary>
        /// <param name="destKey">存储key</param>
        /// <param name="options">计算项</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<int> GetSortedSetInterStoreAsync(string destKey, SortedSetOptions options, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZINTERSTORE, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), new object[] { destKey }.Concat(options.ToArgments()).ToArray());
        /// <summary>
        /// 计算给定的一个或多个有序集的并集并将结果集存储在新的有序集合 destination 中
        /// </summary>
        /// <param name="destKey">存储key</param>
        /// <param name="options">计算项</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public int GetSortedSetUnionStore(string destKey, SortedSetOptions options, int? dbNum = null) => this.Execute(CommandType.ZUNIONSTORE, dbNum, result => result.OK ? (int)result.Value : -1, new object[] { destKey }.Concat(options.ToArgments()).ToArray());
        /// <summary>
        /// 计算给定的一个或多个有序集的并集并将结果集存储在新的有序集合 destination 中 异步
        /// </summary>
        /// <param name="destKey">存储key</param>
        /// <param name="options">计算项</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<int> GetSortedSetUnionStoreAsync(string destKey, SortedSetOptions options, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZUNIONSTORE, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), new object[] { destKey }.Concat(options.ToArgments()).ToArray());
        /// <summary>
        /// 通过索引区间返回有序集合指定区间内的成员 分数递增排序
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public List<string> GetSortedSetRange(string key, int start = 0, int stop = -1, Boolean isWithScores = false, int? dbNum = null)
        {
            var list = new List<object> { key, start, stop };
            if (isWithScores) list.Add("WITHSCORES");
            return this.Execute(CommandType.ZRANGE, dbNum, result => result.OK ? (List<string>)result.Value : null, list.ToArray());
        }
        /// <summary>
        /// 通过索引区间返回有序集合指定区间内的成员 分数递增排序 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<List<string>> GetSortedSetRangeAsync(string key, int start = 0, int stop = -1, Boolean isWithScores = false, int? dbNum = null)
        {
            var list = new List<object> { key, start, stop };
            if (isWithScores) list.Add("WITHSCORES");
            return await this.ExecuteAsync(CommandType.ZRANGE, dbNum, async result => await Task.FromResult(result.OK ? (List<string>)result.Value : null), list.ToArray());
        }
        /// <summary>
        /// 通过索引区间返回有序集合指定区间内的成员 分数递减排序
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public List<string> GetSortedSetRevRange(string key, int start = 0, int stop = -1, Boolean isWithScores = false, int? dbNum = null)
        {
            var list = new List<object> { key, start, stop };
            if (isWithScores) list.Add("WITHSCORES");
            return this.Execute(CommandType.ZREVRANGE, dbNum, result => result.OK ? (List<string>)result.Value : null, list.ToArray());
        }
        /// <summary>
        /// 通过索引区间返回有序集合指定区间内的成员 分数递减排序 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<List<string>> GetSortedSetRevRangeAsync(string key, int start = 0, int stop = -1, Boolean isWithScores = false, int? dbNum = null)
        {
            var list = new List<object> { key, start, stop };
            if (isWithScores) list.Add("WITHSCORES");
            return await this.ExecuteAsync(CommandType.ZREVRANGE, dbNum, async result => await Task.FromResult(result.OK ? (List<string>)result.Value : null), list.ToArray());
        }
        /// <summary>
        /// 通过字典区间返回有序集合的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public List<string> GetSortedSetRangeByLex(string key, string min, string max, int start = 0, int end = -1, int? dbNum = null) => this.Execute(CommandType.ZRANGEBYLEX, dbNum, result => result.OK ? (List<string>)result.Value : null, key, min, max, "LIMIT", start, end);
        /// <summary>
        /// 通过字典区间返回有序集合的成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<List<string>> GetSortedSetRangeByLexAsync(string key, string min, string max, int start = 0, int end = -1, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZRANGEBYLEX, dbNum, async result => await Task.FromResult(result.OK ? (List<string>)result.Value : null), key, min, max, "LIMIT", start, end);
        /// <summary>
        /// 通过分数区间返回有序集合的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public List<string> GetSortedSetRangeByScore(string key, float min, float max, Boolean isWithScores = false, int start = 0, int end = -1, int? dbNum = null)
        {
            var list = new List<object> { key, min, max };
            if (isWithScores) list.Add("WITHSCORES");
            list.Add("LIMIT");
            list.Add(start);
            list.Add(end);
            return this.Execute(CommandType.ZRANGEBYSCORE, dbNum, result => result.OK ? (List<string>)result.Value : null, list.ToArray());
        }
        /// <summary>
        /// 通过分数区间返回有序集合的成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<List<string>> GetSortedSetRangeByScoreAsync(string key, float min, float max, Boolean isWithScores = false, int start = 0, int end = -1, int? dbNum = null)
        {
            var list = new List<object> { key, min, max };
            if (isWithScores) list.Add("WITHSCORES");
            list.Add("LIMIT");
            list.Add(start);
            list.Add(end);
            return await this.ExecuteAsync(CommandType.ZRANGEBYSCORE, dbNum, async result => await Task.FromResult(result.OK ? (List<string>)result.Value : null), list.ToArray());
        }
        /// <summary>
        /// 获取有序集合中指定成员的索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成员索引</returns>
        public int GetSortedSetRank(string key, string member, int? dbNum = null) => this.Execute(CommandType.ZRANK, dbNum, result => result.OK ? (int)result.Value : -1, key, member);
        /// <summary>
        /// 获取有序集合中指定成员的索引 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成员索引</returns>
        public async Task<int> GetSortedSetRankAsync(string key, string member, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZRANK, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key, member);
        /// <summary>
        /// 获取有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成员索引</returns>
        public int GetSortedSetRevRank(string key, string member, int? dbNum = null) => this.Execute(CommandType.ZREVRANK, dbNum, result => result.OK ? (int)result.Value : -1, key, member);
        /// <summary>
        /// 获取有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成员索引</returns>
        public async Task<int> GetSortedSetRevRankAsync(string key, string member, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZREVRANK, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key, member);
        /// <summary>
        /// 通过有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="max">最大分数</param>
        /// <param name="min">最小分数</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public List<string> GetSortedSetRevRangeByScore(string key, float max, float min, Boolean isWithScores = false, int start = 0, int end = -1, int? dbNum = null)
        {
            var list = new List<object> { key, max, min };
            if (isWithScores) list.Add("WITHSCORES");
            list.Add("LIMIT");
            list.Add(start);
            list.Add(end);
            return this.Execute(CommandType.ZREVRANGEBYSCORE, dbNum, result => result.OK ? (List<string>)result.Value : null, list.ToArray());
        }
        /// <summary>
        /// 通过有序集中指定分数区间内的成员，分数从高到低排序 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="isWithScores">是否带上分数值</param>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<List<string>> GetSortedSetRevRangeByScoreAsync(string key, float min, float max, Boolean isWithScores = false, int start = 0, int end = -1, int? dbNum = null)
        {
            var list = new List<object> { key, min, max };
            if (isWithScores) list.Add("WITHSCORES");
            list.Add("LIMIT");
            list.Add(start);
            list.Add(end);
            return await this.ExecuteAsync(CommandType.ZREVRANGEBYSCORE, dbNum, async result => await Task.FromResult(result.OK ? (List<string>)result.Value : null), list.ToArray());
        }
        /// <summary>
        /// 获取有序集中，成员的分数值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成员索引</returns>
        public float GetSortedSetScore(string key, string member, int? dbNum = null) => this.Execute(CommandType.ZSCORE, dbNum, result => result.OK ? result.Value.ToCast<float>() : -1, key, member);
        /// <summary>
        /// 获取有序集中，成员的分数值 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成员索引</returns>
        public async Task<float> GetSortedSetScoreAsync(string key, string member, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZSCORE, dbNum, async result => await Task.FromResult(result.OK ? (float)result.Value : -1), key, member);
        /// <summary>
        /// 查找Hash中字段名
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pattern">模式 支持*和?</param>
        /// <param name="start">开始位置</param>
        /// <param name="count">遍历条数</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>字段名和值</returns>
        public Dictionary<string, float> SearchSortedSetMember(string key, string pattern, int start = 0, int count = 10, int? dbNum = null) => this.Execute(CommandType.ZSCAN, dbNum, result => (result.Value as IDictionary<string, string>).ToDictionary<string, float>(), key, start, "MATCH", pattern, "COUNT", count);
        /// <summary>
        /// 查找Hash中字段名 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pattern">模式 支持*和?</param>
        /// <param name="start">开始位置</param>
        /// <param name="count">遍历条数</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>字段名和值</returns>
        public async Task<Dictionary<string, float>> SearchSortedSetMemberAsync(string key, string pattern, int start = 0, int count = 10, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZSCAN, dbNum, async result => await Task.FromResult((result.Value as IDictionary<string, string>).ToDictionary<string, float>()), key, start, "MATCH", pattern, "COUNT", count);
        #endregion

        #region 有序集合中对指定成员的分数加上增量 increment
        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">元素</param>
        /// <param name="increment">递增量</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public float SetSortedSetIncrement(string key, object value, int increment = 1, int? dbNum = null) => this.Execute(CommandType.ZINCRBY, dbNum, result => result.OK ? result.Value.ToCast<float>() : 0, key, increment, this.GetValue(value));
        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">元素</param>
        /// <param name="increment">递增量</param>
        /// <param name="dbNum">库索引</param>
        /// <returns></returns>
        public async Task<float> SetSortedSetIncrementAsync(string key, object value, int increment = 1, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZINCRBY, dbNum, async result => await Task.FromResult(result.OK ? result.Value.ToCast<float>() : 0), key, increment, this.GetValue(value));
        #endregion

        #region 移除
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="dbNum">库索引</param>
        /// <param name="members">成员</param>
        /// <returns>成功移除个数</returns>
        public int DelSortedSetMember(string key, int? dbNum, params object[] members) => this.Execute(CommandType.ZREM, dbNum, result => result.OK ? (int)result.Value : -1, new object[] { key }.Concat(members).ToArray());
        /// <summary>
        /// 移除有序集合中的一个或多个成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="dbNum">库索引</param>
        /// <param name="members">成员</param>
        /// <returns>成功移除个数</returns>
        public async Task<int> DelSortedSetMemberAsync(string key, int? dbNum, params object[] members) => await this.ExecuteAsync(CommandType.ZREM, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), new object[] { key }.Concat(members).ToArray());
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="members">成员</param>
        /// <returns>成功移除个数</returns>
        public int DelSortedSetMember(string key, params object[] members) => this.DelSortedSetMember(key, null, members);
        /// <summary>
        /// 移除有序集合中的一个或多个成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="members">成员</param>
        /// <returns>成功移除个数</returns>
        public async Task<int> DelSortedSetMemberAsync(string key, params object[] members) => await this.DelSortedSetMemberAsync(key, null, members);
        /// <summary>
        /// 移除有序集合中给定的字典区间的所有成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成功移除个数</returns>
        public int DelSortedSetMemberByLex(string key, string min, string max, int? dbNum = null) => this.Execute(CommandType.ZREMRANGEBYLEX, dbNum, result => result.OK ? (int)result.Value : -1, key, min, max);
        /// <summary>
        /// 移除有序集合中给定的字典区间的所有成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成功移除个数</returns>
        public async Task<int> DelSortedSetMemberByLexAsync(string key, string min, string max, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZREMRANGEBYLEX, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key, min, max);
        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成功移除个数</returns>
        public int DelSortedSetMemberByScore(string key, float min, float max, int? dbNum = null) => this.Execute(CommandType.ZREMRANGEBYSCORE, dbNum, result => result.OK ? (int)result.Value : -1, key, min, max);
        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成功移除个数</returns>
        public async Task<int> DelSortedSetMemberByScoreAsync(string key, float min, float max, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZREMRANGEBYSCORE, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key, min, max);
        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成功移除个数</returns>
        public int DelSortedSetMemberByRank(string key, int min, int max, int? dbNum = null) => this.Execute(CommandType.ZREMRANGEBYRANK, dbNum, result => result.OK ? (int)result.Value : -1, key, min, max);
        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员 异步
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="dbNum">库索引</param>
        /// <returns>成功移除个数</returns>
        public async Task<int> DelSortedSetMemberByRankAsync(string key, int min, int max, int? dbNum = null) => await this.ExecuteAsync(CommandType.ZREMRANGEBYRANK, dbNum, async result => await Task.FromResult(result.OK ? (int)result.Value : -1), key, min, max);
        #endregion

        #endregion
    }
}