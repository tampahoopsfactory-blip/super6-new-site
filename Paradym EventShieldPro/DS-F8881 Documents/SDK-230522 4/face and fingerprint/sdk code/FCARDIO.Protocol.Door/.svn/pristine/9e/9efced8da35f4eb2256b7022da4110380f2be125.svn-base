using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Voice
{
    public class WriteVoice_Parameter : AbstractParameter
    {
        /// <summary>
        /// 功能开关
        /// </summary>
        public byte IsOpen;

        /// <summary>
        /// 卡内余额
        /// </summary>
        public byte CardMoney;

        /// <summary>
        /// 消费金额
        /// </summary>
        public byte PayMoney;

        /// <summary>
        /// 黑名单
        /// </summary>
        public byte BlackList;

        /// <summary>
        /// 错误提示
        /// </summary>
        public byte ErrorTip;

        /// <summary>
        /// 刷卡或密码提示
        /// </summary>
        public byte PasswordTip;

        public WriteVoice_Parameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsOpen"></param>
        /// <param name="CardMoney"></param>
        /// <param name="PayMoney"></param>
        /// <param name="BlackList"></param>
        /// <param name="ErrorTip"></param>
        /// <param name="PasswordTip"></param>
        public WriteVoice_Parameter(byte IsOpen, byte CardMoney, byte PayMoney, byte BlackList, byte ErrorTip, byte PasswordTip)
        {
            this.IsOpen = IsOpen;
            this.CardMoney = CardMoney;
            this.PayMoney = PayMoney;
            this.BlackList = BlackList;
            this.ErrorTip = ErrorTip;
            this.PasswordTip = PasswordTip;

            if (!checkedParameter())
            {
                throw new ArgumentException("Parameter Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (IsOpen != 0 && IsOpen != 1)
                throw new ArgumentException("IsOpen Error!");
            if (CardMoney != 0 && CardMoney != 1)
                throw new ArgumentException("CardMoney Error!");
            if (PayMoney != 0 && PayMoney != 1)
                throw new ArgumentException("PayMoney Error!");
            if (BlackList != 0 && BlackList != 1)
                throw new ArgumentException("BlackList Error!");
            if (ErrorTip != 0 && ErrorTip != 1)
                throw new ArgumentException("ErrorTip Error!");
            if (PasswordTip != 0 && PasswordTip != 1)
                throw new ArgumentException("PasswordTip Error!");
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对有效期参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            databuf.WriteByte(IsOpen);
            databuf.WriteByte(CardMoney);
            databuf.WriteByte(PayMoney);
            databuf.WriteByte(BlackList);
            databuf.WriteByte(ErrorTip);
            databuf.WriteByte(PasswordTip);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 6;
        }

        /// <summary>
        /// 对有效期参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            IsOpen = databuf.ReadByte();
            CardMoney = databuf.ReadByte();
            PayMoney = databuf.ReadByte();
            BlackList = databuf.ReadByte();
            ErrorTip = databuf.ReadByte();
            PasswordTip = databuf.ReadByte();
        }
    }
}
