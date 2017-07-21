using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;


namespace ReWeiboer
{
    class Func
    {
        public static string ReadUserTimeLine(string id)
        {
            string url = "http://api.t.sina.com.cn/statuses/user_timeline/+" + id + ".json?source=2849184197&count=1";
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)hwr.GetResponse();
            string jsonText = "";

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                jsonText = sr.ReadToEnd();
            }
            return jsonText;
        }

        public static string ReadLocalJson(string id)
        {
            string weiboid = "";
            string jsonLocal = "";
            if (File.Exists(Environment.CurrentDirectory + "\\temp\\" + id + "_local.json"))
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\temp\\" + id + "_local.json"))
                {
                    jsonLocal = sr.ReadToEnd();
                }
                JArray weibo = JArray.Parse(jsonLocal);
                weiboid = weibo[0]["id"].ToString();
            }
            else
            {
                File.WriteAllText(Environment.CurrentDirectory + "\\temp\\" + id + "_local.json", ReadUserTimeLine(id));
                weiboid = "";
            }

            return weiboid;

        }

        public static bool IsNewWeibo(string id)
        {
            JArray weibo = JArray.Parse(ReadUserTimeLine(id));
            string weiboid = weibo[0]["id"].ToString();
            if (weiboid == ReadLocalJson(id))
            {
                return false;
            }
            else
            {
                File.WriteAllText(Environment.CurrentDirectory + "\\temp\\" + id + "_local.json", ReadUserTimeLine(id));
                return true;
            }
        }

        public static void ReWeibo()
        {
            foreach (string id in ReadConfig.Usersid)
            {
                if (IsNewWeibo(id))
                {
                    JArray weibo = JArray.Parse(ReadUserTimeLine(id));
                    string weiboText = weibo[0]["text"].ToString();
                    string weiboUser = weibo[0]["user"]["name"].ToString();
                    string weiboUserLink = weibo[0]["user"]["id"].ToString();
                    string tweet = '@' + weiboUser + '：' + weiboText + '\n' + "From: http://weibo.com/" + weiboUserLink;
                    if (weibo[0]["retweeted_status"] == null)
                    {
                        if (!(weibo[0]["original_pic"] == null))
                        {
                            GetImage(weibo[0]["original_pic"].ToString());
                            byte[] buff = File.ReadAllBytes(Environment.CurrentDirectory + "\\temp\\tmp.pic");
                            var media = Upload.UploadImage(buff);
                            Tweet.PublishTweet(tweet, new Tweetinvi.Parameters.PublishTweetOptionalParameters { Medias = new List<Tweetinvi.Models.IMedia> { media } });
                            Console.WriteLine(tweet);
                        }
                        else
                        {
                            Tweet.PublishTweet(tweet);
                            Console.WriteLine(tweet);

                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    GC.Collect();
                }

                System.Threading.Thread.Sleep(2000);
                
            }
        }
        public static void GetImage(string url)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)hwr.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            FileStream fs = new FileStream(Environment.CurrentDirectory + "\\temp\\tmp.pic", FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[response.ContentLength];
            int i = 0;
            while ((i = stream.Read(buff, 0, buff.Length)) > 0)
            {
                fs.Write(buff, 0, i);
            }
            fs.Close();
            stream.Close();
            fs.Dispose();
            stream.Dispose();

        }
    }
}
