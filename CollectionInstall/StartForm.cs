using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CollectionInstall
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectTasks();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAdd;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageSet;
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            SetTheCollectTypeItem();
            SelectTasks();
        }

        /// <summary>
        /// 设置“采集方式”选择框的值
        /// </summary>
        private void SetTheCollectTypeItem()
        {
            ListItem listItem_0 = new ListItem("0", "全部");
            ListItem listItem_1 = new ListItem("1", "Db");
            ListItem listItem_2 = new ListItem("2", "Ftp");
            ListItem listItem_3 = new ListItem("3", "Socket");

            theCollectType.Items.Add(listItem_0);
            theCollectType.Items.Add(listItem_1);
            theCollectType.Items.Add(listItem_2);
            theCollectType.Items.Add(listItem_3);

            theCollectType.SelectedIndex = 0;
            theCollectType.SelectedItem = listItem_0;
        }

        private void SelectTasks()
        {
            tabControl1.SelectedTab = tabPageSelect;

            string configFile = string.Format(@"{0}\{1}\{2}",
                System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), "Config", "TasksConfig.xml");
            if (!File.Exists(configFile))
            {
                return;
            }

            //手动生成DataSet数据
            string autoTable = "TaskConfig";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(autoTable);

            string[] itemArray = {
                                    "序号",
                                    "是否有效",
                                    "采集名称",
                                    "采集方式",
                                    "采集周期（秒）",
                                    "采集状态",
                                    "开始时间",
                                    "结束时间",
                                    "采集源",
                                    "源文件前缀（FTP方式）",
                                    "源文件后缀（FTP方式）",
                                    "数据分割符（FTP方式）",
                                    "数据偏移量（FTP方式）",
                                    "本地保存路径（FTP方式）",
                                    "是否保存至本地（FTP方式）",
                                    "对端文件是否删除（FTP方式）",
                                    "文件编码（FTP方式）",
                                    "字段规则（FTP方式）",
                                    "采集源SQL（DB方式）",
                                    "入库前操作",
                                    "目标数据库",
                                    "入库SQL",
                                    "入库后操作",
                                    "是否开启流式读取",
                                    "是否增量更新",
                                    "已有数据检测SQL",
                                    "增量更新SQL"
                                 };
            foreach (string item in itemArray)
            {
                dt.Columns.Add(new DataColumn(item, typeof(string)));
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFile);
            XmlNodeList tasksList = xmlDoc.GetElementsByTagName("TaskItem");
            if (tasksList == null)
            {
                return;
            }

            int i = 1;
            foreach (XmlNode taskNode in tasksList)
            {
                DataRow dr = dt.NewRow();
                dr["序号"] = i++;
                dr["是否有效"] = GetValueFromAttributes(taskNode.Attributes["IsEnabled"]).ToLower() == "true" ? "有效" : "无效";
                dr["采集名称"] = GetValueFromAttributes(taskNode.Attributes["Name"]);
                dr["采集方式"] = GetValueFromAttributes(taskNode.Attributes["TaskType"]);
                dr["采集周期（秒）"] = GetValueFromAttributes(taskNode.Attributes["Cycle"]);
                dr["采集状态"] = GetValueFromAttributes(taskNode.Attributes["ExeState"]) == "1" ? "正在执行" : "未执行";
                dr["开始时间"] = GetValueFromAttributes(taskNode.Attributes["BeginTime"]);
                dr["结束时间"] = GetValueFromAttributes(taskNode.Attributes["EndTime"]);
                dr["采集源"] = GetValueFromAttributes(taskNode.Attributes["SourceName"]);
                dr["源文件前缀（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["FilePrefix"]);
                dr["源文件后缀（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["FileSuffix"]);
                dr["数据分割符（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["DataSplit"]);
                dr["数据偏移量（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["Offset"]);
                dr["本地保存路径（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["LocalDir"]);
                dr["是否保存至本地（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["IsDelLocalFile"]).ToLower() == "true" ? "是" : "否";
                dr["对端文件是否删除（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["IsDelRemoteFile"]).ToLower() == "true" ? "是" : "否";
                dr["文件编码（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["FileEncoding"]);
                dr["字段规则（FTP方式）"] = GetValueFromAttributes(taskNode.Attributes["Fields"]);
                dr["采集源SQL（DB方式）"] = GetValueFromAttributes(taskNode.Attributes["SourceSqlId"]);
                dr["入库前操作"] = GetValueFromAttributes(taskNode.Attributes["PrefSqlId"]);
                dr["目标数据库"] = GetValueFromAttributes(taskNode.Attributes["DestDbName"]);
                dr["入库SQL"] = GetValueFromAttributes(taskNode.Attributes["DestSqlId"]);
                dr["入库后操作"] = GetValueFromAttributes(taskNode.Attributes["EndSqlId"]);
                dr["是否开启流式读取"] = GetValueFromAttributes(taskNode.Attributes["UserReaderMode"]).ToLower() == "true" ? "是" : "否";
                dr["是否增量更新"] = GetValueFromAttributes(taskNode.Attributes["IsUpdate"]).ToLower() == "true" ? "是" : "否";
                dr["已有数据检测SQL"] = GetValueFromAttributes(taskNode.Attributes["UpdateCheckSqlId"]);
                dr["增量更新SQL"] = GetValueFromAttributes(taskNode.Attributes["UpdateSqlId"]);

                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);

            dataGridViewSelect.AllowUserToAddRows = false;
            dataGridViewSelect.DataSource = ds;
            dataGridViewSelect.DataMember = autoTable;
        }

        private string GetValueFromAttributes(XmlAttribute att)
        {
            return att == null ? "" : att.Value;            
        }
    }
}
