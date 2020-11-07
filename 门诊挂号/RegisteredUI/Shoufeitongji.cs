using RegisteredBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisteredUI
{
	public partial class Shoufeitongji : Form
	{
		public Shoufeitongji()
		{
			InitializeComponent();
		}

		private void Shoufeitongji_Load(object sender, EventArgs e)
		{
			dateTimePicker1.MaxDate = DateTime.Now.AddDays(-1);
			dateTimePicker2.MinDate = DateTime.Now;
		
			dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
	
			dateTimePicker1.Format = DateTimePickerFormat.Custom;

		

			dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";

			dateTimePicker2.Format = DateTimePickerFormat.Custom;

			

		}

		private void button1_Click(object sender, EventArgs e)
		{
			DataTable dataTable = new Bll().SelectByDatetime(dateTimePicker1.Value, dateTimePicker2.Value);
			string time = $"{dateTimePicker1.Value} 到 {dateTimePicker2.Value}";

			dataGridView1.Rows.Add(time,dataTable.Rows.Count,dataTable.Compute("sum(挂号收费)","true").ToString());
		}
	}
}
