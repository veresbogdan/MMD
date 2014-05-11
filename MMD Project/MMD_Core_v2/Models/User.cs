using System;

namespace MMD_Core_v2
{
	public class User
	{
		public String Nickname { get; set;}

		public String Password { get; set;}

		public String Email { get; set;}

        public String Token { get; set;}

		public User ()
		{
		}

		override public String ToString()
		{	
			return Nickname + " " + Email;
		}
	}
}

