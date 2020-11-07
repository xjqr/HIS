using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyMODEL
{
	public class V_inventory_medicine_manfacture
	{
		public int inventory_id { get; set; }
		public int inventory_num { get; set; }
		public int inventory_medicineid { get; set; }
		public int Medicine_id { get; set; }
		public string Medicine_name { get; set; }
		public string Medicine_specifications { get; set; }
		public DateTime Medicine_borndate { get; set; }
		public int Medicine_enabletime { get; set; }
		public sbyte Medicine_enable { get; set; }
		public string Medicine_type { get; set; }
		public float Medicine_sellingPrice { get; set; }
		public float Medicine_PurchasePrice { get; set; }

		public int Medicine_manufacturerid { get; set; }
		public string manufacture_name { get; set; }
		public int manufacture_id { get; set; }

	}
}
