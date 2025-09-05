using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 将人员列表写入到控制器的返回值
    /// </summary>
    public class WritePerson_Result : INCommandResult
    {
        /// <summary>
        /// 无法写入的人员数量
        /// </summary>
        public readonly int FailTotal;

        /// <summary>
        /// 无法写入的人员列表
        /// </summary>
        public List<uint> PersonList;

        /// <summary>
        /// 创建结构 
        /// </summary>
        /// <param name="personList">人员列表</param>
        public WritePerson_Result(List<uint> personList)
        {
            FailTotal = personList.Count;
            PersonList = personList;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            PersonList?.Clear();
            PersonList = null;
        }

        internal void SetBytes(IByteBuffer buf)
        {
            throw new NotImplementedException();
        }
    }
}
