using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.CardType
{
    /// <summary>
    /// 读取所有卡类
    /// </summary>
    public class ReadDataBase : TemplateReadData_Base<CardTypeDetail>
    {
        
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="detail"></param>
        public ReadDataBase(DESDriveCommandDetail detail) : base(detail) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x08, 0x03);
            mReadBuffers = new List<IByteBuffer>();
            _ProcessMax = 1;
        }

        /// 检测下一包指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseNext(DESPacket oPck)
        {
            var subPck = oPck.CommandPacket;
            return (subPck.CmdType == 0x38 &&
                subPck.CmdIndex == 3 &&
                subPck.CmdPar == 0);
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseCompleted(DESPacket oPck)
        {
            var subPck = oPck.CommandPacket;
            return (subPck.CmdType == 0x38 &&
                subPck.CmdIndex == 3 &&
                subPck.CmdPar == 0xff && subPck.DataLen == 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        protected override TemplateResult_Base CreateResult(List<CardTypeDetail> dataList)
        {
            ReadDataBase_Result result = new ReadDataBase_Result(dataList);
            return result;
        }
    }
}
