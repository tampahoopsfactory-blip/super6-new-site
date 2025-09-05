using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Common.Extensions;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor
{
    /// <summary>
    /// 搜索控制器--搜索不是指定网络标记的控制器
    /// </summary>
    public class SearchControltor : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 搜索控制器
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public SearchControltor(INCommandDetail cd, SearchControltor_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {

            Packet(0x01, 0xFE, 0, 2, GetParameter());
            var par = _Parameter as SearchControltor_Parameter;
            if(par.UDPBroadcast)
            {
                DoorPacket.SetUDPBroadcastPacket();
            }
        }

        /// <summary>
        /// 命令数据
        /// </summary>
        /// <returns></returns>
        private DotNetty.Buffers.IByteBuffer GetParameter()
        {
            SearchControltor_Parameter par = Parameter as SearchControltor_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(2);
            par.GetBytes(buf);
            return buf;
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {

            if (CheckResponse(oPck, 0x1, 0xfe, 0, 0x89))
            {
                var buf = oPck.CmdData;
                SearchControltor_Result rst = new SearchControltor_Result();
                rst.SetBytes(buf);

                rst.SNByte = oPck.SN;

                bool SNIsAscII = rst.SNIsAsciiString();
                
                if (SNIsAscII)
                {
                    rst.SN = oPck.SN.GetString();
                }
                else
                {
                    rst.SN = string.Empty;
                }
                _Result = rst;
                _Connector.fireCommandCompleteEventNotRemoveCommand(_EventArgs);
            }
        }
    }
}