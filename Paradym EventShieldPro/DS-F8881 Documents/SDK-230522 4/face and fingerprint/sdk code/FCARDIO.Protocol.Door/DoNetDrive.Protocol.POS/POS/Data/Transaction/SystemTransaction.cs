using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Data
{
    /// <summary>
    /// 系统记录
    /// 1   系统加电--开机
    /// 2   系统错误复位（看门狗）
    /// 3   设备格式化记录
    /// 5   电压过高
    /// 6   电压过低
    /// 7   温度过高
    /// 8   温度过低
    /// 9   透水警告
    /// 10  系统待机
    /// 11  系统唤醒
    /// 12  进入管理菜单
    /// 13  拆机警告
    /// 14  网线断开
    /// 15  网线接入
    /// 16  Wifi断开
    /// 17  Wifi接入
    /// 18  更改Wifi参数
    /// 19  更改IP参数
    /// 20  系统关机
    /// </summary>
    public class SystemTransaction : AbstractTransaction
    {
        /// <summary>
        /// 备用
        /// </summary>
        public int Standby;
        /// <summary>
        /// 创建一个系统记录
        /// </summary>
        public SystemTransaction()
        {
            _TransactionType = 2;
            _IsNull = false;
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
                byte[] time = new byte[6];
                data.ReadBytes(time, 0, 6);
                _TransactionDate = new DateTime(2000 + BCDToByte(time[0]), BCDToByte(time[1]), BCDToByte(time[2]), BCDToByte(time[3]), BCDToByte(time[4]), BCDToByte(time[5]));
                //for (int i = 0; i < time.Length; i++)
                //{
                //    var b = BCDToByte(time[i]);
                //}
                //_TransactionDate = TimeUtil.BCDTimeToDate_ssmmhhddMMyy(time);

                Standby = data.ReadByte();
            }
            catch (Exception e)
            {
            }

            return;
        }

        /// <summary>
        /// BCD 转 字节
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        public static byte BCDToByte(byte iNum)
        {
            int iValue = iNum;
            iValue = ((iValue / 16) * 10) + (iValue % 16);
            return (byte)iValue;
        }


    }
}
