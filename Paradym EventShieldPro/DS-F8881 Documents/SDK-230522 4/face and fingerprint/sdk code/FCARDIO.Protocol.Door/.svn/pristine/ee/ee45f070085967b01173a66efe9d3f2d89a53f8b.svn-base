
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    public class WriteSussidy_Parameter : TemplateParameter_Base<SubsidyDetail>
    {
        public WriteSussidy_Parameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public List<SubsidyDetail> SubsidyDetailList;

        public WriteSussidy_Parameter(List<SubsidyDetail> list) : base(list)
        {
            SubsidyDetailList = list;
        }

        protected override bool CheckedDeleteParameterItem(SubsidyDetail subsidy)
        {
            return true;
        }

        protected override bool CheckedParameterItem(SubsidyDetail subsidy)
        {
            if (subsidy.SubsidyDate.Year < 2000 || subsidy.SubsidyDate.Year > 2099)
            {
                return false;
            }
            if (subsidy.SubsidyState > 1)
            {
                return false;
            }
            if (subsidy.SubsidyMoney < 0 || subsidy.SubsidyMoney > 21474836)
            {
                return false;
            }
            if (subsidy.ActualSubsidyMoney < 0 || subsidy.ActualSubsidyMoney > 21474836)
            {
                return false;
            }
            return true;
        }
    }
}
