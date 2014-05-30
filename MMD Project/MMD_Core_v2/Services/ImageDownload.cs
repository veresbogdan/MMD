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
    public class ImageDownload
    {
        public async static Task<byte[]> Get(string imageId)
        {
            //http://cloud.mm-day.com/Fun/Imagini/:id/:size
            string path = String.Format("http://cloud.mm-day.com/Fun/Imagini/{0}/full", imageId);
            Uri link = new Uri(path);

            HttpClient client = new HttpClient();
            byte[] imageData = null;
            await client.GetAsync(path).ContinueWith(
                async task =>
                {
                    var resp = task.Result;
                    imageData = await resp.Content.ReadAsByteArrayAsync();
                });

            return imageData;
        }
    }
}
