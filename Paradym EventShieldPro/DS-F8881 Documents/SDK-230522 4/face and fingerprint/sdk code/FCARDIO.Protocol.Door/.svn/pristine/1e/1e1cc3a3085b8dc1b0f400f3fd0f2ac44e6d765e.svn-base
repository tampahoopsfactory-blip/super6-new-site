
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Menu
{
    /// <summary>
    /// 删除菜单命令
    /// </summary>
    public class DeleteMenu : TemplateWriteData_Base<WriteMenu_Parameter, Data.MenuDetail>
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
        public DeleteMenu(DESDriveCommandDetail cd, WriteMenu_Parameter par) : base(cd, par)
        {
            par.mIsDeleteCommand = true;
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
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="data"></param>
        protected override void WriteDataBodyToBuf(IByteBuffer databuf, TemplateData_Base data)
        {
            Data.MenuDetail menuDetail = data as Data.MenuDetail;
            menuDetail.GetDeleteBytes(databuf);
        }

        public override int GetBatchCount()
        {
            return 40;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateCommandPacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WriteDataToBuf(buf);
            Packet(0x06, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
        }

        protected override bool CheckResponseCompleted(DESPacket oPck)
        {
            return false;
        }

        protected override void CreateCommandNextPacket(IByteBuffer buf)
        {
            Packet(0x06, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
        }
    }
}
