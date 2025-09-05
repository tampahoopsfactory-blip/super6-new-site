using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Data;
using System.Collections.Generic;
using DoNetDrive.Protocol.POS.TemplateMethod;

namespace DoNetDrive.Protocol.POS.CardType
{
    /// <summary>
    /// 读取到的卡类返回结果
    /// </summary>
    public class ReadDataBase_Result : TemplateResult_Base
    {
        /// <summary>
        /// 读取到的卡类列表
        /// </summary>
        public List<CardTypeDetail> CardTypes { get; set; }


        public ReadDataBase_Result(List<CardTypeDetail> cardTypeDetailList)
        {
            CardTypes = cardTypeDetailList;
        }
        public override void Dispose()
        {
            CardTypes = null;
        }

    }
}
