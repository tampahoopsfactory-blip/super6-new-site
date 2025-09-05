using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data
{
    /// <summary>
    /// 系统记录
    /// TransactionCode 事件代码含义表：
    /// 1   系统加电                       
    /// 2   系统错误复位（看门狗）         
    /// 3   设备格式化记录                 
    /// 4   系统高温记录，温度大于75      
    /// 5   系统UPS供电记录                
    /// 6   温度传感器损坏，温度大于100   
    /// 7   电压过低，小于09V             
    /// 8   电压过高，大于14V             
    /// 9   读卡器接反。                   
    /// 10  读卡器线路未接好。            
    /// 11  无法识别的读卡器              
    /// 12  电压恢复正常，小于14V，大于9V 
    /// 13  网线已断开                    
    /// 14  网线已插入   
    /// </summary>
    public class SystemTransaction : AbstractTransaction
    {
        /// <summary>
        /// 创建一个系统记录
        /// </summary>
        public SystemTransaction()
        {
            _TransactionType = 6;//6	系统记录
        }

        /// <summary>
        /// 指示一个事务记录所占用的缓冲区长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 8;
        }

        /// <summary>
        /// 使用缓冲区构造一个事务实例
        /// </summary>
        /// <param name="data">缓冲区</param>
        public override void SetBytes(IByteBuffer data)
        {
            try
            {
                _IsNull = CheckNull(data, 2);

                if (_IsNull)
                {
                    ReadNullRecord(data);
                    return;
                }

                _TransactionCode = data.ReadByte();
                _TransactionDate = TimeUtil.BCDTimeToDate_yyMMddhhmmss(data);
                data.ReadByte();
            }
            catch (Exception e)
            {
            }

            return;
        }
    }
}
