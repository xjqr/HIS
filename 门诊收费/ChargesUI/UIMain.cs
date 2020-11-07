using System;
using System.Data;
using System.Windows.Forms;
using ChargesBLL;
namespace ChargesUI
{
	public partial class UIMain : Form
	{
		private static DataTable chargelist;
		private int Index;
		public UIMain()
		{
			InitializeComponent();
		}
		public UIMain(string useraccount, string password)
		{
			InitializeComponent();
			label4.Text = useraccount;
		}
		private void UIMain_Load(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
			Updatechargelist();
			
		}
		private void Updatechargelist()
		{
			dataGridView1.Rows.Clear();
			chargelist = new Bll().Getneedchargelist();
			foreach(DataRow row in chargelist.Rows)
			{
				dataGridView1.Rows.Add(row["patient_id"].ToString());
			}

		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
		}

		private void 查询统计ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Chaxuntongji().ShowDialog();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Bll bll = new Bll();
			if (bll.Updatepatient(int.Parse(label15.Text),int.Parse(label13.Text.Substring(0,label13.Text.Length-1))))
			{
				bll.Updatechargerecord(int.Parse(chargelist.Rows[Index]["chargerecord_id"].ToString()),new Bll().Getchargepersonid(label4.Text));
				Updatechargelist();
				label13.Text = label12.Text = label15.Text = "";
				dataGridView2.Rows.Clear();
				MessageBox.Show("收费成功", "提示");
			}
			else
			{
				MessageBox.Show("余额不足", "提示");
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 1)
			{
				Index = e.RowIndex;
				label15.Text= chargelist.Rows[e.RowIndex]["patient_id"].ToString();
				label12.Text = chargelist.Rows[e.RowIndex]["patient_name"].ToString();
				label13.Text = chargelist.Rows[e.RowIndex]["chargerecord_money"].ToString()+"元";
				DataTable dataTable = new Bll().Getchargedetial(int.Parse(chargelist.Rows[e.RowIndex]["chargerecord_id"].ToString()));
				dataGridView2.Rows.Clear();
				foreach (DataRow row in dataTable.Rows)
				{
					
					dataGridView2.Rows.Add(row["ncdm_medicinename"].ToString(), row["ncdm_num"].ToString(), row["ncdm_money"].ToString());
				}
			}
		}
	}
}
