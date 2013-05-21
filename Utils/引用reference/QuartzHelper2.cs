using System;
using System.Collections.Generic;
using System.Text;
using Quartz;
using Quartz.Impl;

namespace QuartzTest
{
    public class QuartzHelper2
    {
        private IScheduler scheduler;
        /// <summary>
        /// quartz使用的是前台线程
        /// </summary>
        public void Start()
        {
            //获取调度器
            ISchedulerFactory sf = new StdSchedulerFactory();
            scheduler = sf.GetScheduler();
            JobDetail jobDetail = new JobDetail("job1", "mygroup", typeof(MyQuartzJob2));
            //立刻开始，重复无限次，间隔5s
            SimpleTrigger trigger = new SimpleTrigger("trigger1", "mygroup", SimpleTrigger.RepeatIndefinitely, new TimeSpan(0, 0, 1));
            //scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.Start();
        }

        public void Stop()
        {
            if (scheduler != null)
            {
                //wait the job completed
                scheduler.Shutdown(true);
            }
        }
    }


    public class MyQuartzJob2 : IJob
    {
        /// <summary>
        /// 每次调度新建一个job实例
        /// </summary>
        public static long I;
        public virtual void Execute(JobExecutionContext context)
        {
            Console.WriteLine("Hello World " + (++I));
        }
    }
}
