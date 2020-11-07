using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManageMODEL
{
	public class User
	{

		public int user_id { get; set; }

		public string user_password { get; set; }
		public string user_type { get; set; }

		public string user_account { get; set; }

		public sbyte user_enable { get; set; }

	}
}
