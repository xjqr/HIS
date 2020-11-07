namespace RegisteredMODEL
{
	/// <summary>
	/// 医生排班实体模型类。
	/// </summary>
	public class DoctorsSchedule
	{
		/// <summary>
		/// 医生的排班星期。
		/// </summary>
		public string doctorsSchedule_weekday { get; set; }
		/// <summary>
		/// 医生的排班时段。
		/// </summary>
		public string doctorsSchedule_worktime { get; set; }
		/// <summary>
		/// 医生的主键号。
		/// </summary>
		public int doctorsSchedule_doctorid { get; set; }
		/// <summary>
		/// 该排班的系统总号数。
		/// </summary>
		public int doctorsSchedule_num { get; set; }
		/// <summary>
		/// 该排班的可加号数。
		/// </summary>
		public int doctorsSchedule_addnum { get; set; }
		/// <summary>
		/// 该排班的总剩余号数。
		/// </summary>
		public int doctorsSchedule_surplusnum { get; set; }
		/// <summary>
		/// 该排班的预约网站总号数。
		/// </summary>
		public int doctorsschedule_numweb { get; set; }
		/// <summary>
		/// 该排班的主键号。
		/// </summary>
		public int doctorsschedule_id { get; set; }
		/// <summary>
		/// 该排班的网站已挂号数。
		/// </summary>
		public int doctorsschedule_surplusnumweb { get; set; }

	}
}
