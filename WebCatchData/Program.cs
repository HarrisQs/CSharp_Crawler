//2016/04/29
//Programmer：張弘瑜
//從網頁抓取上面的資料
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebCatchData
{
    class Program
    {
        static void Main()
        {
            CreateWebRequest();
        }
        private static void CreateWebRequest()
        {
            try
            {
                //建立一個網頁連線並取得上面的資料
                WebRequest request = WebRequest.Create("http://140.138.6.120/sampling.php");
                WebResponse response = request.GetResponse();
                Console.WriteLine("WebSite Response Status : " + ((HttpWebResponse)response).StatusDescription);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseFromServer = reader.ReadToEnd();
                string jsonString = responseFromServer;
                JObject json = JObject.Parse(jsonString);
                String [] catchGoal = { "Co2", "Temperature", "Humidity" };
                for (int i = 0; i < 3; i++) {

                    SaveFile(catchGoal[i]+".txt", json[catchGoal[i]].ToString());
                }
            }
            catch (WebException e)//如果沒連上網頁的話
            {
                Console.WriteLine("Exception thrown.\nThe Original Message is: " + e.Message);
            }
        }
        private static void SaveFile(String filaName, String Data)//把資料存成檔案
        {
            using (StreamWriter outputFile = new StreamWriter(filaName))
            {
                outputFile.WriteLine(Data);
            }
        }
    }
}
