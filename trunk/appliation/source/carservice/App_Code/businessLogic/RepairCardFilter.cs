using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using constants;

namespace businesslogic
{
    /// <summary>
    /// Summary description for RepairCardFilter
    /// </summary>
    public class RepairCardFilter
    {
        private int filterType;

        public int Type
        {
            get { return filterType; }
            set { filterType = value; }
        }
        private string vinChassis;

        public string VinChassis
        {
            get { return vinChassis; }
            set { vinChassis = value; }
        }
        private DateTime startRepair;

        public DateTime StartRepair
        {
            get { return startRepair; }
            set { startRepair = value; }
        }
        private DateTime fromFinishRepair;

        public DateTime FromFinishRepair
        {
            get { return fromFinishRepair; }
            set { fromFinishRepair = value; }
        }
        private DateTime toFinishRepair;

        public DateTime ToFinishRepair
        {
            get { return toFinishRepair; }
            set { toFinishRepair = value; }
        }

        public RepairCardFilter(int filterType)
        {
            this.filterType = filterType;
        }
    }
}
