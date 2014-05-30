using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMD_Core_v2.Models
{
    public class CategoriesResponse
    {
        public IList<Category> Categories { get; set; }

        public static CategoriesResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CategoriesResponse>(json);
        }
    }

    [DebuggerDisplay("Id={Id}, Name={Name}")]
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ImagesResponse
    {
        public IList<ImageData> Images{ get; set; }

        public static ImagesResponse FromJson(string json)
        {
            json = "{\"Images\":" + json + "}";
            return JsonConvert.DeserializeObject<ImagesResponse>(json);
        }
    }

    [DebuggerDisplay("Image={Image}")]
    public class ImageData
    {
        public string Image { get; set; }
    }
}
