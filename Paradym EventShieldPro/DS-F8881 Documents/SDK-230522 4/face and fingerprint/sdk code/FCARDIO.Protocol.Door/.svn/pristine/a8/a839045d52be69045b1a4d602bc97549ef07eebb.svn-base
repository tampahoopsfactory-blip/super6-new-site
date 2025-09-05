using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.ReceiptPrint
{
    public class WriteReceiptPrint_Parameter : AbstractParameter
    {
        /// <summary>
        /// 功能开关
        /// 0表示关，1表示开
        /// </summary>
        public byte IsOpen;

        /// <summary>
        /// 打印份数
        /// </summary>
        public byte PrintCount;

        public WriteReceiptPrint_Parameter()
        {

        }

        byte mCheckType = 0;

        public WriteReceiptPrint_Parameter(List<PrintContent> PrintContents)
        {
            this.PrintContents = PrintContents;
            mCheckType = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsOpen"></param>
        /// <param name="PrintCount"></param>
        public WriteReceiptPrint_Parameter(byte IsOpen, byte PrintCount)
        {
            this.IsOpen = IsOpen;
            this.PrintCount = PrintCount;
            mCheckType = 1;
        }

        public List<PrintContent> PrintContents { get; set; }

        public override bool checkedParameter()
        {
            if (mCheckType == 1)
            {
                if (IsOpen != 0 && IsOpen != 1)
                    throw new ArgumentException("IsOpen Error!");
                if (PrintCount > 5)
                    throw new ArgumentException("IsOpen Error!");
            }
            if (mCheckType == 2)
            {
                if (PrintContents == null)
                    throw new ArgumentException("PrintContents Error!");

                foreach (var item in PrintContents)
                {
                    if (item.IsOpen != 0 && item.IsOpen != 1)
                        throw new ArgumentException("item.IsOpen Error!");

                    if (item.Location != 1 && item.Location != 2)
                        throw new ArgumentException("item.Location Error!");
                }
            }
            
            return true;
        }

        public override void Dispose()
        {

        }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(IsOpen);
            databuf.WriteByte(PrintCount);
            return databuf;
        }

        public override int GetDataLen()
        {
            return 2;
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            IsOpen = databuf.ReadByte();
            PrintCount = databuf.ReadByte();
        }

        public IByteBuffer Content_GetBytes(IByteBuffer databuf)
        {
            foreach (var item in PrintContents)
            {
                databuf.WriteByte(item.Index);
                databuf.WriteByte(item.IsOpen);
                Util.StringUtil.WriteString(databuf, item.Content, 0x1E, Encoding.GetEncoding("GB2312"));
                databuf.WriteByte(item.Location);
            }

            return databuf;
        }
    }
}
