using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.USB.CardReader.ICCard.Sector
{
    /// <summary>
    /// 读取扇区全部内容
    /// </summary>
    public class ReadAllSector : Read_Command
    {
        ReadSector_Parameter mPar;

        List<ReadSector_Result> DataList;
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadAllSector(INCommandDetail cd, ReadSector_Parameter par) : base(cd, par)
        {
            mPar = par;
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            ReadSector_Parameter model = _Parameter as ReadSector_Parameter;
           
            DataList = new List<ReadSector_Result>();
            _ProcessMax = 64;
            
            Packet(2, 1, 0x0A, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 2, 1))
            {
                var buf = oPck.CmdData;
                ReadSector_Result rst = new ReadSector_Result();
                rst.SetBytes(buf);
                DataList.Add(rst);
                if (DataList.Count < _ProcessMax)
                {

                    mPar.MoveNext();
                    var cmdBuf = USBPacket.CmdData;
                    cmdBuf.SetByte(0, mPar.Number);
                    cmdBuf.SetByte(1, mPar.StartBlock);
                    _ProcessStep++;
                    CommandReady();
                }
                else
                {
                    ReadAllSector_Result readAllSector_Result = new ReadAllSector_Result(DataList);
                    _Result = readAllSector_Result;
                    CommandCompleted();
                }
                
            }
        }
    }
}
