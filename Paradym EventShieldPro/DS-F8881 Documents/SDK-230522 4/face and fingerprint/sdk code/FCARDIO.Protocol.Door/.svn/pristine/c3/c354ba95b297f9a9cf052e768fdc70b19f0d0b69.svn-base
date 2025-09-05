using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 准备写文件
    /// </summary>
    public class WriteFeatureCode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 文件类型
        /// 1 - 人员头像照片
        /// 2 - 指纹
        /// 3 - 人脸特征码
        /// 4 - 动态人脸特征码
        /// 10 - 开机图片
        /// 11 - 待机图片
        /// </summary>
        public int FileType;

        /// <summary>
        /// 序号
        /// </summary>
        public int FileNum;

        /// <summary>
        /// 如果发生照片重复消息时，是否等待重复详情，适用于人脸机固件版本4.24以上版本
        /// </summary>
        public bool WaitRepeatMessage;

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] FileDatas;

        /// <summary>
        /// 等待校验的时间，单位毫秒
        /// </summary>
        public int WaitVerifyTime;


        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="userCode">用户号</param>
        /// <param name="filetype">文件类型</param>
        /// <param name="filenum">序号</param>
        public WriteFeatureCode_Parameter(int userCode, int filetype, int filenum, byte[] datas)
        {
            UserCode = userCode;
            FileType = filetype;
            FileNum = filenum;
            FileDatas = datas;
            WaitVerifyTime = 6000;
            WaitRepeatMessage = false;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (FileType < 1 || FileType > 255)
            {
                return false;
            }
            if (FileNum < 0)
            {
                return false;
            }
            if (UserCode < 0)
            {
                return false;
            }
            if (FileDatas == null || FileDatas.Length == 0  )
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
            databuf.WriteInt(UserCode);
            databuf.WriteByte(FileType);
            databuf.WriteByte(FileNum);
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
