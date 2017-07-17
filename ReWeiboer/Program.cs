using System;
using Tweetinvi;
using System.IO;

namespace ReWeiboer
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadConfig.Readconfig();
            Auth.SetUserCredentials(ReadConfig.ConsumerKey, ReadConfig.ConsumerSecret, ReadConfig.AccessToken, ReadConfig.AccessTokenSecret);
            if (!Directory.Exists(Environment.CurrentDirectory + "\\temp"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\temp");
            }
            /*System.Timers.Timer timer = new System.Timers.Timer(ReadConfig.TimerInterval)
            {
                Enabled = true,
                AutoReset = true
            };
            timer.Elapsed += new System.Timers.ElapsedEventHandler(RunPublish);
            void RunPublish(object source, System.Timers.ElapsedEventArgs e)
            {
                Func.ReWeibo();
            }*/
            Func.ReWeibo();
            Console.ReadKey();

        }

    }
}