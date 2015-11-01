using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace TestProj
{
	class Program
	{
		static void Main(string[] args)
		{
            ////定时任务
            //TestService tServer = new TestService();
            //tServer.TimedTask();
            //return;


            //后台服务
            ServiceBase[] serv = new ServiceBase[]
            {
                new TestService()
            };

            ServiceBase.Run(serv);
		}
	}
}
