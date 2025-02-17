﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XiaoFeng;
using XiaoFeng.Data;
using XiaoFeng.IO;

namespace XiaoFeng.Model
{
    /// <summary>
    /// 生成Model类操作类
    /// Verstion : 1.0.0
    /// Author : jacky
    /// Email : jacky@zhuovi.com
    /// QQ : 7092734
    /// Site : www.zhuovi.com
    /// Create Time : 2018/08/24 08:47:12
    /// Update Time : 2018/08/24 09:11:53
    /// 2020-11-24
    /// 更新生成模型匹配度
    /// </summary>
    public class MakeModel: Disposable
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public MakeModel() { }
        #endregion

        #region 属性
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        public IDbHelper DataHelper { get; set; }
        /// <summary>
        /// 保存目录
        /// </summary>
        public string SavePath { get; set; }
        /// <summary>
        /// 是否生成表类
        /// </summary>
        public ModelType ModelType { get; set; }
        /// <summary>
        /// 命令空间
        /// </summary>
        public string Namespace { get; set; } = "XiaoFeng.Models";
        /// <summary>
        /// 类模板
        /// </summary>
        private string ModelTemplate { get
            {
                return @"using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Net;
using System.ComponentModel;
using XiaoFeng;
using XiaoFeng.Data;
using XiaoFeng.Model;

namespace {namespace}
{{
	#region {TableName}操作类
	/*
     ===================================================================
        Author : jacky
        Email : jacky@zhuovi.com
        QQ : 7092734
        Site : www.zhuovi.com
        Create Time : {Time}
        Description : 本类有XiaoFeng类库自动生成
     ===================================================================
     */
    /// <summary>
    /// {TableName} 操作类
    /// Version : 1.0.0
    /// Author : jacky
    /// Email : jacky@zhuovi.com
    /// QQ : 7092734
    /// Site : www.zhuovi.com
    /// Update Time : {Time}
    /// </summary>{ViewAttribute}
    [Table(Name = ""{TableFullName}"", PrimaryKey = ""{PrimaryKey}"", ModelType = ModelType.{modelType}, ConnName = ""{ConnName}"", ConnIndex = {ConnIndex})]
    public class {TableName} : Entity<{TableName}>
	{{
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public {TableName}() {{ }}
		#endregion

		#region 属性{Columns}
        #endregion

		#region 析构器
		/// <summary>
		/// 析构器
		/// </summary>
		~{TableName}() {{ base.Dispose(true); }}
		#endregion
	}}
    #endregion
}}";
            } }
        /// <summary>
        /// 类属性模板
        /// </summary>
        public string ColumnTemplate
        {
            get
            {
                return @"
        /// <summary>
        /// {Description}
        /// </summary>
        private {getType} _{Name};
        /// <summary>
        /// {Description}
        /// </summary>
		[DisplayName(""{Description}"")]
        [Description(""{Description}"")]
		[DataObjectField({PrimaryKey}, {IsIdentity}, {isNull}, {Length})]
		[Column(Name = ""{Name}"", PrimaryKey = {PrimaryKey}, AutoIncrement = {IsIdentity}, DataType = ""{Type}"", DefaultValue = ""{DefaultValue}"", Description = ""{Description}"", IsNullable = {isNull}, Length = {Length}, Digit = {Digits}, IsUnique = {IsUnique}, IsIndex = {IsIndex}, AutoIncrementSeed = {AutoIncrementSeed}, AutoIncrementStep = {AutoIncrementStep})]
        public {getType} {Name}
        {{
            get {{ return this._{Name}; }}
            set
            {{
                if (!this._{Name}.EqualsIgnoreCase(value{State}))
                {{
                    var val = this._{Name};
                    this._{Name} = value;
                    this.AddDirty(""{Name}"", val, value);
                }}
            }}
        }}";
            }
        }
        #endregion

        #region 方法

