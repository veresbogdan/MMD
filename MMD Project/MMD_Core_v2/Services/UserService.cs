using MMD_Core_v2.controller;
using Newtonsoft.Json;
using System;

namespace MMD_Core_v2
{
	public class UserService
	{
        private UserController userController = new UserController();

		public UserService ()
		{
		}

		public User loginUser(String username, String password) 
		{
            String responseUser = userController.loginUser(username, password);

            if (responseUser != null)
            {
                return JsonConvert.DeserializeObject<User>(responseUser);
            }

			return null;
		}
	}
}

