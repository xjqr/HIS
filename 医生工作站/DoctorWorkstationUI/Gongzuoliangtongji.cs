using DoctorWorkstationBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorWorkstationUI
{
	public partial class Gongzuoliangtongji : Form
	{
		public Gongzuoliangtongji()
		{
			InitializeComponent();
		}

		private void Gongzuoliangtongji_Load(object sender, EventArgs e)
		{
			
			dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";

			dateTimePicker1.Format = DateTimePickerFormat.Custom;
			dateTimePicker1.MaxDate = DateTime.Now.AddDays(-1);
			dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";

			dateTimePicker2.Format = DateTimePickerFormat.Custom;
			dateTimePicker2.MinDate = DateTime.Now;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			DataTable dataTable = new Bll().JobStatistics(dateTimePicker1.Value, dateTimePicker2.Value);
			label3.Text = dataTable.Rows.Count.ToString();
			foreach(DataRow row in dataTable.Rows)
			{
				dataGridView1.Rows.Add(row["drd_time"].ToString(), row["patient_name"].ToString());
			}
		}
	}
}
