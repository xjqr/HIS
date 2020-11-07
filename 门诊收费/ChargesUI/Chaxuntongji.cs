using ChargesBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChargesUI
{
	public partial class Chaxuntongji : Form
	{
		public Chaxuntongji()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{DataTable dataTable = new Bll().Selectchargerecord(dateTimePicker1.Value, dateTimePicker2.Value);
			label7.Text = dataTable.Rows.Count.ToString();
			label8.Text = dataTable.Compute("sum(chargerecord_money)", "true").ToString();
			dataGridView1.Rows.Clear();
			foreach (DataRow row in dataTable.Rows)
			{
				dataGridView1.Rows.Add(row["chargerecord_patientid"].ToString(), row["chargerecord_money"].ToString(), row["chargerecord_time"].ToString());
			}

			}
			catch { throw; }
			
		}

		private void Chaxuntongji_Load(object sender, EventArgs e)
		{
			dateTimePicker1.MaxDate = DateTime.Now.AddDays(-1);
			dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			dateTimePicker2.MinDate = DateTime.Now;
			dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
		}
	}
}
