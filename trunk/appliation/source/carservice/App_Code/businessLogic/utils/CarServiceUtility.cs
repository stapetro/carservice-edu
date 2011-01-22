using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using constants;
using System.Globalization;

namespace businesslogic.utils
{
    /// <summary>
    /// Summary description for CarServiceUtility
    /// </summary>
    public class CarServiceUtility
    {
        public CarServiceUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool IsValidDate(string dateValueTxt, out DateTime dateValue)
        {
            dateValue = DateTime.Now;
            bool validDate = DateTime.TryParseExact(dateValueTxt, CarServiceConstants.DATE_FORMAT,
                                new CultureInfo(CarServiceConstants.ENGLISH_CULTURE_INFO),
                                DateTimeStyles.None, out dateValue);           
            return validDate;
        }
    }
}