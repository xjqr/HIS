using System;

namespace RegisteredMODEL
{
	/// <summary>
	/// 挂号数据视图模型类。
	/// </summary>
	public  class Registerdatatransfer
	{
	  /// <summary>
	  /// 挂号时间。
	  /// </summary>
	  public DateTime registrationrecord_time { get; set; }
	  /// <summary>
	  /// 病人主键号。
	  /// </summary>
	  public int patient_id { get; set; }
	  /// <summary>
	  ///挂号员账号。 
	  /// </summary>
	  public string user_account{ get; set; }
	   /// <summary>
	   /// 科室名称。
	   /// </summary>
	   public string department_name { get; set; }
		/// <summary>
		/// 医生姓名。
		/// </summary>
	   public string doctor_name { get; set; }
		/// <summary>
		/// 系统剩余号数。
		/// </summary>
		public int doctorsschedule_surplusnum { get; set; }
		/// <summary>
		/// 挂号费。
		/// </summary>
		public float registrationrecord_money { get; set; }
		/// <summary>
		/// 排班时间段。
		/// </summary>
		public string doctorsschedule_worktime { get; set; }

	}
}
