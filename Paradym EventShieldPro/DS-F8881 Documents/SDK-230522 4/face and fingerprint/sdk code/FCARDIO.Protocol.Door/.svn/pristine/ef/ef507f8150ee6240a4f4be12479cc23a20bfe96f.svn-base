using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.File
{
    public class UploadFile_Parameter : AbstractParameter
    {
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] Datas;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize;

        /// <summary>
        /// 文件类型
        /// 1、人员照片，
        /// 2、升级文件，
        /// 3、固件资源包（图标，字库等）
        /// </summary>
        public int FileType;



        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="userCode">用户号</param>
        /// <param name="type">文件类型</param>
        /// <param name="serialNumber">序号</param>
        public UploadFile_Parameter(string FileName, int FileSize, int FileType, byte[] Datas)
        {
            this.FileName = FileName;
            this.FileSize = FileSize;
            this.FileType = FileType;
            this.Datas = Datas;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (FileType < 1 || FileType > 3)
            {
                return false;
            }
            if (FileSize < 0)
            {
                return false;
            }
            if (FileType == 1)
            {
                if (FileName == null || FileName.Length > 4)
                {
                    return false;
                }
            }
            if (FileType == 2)
            {
                if (FileName == null || FileName.Length > 4)
                {
                    return false;
                }
            }
            if (Datas == null || Datas.Length != FileSize)
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
            return 13;
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
            //Util.StringUtil.WriteString(databuf, FileName, 8, Encoding.GetEncoding("GB2312"));
            if (FileType == 1)
            {
                byte[] bList = ASCIIEncoding.ASCII.GetBytes(FileName);
                databuf.WriteBytes(bList);
            }
            if (FileType == 2)
            {
                Util.StringUtil.WriteString(databuf, FileName, 8, Encoding.GetEncoding("GB2312"));
            }
            if (FileType == 3)
            {
                Util.StringUtil.WriteString(databuf, "_Packet", 8, Encoding.GetEncoding("GB2312"));
            }
            databuf.WriteInt(FileSize);
            databuf.WriteByte(FileType);
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
