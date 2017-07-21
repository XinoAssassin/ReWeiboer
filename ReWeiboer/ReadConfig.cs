using System;
using System.Configuration;

namespace ReWeiboer
{
    public static class ReadConfig
    {
        public static int TimerInterval;
        public static string[] Usersid;
        public static string ConsumerKey;
        public static string ConsumerSecret;
        public static string AccessToken;
        public static string AccessTokenSecret;
        public static bool IsConsoleHide;

        public static void Readconfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;

            string text = ConfigurationManager.AppSettings["usersID"];
            Usersid = text.Split(',');

            string num = ConfigurationManager.AppSettings["timerInterval"];
            TimerInterval = Convert.ToInt32(num);

            ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            AccessToken = ConfigurationManager.AppSettings["AccessToken"];
            AccessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];

            string isConsoleHide = ConfigurationManager.AppSettings["HideConsole"];
            if (isConsoleHide.ToLower() == "true")
            {
                IsConsoleHide = true;
            }
            else
            {
                IsConsoleHide = false;
            }


        }

    }
}
