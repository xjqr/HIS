using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWorkstationMODEL
{
	public class ncdm
	{
		public int ncdm_patientid { get; set; }
		public string ncdm_medicinename { get; set; }

		public int ncdm_num { get; set; }

		public float ncdm_money { get; set; }
		public sbyte ncdm_chargedstate { get; set; }
		public sbyte ncdm_pstate { get; set; }
		public int ncdm_chargerecordid { get; set; }

	}
}
