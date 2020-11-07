namespace RegisteredMODEL
{
	/// <summary>
	/// 科室实体模型类。
	/// </summary>
	public class Department
	{
		/// <summary>
		/// 科室主键号。
		/// </summary>
		public int department_id { get; set; }
		/// <summary>
		/// 科室名称。
		/// </summary>
		public string department_name { get; set; }
		/// <summary>
		/// 科室路径。
		/// </summary>
		public string department_route { get; set; }
		/// <summary>
		/// 科室有效性。
		/// </summary>
		public sbyte department_enable { get; set; }

	}
}
