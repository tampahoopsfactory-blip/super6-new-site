using DoNetDrive.Protocol.Door.Door8800.Card;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.DeleteCard
{
    /// <summary>
    ///  删除卡片
    /// </summary>
    public class DeleteCard_Parameter : WriteCardList_Parameter_Base<Data.CardDetail>
    {
        /// <summary>
        /// 创建 将卡片列表从到控制器中删除 指令的参数
        /// </summary>
        /// <param name="cardList">需要删除的卡片列表</param>
        public DeleteCard_Parameter(List<Data.CardDetail> cardList) : base(cardList)
        {
            cardList?.Sort();
        }
    }
}
