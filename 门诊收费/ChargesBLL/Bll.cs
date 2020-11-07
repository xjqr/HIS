using System;
using System.Data;
using IDAL;
using DAL;
using ChargesMODEL;
namespace ChargesBLL
{
	public class Bll
	{
		/// <summary>
		/// 获取待收费病人信息和收费信息。
		/// </summary>
		/// <returns></returns>
		public DataTable Getneedchargelist()
		{
			Idal idal = new Dal();
			return idal.GetDataTable($@"select * from his.v_chargerecord_patient where chargerecord_enable=1");
		}
		/// <summary>
		/// 获取收费的药品明细。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataTable Getchargedetial(int id)
		{
			Idal idal = new Dal();
			return idal.GetDataTable($@"select * from his.ncdm where ncdm_chargerecordid='{id}'");
		}
		/// <summary>
		/// 更新收费记录状态，将收费状态置为0表示已经收费。
		/// </summary>
		/// <param name="chargerecordid"></param>
		/// <param name="ChargedPersonnel_id"></param>
		/// <returns></returns>
		public bool Updatechargerecord(int chargerecordid, int ChargedPersonnel_id)
		{
			Idal idal = new Dal();
			return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.chargerecord set chargerecord_enable=0,chargerecord_chargedpersonnelid='{ChargedPersonnel_id}',chargerecord_time='{DateTime.Now}'where chargerecord_id='{chargerecordid}';SET FOREIGN_KEY_CHECKS=1;");		
		}
		/// <summary>
		/// 更新病人表中病人的余额。
		/// </summary>
		/// <param name="patientid"></param>
		/// <param name="summoney"></param>
		/// <returns></returns>
		public bool Updatepatient(int patientid, float summoney)
		{
			Idal idal = new Dal();
			float m = idal.SelectModel<Patient>($"patient_id={patientid}").patient_money - summoney;
			if (m < 0) { return false; }
			return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.patient set patient_money='{m}' where patient_id='{patientid}';SET FOREIGN_KEY_CHECKS=1;");
		}
		/// <summary>
		/// 更加收费员账号获取对应收费员表的主键号。
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public int Getchargepersonid(string account)
		{
			Idal idal = new Dal();
			return int.Parse(idal.GetDataTable($"select * from his.v_chargedpersonnel_user where user_account='{account}'").Rows[0]["chargedpersonnel_id"].ToString());
		}
		/// <summary>
		/// 根据时间查询收费记录。
		/// </summary>
		/// <param name="starttime">起始时间</param>
		/// <param name="endtime">终止时间</param>
		/// <returns></returns>
		public DataTable Selectchargerecord(DateTime starttime,DateTime endtime)
		{
			Idal idal = new Dal();
			return idal.GetDataTable($@"select * from his.chargerecord where chargerecord_time between '{starttime:yyyy-MM-dd HH:mm:ss}' and '{endtime:yyyy-MM-dd HH:mm:ss}'");
		}
	}
}
