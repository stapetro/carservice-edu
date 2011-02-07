using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using constants;
using System.Globalization;
using System.Web.SessionState;
using persistence;
using System.Web.UI.WebControls;
using System.Data.Objects;

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

        public static void ClearSessionAttributes(HttpSessionState session)
        {
            session[CarServiceConstants.AUTOMOBILE_ID_REQUEST_PARAM_NAME] = null;
            session[CarServiceConstants.REPAIR_CARD_ID_PARAM_NAME] = null;
            session[CarServiceConstants.REPAIR_CARDS_FILTERED_SESSION_ATTR_NAME] = null;
        }

        public static IQueryable<RepairCard> SortRepairCards(ObjectSet<RepairCard> repairCards, string sortExpression, SortDirection sortDirection)
        {
            bool acsending = sortDirection.Equals(SortDirection.Ascending);
            IQueryable<RepairCard> sortedCards;
            if (sortExpression.Equals(CarServiceConstants.REPAIR_CARD_ID_SORT_EXPRESSION))
            {
                if (acsending)
                {
                    sortedCards = repairCards.OrderBy(card => card.CardId);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.CardId);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.REPAIR_CARD_CHASSIS_SORT_EXPRESSION))
            {
                if (acsending)
                {
                    sortedCards = repairCards.OrderBy(card => card.Automobile.ChassisNumber);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.Automobile.ChassisNumber);
                }
            }
            else
            {
                sortedCards = repairCards.OrderBy(card => card.CardId);
            }
            return sortedCards;
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