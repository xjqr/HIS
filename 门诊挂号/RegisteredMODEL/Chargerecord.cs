using System;

namespace RegisteredMODEL
{
	/// <summary>
	/// 收费记录视图模型类。
	/// </summary>
	public class Chargerecord
	{
		/// <summary>
		/// 收费记录主键号。
		/// </summary>
		public int chargerecord_id { get; set; }
		/// <summary>
		/// 收费员主键号。
		/// </summary>
		public int chargerecord_chargedpersonnelid { get; set; }
		/// <summary>
		/// 收费时间。
		/// </summary>
		public DateTime chargerecord_time { get; set; }
		/// <summary>
		/// 收费病人主键号。
		/// </summary>
		public int chargerecord_patientid { get; set; }
		/// <summary>
		/// 收费金额。
		/// </summary>
		public float chargerecord_money { get; set; }
		/// <summary>
		/// 挂号员主键号。
		/// </summary>
		public int chargerecord_registrarsid { get; set; }
		/// <summary>
		/// 收费记录状态。
		/// </summary>
		public sbyte chargerecord_enable { get; set; }
	}
}
