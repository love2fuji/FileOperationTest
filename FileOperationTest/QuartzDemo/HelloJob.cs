using Common.Logging;
using Quartz;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationTest.QuartzDemo
{
    public class HelloJob:IJob
    {
        NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public void Execute(IJobExecutionContext context)
        {
            log.Info("void: Greetings from HelloJob!");
            Console.WriteLine("Greetings from HelloJob!");
        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            log.Info("Task: Greetings from HelloJob!");
            Console.WriteLine("Task: Greetings from HelloJob!");
            return Task.CompletedTask;
        }
    }
}
