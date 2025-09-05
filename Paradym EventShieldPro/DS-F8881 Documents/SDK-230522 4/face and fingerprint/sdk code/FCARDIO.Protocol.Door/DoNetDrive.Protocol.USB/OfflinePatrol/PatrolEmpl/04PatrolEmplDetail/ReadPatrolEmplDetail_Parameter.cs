using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDetail
{
    /// <summary>
    /// 读取单个巡更人员资料 参数
    /// </summary>
    public class ReadPatrolEmplDetail_Parameter : AbstractParameter
    {
        /// <summary>
        /// 查询方式：1字节；01--表示按照工号来查询；02--表示按照卡号来查询
        /// </summary>
        public int Mode;

        /// <summary>
        /// 查询参数
        /// </summary>
        public int Param;

        public Data.PatrolEmpl PatrolEmpl;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public ReadPatrolEmplDetail_Parameter()
        {

        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="param"></param>
        public ReadPatrolEmplDetail_Parameter(byte mode, int param)
        {
            Mode = mode;
            Param = param;
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode != 1 || Mode != 2)
                throw new ArgumentException("Mode Error!");
            if (Param == 0 || Param > 16777215)
                throw new ArgumentException("Param Error!");
            return true;
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Mode);
            byte[] b = DoNetDrive.Common.NumUtil.Int24ToByte(Param);
            databuf.WriteBytes(b);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            PatrolEmpl = new Data.PatrolEmpl();
            PatrolEmpl.SetBytes(databuf);
        }
    }
}
