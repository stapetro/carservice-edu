using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using persistence;
using System.Data.Objects;
using presentation.utils;
using constants;
using System.Globalization;
using businesslogic.utils;
using System.Data;
using businesslogic;

namespace presentation
{
    public partial class MembersRepairCards : System.Web.UI.Page
    {
        private ICarServicePersister persister;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (persister == null)
            {
                this.persister = new CarServicePersister();
            }
            if (IsPostBack == false)
            {
                BindRepairCardsGrid();
                ViewState[CarServiceConstants.SORT_DIRECTION_VIEW_STATE_ATTR] = SortDirection.Ascending;
                ViewState[CarServiceConstants.SORT_EXPRESSION_VIEW_STATE_ATTR] = CarServiceConstants.REPAIR_CARD_ID_SORT_EXPRESSION;
                CarServiceUtility.ClearSessionAttributes(Session);
            }
            this.notificationMsg.Visible = false;
        }

        protected void EditRepairCardEventHandler_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;

            string repairCardId = CarServicePresentationUtility.GetGridCellContent(this.repairCardsGrid, rowIndex, 0);
            if (string.IsNullOrEmpty(repairCardId) == false)
            {
                Session[CarServiceConstants.REPAIR_CARD_ID_PARAM_NAME] = repairCardId;
                string editAutomobilePageUrl = "~/Members/RepairCards/AddRepairCard.aspx";
                Response.Redirect(editAutomobilePageUrl);
            }
        }

        protected void RepairCardsGridView_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            this.repairCardsGrid.PageIndex = e.NewPageIndex;
            object repairCardsFilterObject = Session[CarServiceConstants.REPAIR_CARDS_FILTER_SESSION_ATTR_NAME];
            IQueryable<RepairCard> repairCards;
            if (repairCardsFilterObject != null)
            {
                RepairCardFilter filter = (RepairCardFilter)repairCardsFilterObject;
                repairCards = FilterRepairCards(filter);
            }
            else
            {
                repairCards = this.persister.GetRepairCards();
            }
            object sortDirectionObj = ViewState[CarServiceConstants.SORT_DIRECTION_VIEW_STATE_ATTR];
            object sortExpressionObj = ViewState[CarServiceConstants.SORT_EXPRESSION_VIEW_STATE_ATTR];
            if (sortDirectionObj != null && sortExpressionObj != null)
            {
                repairCards = CarServiceUtility.SortRepairCards(repairCards, 
                    sortExpressionObj.ToString(), (SortDirection)sortDirectionObj);
            }
            BindRepairCardsGrid(repairCards);
        }

        protected void FilterRepairCards_OnClick(object sender, EventArgs e)
        {
            IQueryable<RepairCard> customRepairCards = null;
            int filterType = this.repairCardsFilterType.SelectedIndex;
            RepairCardFilter filter = new RepairCardFilter(filterType);
            if (filterType == CarServiceConstants.ALL_REPAIR_CARDS_FILTER_TYPE)
            {
                filter.VinChassis = this.VinChassisAllRepairCardsTxt.Text;
            }
            else if (filterType == CarServiceConstants.FINISHED_REPAIR_CARDS_FILTER_TYPE)
            {
                DateTime? fromFinishRepairDate = null;
                bool validFromFinishRepairDate = false;
                string fromFinishDateTxt = this.fromFinishRepairDate.SelectedDate;
                if (string.IsNullOrEmpty(fromFinishDateTxt) == false)
                {
                    DateTime fromFinishRepairDateValue = DateTime.Now;
                    validFromFinishRepairDate = CarServiceUtility.IsValidDate(fromFinishDateTxt, out fromFinishRepairDateValue);
                    if (validFromFinishRepairDate == true)
                    {
                        fromFinishRepairDate = fromFinishRepairDateValue;
                    }
                }
                DateTime? toFinishRepairDate = null;
                bool validToFinishRepairDate = false;
                string toFinishRepairDateTxt = this.toFinishRepairDate.SelectedDate;
                if (string.IsNullOrEmpty(toFinishRepairDateTxt) == false)
                {
                    DateTime toFinishRepairDateValue = DateTime.Now;
                    validToFinishRepairDate = CarServiceUtility.IsValidDate(toFinishRepairDateTxt, out toFinishRepairDateValue);
                    if (validToFinishRepairDate == true)
                    {
                        toFinishRepairDate = toFinishRepairDateValue;
                    }
                }
                string notificationMsg = string.Empty;
                if (validFromFinishRepairDate == false)
                {
                    notificationMsg += "From finish repair date is not valid format.<br/>";
                }
                if (validToFinishRepairDate == false)
                {
                    notificationMsg += "To finish repair date is not valid format.<br/>";
                }
                if (string.IsNullOrEmpty(notificationMsg) == false)
                {
                    this.notificationMsg.Text = notificationMsg;
                    this.notificationMsg.Visible = true;
                    return;
                }
                if (validFromFinishRepairDate && validToFinishRepairDate)
                {
                    filter.FromFinishRepair = fromFinishRepairDate.Value;
                    filter.ToFinishRepair = toFinishRepairDate.Value;
                }
            }
            else if (filterType == CarServiceConstants.UNFINISHED_REPAIR_CARDS_FILTER_TYPE)
            {
                bool validDate = true;
                string startRepairDateTxt = this.startRepairDate.SelectedDate;
                if (string.IsNullOrEmpty(startRepairDateTxt) == false)
                {
                    DateTime startRepairDateValue = DateTime.Now;
                    validDate = CarServiceUtility.IsValidDate(startRepairDateTxt, out startRepairDateValue);
                    if (validDate == true)
                    {
                        filter.StartRepair = startRepairDateValue;
                    }
                    else
                    {
                        this.notificationMsg.Text = "Start repair date is not valid format<br/>";
                        this.notificationMsg.Visible = true;
                        return;
                    }
                }
                filter.VinChassis = this.VinChassisTxt.Text;                   
            }
            customRepairCards = FilterRepairCards(filter);
            ViewState[CarServiceConstants.SORT_DIRECTION_VIEW_STATE_ATTR] = SortDirection.Ascending;
            ViewState[CarServiceConstants.SORT_EXPRESSION_VIEW_STATE_ATTR] = CarServiceConstants.REPAIR_CARD_ID_SORT_EXPRESSION;                        
            Session[CarServiceConstants.REPAIR_CARDS_FILTER_SESSION_ATTR_NAME] = filter;
            BindRepairCardsGrid(customRepairCards);
        }


        protected void ReportType_IndexChanged(object sender, EventArgs e)
        {
            int filterType = this.repairCardsFilterType.SelectedIndex;
            string filterBtnValidationGroupName = string.Empty;
            if (filterType == CarServiceConstants.ALL_REPAIR_CARDS_FILTER_TYPE)
            {
                this.allRepairCardsFilter.Visible = true;
                this.finishedRepairCardsFilter.Visible = false;
                this.unfinishedRepairCardsFilter.Visible = false;
                filterBtnValidationGroupName = "AllRepairCardsFilterValidationGroup";
            }
            else if (filterType == CarServiceConstants.FINISHED_REPAIR_CARDS_FILTER_TYPE)
            {
                this.finishedRepairCardsFilter.Visible = true;
                this.allRepairCardsFilter.Visible = false;
                this.unfinishedRepairCardsFilter.Visible = false;
                filterBtnValidationGroupName = "UnfinishedRepairCardsFilterValidationGroup";
            }
            else if (filterType == CarServiceConstants.UNFINISHED_REPAIR_CARDS_FILTER_TYPE)
            {
                this.unfinishedRepairCardsFilter.Visible = true;
                this.finishedRepairCardsFilter.Visible = false;
                this.allRepairCardsFilter.Visible = false;
                filterBtnValidationGroupName = "FinishedRepairCardsFilterValidationGroup";
            }
            this.filterButton.ValidationGroup = filterBtnValidationGroupName;
        }

        protected void RepairCardsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection newSortDirection = SortDirection.Descending;
            object currentSortDirectionObject = ViewState[CarServiceConstants.SORT_DIRECTION_VIEW_STATE_ATTR];
            if (currentSortDirectionObject != null)
            {
                SortDirection currentSortDirection = (SortDirection)currentSortDirectionObject;
                newSortDirection = (currentSortDirection.Equals(SortDirection.Ascending)) ? SortDirection.Descending : SortDirection.Ascending;
            }
            ViewState[CarServiceConstants.SORT_DIRECTION_VIEW_STATE_ATTR] = newSortDirection;
            ViewState[CarServiceConstants.SORT_EXPRESSION_VIEW_STATE_ATTR] = e.SortExpression;
            object repairCardsFilterObj = Session[CarServiceConstants.REPAIR_CARDS_FILTER_SESSION_ATTR_NAME];
            IQueryable<RepairCard> repairCards;
            if (repairCardsFilterObj != null)
            {
                RepairCardFilter filter = (RepairCardFilter)repairCardsFilterObj;
                repairCards = FilterRepairCards(filter);
            }
            else
            {
                repairCards = this.persister.GetRepairCards();
            }
            IQueryable<RepairCard> sortedCards = 
                CarServiceUtility.SortRepairCards(repairCards, e.SortExpression, newSortDirection);
            BindRepairCardsGrid(sortedCards);
        }

        #region Private methods

        private IQueryable<RepairCard> FilterRepairCards(RepairCardFilter filter)
        {
            IQueryable<RepairCard> repairCards = null;
            int filterType = filter.Type;
            switch (filterType)
            {
                case CarServiceConstants.ALL_REPAIR_CARDS_FILTER_TYPE:
                    repairCards = FilterRepairCards(filter.VinChassis);
                    break;
                case CarServiceConstants.UNFINISHED_REPAIR_CARDS_FILTER_TYPE:
                    repairCards = FilterUnfinishedRepairCards(filter.StartRepair, filter.VinChassis);
                    break;
                case CarServiceConstants.FINISHED_REPAIR_CARDS_FILTER_TYPE:
                    repairCards = FilterFinishedRepairCards(filter.FromFinishRepair, filter.ToFinishRepair);
                    break;
            }
            return repairCards;
        }


        private IQueryable<RepairCard> FilterRepairCards(string vinChassis)
        {
            IQueryable<RepairCard> repairCards;
            if (string.IsNullOrEmpty(vinChassis))
            {
                repairCards = this.persister.GetRepairCards();
            }
            else
            {
                repairCards = this.persister.GetRepairCards(vinChassis);
            }
            return repairCards;
        }

        private IQueryable<RepairCard> FilterUnfinishedRepairCards(DateTime startRepairDate, string vinChassis)
        {
            IQueryable<RepairCard> foundRepairCards = 
                this.persister.GetUnfinishedRepairCards(startRepairDate, vinChassis);
            return foundRepairCards;
        }

        private IQueryable<RepairCard> FilterFinishedRepairCards(DateTime fromFinishRepairDate, DateTime toFinishRepairDate)
        {
            IQueryable<RepairCard> foundRepairCards = 
                this.persister.GetFinishedRepairCards(fromFinishRepairDate, toFinishRepairDate);
            return foundRepairCards;
        }

        //TODO To be moved to presentaiton utility
        private object GetRepairCardsFormatForGrid(IQueryable<RepairCard> repairCards)
        {            
            var customRepairCards =
                from repairCard in repairCards
                select new
                {
                    repairCard.CardId,
                    repairCard.Automobile.Vin,
                    repairCard.Automobile.ChassisNumber,
                    repairCard.StartRepair,
                    repairCard.FinishRepair,
                    repairCard.CardPrice
                };
            return customRepairCards;
        }

        //TODO To be moved to presentaiton utility
        private object GetRepairCardsFormatForGrid(ObjectSet<RepairCard> repairCards)
        {            
            /*
            DataTable dataTable = new DataTable("repairCards");
            dataTable.Columns.Add("CardId", Type.GetType("System.String"), "CardId");
            dataTable.Columns.Add("Vin", Type.GetType("System.String"), "Vin");
            dataTable.Columns.Add("ChassisNumber", Type.GetType("System.String"), "ChassisNumber");
            dataTable.Columns.Add("StartRepair", Type.GetType("System.DateTime"), "StartRepair");
            dataTable.Columns.Add("FinishRepair", Type.GetType("System.DateTime"), "FinishRepair");
            dataTable.Columns.Add("CardPrice", Type.GetType("System.Decimal"), "FinishRepair");

            foreach (RepairCard card in repairCards)
            {
                DataRow dataRow = dataTable.NewRow();

                dataRow["CardId"] = card.CardId;
                dataRow["Vin"] = card.Automobile.Vin;
                dataRow["Chassis"] = card.Automobile.ChassisNumber;
                dataRow["StartRepair"] = card.StartRepair;
                dataRow["FinishRepair"] = card.FinishRepair;
                dataRow["CardPrice"] = card.CardPrice;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
             */ 
            
            var customRepairCards =
                from repairCard in repairCards
                select new
                {
                    repairCard.CardId,
                    repairCard.Automobile.Vin,
                    repairCard.Automobile.ChassisNumber,
                    repairCard.StartRepair,
                    repairCard.FinishRepair,
                    repairCard.CardPrice
                };
            return customRepairCards;
        }

        private void BindRepairCardsGrid()
        {
            ObjectSet<RepairCard> repairCards = this.persister.GetRepairCards();
            BindRepairCardsGrid(repairCards);            
        }

        private void BindRepairCardsGrid(IQueryable<RepairCard> repairCards)
        {
            this.repairCardsGrid.DataSource = GetRepairCardsFormatForGrid(repairCards); ;
            this.repairCardsGrid.DataBind();
        }

        #endregion
    }
}