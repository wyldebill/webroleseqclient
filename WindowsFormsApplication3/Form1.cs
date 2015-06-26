using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Serilog;
using Serilog.Debugging;
using Serilog.Events;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {

           

            InitializeComponent();
            var quota = "10GB";
            var userr = "william";



            var log = new LoggerConfiguration().MinimumLevel.Verbose()
                .WriteTo.RollingFile(@"d:\log-{Date}.txt")
                .WriteTo.Seq("http://seqtestbill.cloudapp.net:5341")
                // .addEnricherThingee(any data you want)
                .Enrich.WithProperty("companyid", 123)  // send api key here to auth? https?
                .CreateLogger();

            Log.Logger = log;


            while (true)
            {

                var x = new test();

                var dlog = log.ForContext<Form1>();

                dlog.Warning("Disk quota {Quota} exceeded by user {User}", quota, userr);

                dlog.Information("Starting up on {machine} with {WorkingSet} bytes allocated", 
                    Environment.MachineName,
                    Environment.WorkingSet);

                Log.Information("Hello, {Name}!", Environment.UserName);

                dlog.Fatal("System goes boom! {stack}, {thread}", Environment.StackTrace, Environment.CurrentManagedThreadId);
                
                
                try
                {

                    throw new SmtpException("No SMTP server found");
                }
                catch (Exception ex)
                {
                    
                    //dlog.Fatal("Exception was {exception}", ex);
                    dlog.Error(ex, "caught an exception {Exception}");
                }
                
                
                
                
                #region metrics, experimental
                
                var queue = new Queue<int>();
                var gauge = dlog.GaugeOperation("firearms on transaction", "firearm(s)", () => queue.Count());

                gauge.Write();

                for (int z = 0; z < 3; z++)
                {
                    queue.Enqueue(20);
                    gauge.Write();
                }

                queue.Dequeue();

                gauge.Write();
                
                #endregion


                // timer
                using (dlog.BeginTimedOperation("This should execute within 1 second.", null, LogEventLevel.Information, TimeSpan.FromSeconds(1)))
                {
                    Thread.Sleep(1250);
                }

            }
        }
    }
}
