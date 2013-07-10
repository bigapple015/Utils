using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace Commons
{
    


    public class HPCHelper:IDisposable
    {
        #region member
        /// <summary>
        /// 通知一个或者多个等待的线程已发生事件
        /// </summary>
        private static ManualResetEvent manualReset = new ManualResetEvent(false);

        /// <summary>
        /// methods used to schedule and manage the jobs and tasks in a compute cluster
        /// </summary>
        private IScheduler _scheduler;

        /// <summary>
        /// the specified cluster need to connect
        /// </summary>
        private string clusterName;

        /// <summary>
        /// singleton
        /// </summary>
        private static HPCHelper _instance;

        /// <summary>
        /// get the lastest exception occurs
        /// </summary>
        private Exception _lastException;

        /// <summary>
        /// get the lastest exception occurs
        /// </summary>
        public Exception LastException { get { return _lastException; } }

        #endregion

        #region private constructor

        /// <summary>
        /// private constructor
        /// </summary>
        /// <param name="clusterName"></param>
        private HPCHelper(string clusterName)
        {
            this.clusterName = clusterName;
            _scheduler = new Scheduler();
        }

        #endregion

        #region singleton

        /// <summary>
        /// singleton implement
        /// </summary>
        /// <param name="clusterName"></param>
        /// <returns></returns>
        public static HPCHelper GetInstance(string clusterName)
        {
            if (_instance == null)
            {
                _instance = new HPCHelper(clusterName);
            }

            _instance.clusterName = clusterName;
            return _instance;
        }

        #endregion

        #region get methods

        /// <summary>
        /// connect to a hpc cluster
        /// </summary>
        /// <returns></returns>
        public bool ConnectToHpc()
        {
            try
            {
                _scheduler.Connect(clusterName);
                return true;
            }
            catch (Exception ex)
            {
                _lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// retrieves node list based on the specified filters
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public List<ISchedulerNode> GetNodeList(IFilterCollection filters,ISortCollection sorts)
        {
            List<ISchedulerNode> nodes = new List<ISchedulerNode>();
            foreach (ISchedulerNode schedulerNode in _scheduler.GetNodeList(filters,sorts))
            {
                nodes.Add(schedulerNode);
            }

            return nodes;
        }


        /// <summary>
        /// get list of free nodes (defined none of the nodes are busy or offline)
        /// </summary>
        /// <returns></returns>
        public List<ISchedulerNode> GetFreeNodeList()
        {
            List<ISchedulerNode> frees = new List<ISchedulerNode>();
            foreach (ISchedulerNode node in GetNodeList(null,null))
            {
                ISchedulerNodeCounters counters = node.GetCounters();

                if (counters.BusyCoreCount == 0 && counters.OfflineCoreCount == 0 && counters.NumberOfCores > 0)
                {
                    frees.Add(node);
                }
            }

            return frees;
        }

        /// <summary>
        /// get list of ready nodes
        /// </summary>
        /// <returns></returns>
        public List<ISchedulerNode> GetReadyNodes()
        {
            List<ISchedulerNode> readylist = new List<ISchedulerNode>();
            foreach (ISchedulerNode node in _scheduler.GetNodeList(null,null))
            {
                if (node.State == NodeState.Online && node.Reachable && !node.MoveToOffline)
                {
                    readylist.Add(node);
                }
            }

            return readylist;
        }

        /// <summary>
        /// retrieves a list of jobs based on the specified filters
        /// </summary>
        /// <returns></returns>
        public List<ISchedulerJob> GetJobList(IFilterCollection filters,ISortCollection sorts)
        {
            List<ISchedulerJob> jobs = new List<ISchedulerJob>();
            foreach (ISchedulerJob schedulerJob in _scheduler.GetNodeList(filters,sorts))
            {
                jobs.Add(schedulerJob);
            }

            return jobs;
        }


        /// <summary>
        /// closes the connection between the application and the HPC Job Scheduler Service
        /// </summary>
        public void Close()
        {
            if (_scheduler != null)
            {
                _scheduler.Close();
            }
        }
        #endregion

        #region create or submit

        /// <summary>
        /// The job template that you specify for the job defines the initial default values and constraints for many of the job properties.
        /// Any changes that you make to properties must be made within the constraints of the template.
        /// </summary>
        /// <param name="jobName">the name of job</param>
        /// <param name="commandLine">the command line for the task</param>
        /// <param name="username">the name of the runas user, in the form domain\username</param>
        /// <param name="password">the password for the runas user</param>
        public void CreateJob(string jobName,string commandLine,string username,string password)
        {
            ISchedulerJob job = null;
            ISchedulerTask task = null;

            manualReset.Reset();
            //create job
            job = _scheduler.CreateJob();
            job.Name = jobName;

            //create task
            task = job.CreateTask();
            task.CommandLine = commandLine;

            //add task to job
            job.AddTask(task);

            //specify the events that you want to receive
            job.OnJobState += OnJobStateCallback;
            job.OnTaskState += OnTaskStateCallback;

            //start the job
            _scheduler.SubmitJob(job,username,password);

            //block so the events get delivered
            manualReset.WaitOne();
        }

        /// <summary>
        /// a parametric sweep is a parallel computing job that consists of running multiple iterations of same command using different input values and output files
        /// each occurrence of an asterisk found on the command line is replaced with the value
        /// </summary>
        /// <param name="jobName">the name of job</param>
        /// <param name="commandLine">a parametric command for the task</param>
        /// <param name="startValue">the starting instance value. the value must be > 0, inclusive</param>
        /// <param name="endValue">the ending value. >= startValue. inclusive</param>
        /// <param name="incrementValue">the increment value</param>
        /// <param name="username">the name of the runas user, in the form domain\username</param>
        /// <param name="password">the password for the runas user</param>
        public void CreateParametricJob(string jobName,string commandLine,int startValue,int endValue,int incrementValue,string username,string password)
        {
            ISchedulerJob job = null;
            ISchedulerTask task = null;

            manualReset.Reset();
            //create a job
            job = _scheduler.CreateJob();
            job.OnJobState += OnJobStateCallback;
            job.OnTaskState += OnTaskStateCallback;
            job.Name = jobName;
            

            task = job.CreateTask();
            //each occurrence of an asterisk found on the command line is replaced with the value
            task.CommandLine = commandLine;
            task.Type = TaskType.ParametricSweep;
            task.StartValue = startValue;
            task.EndValue = endValue;
            task.IncrementValue = incrementValue;
            job.AddTask(task);

            _scheduler.SubmitJob(job,username,password);
            manualReset.WaitOne();
        }

        private static void OnJobStateCallback(object sender, JobStateEventArg args)
        {
            if (JobState.Canceled == args.NewState || JobState.Failed == args.NewState ||
                JobState.Finished == args.NewState)
            {
                manualReset.Set();
            }
            else
            {
                try
                {
                    IScheduler scheduler = (IScheduler) sender;
                    ISchedulerJob job = scheduler.OpenJob(args.JobId);

                    //TODO: do something with the job
                }
                catch (Exception)
                {
                    
                }
            }
        }


        private static void OnTaskStateCallback(object sender, TaskStateEventArg args)
        {
            if (TaskState.Finished == args.NewState || TaskState.Failed == args.NewState ||
                TaskState.Canceled == args.NewState)
            {
                try
                {
                    IScheduler scheduler = (IScheduler) sender;
                    ISchedulerJob job = scheduler.OpenJob(args.JobId);

                    ISchedulerTask task = job.OpenTask(args.TaskId);
                    
                }
                catch (Exception)
                {
                    
                }
            }
        }

        #endregion

        #region Executing commands

        /// <summary>
        /// commands are special job that contains a single task and cab be run only by administrators
        /// A command job is not scheduled (it runs immediately)
        /// </summary>
        /// <param name="commandLine">the command to execute</param>
        /// <param name="info">provides additional property values used by command. if no additional property values, set to null</param>
        /// <param name="nodes">indentify the nodes on which the command will run.</param>
        /// <param name="userName">the name of the runas user, in the form domain\username</param>
        /// <param name="password">the password for the runas user</param>
        public void CreateCommand(string commandLine,ICommandInfo info,IStringCollection nodes,string userName,string password)
        {
            if (UserPrivilege.Admin != _scheduler.GetUserPrivilege())
            {
                return;
            }

            manualReset.Reset();

            //create the command
            IRemoteCommand command = _scheduler.CreateCommand(commandLine,info,nodes);

            //subscribe to one or more events before starting the command
            command.OnCommandJobState += OnJobStateCallback;
            command.OnCommandTaskState += OnCommandTaskStateCallback;
            command.OnCommandOutput += OnCommandOutputCallback;
            command.OnCommandRawOutput += OnCommandRawOutputCallback;

            command.StartWithCredentials(userName,password);

            manualReset.WaitOne();
        }


        public static void OnCommandRawOutputCallback(object sender, CommandRawOutputEventArg args)
        {
            
        }


        public static void OnCommandTaskStateCallback(object sender, CommandTaskStateEventArg args)
        {
            if (args.IsProxy)
            {
                return;
            }

            if (args.NewState == TaskState.Finished || args.NewState == TaskState.Canceled ||
                args.NewState == TaskState.Failed)
            {
                
            }
        }



        public static void OnCommandOutputCallback(object sender, CommandOutputEventArg args)
        {
            if (args.Type == CommandOutputType.Eof)
            {
                ;
            }
            else if (args.Type == CommandOutputType.Output)
            {
                Console.WriteLine("Formatted output for node {0} msg: {1}",args.NodeName,args.Message);
            }
            else if(args.Type == CommandOutputType.Error)
            {
                Console.WriteLine("Error message for node {0} msg: {1}",args.NodeName,args.Message);
            }
        }



        #endregion

        #region implement IDisposable
        /// <summary>
        /// release resource
        /// </summary>
        public void Dispose()
        {
            Close();
        }
        #endregion
    }

}
