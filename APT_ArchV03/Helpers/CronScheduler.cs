using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using APT_ArchV03.Helpers;
using APT_ArchV03.Models;
using Quartz;
using Quartz.Impl;

namespace APT_ArchV03.Helpers
{

    public class JobScheduler

    {        
        private Db_APT_ArchEntities db = new Db_APT_ArchEntities(); //da all'accesso al database

        public static void Start()

        {

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            IJobDetail job = JobBuilder.Create<DelayedAPT>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("IDGJob", "IDG")
                .StartNow()
                .WithSimpleSchedule(s => s
                .WithIntervalInSeconds(120)
                .RepeatForever())
                .Build();

            //ITrigger trigger = TriggerBuilder.Create()

            //  .WithIdentity("IDGJob", "IDG")

            //  .WithCronSchedule("0 2 0 1/1 * ? *")

            //  .StartAt(DateTime.UtcNow)

            //  .WithPriority(1)

            //  .Build();

            scheduler.ScheduleJob(job, trigger);

        }

    }

    public class DelayedAPT : IJob
    {
        private Db_APT_ArchEntities db = new Db_APT_ArchEntities();

        public void Execute(IJobExecutionContext context)
        {
            //Logging

            NLogHandler createNLogHandler = new NLogHandler();

            

            //Implement here the logic for sending delayed APT alerts

            //1.- Find and change stage for all delayed APTs

            var today = DateTime.Now;

            int i = 0;


            foreach (var caw in db.Caws.ToList())
            {
                if (today > caw.caw_dldate && caw.caw_status == 2)
                {
                    caw.caw_status = 4;
                    

                    createNLogHandler.APTLoggerUser("CronJob: APT " + caw.caw_name + " Delayed", "Info");

                    ////Creating Email object
                    //EmailHandler emailHandler = new EmailHandler();

                    //string cc = db.NavResources.FirstOrDefault(x => x.User_name == caw.caw_usrcreator_code).Email.ToLower();
                    //string to = db.NavResources.FirstOrDefault(x => x.Staff_NO == caw.caw_manager_code).Email.ToLower();

                    //emailHandler.EmailSender(to, "URGENT - Delayed CAW for client " + caw.caw_client_code, emailHandler.BodyConstructor(4, caw));

                    //Debugging into a file
                    //using (StreamWriter streamWriter = new StreamWriter(@"C:\tmp\caws.txt", true))
                    //{
                    //    streamWriter.WriteLine(DateTime.Now.ToString() + " " + caw.caw_id + " " + caw.caw_name);
                    //}
                    i++;
                    db.Entry(caw).State = EntityState.Modified;
                    db.SaveChanges();
                }                

            }

            createNLogHandler.APTLoggerUser("Finished Cron at " + DateTime.Now.ToString() + ". " + i + "APTs " + " Modified", "Info");

        }
    }

}