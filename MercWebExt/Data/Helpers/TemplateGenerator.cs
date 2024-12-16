using MercWebExt.Models.DataBase;
using System.Collections.Generic;
using System.Text;

namespace MercWebExt.Helpers
{
    public class TemplateGenerator
    {
        public static string MakeInduction(List<InductionQuestion> headers, InductionInduction answer)
        {            
            var sb = new StringBuilder();
            sb.Append(@"<br /><br />");
            sb.Append(@"<table style='background-color:white;border-top:0.1em solid #000000;border-bottom:0.1em solid #000000; border-left:0.1em solid #000000;border-right:0.1em solid #000000;' cellspacing='0' cellpadding='0' width='620'>");
            sb.Append(@"<tr width='620' nowrap>");
            sb.Append(@"<td width='95' class='text-center'>");
            sb.Append(@"<img src='~/images/logo-black.png' style='width: 60pt; height: 60pt;'/></td>");
            sb.Append(@"<td width='215'>");
            sb.Append(@"<p style='padding:0px;margin:0px;font-size:8pt;'>");
            sb.Append(@"Note :<br />");
            sb.Append(@"1. You must carry this email with you at all times while onsite.<br />");
            sb.Append(@"2. You must work in a safe manner at all times.<br />");
            sb.Append(@"3. You must wear HiViz clothing and Approved Safety Footwear when onsite.<br />");
            sb.Append(@"4. Make sure you know what to do and where to go in case of an emergency evacuation.<br />");
            sb.Append(@"5. Stay near your vehicle during loading/unloading.<br />");
            sb.Append(@"</p></td>");
            sb.Append(@"<td class='text-center' width='310' style='border-left:0.1em solid #000000;'>");
            sb.Append(@"<p style='padding:0px;margin:0px;' >");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");
            sb.Append(@"");


            return sb.ToString();
            }

    }
}