        #region 生成类
        /// <summary>
        /// 生成类
        /// </summary>
        /// <param name="savePath">保存目录</param>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="connName">数据库连接名</param>
        /// <param name="connIndex">数据库索引</param>
        /// <returns></returns>
        public Boolean CreateModel(string savePath = "", string tableName = "", string connName = "", int connIndex = 0)
        {
            if (savePath.IsNotNullOrEmpty()) this.SavePath = savePath;
            if (this.SavePath.IsNullOrEmpty()) this.SavePath = "Model";
            this.SavePath = this.SavePath.GetBasePath();
            FileHelper.Create(this.SavePath, FileAttribute.Directory);
            List<TableModel> Tables = new List<TableModel>();
            /*if (this.ModelType == 0)
            {
                Tables = this.DataHelper.GetTables().Select(a => new TableModel { Name = a, ModelType = ModelType.Table }).Concat(this.DataHelper.GetViews().Select(a => new TableModel { Name = a, ModelType = ModelType.View })).ToList();
            }
            else
                Tables = (this.ModelType== ModelType.Table ? this.DataHelper.GetTables().Select(a => new TableModel { Name = a, ModelType = ModelType.Table }) : this.DataHelper.GetViews().Select(a => new TableModel { Name = a, ModelType = ModelType.View })).ToList();*/
            /*所有表*/
            Tables.AddRange(this.DataHelper.GetTables().Where(a => !a.EqualsIgnoreCase("sqlite_sequence")).Select(a => new TableModel { Name = a, ModelType = ModelType.Table }));
            /*所有视图*/
            var Views = this.DataHelper.GetViews();
            Tables.AddRange(Views.Select(a => new TableModel { Name = a.Name, ModelType = ModelType.View }));

            if (tableName.IsNotNullOrEmpty())
            {
                var tableNames = tableName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //this.ModelType = !tableName.IsMatch(@"VIEW_") ? ModelType.Table : ModelType.View;
                Tables = Tables.Where(a => tableNames.Contains(a.Name)).ToList();
            }
            var providerType = this.DataHelper.ProviderType;
            DataType dataType = new DataType(providerType);
            Tables.Each(table =>
            {
                string classPath = this.SavePath.Trim('\\') + "\\" + table.Name + "_Model.cs";
                string tbName = table.Name;
                Dictionary<string, string> keys = new Dictionary<string, string>
                {
                    { "TableFullName", tbName },
                    { "TableName", /*tbName.Substring(tbName.LastIndexOf("_") + 1)*/tbName.RemovePattern(@"[_-]") + "Model" },
                    { "Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "namespace", this.Namespace.IsNullOrEmpty()?"FayElf.Models":this.Namespace },
                    { "ModelType", table.ModelType.ToString() },
                    { "ProviderType", "DbProviderType." + providerType.ToString() },
                    { "ConnName", connName },
                    { "ConnIndex", connIndex.ToString() },
                    { "ViewAttribute",table.ModelType== ModelType.Table?"":(Environment.NewLine+ $@"    [View(Name = ""{tbName}"", Definition = ""{Views.First(a=>a.Name == tbName)?.Definition.RemovePattern(@"^Create\s+view\s+[a-z0-9-_]*?\s+AS\s+").RemovePattern(@"[\r\n""]+").ReplacePattern(@"\s{2,}"," ")}"")]")  }
                };
                IList<DataColumns> Columns = this.DataHelper.GetColumns(table.Name);
                string ColumnString = "";
                var PrimaryKey = "";
                Columns.Each(c =>
                {
                    if (!c.Name.IsMatch(@"^[\u4e00-\u9fa5a-z0-9_]+$")) return;
                    if (c.Description.IsNullOrWhiteSpace()) c.Description = c.Name;
                    else c.Description = c.Description.RemovePattern(@"[\r\n\t\s]+");
                    IDictionary<string, string> dic = c.ObjectToDictionary();
                    string _type = dataType.GetDotNetType(c.Type);
                    dic.Add("getType", _type + (_type.ToLower() != "string" ? "?" : ""));
                    dic["Type"] = c.Type;
                    dic["State"] = _type.EqualsIgnoreCase("string") ? " ?? string.Empty" : "";
                    var dValue = c.DefaultValue;
                    while (dValue.IsMatch(@"^\(.*?\)$"))
                    {
                        dValue = dValue.ReplacePattern(@"^\((.*?)\)$", "$1");
                    }
                    if (dValue.IsMatch(@"(randomblob\(4\)|newid|uuid)"))
                        dValue = "UUID";
                    dic["DefaultValue"] = dValue;
                    if (dValue.IsMatch(@"(GUID|NEWID)")) dic["getType"] = "Guid?";
                    ColumnString += this.ColumnTemplate.format(dic);
                });
                keys.Add("Columns", ColumnString);
                var pc = Columns.Where(a => a.PrimaryKey && a.IsIdentity);
                if (!pc.Any())
                    pc = Columns.Where(a => a.PrimaryKey);
                if (pc.Any())
                    PrimaryKey = pc.FirstOrDefault().Name;
                keys.Add("PrimaryKey", PrimaryKey);
                FileHelper.WriteText(classPath, this.ModelTemplate.format(keys));
            });
            return true;
        }
        #endregion

        #region Model类
        /// <summary>
        /// Model类
        /// </summary>
        public class TableModel
        {
            /// <summary>
            /// 表名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 是否是表
            /// </summary>
            public ModelType ModelType { get; set; }
        }
        #endregion

        #region 析构器
        /// <summary>
        /// 析构器
        /// </summary>
        ~MakeModel()
        {
            base.Dispose(true);
        }
        #endregion

        #endregion
    }
}