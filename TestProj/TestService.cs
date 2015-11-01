using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using LogHelper;

namespace TestProj
{
    partial class TestService : ServiceBase
    {
        System.Timers.Timer _timer;

        public TestService()
        {
            InitializeComponent();

            _timer = new System.Timers.Timer();
            _timer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["时间间隔(单位分钟)"]) * 60000;
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SysLogger.Instance.WriteErrorLog(string.Format(@"致命错误:{0}", e.ExceptionObject.ToString()));
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        /// <summary>
        /// 定时任务执行
        /// </summary>
        public void TimedTask()
        {

        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
