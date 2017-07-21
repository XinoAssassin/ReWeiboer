using System;
using Tweetinvi;
using System.IO;
using System.Runtime.InteropServices;

namespace ReWeiboer
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static void Main(string[] args)
        {
            Console.Title = "ReWeiboer";
            ReadConfig.Readconfig();
            if (ReadConfig.IsConsoleHide == true)
            {
                IntPtr intptr = FindWindow("ConsoleWindowClass", "ReWeiboer");
                if (intptr != IntPtr.Zero)
                {
                    ShowWindow(intptr, 0);
                }

            }

            Auth.SetUserCredentials(ReadConfig.ConsumerKey, ReadConfig.ConsumerSecret, ReadConfig.AccessToken, ReadConfig.AccessTokenSecret);
            if (!Directory.Exists(Environment.CurrentDirectory + "\\temp"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\temp");
            }
            System.Timers.Timer timer = new System.Timers.Timer(ReadConfig.TimerInterval)
            {
                Enabled = true,
                AutoReset = true
            };
            
            timer.Elapsed += new System.Timers.ElapsedEventHandler(RunPublish);
            timer.Start();
            Func.ReWeibo();
            void RunPublish(object source, System.Timers.ElapsedEventArgs e)
            {
                Func.ReWeibo();
            }
            
            Console.ReadKey();

        }

    }
}