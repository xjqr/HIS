using RegisteredBLL;
using RegisteredMODEL;
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
	public partial class Guanliyuanyanzheng : Form
	{
		public Guanliyuanyanzheng()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if(new Bll().Check(new User() { user_account = "partadmin",user_password=textBox1.Text }))
			{
			Close();
				new Xitongguanli() { TopMost = true }.Show();

			}
			

		}
	}
}
