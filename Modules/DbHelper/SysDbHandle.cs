using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DbHelper
{
    public class SysDbHandle : IDataAccess
    {
        private IDbConnection _IDbConnection = null;
        private IDbCommand _IDbCommand = null;
        private IDbDataAdapter _IDbDataAdapter = null;
        private string _ConnectionString = "";

        /// <summary>
        /// 是否初始化，是：true，否：false
        /// </summary>
        public bool IsInit { get; set; }

        /// <summary>
        /// 初始化失败时的错误信息
        /// </summary>
        public string InitErrorInfo { get; set; }

        /// <summary>
        /// 数据库类型：Informix, Oracle, Mysql, SqlServer, Sqlite
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }

            set
            {
                this._ConnectionString = value;
            }
        }
		
        public IDbConnection Connection
        {
            get
            {
                return this._IDbConnection;
            }
        }

        public IDbCommand Command
        {
            get
            {
                return this._IDbCommand;
            }
        }
        public IDbDataAdapter DataAdapter
        {
            get
            {
                return this._IDbDataAdapter;
            }
        }

        public SysDbHandle(string dbType)
        {

        }

        private void InitDb(string dbType, string connStr)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {            
            if (this._IDbConnection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    this._IDbConnection.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }    
            }                   
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (this._IDbConnection != null)
                {
                    if (this._IDbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        this._IDbConnection.Close();
                    }
                    this._IDbConnection.Dispose();
                    this._IDbConnection = null;

                    if (this._IDbCommand != null)
                    {
                        this._IDbCommand.Dispose();
                        this._IDbCommand = null;
                    }
                    if (this._IDbDataAdapter != null)
                    {
                        this._IDbDataAdapter = null;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
