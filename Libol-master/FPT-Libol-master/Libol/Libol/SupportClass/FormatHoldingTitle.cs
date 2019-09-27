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
    }
}