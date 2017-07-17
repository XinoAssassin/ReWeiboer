using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi;
using System.Net;
using System.IO;

namespace ReWeiboer
{
    public class Func
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
                sr.Close();
                response.Close();
            }
            return jsonText;
        }
        
        public static void PublishTweet()
        {
            foreach (string id in ReadConfig.Usersid)
            {
                JArray weibo = JArray.Parse(ReadUserTimeLine(id));
                string weiboText = weibo[0]["text"].ToString();
                string weiboUser = weibo[0]["user"]["name"].ToString();
                string tweet = '@' + weiboUser + ':' + weiboText;
                if (weibo[0]["retweeted_status"] == null)
                {
                    if (!(weibo[0]["original_pic"] == null))
                    {
                        GetImage(weibo[0]["original_pic"].ToString());
                        byte[] buff = File.ReadAllBytes(Environment.CurrentDirectory + "\\tmp12313123");
                        var media = Upload.UploadImage(buff);
                        Tweet.PublishTweet(tweet, new Tweetinvi.Parameters.PublishTweetOptionalParameters { Medias = new List<Tweetinvi.Models.IMedia> { media } });
                    }
                    else
                    {
                        Tweet.PublishTweet(tweet);
                    }
                }
                else
                {
                    break;
                }

                
                Console.WriteLine(tweet);
            }
        }

        public static void GetImage(string url)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)hwr.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            FileStream fs = new FileStream(Environment.CurrentDirectory + "\\tmp12313123", FileMode.OpenOrCreate, FileAccess.Write);
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
