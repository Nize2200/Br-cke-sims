//  This file contains the code for your service.
//  Add your service logic here.

// Test for Windows Service app
// 2024-10-07

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO; //StreamWriter Class
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService1
{
    // inherited ServiceBase class
    public partial class Service1 : ServiceBase
    {    
        private Timer timer;
        // where to save the text file
        private const string folderPath = "C:\\temp\\scan";
        private const string filePath = folderPath + "service.txt";

        StreamWriter sw;
    
        public Service1()
        {
            InitializeComponent();
        }

        // runs when the service starts
        // You can initialize timers or tasks here.
        protected override void OnStart(string[] args)
        {
            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            writeLog("Service OnStart!");

            timer = new Timer();
            timer.Interval = 1000; // execute every 1 second
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        // This is where you can define the periodic task
        // (e.g., writing to a log file, running background jobs).
        private void Timer_Elapsed(object source, ElapsedEventArgs e)
        {
            sw = new StreamWriter(filePath, true);
            sw.WriteLine(DateTime.Now.ToString());
            sw.Close();

        }

        //  you clean up resources when the service stops
        protected override void OnStop()
            {
            // Clean up resources when the service stops
            writeLog("Service Onstop!");
        }

        private void writeLog(string str)
        {
            sw = new StreamWriter(filePath, true);
            sw.WriteLine(str);
            sw.Close();
        }
    }
}
