using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogHelper
{
    public class SysLogger
    {
		private string _LogPath = string.Empty;
		private string _ErrLogPath = string.Empty;
		private string _SqlLogPath = string.Empty;

		private static object _LogLock = new object();

		public static readonly SysLogger Instance = new SysLogger();		


		private SysLogger()
		{
			string str = AppDomain.CurrentDomain.BaseDirectory + "Log";
			this._LogPath = string.Format(@"{0}\Log", str);
			this._ErrLogPath = string.Format(@"{0}\Error", str);
			this._SqlLogPath = string.Format(@"{0}\SqlLog", str);

			SysFileHandle.CreateFolder(this._LogPath);
			SysFileHandle.CreateFolder(this._ErrLogPath);
			SysFileHandle.CreateFolder(this._SqlLogPath);
		}

		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="content">写入的内容</param>
		public void WriteLog(string content)
		{
			try
			{
				lock (SysLogger._LogLock)
				{
					string logFile = string.Format(@"{0}\Log_{1}.log", _LogPath, DateTime.Now.ToString("yyyy-MM-dd"));
					SysFileHandle.WriteToFile(logFile, content);
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="content">写入的内容</param>
		/// <param name="destFile">欲写入的文件，可以包含路径，也可以不包含路径（此时默认在运行目录的Log\Log下）</param>
		public void WriteLog(string content, string destFile)
		{
			try
			{
				string file = destFile;
				if (string.IsNullOrEmpty(file))
				{
					file = string.Format(@"{0}\Log_{1}.log", _LogPath, DateTime.Now.ToString("yyyy-MM-dd"));
				}
                else
                {
                    file = GetFilePath(file, "Log", _LogPath);
                }

				lock (SysLogger._LogLock)
				{
					SysFileHandle.WriteToFile(file, content);
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 写错误日志
		/// </summary>
		/// <param name="content">写入的内容</param>
		public void WriteErrorLog(string content)
		{
			try
			{
				lock (SysLogger._LogLock)
				{
					string logFile = string.Format(@"{0}\Error_{1}.log", _ErrLogPath, DateTime.Now.ToString("yyyy-MM-dd"));
					SysFileHandle.WriteToFile(logFile, content);
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 写错入日志
		/// </summary>
		/// <param name="content">写入的内容</param>
		/// <param name="destFile">欲写入的文件，可以包含路径，也可以不包含路径（此时默认在运行目录的Log\Error下）</param>
		public void WriteErrorLog(string content, string destFile)
		{
			try
			{
				string file = destFile;                
				if (string.IsNullOrEmpty(file))
				{
					file = string.Format(@"{0}\Error_{1}.log", _ErrLogPath, DateTime.Now.ToString("yyyy-MM-dd"));
				}
                else
                {
                    file = GetFilePath(file, "Error", _ErrLogPath);
                }

				lock (SysLogger._LogLock)
				{
					SysFileHandle.WriteToFile(file, content);
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 记录sql
		/// </summary>
		/// <param name="content">sql语句</param>
		public void WriteSqlLog(string content)
		{
			try
			{
				lock (SysLogger._LogLock)
				{
					string logFile = string.Format(@"{0}\Sql_{1}.log", _SqlLogPath, DateTime.Now.ToString("yyyy-MM-dd"));
					SysFileHandle.WriteToFile(logFile, content);
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 记录sql
		/// </summary>
		/// <param name="content">sql语句</param>
		/// <param name="destFile">欲写入的文件，可以包含路径，也可以不包含路径（此时默认在运行目录的Log\SqlLog下）</param>
		public void WriteSqlLog(string content, string destFile)
		{
			try
			{
				string file = destFile;
				if (string.IsNullOrEmpty(file))
				{
					file = string.Format(@"{0}\Sql_{1}.log", _SqlLogPath, DateTime.Now.ToString("yyyy-MM-dd"));
				}
                else
                {
                    file = GetFilePath(file, "Sql", _SqlLogPath);
                }

				lock (SysLogger._LogLock)
				{
					SysFileHandle.WriteToFile(file, content);
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

        private string GetFilePath(string file, string fileType, string path)
        {
            string fileName = file;
            try
            {                
                //文件名后缀                
                string fileExtension = System.IO.Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(fileExtension))
                {
                    fileName = string.Format(@"{0}.log", fileName);
                }

                if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(fileName)) && !string.IsNullOrEmpty(path))
                {
                    fileName = string.Format(@"{0}\{1}_{2}", path, fileType, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileName;
        }
    }
}
