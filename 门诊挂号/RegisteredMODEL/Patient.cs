using System;

namespace RegisteredMODEL
{
	/// <summary>
	/// 病人实体模型类。
	/// </summary>
	public class Patient
	{
		/// <summary>
		/// 病人姓名。
		/// </summary>
		public string patient_name { get; set; }
		/// <summary>
		/// 病人性别。
		/// </summary>
		public string patient_sex { get; set; }
		/// <summary>
		/// 病人出生日期。
		/// </summary>
		public DateTime patient_borndate { get; set; }
		/// <summary>
		/// 病人余额。
		/// </summary>
		public float patient_money { get; set; }
		/// <summary>
		/// 病人身份证号。
		/// </summary>
		public string patient_icd { get; set; }
		/// <summary>
		/// 病人住址。
		/// </summary>
		public string patient_address { get; set; }
		/// <summary>
		/// 病人主键号。
		/// </summary>
		public int patient_id { get; set; }
		/// <summary>
		/// 病人有效性。
		/// </summary>
		public sbyte patient_enable { get; set; }
		/// <summary>
		/// 病人手机号。
		/// </summary>
		public string patient_phone { get; set; }
		/// <summary>
		/// 病人过敏病史。
		/// </summary>
		public string patient_gms { get; set; }


	}
}
