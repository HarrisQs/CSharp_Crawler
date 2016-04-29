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
        private static void SaveFile(String filaName, String Data)
        {
            using (StreamWriter outputFile = new StreamWriter(filaName))
            {
                outputFile.WriteLine(Data);
            }
        }
    }
}
