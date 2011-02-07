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
using System.Text.RegularExpressions;

namespace businesslogic.utils
{
    /// <summary>
    /// Summary description for CarServiceUtility
    /// </summary>
    public class CarServiceUtility
    {
        private static Regex guidRegEx = new Regex("^[A-Fa-f0-9]{32}$|" +
                              "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                              "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$", RegexOptions.Compiled);


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
            session[CarServiceConstants.REPAIR_CARDS_FILTER_SESSION_ATTR_NAME] = null;
        }

        public static bool GuidTryParse(string guidTxt, out Guid result)
        {
            if (String.IsNullOrEmpty(guidTxt) == false && guidRegEx.IsMatch(guidTxt))
            {
                result = new Guid(guidTxt);
                return true;
            }
            result = default(Guid);
            return false;
        }

        #region Sorting utility

        public static IQueryable<RepairCard> SortRepairCards(IQueryable<RepairCard> repairCards, 
            string sortExpression, SortDirection sortDirection)
        {
            bool ascending = sortDirection.Equals(SortDirection.Ascending);
            IQueryable<RepairCard> sortedCards;
            if (sortExpression.Equals(CarServiceConstants.REPAIR_CARD_ID_SORT_EXPRESSION))
            {
                if (ascending)
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
                if (ascending)
                {
                    sortedCards = repairCards.OrderBy(card => card.Automobile.ChassisNumber);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.Automobile.ChassisNumber);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.REPAIR_CARD_Vin_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedCards = repairCards.OrderBy(card => card.Automobile.Vin);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.Automobile.Vin);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.REPAIR_CARD_START_DATE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedCards = repairCards.OrderBy(card => card.StartRepair);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.StartRepair);
                }
            }
            else if(sortExpression.Equals(CarServiceConstants.REPAIR_CARD_FINISH_DATE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedCards = repairCards.OrderBy(card => card.FinishRepair);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.FinishRepair);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.REPAIR_CARD_PRICE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedCards = repairCards.OrderBy(card => card.CardPrice);
                }
                else
                {
                    sortedCards = repairCards.OrderByDescending(card => card.CardPrice);
                }
            }
            else
            {
                sortedCards = repairCards;
            }
            return sortedCards;
        }

        public static IEnumerable<CarServiceUser> SortUsers(List<CarServiceUser> users, 
            string sortExpression, SortDirection sortDirection)
        {
            bool ascending = sortDirection.Equals(SortDirection.Ascending);
            IEnumerable<CarServiceUser> sortedUsers = null;
            if (sortExpression.Equals(CarServiceConstants.USER_NAME_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedUsers = users.OrderBy(user => user.UserName);
                }
                else
                {
                    sortedUsers = users.OrderByDescending(user => user.UserName);
                }
                
            }
            else if(sortExpression.Equals(CarServiceConstants.USER_EMAIL_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedUsers = users.OrderBy(user => user.Email);
                }
                else
                {
                    sortedUsers = users.OrderByDescending(user => user.Email);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.USER_FIRST_NAME_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedUsers = users.OrderBy(user => user.FirstName);
                }
                else
                {
                    sortedUsers = users.OrderByDescending(user => user.FirstName);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.USER_LAST_NAME_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedUsers = users.OrderBy(user => user.LastName);
                }
                else
                {
                    sortedUsers = users.OrderByDescending(user => user.LastName);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.USER_ACTIVE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedUsers = users.OrderBy(user => user.IsActive);
                }
                else
                {
                    sortedUsers = users.OrderByDescending(user => user.IsActive);
                }
            }
            return sortedUsers;
        }

        public static IQueryable<SparePart> SortSpareParts(IQueryable<SparePart> spareParts,
                    string sortExpression, SortDirection sortDirection)
        {
            bool ascending = sortDirection.Equals(SortDirection.Ascending);
            IQueryable<SparePart> sortedSpareParts = spareParts;
            if (sortExpression.Equals(CarServiceConstants.SPARE_PART_ID_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedSpareParts = spareParts.OrderBy(part => part.PartId);
                }
                else
                {
                    sortedSpareParts = spareParts.OrderByDescending(part => part.PartId);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.SPARE_PART_NAME_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedSpareParts = spareParts.OrderBy(part => part.Name);
                }
                else
                {
                    sortedSpareParts = spareParts.OrderByDescending(part => part.Name);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.SPARE_PART_PRICE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedSpareParts = spareParts.OrderBy(part => part.Price);
                }
                else
                {
                    sortedSpareParts = spareParts.OrderByDescending(part => part.Price);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.SPARE_PART_ACTIVE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedSpareParts = spareParts.OrderBy(part => part.IsActive);
                }
                else
                {
                    sortedSpareParts = spareParts.OrderByDescending(part => part.IsActive);
                }
            }
            return sortedSpareParts;
        }

        public static IQueryable<Automobile> SortAutomobiles(IQueryable<Automobile> automobiles,
                    string sortExpression, SortDirection sortDirection)
        {
            bool ascending = sortDirection.Equals(SortDirection.Ascending);
            IQueryable<Automobile> sortedAutomobiles = automobiles;
            if (sortExpression.Equals(CarServiceConstants.AUTOMOBILE_ID_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedAutomobiles = automobiles.OrderBy(auto => auto.AutomobileId);
                }
                else
                {
                    sortedAutomobiles = automobiles.OrderByDescending(auto => auto.AutomobileId);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.AUTOMOBILE_VIN_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedAutomobiles = automobiles.OrderBy(auto => auto.Vin);
                }
                else
                {
                    sortedAutomobiles = automobiles.OrderByDescending(auto => auto.Vin);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.AUTOMOBILE_CHASSIS_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedAutomobiles = automobiles.OrderBy(auto => auto.ChassisNumber);
                }
                else
                {
                    sortedAutomobiles = automobiles.OrderByDescending(auto => auto.ChassisNumber);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.AUTOMOBILE_MAKE_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedAutomobiles = automobiles.OrderBy(auto => auto.Make);
                }
                else
                {
                    sortedAutomobiles = automobiles.OrderByDescending(auto => auto.Make);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.AUTOMOBILE_MODEL_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedAutomobiles = automobiles.OrderBy(auto => auto.Model);
                }
                else
                {
                    sortedAutomobiles = automobiles.OrderByDescending(auto => auto.Model);
                }
            }
            else if (sortExpression.Equals(CarServiceConstants.AUTOMOBILE_OWNER_SORT_EXPRESSION))
            {
                if (ascending)
                {
                    sortedAutomobiles = automobiles.OrderBy(auto => auto.Owner);
                }
                else
                {
                    sortedAutomobiles = automobiles.OrderByDescending(auto => auto.Owner);
                }
            }
            return sortedAutomobiles;
        }

        #endregion

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