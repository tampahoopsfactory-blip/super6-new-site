
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    public class ReadAllSubsidy_Result : TemplateResult_Base
    {

        public List<SubsidyDetail> SubsidyDetails { get; set; }

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadAllSubsidy_Result(List<SubsidyDetail> DataList)
        {
            this.SubsidyDetails = DataList;
        }

        public override void Dispose()
        {
            SubsidyDetails = null;
        }
    }
}
