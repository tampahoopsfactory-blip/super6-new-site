using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.CardType
{
    public class WriteCardTypeDetail_Parameter : TemplateParameter_Base<CardTypeDetail>
    {
        public WriteCardTypeDetail_Parameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public List<CardTypeDetail> CardTypeList;

        public WriteCardTypeDetail_Parameter(List<CardTypeDetail> cardTypeList) : base(cardTypeList)
        {
            CardTypeList = cardTypeList;
        }


        protected override bool CheckedParameterItem(CardTypeDetail cardType)
        {
            
            return true;
        }

        protected override bool CheckedDeleteParameterItem(CardTypeDetail cardType)
        {
            return true;
        }
    }
}
