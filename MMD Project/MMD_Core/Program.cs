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
        static void Main(string[] args)
        {
            string nick = "miki";
            string pass = "pass12#$";
            //string email = "miky_santa@yahoo.com";
            //RegisterUser(nick, pass, email);

            SignIn(nick, pass);

            Console.ReadLine();
        }

        private static void SignIn(string nick, string pass)
        {
            var user = new UserSignIn()
            {
                NICKNAME = nick,
                PASSWORD = pass
            };

            string json = JsonConvert.SerializeObject(user);

            WebRequest request = WebRequest.Create("http://cloud.mm-day.com/User/Auth");
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
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

            WebRequest request = WebRequest.Create("http://cloud.mm-day.com/User/Auth");
            request.Method = "PUT";
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
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
