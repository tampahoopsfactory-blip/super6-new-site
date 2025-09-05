using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door
{
    /// <summary>
    /// 表示门的数值详情
    /// </summary>
    /// <typeparam name="T">门数值类型</typeparam>
    public class DoorDetail<T>
    {
        private T[] Doors;

        /// <summary>
        /// 初始化门详情数组
        /// </summary>
        public DoorDetail()
        {
            Doors = new T[4];
        }

        /// <summary>
        /// 获取门数值
        /// </summary>
        /// <param name="Index">门索引号，起始序号为1，最大值为4</param>
        /// <returns></returns>
        public T GetDoor(int Index)
        {
            if (Index < 1 || Index > 4)
                throw new ArgumentException("Index Error");
            return Doors[Index-1];
        }

        /// <summary>
        /// 设置1-4门 权限
        /// </summary>
        /// <param name="Index">门索引号，起始序号为1，最大值为4</param>
        /// <param name="Value">值</param>
        public void SetDoor(int Index,T Value)
        {
            if (Index < 1 || Index > 4)
                throw new ArgumentException("Index Error");
            Doors[Index - 1] = Value;
        }
        



    }
}
