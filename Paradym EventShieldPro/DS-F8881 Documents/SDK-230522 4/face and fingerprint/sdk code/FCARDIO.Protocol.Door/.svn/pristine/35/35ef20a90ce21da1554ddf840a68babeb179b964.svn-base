using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Menu
{
    /// <summary>
    /// 添加菜单
    /// </summary>
    public class AddMenu : TemplateWriteData_Base<WriteMenu_Parameter, Data.MenuDetail>
    {
        /// <summary>
        /// 当前命令进度
        /// </summary>
        protected int mStep = 0;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public AddMenu(DESDriveCommandDetail cd, WriteMenu_Parameter par) : base(cd, par)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateCommandPacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WriteDataToBuf(buf);
            Packet(0x06, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataList"></param>
        /// <returns></returns>
        protected override TemplateResult_Base CreateResult(List<Data.MenuDetail> DataList)
        {
            ReadAllMenu_Result result = new ReadAllMenu_Result(DataList);
            return result;
        }


        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseCompleted(DESPacket oPck)
        {
            var subPck = oPck.CommandPacket;
            return (subPck.CmdType == 0x36 &&
                subPck.CmdIndex == 4 &&
                subPck.CmdPar == 0xff);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="data"></param>
        protected override void WriteDataBodyToBuf(IByteBuffer databuf, TemplateData_Base data)
        {
            Data.MenuDetail menuDetail = data as Data.MenuDetail;
            menuDetail.GetBytes(databuf);
        }

        public override int GetBatchCount()
        {
            return 2;
        }

        protected override void CreateCommandNextPacket(IByteBuffer buf)
        {
            Packet(0x06, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }
    }
}
