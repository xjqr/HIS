using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargesMODEL
{
	/// <summary>
	/// 收费员实体模型类。
	/// </summary>
	public class Chargedpersonnel
	{
		/// <summary>
		/// 收费员主键号。
		/// </summary>
		public int chargedpersonnel_id { get; set; }
		/// <summary>
		/// 收费员姓名。
		/// </summary>
		public string chargedpersonnel_name { get; set; }
		/// <summary>
		/// 收费员有效性。
		/// </summary>
		public sbyte chargedpersonnel_enable { get; set; }
		/// <summary>
		/// 收费员账号对应的主键号。
		/// </summary>
		public int chargedpersonnel_userid { get; set; }
	}
}
