using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Card;
using DoNetDrive.Protocol.Elevator.FC8864.Data;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardDataBase
{
    /// <summary>
    /// FC88/MC58 读取卡片数据库中的所有卡数据
    /// </summary>
    public class ReadCardDataBase_Result : ReadCardDataBase_Base_Result<Data.CardDetail>
    {

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="cardList">卡列表</param>
        /// <param name="dataBaseSize">读取到的卡数量</param>
        /// <param name="cardType">卡数据库类型</param>
        public ReadCardDataBase_Result(List<Data.CardDetail> cardList, int dataBaseSize, int cardType) 
            : base(cardList, dataBaseSize, cardType)
        {
        }
    }
}
