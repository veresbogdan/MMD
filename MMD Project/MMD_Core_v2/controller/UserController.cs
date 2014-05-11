using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMD_Core_v2.controller
{
    class UserController : BaseController
    {
        private const String loginUrl = "http://cloud.mm-day.com/User/Auth";

        internal String loginUser(String username, String password)
        {
            var content = new Dictionary<string, string>();
            content.Add("NICKNAME", username);
            content.Add("PASSWORD", password);

            return sendHttpPost(loginUrl, JsonConvert.SerializeObject(content));
        }
    }
}
