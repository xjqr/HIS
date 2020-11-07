using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicMODEL
{
	/// <summary>
	/// 系统用户实体模型类。
	/// </summary>
	public class User
	{
		/// <summary>
		/// 主键号。
		/// </summary>
		public int user_id { get; set; }
		/// <summary>
		/// 用户密码。
		/// </summary>
		public string user_password { get; set; }
		/// <summary>
		/// 用户权限。
		/// </summary>
		public string user_type { get; set; }
		/// <summary>
		/// 用户账号。
		/// </summary>
		public string user_account { get; set; }
		/// <summary>
		/// 用户有效性。
		/// </summary>
		public sbyte user_enable { get; set; }

	}
}
