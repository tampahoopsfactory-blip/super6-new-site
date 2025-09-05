
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Card
{
    public class ReadAllCard_Result : TemplateResult_Base
    {

        public List<CardDetail> CardDetails { get; set; }

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadAllCard_Result(List<CardDetail> DataList)
        {
            this.CardDetails = DataList;
        }


        public override void Dispose()
        {
            CardDetails = null;
        }
    }
}
