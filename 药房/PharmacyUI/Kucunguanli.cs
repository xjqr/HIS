using PharmacyBLL;
using PharmacyMODEL;
using PharmacyUI;
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
	public partial class Kucunguanli : Form
	{
		public Kucunguanli()
		{
			InitializeComponent();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void Kucunguanli_Load(object sender, EventArgs e)
		{
			Updatelist();


		}
		public void Updatelist()
		{
			dataGridView1.Rows.Clear();
			List<V_inventory_medicine_manfacture> list = new Bll().Getinventory();
			foreach (var item in list)
			{
				dataGridView1.Rows.Add(item.Medicine_name, item.Medicine_specifications, item.manufacture_name, item.Medicine_type, item.Medicine_sellingPrice + "元", item.Medicine_PurchasePrice + "元", item.Medicine_borndate.ToShortDateString(), item.Medicine_enabletime + "天", item.inventory_num);
			}
		}
		private void 入库ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Updatelist();
		}
	}
}
