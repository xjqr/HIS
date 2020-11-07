using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using DoctorWorkstationBLL;

namespace DoctorWorkstationUI
{
	public partial class 西药处方 : UserControl
	{
		public static string chufangtext;
		public int Index { get; set; }
		public 西药处方()
		{
			InitializeComponent();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			UiMain.RemoveTabpageIndex = Index;
		}
		public void SetIndex(int newindex)
		{
			Index = newindex;
			
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 5 && e.RowIndex < dataGridView1.Rows.Count - 1)
			{
				dataGridView1.Rows.RemoveAt(e.RowIndex);
			}
		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void 西药处方_Load(object sender, EventArgs e)
		{
			DataGridViewComboBoxColumn dc = new DataGridViewComboBoxColumn();
			dc.HeaderText = "药品";
			dc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
			DataTable dataTable = new Bll().GetMedicine("西药");
			foreach (DataRow row in dataTable.Rows)
			{
				dc.Items.Add($@"{row["medicine_name"]}({row["medicine_specifications"]})");
			}
			dataGridView1.Columns.Insert(0, dc);
		}
	}
}
