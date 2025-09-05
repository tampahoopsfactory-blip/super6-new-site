using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.ReaderOption
{

    /// <summary>
    /// 89H 控制器4个门的读卡器字节数
    /// </summary>
    public class ReaderOption_Parameter : Door8800.Door.ReaderOption.ReaderOption_Parameter
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ReaderOption_Parameter()
        {

        }
        /// <summary>
        /// 控制器4个门的读卡器字节数初始化实例
        /// </summary>
        /// <param name="_Door"></param>
        public ReaderOption_Parameter(byte[] _Door) :base(_Door)
        {

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door == null)
                throw new ArgumentException("door Is Null!");
            if (Door.Length != DataLength)
                throw new ArgumentException("door Length Error!");
            foreach (var item in Door)
            {
                if (item < 0 || item > 6)
                {
                    throw new ArgumentException("door must between 1 and 6 Error!");
                }
            }
            return true;
        }
    }
}
