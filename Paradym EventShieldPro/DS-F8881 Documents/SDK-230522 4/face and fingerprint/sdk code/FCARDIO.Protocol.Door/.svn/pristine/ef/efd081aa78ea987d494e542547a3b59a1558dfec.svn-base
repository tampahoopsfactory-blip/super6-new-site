using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Data.Transaction
{
    /// <summary>
    /// 门磁记录
    /// TransactionCode 事件代码含义表：
    /// 1  开门
    /// 2  关门 
    /// 3  进入门磁报警状态
    /// 4  退出门磁报警状态
    /// 5  门未关好
    /// 6  使用按钮开门
    /// 7  按钮开门时门已锁定
    /// 8  按钮开门时控制器已过期
    public class DoorSensorTransaction : SystemTransaction
    {


        public DoorSensorTransaction() 
        {
            _TransactionType = 2;
        }
    }
}
