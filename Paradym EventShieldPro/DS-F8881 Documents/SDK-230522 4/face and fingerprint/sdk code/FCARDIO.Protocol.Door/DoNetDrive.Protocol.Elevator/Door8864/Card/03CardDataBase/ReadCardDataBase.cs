using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Card;
using DoNetDrive.Protocol.OnlineAccess;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardDataBase
{
    /// <summary>
    /// 从控制器中读取卡片数据
    /// </summary>
    public class ReadCardDataBase : ReadCardDataBase_Base<Data.CardDetail>
    {

        /// <summary>
        /// 初始化命令结构 
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadCardDataBase(INCommandDetail cd, ReadCardDataBase_Parameter parameter) : base(cd, parameter)
        {
            CmdType = 0x47;
            CheckResponseCmdType = 0x27;
        }

        /// <summary>
        /// 处理返回值 方便调试，可删除
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            base.CommandNext1(oPck);
        }

        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="cardList">授权卡集合</param>
        /// <param name="dataBaseSize">容量信息</param>
        /// <param name="cardType">区域代码</param>
        protected override ReadCardDataBase_Base_Result<Data.CardDetail> CreateResult(List<Data.CardDetail> cardList, int dataBaseSize, int cardType)
        {
            ReadCardDataBase_Result result = new ReadCardDataBase_Result(cardList, dataBaseSize, cardType);
            return result;
        }
    }
}
