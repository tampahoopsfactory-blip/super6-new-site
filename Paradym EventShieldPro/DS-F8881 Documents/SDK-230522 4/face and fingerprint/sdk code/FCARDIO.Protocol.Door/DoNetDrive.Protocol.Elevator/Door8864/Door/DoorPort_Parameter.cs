using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door
{
    /// <summary>
    /// 门号参数，取值范围 1-65
    /// </summary>
    public class DoorPort_Parameter : Protocol.Door.Door8800.Door.DoorPort_Parameter
    {
        /// <summary>
        ///  门索引号
        ///  取值范围 1-65
        /// </summary>
        //public int Door;

        /// <summary>
        /// 门号参数初始化实例
        /// </summary>
        /// <param name="iDoor"></param>
        public DoorPort_Parameter(int iDoor):base(iDoor)
        {
            Door = iDoor;
            checkedParameter();
        }


        /// <summary>
        /// 检查参数的统一接口
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 65)
                throw new ArgumentException("Door Error!");
            return true;
        }


    }
}