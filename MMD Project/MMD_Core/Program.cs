using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMD_Core
{
    class Program
    {
        private static string KServerAddress = "http://cloud.mm-day.com";

        static void Main(string[] args)
        {
            string nick = "miki";
            string pass = "pass12#$";
            //string email = "miky_santa@yahoo.com";
            //RegisterUser(nick, pass, email);

            var images = GetImageList();

            //SignIn(nick, pass);

            Console.ReadLine();
        }

        public enum ERequestType
        {
            POST,
            PUT,
            GET,
            DELETE
        }

        private static void RegisterUser(string nickname, string pass, string email)
        {
            var user = new UserRegistration()
            {
                NICKNAME = nickname,
                PASSWORD = pass,
                EMAIL = email
            };

            string json = JsonConvert.SerializeObject(user);

            Stream dataStream;
            SendRequest(ERequestType.PUT, json, KServerAddress + "/User/Auth", out dataStream);

            Debug_PrintStream(dataStream);
            dataStream.Close();
        }

        private static void SignIn(string nick, string pass)
        {
            var user = new UserSignIn()
            {
                NICKNAME = nick,
                PASSWORD = pass
            };

            string json = JsonConvert.SerializeObject(user);

            Stream dataStream;
            SendRequest(ERequestType.POST, json, KServerAddress + "/User/Auth", out dataStream);
            Debug_PrintStream(dataStream);

            dataStream.Close();
        }

        private static IList<string> GetImageList()
        {
            IList<string> images = new List<string>();

            WebRequest request = WebRequest.Create("http://cloud.mm-day.com/Fun/Imagini");
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes("");
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return images;
        }

        private static void SendRequest(ERequestType eRequestType, string json, string addr, out Stream respStream)
        {
            WebRequest request = WebRequest.Create(addr);
            request.Method = eRequestType.ToString();//TODO : fix this shit. This is not safe
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            respStream = response.GetResponseStream();

            //cleanup the streams
            response.Close();
        }

        private static void Debug_PrintStream(Stream dataStream)
        {
            using (StreamReader reader = new StreamReader(dataStream))
            {
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
            }
        }
    }

    class UserRegistration
    {
        //the properties are uppercased because 
        //this is the format asked by the server and we
        //want to use the automatic json serializer.
        public string NICKNAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        //public string LOCATION { get; set; }
    }

    class UserSignIn
    {
        //the properties are uppercased because 
        //this is the format asked by the server and we
        //want to use the automatic json serializer.
        public string NICKNAME { get; set; }
        public string PASSWORD { get; set; }
    }
}
