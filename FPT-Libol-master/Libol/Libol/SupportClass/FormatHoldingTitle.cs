using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.SupportClass
{
    public class FormatHoldingTitle
    {
        public string OnFormatHoldingTitle(string value)
        {

            int index = 0;
            //Loc ki tu $x 
            while (true)
            {
                index = value.IndexOf("$");
                if (index == -1) break;
                value = value.Substring(0, index) +" "+ value.Substring(index + 2);
            }

            return value;

            //string validate = title.Replace("$a", "");
            //validate = validate.Replace("$b", "");
            //validate = validate.Replace("$c", "");
            //validate = validate.Replace("=$b", "");
            //validate = validate.Replace(":$b", "");
            //validate = validate.Replace("/$c", "");
            //validate = validate.Replace(".$n", "");
            //validate = validate.Replace(":$p", "");
            //validate = validate.Replace(";$c", "");
            //validate = validate.Replace("+$e", "");
            //return validate;
        }
		public string getContent(string[] str,string tag)
		{
			string rt = "";
			foreach (string st in str)
			{
				if (!st.Equals(""))
				{
					string a = st[0].ToString();
					if (st[0].ToString().Equals(tag))
					{
						
						rt = st.Substring(1,st.Length-1);
						string q = rt[rt.Length - 1].ToString();
						if (q.Equals("/")|| q.Equals("=")||q.Equals(":"))
						{
							rt=rt.Replace(q, "");
						}
						break;
					}
				}
				
			}
			return rt;
		}
		public string getTagOnMarc(string str,string tag)
		{
			string result = "";
			string[] arr = str.Split('$');
			if (arr.Length == 1)
			{
				result = str;
			}
			else
			{
				
				result=	getContent(arr, tag);
				
			}
			return result;

		}
    }
}