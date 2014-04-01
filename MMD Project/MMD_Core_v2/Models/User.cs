using System;

namespace MMD_Core_v2
{
	public class User
	{
		public String name { get; set;}

		public String password { get; set;}

		public String email { get; set;}

		public User ()
		{
		}

		override public String ToString()
		{	
			return name + " " + password + " " + email;
		}
	}
}

