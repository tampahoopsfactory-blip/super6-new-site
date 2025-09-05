
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Card
{
    /// <summary>
    /// 添加名单命令参数
    /// </summary>
    public class WriteCard_Parameter : TemplateParameter_Base<CardDetail>
    {
        public WriteCard_Parameter()
        {

        }

        public WriteCard_Parameter(List<CardDetail> list) : base(list)
        {
        }

        protected override bool CheckedParameterItem(CardDetail card)
        {
            if (card.CardData < 0)
            {
                return false;
            }
            if (card.CardType > 2)
            {
                return false;
            }
            return true;
        }

        protected override bool CheckedDeleteParameterItem(CardDetail card)
        {
            if (card.CardData < 0)
            {
                return false;
            }
            return true;
        }

        public override int GetDataLen()
        {
            return 0x08;
        }

        public override int GetDeleteDataLen()
        {
            return 4;
        }

       
    }
}