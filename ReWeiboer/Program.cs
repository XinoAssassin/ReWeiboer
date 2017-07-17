using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using System.IO;
using System.Net;

namespace ReWeiboer
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadConfig.Readconfig();
            Auth.SetUserCredentials(ReadConfig.ConsumerKey, ReadConfig.ConsumerSecret, ReadConfig.AccessToken, ReadConfig.AccessTokenSecret);
            /*System.Timers.Timer timer = new System.Timers.Timer(ReadConfig.TimerInterval)
            {
                Enabled = true,
                AutoReset = true
            };
            timer.Elapsed += new System.Timers.ElapsedEventHandler(RunPublish);
            void RunPublish(object source, System.Timers.ElapsedEventArgs e)
            {
                Func.PublishTweet();
            }
            */

            Func.PublishTweet();
            Console.ReadKey();
        }

    }
}