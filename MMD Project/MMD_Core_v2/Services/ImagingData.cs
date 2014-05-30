using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using MMD_Core_v2.Models;

namespace MMD_Core_v2.Services
{
    public class ImagingData
    {
        private string m_token;
        private IList<Category> m_categories;
        private Dictionary<string, IList<ImageData>> m_images;

        public IList<Category> Categories
        {
            get { return m_categories; }
        }

        public Dictionary<string, IList<ImageData>> ImagesList
        {
            get { return m_images; }
        }

        public ImagingData()
        {
            //todo : this should be cleaned up here
            m_token = String.Empty;
            m_categories = null;
            m_images = new Dictionary<string, IList<ImageData>>();
        }

        public void Init()
        {
            //LoginAsync().Wait();
            Login();
            m_categories = GetCategories();
            foreach(var c in m_categories)
            {
                m_images.Add(c.Id, GetImages(c.Id));
            }
        }

        private async Task LoginAsync()
        {
            //TODO : delete this method
            Uri authLink = new Uri("http://cloud.mm-day.com/User/Auth");
            HttpClientHandler handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            var content = new StringContent("{\"NICKNAME\":\"MMD\",\"PASSWORD\":\"MMD\"}");

            //async
            HttpResponseMessage respMsg = await client.PostAsync(authLink, content);
            m_token = await respMsg.Content.ReadAsStringAsync();
            //m_token = GetLoginToken(resp.Result);
        }

        private void Login()
        {
            //TODO : delete this method
            Uri authLink = new Uri("http://cloud.mm-day.com/User/Auth");
            HttpClientHandler handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            var content = new StringContent("{\"NICKNAME\":\"MMD\",\"PASSWORD\":\"MMD\"}");
            
            Task<HttpResponseMessage> respMsg = client.PostAsync(authLink, content);
            respMsg.Wait();

            Task<string> resp = respMsg.Result.Content.ReadAsStringAsync();
            resp.Wait();
            m_token = GetLoginToken(resp.Result);
        }

        private string GetLoginToken(string json)
        {
            string token = null;

            var resp = LoginResponse.FromJson(json);
            token = resp.Token;

            return token;
        }
        
        public IList<Category> GetCategories()
        {
            Uri authLink = new Uri("http://cloud.mm-day.com/Fun/Imagini/Categories");
            HttpClientHandler handler = new HttpClientHandler();
            var client = new HttpClient(handler);

            Task<string> resp = client.GetStringAsync(authLink);
            resp.Wait();

            CategoriesResponse categs = CategoriesResponse.FromJson(resp.Result);
            return categs.Categories;
        }

        public IList<ImageData> GetImages(string categId)
        {
            Uri authLink = new Uri("http://cloud.mm-day.com/Fun/Imagini");
            HttpClientHandler handler = new HttpClientHandler();
            var client = new HttpClient(handler);

            var content = new StringContent(String.Format("{{ \"CATEGORY\":\"{0}\"}}", categId));
            Task<HttpResponseMessage> resp = client.PostAsync(authLink, content);
            resp.Wait();

            Task<string> respContent = resp.Result.Content.ReadAsStringAsync();
            respContent.Wait();

            ImagesResponse images = ImagesResponse.FromJson(respContent.Result);
            return images.Images;
        }
    }
}
