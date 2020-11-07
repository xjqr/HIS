using System;
using System.Text;
using IDAL;
using DAL;
using RegisteredMODEL;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RegisteredBLL
{
	public class Bll
	{
		private string[] Texts;
		/// <summary>
		/// 验证管理员身份。
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public bool Check(User user)
		{
			user.user_password = Md5Hash(user.user_password);
			Idal idal = new Dal();
			if (idal.SelectModel<User>($" user_account='Partadmin' and user_password='{user.user_password}' and user_type='系统管理员'").user_account == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		/// <summary>
		/// 根据时间段查询挂号记录信息。
		/// </summary>
		/// <param name="startime">起始时间</param>
		/// <param name="endtime">终止时间</param>
		/// <returns></returns>
		public DataTable SelectByDatetime(DateTime startime,DateTime endtime)
		{
			string alwaystring = "registrars_name as 挂号员,registrationrecord_num as 排队号,registrationrecord_time as 挂号时间,registrationrecord_workday as 时间段,registrationrecord_money as 挂号收费,patient_name as 病人,department_name as 科室,doctor_name as 医生,patient_id as 诊疗卡号";
			Idal idal = new Dal();
			return idal.GetDataTable($@"select {alwaystring} from his.v_registrationrecord where registrationrecord_time between '{startime:yyyy-MM-dd HH:mm:ss}' and '{endtime:yyyy-MM-dd HH:mm:ss}'");
		}
		/// <summary>
		/// 根据条件筛选挂号记录信息。
		/// </summary>
		/// <param name="condictions"></param>
		/// <returns></returns>
		public DataTable SelectIntegrated(params string[]condictions)
		{
			
			string[] fileds = new string[] {"department_name","doctor_name","patient_id"};
			string alwaystring = "registrars_name as 挂号员,registrationrecord_num as 排队号,registrationrecord_time as 挂号时间,registrationrecord_workday as 时间段,registrationrecord_money as 挂号收费,patient_name as 病人,department_name as 科室,doctor_name as 医生,patient_id as 诊疗卡号";
			string querystring = "";
			for (int i=0;i<condictions.Length;i++)
			{

				if (condictions[i] != "") {
					querystring += $@"{fileds[i]}='{condictions[i]}' and ";
	;
				}
			}
			querystring = querystring.Substring(0, querystring.Length - 5);

			Idal idal = new Dal();
			return idal.GetDataTable($@"select {alwaystring} from his.v_registrationrecord where {querystring}");
		}
		/// <summary>
		/// 获取医生挂号排班情况。
		/// </summary>
		/// <param name="time">日期时间</param>
		/// <param name="worktime">时间段</param>
		/// <returns></returns>
		public DataTable GetRegistrationStatus(DateTime time,string worktime)
		{
			string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六","星期日" };
			int m = time.Month;
			if (m == 1) m = 13;
			if (m== 2) m = 14;
			int week = (time.Day+ 2 * m + 3 * (m + 1) / 5 + time.Year + time.Year / 4 - time.Year/ 100 + time.Year / 400) % 7 + 1;
			Idal idal = new Dal();
			return idal.GetDataTable($@"SELECT * FROM his.indexviewmodel WHERE doctorsschedule_worktime='{worktime}' and doctorsschedule_weekday='{ Day[week]}';");
			
		}
		/// <summary>
		/// 根据病人诊疗卡号查询病人挂号记录。
		/// </summary>
		/// <param name="patientid">诊疗卡号</param>
		/// <returns></returns>
		public DataTable GetRegistrationrecords(string patientid)
		{
			Idal idal = new Dal();
			return idal.GetDataTable($@"select * from v_registrationrecord where registrationrecord_patientid='{patientid}'");
		}
		/// <summary>
		/// 根据条件从数据库中获取一个病人模型。
		/// </summary>
		/// <param name="queryconditional"></param>
		/// <returns></returns>
		public Patient GetPatient(string queryconditional)
		{
			try
			{
            Idal idal = new Dal();
			return idal.SelectModel<Patient>(queryconditional);
			}
			catch { throw; }
			
		}
		/// <summary>
		/// 执行挂号操作,返回排队号。
		/// </summary>
		/// <param name="registerdatatransfer"></param>
		/// <returns></returns>
		public int Register(Registerdatatransfer registerdatatransfer)
		{
			string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
			int m = registerdatatransfer.registrationrecord_time.Month;
			if (m == 1) m = 13;
			if (m == 2) m = 14;
			int week = (registerdatatransfer.registrationrecord_time.Day + 2 * m + 3 * (m + 1) / 5 + registerdatatransfer.registrationrecord_time.Year + registerdatatransfer.registrationrecord_time.Year / 4 - registerdatatransfer.registrationrecord_time.Year / 100 + registerdatatransfer.registrationrecord_time.Year / 400) % 7 + 1;
			string weekday = Day[week];
			Idal idal = new Dal();
			DoctorsSchedule doctorsSchedule = idal.SelectModel<DoctorsSchedule>($@"doctorsschedule_worktime='{registerdatatransfer.doctorsschedule_worktime}' and doctorsschedule_weekday='{weekday}' and doctorsschedule_doctorid='{idal.GetDataTable($@"select `doctor_id` from `his`.`doctor` where doctor_name = '{registerdatatransfer.doctor_name}'").Rows[0][0]}'");
			
			Registrationrecord registrationrecord = new Registrationrecord()
			{
				registrationrecord_workday = registerdatatransfer.doctorsschedule_worktime,
				registrationrecord_enable = 1,
				registrationrecord_money = registerdatatransfer.registrationrecord_money,
				registrationrecord_patientid = registerdatatransfer.patient_id,
				registrationrecord_time = registerdatatransfer.registrationrecord_time,
				registrationrecord_num=doctorsSchedule.doctorsSchedule_num+doctorsSchedule.doctorsschedule_numweb-doctorsSchedule.doctorsSchedule_surplusnum+1,
				registrationrecord_departmentid =(int)idal.GetDataTable($@"select `department_id` from `his`.`department` where department_name='{registerdatatransfer.department_name}'").Rows[0][0],
				registrationrecord_doctorid= (int)idal.GetDataTable($@"select `doctor_id` from `his`.`doctor` where doctor_name='{registerdatatransfer.doctor_name}'").Rows[0][0],
				registrationrecord_registrarsid= (int)idal.GetDataTable($@"select `registrars_id` from `his`.`v_registrars_user` where user_account='{registerdatatransfer.user_account}'").Rows[0][0]
			};
			doctorsSchedule.doctorsSchedule_surplusnum = doctorsSchedule.doctorsSchedule_surplusnum - 1;
			idal.UpdateModelToDb(doctorsSchedule);
			idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;Update  `his`.`patient` SET `patient_money`='{idal.SelectModel<Patient>($@"patient_id='{registerdatatransfer.patient_id}'").patient_money-registerdatatransfer.registrationrecord_money}' WHERE (patient_id='{registerdatatransfer.patient_id}');SET FOREIGN_KEY_CHECKS=1;");
			idal.AddModelToDb(registrationrecord);
			return registrationrecord.registrationrecord_num;
		}
		/// <summary>
		/// 删除科室。
		/// </summary>
		/// <param name="route">科室路径</param>
		/// <returns></returns>
		public bool DeleteDepartment(string route)
		{
			Idal idal = new Dal();
		   return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;delete from his.department where department_route='{route}';SET FOREIGN_KEY_CHECKS=1;");
		}
		/// <summary>
		/// 添加科室。
		/// </summary>
		/// <param name="route">科室路径</param>
		/// <returns></returns>
		public bool AddDepartment(string route)
		{
			

			Idal idal = new Dal();
			return idal.AddModelToDb
			(
				new Department()
				{
					department_enable = 1,
					department_route = route,
					department_name = route.Substring(route.LastIndexOf('/')+1)
				}
			) ;
		}
		/// <summary>
		/// 添加病人。
		/// </summary>
		/// <param name="patient"></param>
		/// <returns></returns>
		public bool Addpatient(Patient patient)
		{
			Idal idal = new Dal();
			return idal.AddModelToDb<Patient>(patient);
		}
		/// <summary>
		/// 取消挂号。
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public bool CannelRegister(DataRow row)
		{
			Idal idal = new Dal();
			float moneyp = float.Parse(row["patient_money"].ToString()) + float.Parse(row["registrationrecord_money"].ToString());
			idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.registrationrecord set registrationrecord_enable='0' where registrationrecord_id='{row["registrationrecord_id"]}';SET FOREIGN_KEY_CHECKS=1;");
			idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.patient set patient_money='{moneyp}' where patient_id='{row["patient_id"]}';SET FOREIGN_KEY_CHECKS=1;");
			//int n=int.Parse(idal.GetDataTable("select * from his.doctorsschedule where "))
			return true;
		}
		/// <summary>
		/// 根据医生姓名获取医生ID号。
		/// </summary>
		/// <param name="doctorname"></param>
		/// <returns></returns>
		public int GetdoctorID(string doctorname)
		{
			
			Idal idal = new Dal();
			return int.Parse(idal.GetDataTable($@"select doctor_id from his.doctor where doctor_name='{doctorname}'").Rows[0][0].ToString());

			
		}
		/// <summary>
		/// 修改病人信息。
		/// </summary>
		/// <param name="patient"></param>
		/// <returns></returns>
		public bool UpdatePatient(Patient patient)
		{
			Idal idal = new Dal();

			return idal.UpdateModelToDb<Patient>(patient);
		}
		/// <summary>
		/// 打印挂号单。
		/// </summary>
		/// <param name="texts">挂号单内容</param>
		public void PrintingRegisterrecord(string[]texts)
		{
			this.Texts = texts;
			PrintDocument printDocument = new PrintDocument();
			printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 200, 250);
			printDocument.DocumentName = "挂号单";

			printDocument.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);
			PrintDialog printDialog = new PrintDialog();
			printDialog.ShowDialog();

			PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
			printPreviewDialog.Document = printDocument;
			printPreviewDialog.ShowDialog();

		}
		/// <summary>
		/// 定义挂号单打印模板。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			string time = System.DateTime.Now.ToString();
			e.Graphics.DrawString("XXX医院挂号单", new Font(new FontFamily("黑体"), 10), Brushes.Black, 50, 20);
			e.Graphics.DrawString("打印时间：" + time, new Font(new FontFamily("黑体"), 4), Brushes.Black, 60, 40);
			e.Graphics.DrawString("姓名：" + Texts[0], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 80);
			e.Graphics.DrawString("诊疗卡号：" + Texts[1], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 100);
			e.Graphics.DrawString("就诊医师：" + Texts[2], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 120);
			e.Graphics.DrawString("科室：" + Texts[3], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 140);
			e.Graphics.DrawString("挂号时间：" + Texts[4], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 160);
			e.Graphics.DrawString("排队号：" + Texts[5], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 180);
			e.Graphics.DrawString("收费总额：" + Texts[6] + "元", new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 200);
		}
		/// <summary>
		/// 查询医生是否支持加号。
		/// </summary>
		/// <param name="doctorname"></param>
		/// <param name="time"></param>
		/// <param name="workday"></param>
		/// <returns></returns>
		public bool IsAddnum(string doctorname,DateTime time,string workday)
		{
			string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
			int m = time.Month;
			if (m == 1) m = 13;
			if (m == 2) m = 14;
			int week = (time.Day + 2 * m + 3 * (m + 1) / 5 + time.Year + time.Year / 4 - time.Year / 100 + time.Year / 400) % 7 + 1;
			Idal idal = new Dal();
			string doctor_id = idal.GetDataTable($@"select `doctor_id` from `his`.`doctor` where doctor_name='{doctorname}'").Rows[0][0].ToString();
			var c = idal.SelectModel<DoctorsSchedule>($@"doctorsschedule_doctorid='{doctor_id}' and doctorsSchedule_weekday='{Day[week]}' and doctorsSchedule_worktime='{workday}'");
			if (c.doctorsSchedule_addnum> 0) 
			{
				c.doctorsSchedule_surplusnum += 1;
				c.doctorsSchedule_addnum -= 1;
			   return idal.UpdateModelToDb(c);

			}
			else { return false; }
		}

		/// <summary>
		/// 32位MD5加密
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Md5Hash(string input)
		{
			MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}
		/// <summary>
		/// 添加一个医生排班记录。
		/// </summary>
		/// <param name="doctorsSchedule"></param>
		/// <returns></returns>
		public bool AddDoctorsSchedul(DoctorsSchedule doctorsSchedule)
		{
			Idal idal = new Dal();
			return idal.AddModelToDb(doctorsSchedule);
		}
		/// <summary>
		/// 获取所有医生信息和对应的排班信息。
		/// </summary>
		/// <returns></returns>
		public DataTable GetDoctorInfo()
		{
			Idal idal = new Dal();
			return idal.GetDataTable("select * from his.v_doctor_department;");
		}
		/// <summary>
		/// 添加一个医生。
		/// </summary>
		/// <param name="doctor"></param>
		/// <returns></returns>
		public bool AddDoctor(Doctor doctor)
		{
			Idal idal = new Dal();
			return idal.AddModelToDb(doctor);

		}
		/// <summary>
		/// 删除一个医生。
		/// </summary>
		/// <param name="id">医生表主键</param>
		/// <returns></returns>
		public bool DeleteDoctorInfo(int id)
		{
			Idal idal = new Dal();
			return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;delete from his.doctor where doctor_id='{id}';SET FOREIGN_KEY_CHECKS=1;");
		}
		/// <summary>
		/// 根据科室名称获取其对应的主键号。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetDepartmentId(string name)
		{
			Idal idal = new Dal();
			return idal.SelectModel<Department>($@"department_name='{name}'").department_id.ToString();
		}
		/// <summary>
		/// 加载科室分类树。
		/// </summary>
		/// <param name="treeView"></param>
		public void LoadTree(TreeView treeView)
		{
			Idal idal = new Dal();
			List<Department> departments = idal.SelectModels<Department>("");
			foreach(var item in departments)
			{
				AddNodes(treeView, treeView.Nodes, item.department_route);
			}			
		}
		/// <summary>
		/// 更新医生记录某个字段的信息。
		/// </summary>
		/// <param name="filed">字段名</param>
		/// <param name="value">新值</param>
		/// <param name="id">医生记录主键号</param>
		/// <returns></returns>
		public bool UpdateDoctorInfo(string filed,string value,int id)
		{
			Idal idal = new Dal();
			return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.doctor set {filed}='{value}' where doctor_id='{id}';SET FOREIGN_KEY_CHECKS=1;");
		}
		/// <summary>
		/// 获取科室表。
		/// </summary>
		/// <returns></returns>
		public DataTable GetDepartment()
		{
			Idal idal = new Dal();
			return idal.GetDataTable("select department_name from his.department;");
		}
		/// <summary>
		/// 删除医生排班记录。
		/// </summary>
		/// <param name="id">医生排班表主键号</param>
		/// <returns></returns>
		public bool DeleteDoctorsschedule(int id)
		{
			Idal idal = new Dal();
			return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;delete from his.doctorsschedule where doctorsschedule_id='{id}';SET FOREIGN_KEY_CHECKS=1;");
		}
		/// <summary>
		/// 获取医生排班表
		/// </summary>
		/// <returns></returns>
		public DataTable Loaddoctorsschedule()
		{
			Idal idal = new Dal();
			string sql = $@"select doctorsschedule_weekday as 星期,doctorsschedule_worktime as 时段,doctorsschedule_num as 系统总号数,doctorsschedule_addnum as 系统可加号数,doctorsschedule_surplusnum as 总剩余号数,doctorsschedule_numweb as 网站总号数,doctorsschedule_surplusnumweb as 网站已挂号数,doctor_name as 医生姓名,department_name as 科室,doctorsschedule_id from v_doctorsschedule_doctor_department;";
			return idal.GetDataTable(sql);
		}
		/// <summary>
		/// 为科室分类树添加一个节点。
		/// </summary>
		/// <param name="tv"></param>
		/// <param name="tnc"></param>
		/// <param name="tnPath"></param>
		private  void AddNodes(TreeView tv, TreeNodeCollection tnc, string tnPath)
		{
			TreeNode tn = new TreeNode();
			int id = tnPath.IndexOf('/');
			if (id != -1)
			{
				string sLeft = tnPath.Substring(0, tnPath.IndexOf('/'));
				string sRight = tnPath.Substring(tnPath.IndexOf('/') + 1);
				for (int i = 0; i < tnc.Count; i++)
				{//查找结点
					if (tnc[i].Text == sLeft)
					{
						AddNodes(tv, tnc[i].Nodes, sRight);
						return;
					}
				}
				tnc.Add(sLeft);
				AddNodes(tv, tnc[tnc.Count - 1].Nodes, sRight);
			}
			else
			{
				if (FindNode(tnc, tnPath) == -1)
				{//判断子节点是否存在
					tnc.Add(tnPath);
				}
			}
		}
		/// <summary>
		/// 查询某个科室对应的所有医生姓名。
		/// </summary>
		/// <param name="departmentname">科室名称</param>
		/// <returns></returns>
		public DataTable GetDoctorName(string departmentname)
		{
			Idal idal = new Dal();
			return idal.GetDataTable($@"select doctor_name from his.v_doctor_department where department_name='{departmentname}'");
		}
		/// <summary>
		/// 获取所有医生的姓名。
		/// </summary>
		/// <returns></returns>
		public DataTable GetDoctorName()
		{
			Idal idal = new Dal();
			return idal.GetDataTable("select doctor_name from his.doctor");
		}
		/// <summary>
		/// 查找某个节点是否存在，不存在返回-1。
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		private int FindNode(TreeNodeCollection nodes, string s)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].Text == s)
				{
					return i;
				}
			}
			return -1;
		}

	}
}
