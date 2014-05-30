using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMD_Core_v2.Models
{
    internal class LoginResponse
    {
        public string Token { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }

        public static LoginResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LoginResponse>(json);
        }
    }
}
