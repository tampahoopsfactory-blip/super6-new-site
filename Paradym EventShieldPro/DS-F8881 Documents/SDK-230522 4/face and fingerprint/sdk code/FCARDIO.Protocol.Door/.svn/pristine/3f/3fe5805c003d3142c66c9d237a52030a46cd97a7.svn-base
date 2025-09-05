using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置识别结果查询二维码生成开关的参数
    /// </summary>
    public class WriteRecordQRCode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 二维码生成开关
        /// </summary>
        public bool QRCodeSwitch;

        /// <summary>
        /// 二维码中包含的网址；
        /// 示例： http://www.abc.com/FaceRecord/QueryQRcode.html；
        /// 最大120个字符。
        /// </summary>
        public string ServerURL;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteRecordQRCode_Parameter() { QRCodeSwitch = false; }

        /// <summary>
        /// 创建设置识别结果查询二维码生成开关的参数
        /// </summary>
        /// <param name="bSwitch">识别结果查询二维码生成开关</param>
        /// <param name="sURL">二维码中包含的网址</param>
        public WriteRecordQRCode_Parameter(bool bSwitch, string sURL)
        {
            QRCodeSwitch = bSwitch;
            ServerURL = sURL;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (QRCodeSwitch)
            {
                if (string.IsNullOrEmpty(ServerURL) || ServerURL.Length > 120)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 121;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(QRCodeSwitch);

            var buf1 = System.Text.Encoding.ASCII.GetBytes(ServerURL);

            databuf.WriteBytes(buf1);
            var iLen = buf1.Length;
            if (iLen != 120)
            {
                byte[] buf = new byte[120 - iLen];
                databuf.WriteBytes(buf);
            }
            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            QRCodeSwitch = databuf.ReadBoolean();
            ServerURL = string.Empty;
            if (QRCodeSwitch)
            {
                byte[] buf = new byte[120];
                databuf.ReadBytes(buf, 0, 120);
                int iLen = 0;
                for (int i = 0; i < 120; i++)
                {
                    if (buf[i] == 0)
                        break;
                    else
                        iLen++;
                }
                if (iLen > 0)
                    ServerURL = System.Text.Encoding.ASCII.GetString(buf, 0, iLen);
            }
        }
    }
}
