using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Data.Transaction
{
    /// <summary>
    /// 系统记录
    /// 1   软件开门
    /// 2   软件关门
    /// 3   软件常开
    /// 4   控制器自动进入常开
    /// 5   控制器自动关闭门
    /// 6   长按出门按钮常开
    /// 7   长按出门按钮常闭
    /// 8   软件锁定
    /// 9   软件解除锁定
    /// 10  控制器定时锁定--到时间自动锁定
    /// 11  控制器定时锁定--到时间自动解除锁定
    /// 12  报警--锁定
    /// 13  报警--解除锁定
    /// 14  非法认证报警
    /// 15  门磁报警
    /// 16  胁迫报警
    /// 17  开门超时报警
    /// 18  黑名单报警
    /// 19  消防报警
    /// 20  防拆报警
    /// 21  非法认证报警解除
    /// 22  门磁报警解除
    /// 23  胁迫报警解除
    /// 24 开门超时报警解除
    /// 25 黑名单报警解除
    /// 26 消防报警解除
    /// 27 防拆报警解除
    /// 28 系统加电
    /// 29 系统错误复位（看门狗）
    /// 30 设备格式化记录
    /// 31 读卡器接反
    /// 32 读卡器线路未接好
    /// 33 无法识别的读卡器
    /// 34 网线已断开
    /// 35 网线已插入
    /// 36 WIFI 已连接
    /// 37 WIFI 已断开
    /// </summary>
    public class SystemTransaction : AbstractTransaction
    {
        /// <summary>
        /// 门号
        /// </summary>
        public int Door;
        /// <summary>
        /// 创建一个系统记录
        /// </summary>
        public SystemTransaction()
        {
            _TransactionType = 3;
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
                Door = data.ReadByte();
                byte[] time = new byte[6];

                data.ReadBytes(time, 0, 6);
                _TransactionDate = TimeUtil.BCDTimeToDate_ssmmhhddMMyy(time);
                _TransactionCode = data.ReadByte();
            }
            catch (Exception e)
            {
            }

            return;
        }
    }
}
