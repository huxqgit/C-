using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogHelper
{
	public class SysFileHandle
	{
		public static void CreateFolder(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		/// <summary>
		/// 写文件
		/// </summary>
		/// <param name="destFile">含路径的文件名</param>
		/// <param name="content">写入的内容</param>
		public static void WriteToFile(string destFile, string content)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(destFile);
				if (!Directory.Exists(fileInfo.DirectoryName))
				{
					Directory.CreateDirectory(fileInfo.DirectoryName);
				}

				string str = string.Format(@"[{0}]{1}{2}{3}{4}",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					System.Environment.NewLine,
					content,
					System.Environment.NewLine,
					System.Environment.NewLine);
				File.AppendAllText(destFile, str, Encoding.Default);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
	}
}
