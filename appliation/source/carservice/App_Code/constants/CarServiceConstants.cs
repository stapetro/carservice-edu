using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace constants
{
    /// <summary>
    /// Summary description for CarServiceConstants
    /// </summary>
    public class CarServiceConstants
    {
        public const string OPERATOR_ROLE_NAME = "operator";
        public const string USER_NAME_REQUEST_PARAM_NAME = "userName";
        public const string EMAIL_REQUEST_PARAM_NAME = "email";
        public const string SPARE_PART_ID_REQUEST_PARAM_NAME = "sparePartId";
        public const string AUTOMOBILE_ID_REQUEST_PARAM_NAME = "automobileId";
        public const string REPAIR_CARD_ID_PARAM_NAME = "repairCardId";
        public const string REPAIR_CARDS_FILTERED_SESSION_ATTR_NAME = "filteredRepairCards";

        public const string INACTIVE_CSS_CLASS_NAME = "inactive";
        public const string POSITIVE_CSS_CLASS_NAME = "positiveMsg";
        public const string NEGATIVE_CSS_CLASS_NAME = "negativeMsg";

        public const string DATE_FORMAT = "yyyy-MM-dd";

        public const string ENGLISH_CULTURE_INFO = "en-US";

        public const int ALL_REPAIR_CARDS_FILTER_TYPE = 0;
        public const int UNFINISHED_REPAIR_CARDS_FILTER_TYPE = 1;
        public const int FINISHED_REPAIR_CARDS_FILTER_TYPE = 2;

        public const string SORT_DIRECTION_VIEW_STATE_ATTR = "SortDirection";
        public const string SORT_EXPRESSION_VIEW_STATE_ATTR = "SortExpression";

        public const string REPAIR_CARD_ID_SORT_EXPRESSION = "CardId";
        public const string REPAIR_CARD_CHASSIS_SORT_EXPRESSION = "ChassisNumber";

        public CarServiceConstants()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}