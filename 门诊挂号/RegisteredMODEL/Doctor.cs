namespace RegisteredMODEL
{
	/// <summary>
	/// 医生实体模型类。
	/// </summary>
	public	class Doctor
	{
		/// <summary>
		/// 医生主键号。
		/// </summary>
		public int doctor_id { get; set; }
		/// <summary>
		/// 医生姓名。
		/// </summary>
		public string doctor_name { get; set; }
		/// <summary>
		/// 医生科室主键号。
		/// </summary>
		public int doctor_departmentid { get; set; }
		/// <summary>
		/// 医生号别。
		/// </summary>
		public string doctor_level{ get; set; }
		/// <summary>
		/// 医生性别。
		/// </summary>
		public string doctor_sex { get; set; }
		/// <summary>
		/// 医生有效性。
		/// </summary>
		public sbyte doctor_enable { get; set; }
		/// <summary>
		/// 医生账号对应的主键号。
		/// </summary>
		public int doctor_userid { get; set; }
		/// <summary>
		/// 医生的类型。
		/// </summary>
		public string doctor_type { get; set; }
		/// <summary>
		/// 医生的简介。
		/// </summary>
		public string doctor_info { get; set; }
	}
}
