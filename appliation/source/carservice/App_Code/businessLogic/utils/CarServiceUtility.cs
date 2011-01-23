using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using constants;
using System.Globalization;
using persistence;

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


        public static void AddEntityId(List<int> enityIds, string entityIdTxt)
        {
            int partId;
            if (Int32.TryParse(entityIdTxt, out partId))
            {
                enityIds.Add(partId);
            }
        }

        #region Automobile specific

        public static Automobile GetAutomobile(string automobileIdTxt, ICarServicePersister persister)
        {
            Automobile automobile = null;
            int automobileId;
            if (Int32.TryParse(automobileIdTxt, out automobileId))
            {
                automobile = persister.GetAutomobilById(automobileId);
            }
            return automobile;
        }
        #endregion
    }
}