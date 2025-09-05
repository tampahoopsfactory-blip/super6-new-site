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
    /// 
    /// </summary>
    public class DeleteFeatureCode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 人员头像列表
        /// 表示5个头像序号的使用情况
        /// 第一个字节表示序号为1的头像
        /// 每个字节值1表示删除，0表示跳过
        /// </summary>
        public byte[] PhotoList;

        /// <summary>
        /// 指纹列表
        /// 表示10个指纹序号的使用情况
        /// 第一个字节表示序号为0的指纹
        /// 每个字节值1表示删除，0表示跳过
        /// </summary>
        public byte[] FingerprintList;

        /// <summary>
        /// 人脸特征码
        /// </summary>
        public bool DelFace;


        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="userCode">UserCode</param>
        /// <param name="photoList">人员头像列表</param>
        /// <param name="fingerprintList">指纹列表</param>
        /// <param name="DelFace">人脸特征码</param>
        public DeleteFeatureCode_Parameter(int userCode, byte[] photoList, byte[] fingerprintList, bool delFace)
        {
            UserCode = userCode;
            PhotoList = photoList;
            FingerprintList = fingerprintList;
            DelFace = delFace;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PhotoList == null || PhotoList.Length != 5)
            {
                return false;
            }
            if (FingerprintList == null || FingerprintList.Length != 10)
            {
                return false;
            }
            if (PhotoList.Any(t=> t > 1))
            {
                return false;
            }
            if (FingerprintList.Any(t => t > 1))
            {
                return false;
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
            return 20;
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
            databuf.WriteInt(UserCode);
            databuf.WriteBytes(PhotoList);
            databuf.WriteBytes(FingerprintList);
            databuf.WriteBoolean(DelFace);
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
