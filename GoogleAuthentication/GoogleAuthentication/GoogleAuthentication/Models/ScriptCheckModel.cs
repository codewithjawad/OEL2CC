using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAuthentication.Models
{
	public class ScriptCheckModel
	{
		public string ScriptContent { get; set; }
		public string Result { get; set; } // "Safe" or "Malicious"
	}
}