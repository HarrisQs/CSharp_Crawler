﻿//2016/04/29
//Programmer：張弘瑜
//每五分鐘從網頁上抓取資料
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebCatchData
{
    class Program
    {
        private static System.Timers.Timer _Timer = new System.Timers.Timer(300000);//五分鐘
        [DllImport("User32.dll", EntryPoint = "FindWindow")]//FindWindow
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]   //ShowWindow
        private static extern bool ShowWindow(IntPtr hWnd, int type);
 
        static void Main()
        {
            Console.Title = "CatchCo2";
            IntPtr ParenthWnd = new IntPtr(0);
            ParenthWnd = FindWindow(null, "CatchCo2");
            ShowWindow(ParenthWnd, 0);//隱藏本dos窗體, 0: 後台執行；1:正常啟動；2:最小化到開始列；3:最大化

            Console.WriteLine("Press the Enter key to exit the program at any time... ");
            Console.WriteLine("\nThe Catch event was raised at {0}", DateTime.Now.ToString("yyyy/MM/dd tt hh:mm:ss"));     
            CreateWebRequest();
            _Timer.Elapsed += OnTimedEvent;
            _Timer.AutoReset = true;//repeated events
            _Timer.Enabled = true; // Start the timer
            Console.ReadLine();
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("\nThe Catch event was raised at {0}", e.SignalTime);
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

                    Console.WriteLine(catchGoal[i] + " : " + json[catchGoal[i]].ToString());
                    SaveData(catchGoal[i]+".txt", json[catchGoal[i]].ToString());
                }
            }
            catch (Exception e)//如果沒連上網頁的話
            {
                Console.WriteLine("Exception thrown.\nThe Original Message is: " + e.Message);
            }
        }
        private static void SaveData(String filaName, String Data)//把資料存成檔案
        {
            using (StreamWriter outputFile = new StreamWriter(filaName))
            {
                outputFile.WriteLine(Data);
            }
        }
    }
}
