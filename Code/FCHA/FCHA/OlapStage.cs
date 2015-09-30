using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public struct OlapStage
	{
		public string[] left;
		public string[] top;
		public string[] aggregations;

		public OlapStage(string left, string top, string data)
			: this(left, top, data, "SUM")
		{

		}

		public OlapStage(string left, string top, string data, string aggregation)
		{
			this.left = new string[] { left };
			this.top = new string[] { top };
			this.aggregations = new string[] { string.Format("{0}({1}) AS {1}", aggregation, data) };
		}
	}
}
