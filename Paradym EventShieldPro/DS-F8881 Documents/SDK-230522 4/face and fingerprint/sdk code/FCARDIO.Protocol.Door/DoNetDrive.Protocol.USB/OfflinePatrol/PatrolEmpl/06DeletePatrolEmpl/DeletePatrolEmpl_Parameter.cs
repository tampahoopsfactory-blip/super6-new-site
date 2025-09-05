using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.DeletePatrolEmpl
{
    /// <summary>
    /// 删除巡更人员 参数
    /// </summary>
    public class DeletePatrolEmpl_Parameter : AbstractParameter
    {

        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected int MaxBufSize = 350;


        /// <summary>
        /// 被删除巡更人员列表
        /// </summary>
        public List<ushort> PCodeList;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public DeletePatrolEmpl_Parameter(List<ushort> list)
        {
            PCodeList = list;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PCodeList == null || PCodeList.Count == 0)
                throw new ArgumentException("PCodeList Error!");
            foreach (var pcode in PCodeList)
            {
                if (pcode < 1 || pcode > 999)
                    throw new ArgumentException("PCode Error!");
            }
            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
