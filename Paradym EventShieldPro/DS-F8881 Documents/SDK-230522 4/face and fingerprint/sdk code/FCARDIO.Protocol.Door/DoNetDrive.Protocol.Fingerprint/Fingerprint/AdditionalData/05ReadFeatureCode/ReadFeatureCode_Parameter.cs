using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 读取人员照片/记录照片/指纹
    /// </summary>
    public class ReadFeatureCode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 文件类型
        /// 1 - 人员头像
        /// 2 - 指纹
        /// 3 - 记录照片
        /// 4 - 红外人脸特征码
        /// 5 - 动态人脸特征码
        /// 6 - 现场视频截图
        /// </summary>
        public int Type;

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="userCode">用户号</param>
        /// <param name="type">文件类型</param>
        /// <param name="serialNumber">序号</param>
        public ReadFeatureCode_Parameter(int userCode, int type, int serialNumber)
        {
            UserCode = userCode;
            Type = type;
            SerialNumber = serialNumber;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Type < 1 || Type > 5)
            {
                return false;
            }
            if (Type == 1)
            {
                if (SerialNumber < 1 || SerialNumber > 5)
                {
                    return false;
                }
            }
            if (Type == 2)
            {
                if (SerialNumber < 0 || SerialNumber > 9)
                {
                    return false;
                }
            }
            if (Type > 2)
            {
                if (SerialNumber != 1)
                {
                    return false;
                }
            }
            if (UserCode < 0)
            {
                return false;
            }
            return true;
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
            databuf.WriteByte(Type);
            databuf.WriteByte(SerialNumber);
            databuf.WriteInt(UserCode);
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
