using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MMD_Core_v2.controller
{
    class BaseController
    {
        internal String sendHttpPost(String url, String content)
        {
            var client = new HttpClient();
      
            var stringContent = new StringContent(content);

            var response = client.PostAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

            return null;
        }
    }
}
