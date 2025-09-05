using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    /// 从控制器中读取卡片数据<br/>
    /// 成功返回结果参考 @link ReadCardDataBase_Result 
    /// </summary>
    public class ReadCardDataBase : ReadCardDataBase_Base<Data.CardDetail>
    {

        /// <summary>
        /// 初始化命令结构 
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadCardDataBase(INCommandDetail cd, ReadCardDataBase_Parameter parameter) : base(cd, parameter) { }


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
