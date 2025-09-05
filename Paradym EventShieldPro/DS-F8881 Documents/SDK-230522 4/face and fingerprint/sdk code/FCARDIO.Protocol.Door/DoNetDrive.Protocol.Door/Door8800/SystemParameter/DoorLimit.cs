using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter
{
    /// <summary>
    /// 门内人数限制_模型
    /// </summary>
    public class DoorLimit
    {
        /// <summary>
        /// 全局上限
        /// </summary>
        public uint GlobalLimit;

        /// <summary>
        /// 1-4号门上限数组
        /// </summary>
        public uint[] DoorLimitArray;

        /// <summary>
        /// 全局人数
        /// </summary>
        public uint GlobalEnter;

        /// <summary>
        /// 1-4号门人数数组
        /// </summary>
        public uint[] DoorEnterArray;

        /// <summary>
        /// 实例化门上限数组和门人数数组的长度
        /// </summary>
        public DoorLimit()
        {
            DoorLimitArray = new uint[4];
            DoorEnterArray = new uint[4];
        }
    }
}