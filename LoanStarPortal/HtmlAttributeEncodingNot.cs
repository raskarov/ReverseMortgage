using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanStarPortal
{
    public class HtmlAttributeEncodingNot : System.Web.Util.HttpEncoder
    {
        protected override void HtmlAttributeEncode(string value, System.IO.TextWriter output)
        {
            var val = Encoder.HtmlEncode(value);
            val = val.Replace("&#39;", "'");
            output.Write(val);
        }
    }
}