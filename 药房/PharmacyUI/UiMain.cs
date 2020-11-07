using ChargesUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PharmacyBLL;
namespace PharmacyUI
{
	public partial class UiMain : Form
	{
		private static DataTable fayaolist;
		private static DataTable dt;
		public UiMain()
		{
			InitializeComponent();
		}
		public UiMain(string account)
		{
			InitializeComponent();
			label4.Text = account;
		}
		private void UiMain_Load(object sender, EventArgs e)
		{
			updatelist();
			
		}
		public void updatelist()
		{
			dataGridView1.Rows.Clear();
			
			dt= new Bll().GetFayaolist();
			fayaolist = new Bll().GetFayaolist();
			int p=0;
			for(int i = 0; i < dt.Rows.Count; i++)
			{
				if(p!= int.Parse(dt.Rows[i]["ncdm_chargerecordid"].ToString()))
				{
					p = int.Parse(dt.Rows[i]["ncdm_chargerecordid"].ToString());
					continue;
				}
				fayaolist.Rows.RemoveAt(i);

			}
			foreach(DataRow row in fayaolist.Rows)
			{
				dataGridView1.Rows.Add(row["chargerecord_patientid"].ToString());
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
		}

		private void 库存管理ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Kucunguanli() { TopMost = true }.Show();
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			updatelist();
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 1)
			{
				dataGridView2.Rows.Clear();
			 DataRow[]a=dt.Select($"chargerecord_patientid={dataGridView1.Rows[e.RowIndex].Cells[0].Value}");
				foreach(DataRow row in a)
				{
					dataGridView2.Rows.Add(row["ncdm_medicinename"].ToString(),row["ncdm_num"].ToString());
				}
			}
			if (e.ColumnIndex == 2)
			{
				if (new Bll().Updatencdm(int.Parse(fayaolist.Rows[e.RowIndex]["ncdm_chargerecordid"].ToString()))){ MessageBox.Show("发药成功", "提示");dataGridView2.Rows.Clear();updatelist(); }
				else { MessageBox.Show("发药失败,请检查库存！", "提示"); }
			}
		}
	}
}
