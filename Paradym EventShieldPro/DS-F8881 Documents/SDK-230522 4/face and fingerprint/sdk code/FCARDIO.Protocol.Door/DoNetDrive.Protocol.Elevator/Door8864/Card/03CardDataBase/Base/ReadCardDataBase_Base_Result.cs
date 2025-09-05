using DotNetty.Buffers;
using FCARDIO.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCARDIO.Protocol.Elevator.FC8864.Card.CardDataBase
{
    /// <summary>
    /// 从控制器中读取卡片数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReadCardDataBase_Base_Result<T> : INCommandResult
        where T : Data.CardDetailBase
    {
        /// <summary>
        /// 读取到的卡片列表
        /// </summary>
        public List<T> CardList;

        /// <summary>
        /// 读取到的卡片数量
        /// </summary>
        public int DataBaseSize;

        /// <summary>
        /// 带读取的卡片数据类型
        /// 1 排序卡区域 
        /// 2 非排序卡区域 
        /// 3 所有区域 
        /// </summary>
        public readonly int CardType;


        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="cardList">卡列表</param>
        /// <param name="dataBaseSize">读取到的卡数量</param>
        /// <param name="cardType">卡数据库类型</param>
        public ReadCardDataBase_Base_Result(List<T> cardList, int dataBaseSize, int cardType)
        {
            CardList = cardList;
            DataBaseSize = dataBaseSize;
            CardType = cardType;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CardList?.Clear();
            CardList = null;
        }

    }
}
