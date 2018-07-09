using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMonitor
{
    class Tools
    {
        public string GetDomainIp(string DomainName) {
            return "";
        }

        public void UpdateLatency(List<UrlInfo> UrlList)
        {
            return;
        }

        public static List<string> ReadFileByLine(string FilePath)
        {
            List<string> temp = new List<string>();
            StreamReader sr = new StreamReader(FilePath, Encoding.Default);
            int LineCount = 0;

            while (!sr.EndOfStream)
            {
                string tmpStrLint = sr.ReadLine();
                if (tmpStrLint == "")
                {
                    //MessageBox.Show("检测到空行。已忽略");
                    LineCount++;

                    continue;
                }
                try
                {
                    Uri URI = new Uri(tmpStrLint);
                }
                catch (Exception e2)
                {
                    if (e2.Message.Contains("为空"))
                    {
                        //tempTarget.Remove(Target);
                        LineCount++;

                        continue;
                    }
                    LineCount++;
                    try
                    {
                        Uri URI = new Uri("http://" + tmpStrLint);
                        tmpStrLint = "http://" + tmpStrLint;
                        LineCount++;
                        temp.Add(tmpStrLint);
                        continue;
                    }
                    catch (Exception e23)
                    {
                        // MessageBox.Show("文件中的URL不合法。该数据位于第" + LineCount + "行附近：" + tmpStrLint + "\r\n已忽略该url。");
                        continue;
                    }


                }

                temp.Add(tmpStrLint);
                LineCount++;
            }

            sr.Close();
            sr.Dispose();
            return temp;
        }


    }
}
