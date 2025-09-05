using DoNetDrive.Protocol.Door.Door8800.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Card
{
    /// <summary>
    /// Door89H 删除卡片
    /// </summary>
    public class DeleteCard_Parameter : WriteCardList_Parameter_Base<Door89H.Data.CardDetail>
    {
        /// <summary>
        /// 创建 将卡片列表从到控制器中删除 指令的参数
        /// </summary>
        /// <param name="cardList">需要删除的卡片列表</param>
        public DeleteCard_Parameter(List<Door89H.Data.CardDetail> cardList) : base(cardList)
        {
            cardList?.Sort();
        }
    }
}
