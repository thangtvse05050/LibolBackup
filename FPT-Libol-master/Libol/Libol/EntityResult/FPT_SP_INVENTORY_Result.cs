using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
	public class FPT_SP_INVENTORY_Result
	{
		public string Content { get; set; }
		public string Code { get; set; }
		public string CopyNumber { get; set; }
		public string CallNumber { get; set; }
		public Nullable<float> Price { get; set; }
		public string Note { get; set; }
		public string Currency { get; set; }
	}
}