using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 注册人员识别信息参数
    /// </summary>
    public class RegisterIdentificationData_Parameter : AbstractParameter
    {
        /// <summary>
        /// 需要注册识别信息的人员
        /// </summary>
        public Data.Person PersonDetail;

        /// <summary>
        /// 识别信息的类型,取值范围：1-5
        /// 1、指纹
        /// 2、红外人脸
        /// 3、动态人脸
        /// 4、刷卡
        /// 5、密码
        /// </summary>
        public int DataType;

        /// <summary>
        /// 数据序号，仅注册指纹时有效，数据类型为1时，取值范围：0-9
        /// </summary>
        public int DataNum;

        /// <summary>
        /// 创建注册人员识别信息参数
        /// </summary>
        /// <param name="per">需要注册识别信息的人员</param>
        /// <param name="dtype">识别信息的类型,取值范围：1-5</param>
        public RegisterIdentificationData_Parameter(Data.Person per, int dtype)
        {
            PersonDetail = per;
            DataType = dtype;
            if (DataType != 1) DataNum = 1;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PersonDetail == null)
            {
                throw new ArgumentException("PersonDetail Error!");
            }
            if (PersonDetail.UserCode == 0)
            {
                throw new ArgumentException("UserCode Error!");
            }
            if (DataType < 1 || DataType > 5)
            {
                throw new ArgumentException("DataType Error!");
            }
            if (DataNum < 0 || DataNum > 9)
            {
                throw new ArgumentException("DataNum Error!");
            }
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
        /// 将结构编码为 字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < 6)
            {
                throw new ArgumentException("GetBytes -- databuf Error");
            }
            databuf.WriteByte(DataType);
            databuf.WriteInt((int)PersonDetail.UserCode);
            databuf.WriteByte(DataNum);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 6;
        }

        /// <summary>
        /// 未实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
