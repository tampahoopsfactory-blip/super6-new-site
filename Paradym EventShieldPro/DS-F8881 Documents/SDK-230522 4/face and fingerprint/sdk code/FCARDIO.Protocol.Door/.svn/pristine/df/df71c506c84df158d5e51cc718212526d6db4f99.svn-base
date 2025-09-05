using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabaseDetail
{
    /// <summary>
    /// 读取巡更人员信息 返回结果
    /// </summary>
    public class PatrolEmplDatabaseDetail_Result : INCommandResult
    {
        /// <summary>
        /// 数据区容量上限
        /// </summary>
        public ushort DataBaseSize;

        /// <summary>
        /// 数据区已使用数量
        /// </summary>
        public ushort PatrolEmplSize;

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="buf"></param>
        public void SetBytes(IByteBuffer buf)
        {
            DataBaseSize = buf.ReadUnsignedShort();
            PatrolEmplSize = buf.ReadUnsignedShort();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
