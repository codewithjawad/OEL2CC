using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace lab10webservice
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class calculator : System.Web.Services.WebService
	{
		private static readonly List<string> MaliciousPatterns = new List<string>
		{
			"select\\s+\\*",       // select *
            "drop\\s+table",       // drop table
            "<script.*?>",         // <script>
            "rm\\s+-rf",           // rm -rf
            "--",                  // SQL comment
            "insert\\s+into",      // insert into
            "delete\\s+from",      // delete from
            "update\\s+.*\\s+set"  // update ... set
        };

		[WebMethod]
		public string IsScriptMalicious(string scriptInput)
		{
			foreach (var pattern in MaliciousPatterns)
			{
				if (Regex.IsMatch(scriptInput, pattern, RegexOptions.IgnoreCase))
				{
					return "Malicious";
				}
			}

			return "Safe";
		}
	}
}

