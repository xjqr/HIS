using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DAL;
using PharmacyMODEL;

namespace PharmacyBLL
{
	public class Bll
	{
		 public List<V_inventory_medicine_manfacture> Getinventory()
		{
			Idal idal = new Dal();
			return idal.SelectModels<V_inventory_medicine_manfacture>("");
		}
		public DataTable GetFayaolist()
		{
			Idal idal = new Dal();
			return idal.GetDataTable("select * from his.v_ncdm_chargerecord where chargerecord_enable=0 and ncdm_pstate=0");
		}
		
		public bool Updatencdm(int id)
		{
			Idal idal = new Dal();
			DataTable table = idal.GetDataTable($@"select * from his.ncdm where ncdm_chargerecordid='{id}'");
			int i = 0;
			for(; i < table.Rows.Count; i++)
			{
				DataTable table1 = idal.GetDataTable($@"select * from his.v_inventory_medicine where name='{table.Rows[i]["ncdm_medicinename"]}'");
             if (int.Parse(table.Rows[i]["ncdm_num"].ToString()) > int.Parse(table1.Rows[0]["inventory_num"].ToString()))
			{
					break;
			}
				
					
				
			}
			if (i < table.Rows.Count)
			{
				return false;
			}
			else
			{
				i = 0;
				for (; i < table.Rows.Count; i++)
				{
					DataTable table2 = idal.GetDataTable($@"select * from his.v_inventory_medicine where name='{table.Rows[i]["ncdm_medicinename"]}'");
					int num = int.Parse(table2.Rows[0]["inventory_num"].ToString()) - int.Parse(table.Rows[i]["ncdm_num"].ToString());

					idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.inventory set inventory_num='{num}' where inventory_id='{table2.Rows[0]["inventory_id"]}';SET FOREIGN_KEY_CHECKS=1;");
					
				}
			}
			idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.ncdm set ncdm_pstate=1 where ncdm_chargerecordid='{id}';SET FOREIGN_KEY_CHECKS=1;");

			return true;

		}
	}
}
