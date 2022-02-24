using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory.DAL.Extensions
{
    public class CustomValidations
    {
        public bool ValidateGst(string gstnumber)
        {
            string gstRegexPattern = @"\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}";
            Regex regex = new Regex(gstRegexPattern);
            return regex.IsMatch(gstnumber);
        }

        public bool ValidatePan(string strPan)
        {
            string panRegexPattern = "[A-Z]{5}[0-9]{4}[A-Z]{1}";
            Regex regex = new Regex(panRegexPattern);
            return regex.IsMatch(strPan);
        }
    }
}
