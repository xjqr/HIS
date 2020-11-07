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
	public partial class Bingliinfo : Form
	{
		private int Index;
		public Bingliinfo(int index)
		{
			InitializeComponent();
			Index = index;
		}

		private void Bingliinfo_Load(object sender, EventArgs e)
		{
			DataTable data = Bingli.Binglitable;
			textBox1.Text = data.Rows[Index]["drd_mainRemarks"].ToString();
			textBox2.Text = data.Rows[Index]["drd_nowHistory"].ToString();
			textBox3.Text = data.Rows[Index]["drd_pastHistory"].ToString();
			textBox4.Text = data.Rows[Index]["drd_checkresult"].ToString();
			textBox5.Text = data.Rows[Index]["drd_diagnosticResults"].ToString();
			textBox6.Text = data.Rows[Index]["drd_yizhu"].ToString();
			richTextBox1.Text = data.Rows[Index]["drd_prescription"].ToString();
		}
	}
}
