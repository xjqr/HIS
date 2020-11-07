using System;

namespace RegisteredMODEL
{
	/// <summary>
	/// 挂号记录实体模型类。
	/// </summary>
	public class Registrationrecord
	{
		/// <summary>
		/// 挂号记录主键号。
		/// </summary>
		public int registrationrecord_id { get; set; }
		/// <summary>
		/// 挂号时间。
		/// </summary>
		public DateTime registrationrecord_time { get; set; }
		/// <summary>
		/// 挂号病人主键号。
		/// </summary>
		public int registrationrecord_patientid { get; set; }
		/// <summary>
		/// 挂号员主键号。
		/// </summary>
		public int registrationrecord_registrarsid { get; set; }
		/// <summary>
		/// 挂号科室主键号。
		/// </summary>
		public int registrationrecord_departmentid { get; set; }
		/// <summary>
		/// 挂号医生主键号。
		/// </summary>
		public int registrationrecord_doctorid { get; set; }
		/// <summary>
		/// 排队号。
		/// </summary>
		public int registrationrecord_num { get; set; }
		/// <summary>
		/// 挂号记录有效性。
		/// </summary>
		public sbyte registrationrecord_enable { get; set; }
		/// <summary>
		/// 挂号金额。
		/// </summary>
		public float registrationrecord_money { get; set; }
		/// <summary>
		/// 挂号星期。
		/// </summary>
		public string registrationrecord_workday { get; set; }
	}
}
