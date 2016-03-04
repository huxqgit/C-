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

            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //禁止使用最大化按钮
            //this.MaximizeBox = false;
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
            tabControl1.SelectedTab = tabPageSetDb;
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
                                    "采集名称",
                                    "是否有效",                                    
                                    "采集方式",
                                    "采集周期（秒）",
                                    "采集状态",
                                    "开始时间",
                                    "结束时间",
                                    "采集源",
                                    "源文件前缀",
                                    "源文件后缀",
                                    "数据分割符",
                                    "数据偏移量",
                                    "本地保存路径",
                                    "是否保存至本地",
                                    "对端文件是否删除",
                                    "文件编码",
                                    "字段规则",
                                    "采集源SQL",
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

            int num = 1;
            foreach (XmlNode taskNode in tasksList)
            {
                DataRow dr = dt.NewRow();
                string taskType = GetValueFromAttributes(taskNode.Attributes["TaskType"]);
                dr["序号"] = num++;
                dr["采集名称"] = GetValueFromAttributes(taskNode.Attributes["Name"]);
                dr["是否有效"] = GetValueFromAttributes(taskNode.Attributes["IsEnabled"]).ToLower() == "true" ? "有效" : "无效";                
                dr["采集方式"] = taskType;
                dr["采集周期（秒）"] = GetValueFromAttributes(taskNode.Attributes["Cycle"]);
                dr["采集状态"] = GetValueFromAttributes(taskNode.Attributes["ExeState"]) == "1" ? "正在执行" : "未执行";
                dr["开始时间"] = GetValueFromAttributes(taskNode.Attributes["BeginTime"]);
                dr["结束时间"] = GetValueFromAttributes(taskNode.Attributes["EndTime"]);
                dr["采集源"] = GetValueFromAttributes(taskNode.Attributes["SourceName"]);
                if (taskType.ToLower() == "ftp")
                {
                    dr["源文件前缀"] = GetValueFromAttributes(taskNode.Attributes["FilePrefix"]);
                    dr["源文件后缀"] = GetValueFromAttributes(taskNode.Attributes["FileSuffix"]);
                    dr["数据分割符"] = GetValueFromAttributes(taskNode.Attributes["DataSplit"]);
                    dr["数据偏移量"] = GetValueFromAttributes(taskNode.Attributes["Offset"]);
                    dr["本地保存路径"] = GetValueFromAttributes(taskNode.Attributes["LocalDir"]);
                    dr["是否保存至本地"] = GetValueFromAttributes(taskNode.Attributes["IsDelLocalFile"]).ToLower() == "true" ? "是" : "否";
                    dr["对端文件是否删除"] = GetValueFromAttributes(taskNode.Attributes["IsDelRemoteFile"]).ToLower() == "true" ? "是" : "否";
                    dr["文件编码"] = GetValueFromAttributes(taskNode.Attributes["FileEncoding"]);
                    dr["字段规则"] = GetValueFromAttributes(taskNode.Attributes["Fields"]);
                    dr["采集源SQL"] = @"\";
                }
                else
                {
                    dr["源文件前缀"] = @"\";
                    dr["源文件后缀"] = @"\";
                    dr["数据分割符"] = @"\";
                    dr["数据偏移量"] = @"\";
                    dr["本地保存路径"] = @"\";
                    dr["是否保存至本地"] = @"\";
                    dr["对端文件是否删除"] = @"\";
                    dr["文件编码"] = @"\";
                    dr["字段规则"] = @"\";
                    dr["采集源SQL"] = GetValueFromAttributes(taskNode.Attributes["SourceSqlId"]);
                }
                
                
                dr["入库前操作"] = GetValueFromAttributes(taskNode.Attributes["PrefSqlId"]);
                dr["目标数据库"] = GetValueFromAttributes(taskNode.Attributes["DestDbName"]);
                dr["入库SQL"] = GetValueFromAttributes(taskNode.Attributes["DestSqlId"]);
                dr["入库后操作"] = GetValueFromAttributes(taskNode.Attributes["EndSqlId"]);
                dr["是否开启流式读取"] = GetValueFromAttributes(taskNode.Attributes["UserReaderMode"]).ToLower() == "true" ? "是" : "否";
                dr["是否增量更新"] = GetValueFromAttributes(taskNode.Attributes["IsUpdate"]).ToLower() == "true" ? "是" : "否";
                dr["已有数据检测SQL"] = GetValueFromAttributes(taskNode.Attributes["UpdateCheckSqlId"]);
                dr["增量更新SQL"] = GetValueFromAttributes(taskNode.Attributes["UpdateSqlId"]);
                //dr["操作"] = "修改";
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);

            if (!dataGridViewSelect.Columns.Contains("详情"))
            {
                DataGridViewButtonColumn editcCol = new DataGridViewButtonColumn();
                editcCol.Name = "详情";
                editcCol.UseColumnTextForButtonValue = true;
                editcCol.Text = "详情";
                dataGridViewSelect.Columns.Add(editcCol);
                dataGridViewSelect.Columns[0].Width = 40;
            }

            if (!dataGridViewSelect.Columns.Contains("操作"))
            {
                DataGridViewButtonColumn editcCol = new DataGridViewButtonColumn();
                editcCol.Name = "操作";
                editcCol.UseColumnTextForButtonValue = true;
                editcCol.Text = "修改";
                dataGridViewSelect.Columns.Add(editcCol);
                dataGridViewSelect.Columns[1].Width = 40;
            } 

            dataGridViewSelect.AllowUserToAddRows = false;
            dataGridViewSelect.DataSource = ds;
            dataGridViewSelect.DataMember = autoTable;
            dataGridViewSelect.ReadOnly = true; //禁止编辑

            dataGridViewSelect.Columns[2].Width = 40;
            dataGridViewSelect.Columns[3].Width = 80;
            dataGridViewSelect.Columns[5].Width = 80;
            dataGridViewSelect.Columns[6].Width = 80;
            dataGridViewSelect.Columns[7].Width = 80;
            
            
            //dataGridViewSelect.Columns[27].DefaultCellStyle.ForeColor = Color.Blue;        
        }

        //鼠标移动到某行时更改背景色  
        //private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)   
        //{ 
        //    if (e.RowIndex >= 0)        
        //    {
        //        dataGridViewSelect.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;        
        //    }  
        //} 


        private string GetValueFromAttributes(XmlAttribute att)
        {
            return att == null ? "" : att.Value;            
        }

        /// <summary>
        /// 鼠标悬停事件，只针对'操作'列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex == 27 && this.Cursor != Cursors.Hand)
            {
                //dataGridViewSelect.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                //this.Cursor = Cursors.Hand;//设置为手型
            }      
        }

        /// <summary>
        /// 鼠标悬停离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 27)
            {
                //dataGridViewSelect.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                //this.Cursor = Cursors.Arrow;
                //this.Cursor = Cursors.Default;//离开时恢复默认             
            }            
        }

        /// <summary>
        /// 单元格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSelect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridViewSelect.Columns[e.ColumnIndex].Name == "操作")
            {
                if (dataGridViewSelect.CurrentRow.Cells["采集方式"].Value.ToString().ToLower() == "ftp")
                {
                    tabControl1.SelectedTab = tabPageSetFtp;
                }
                else if (dataGridViewSelect.CurrentRow.Cells["采集方式"].Value.ToString().ToLower() == "db")
                {
                    tabControl1.SelectedTab = tabPageSetDb;
                }
                else if (dataGridViewSelect.CurrentRow.Cells["采集方式"].Value.ToString().ToLower() == "socket")
                {
                    tabControl1.SelectedTab = tabPageSocket;
                }                
            }
            
            if(dataGridViewSelect.Columns[e.ColumnIndex].Name == "详情")
            {
                if (dataGridViewSelect.CurrentRow.Cells["采集方式"].Value.ToString().ToLower() == "ftp")
                {
                    tabControl1.SelectedTab = tabPageSetFtp;
                    SetControlEnabled(groupBox3, false);                      
                }
                else if (dataGridViewSelect.CurrentRow.Cells["采集方式"].Value.ToString().ToLower() == "db")
                {
                    tabControl1.SelectedTab = tabPageSetDb;
                    SetControlEnabled(groupBox2, false);
                }
                else if (dataGridViewSelect.CurrentRow.Cells["采集方式"].Value.ToString().ToLower() == "socket")
                {
                    tabControl1.SelectedTab = tabPageSocket;
                    SetControlEnabled(groupBox4, false);
                }       
            }
        }

        private void ViewTabPage(string pageType, bool isEdit)
        {
            if(pageType == "ftp")
            {

            }
            else if (pageType == "db")
            {

            }
            else if (pageType == "socket")
            {

            }
        }

        private void SetContr

        /// <summary>
        /// 设置TextBox、ComboBox控件是否可编辑
        /// </summary>
        /// <param name="ctr"></param>
        /// <param name="isEnabled"></param>
        private void SetControlEnabled(Control ctr, bool isEnabled)
        {
            if(ctr is GroupBox)
            {
                foreach (Control control in ctr.Controls)
                {
                    if (control is TextBox || control is ComboBox)
                    {
                        control.Enabled = isEnabled;
                    }
                }
            }
            else if(ctr is TextBox || ctr is ComboBox)
            {
                ctr.Enabled = isEnabled;
            }            
        }


    }
}
