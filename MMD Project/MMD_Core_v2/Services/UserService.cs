using System;

namespace MMD_Core_v2
{
	public class UserService
	{
		public UserService ()
		{
		}

		public User loginUser(String username, String password) 
		{
			User user = new User ();

			user.name = username;
			user.password = password;

			return user;
		}
	}
}

