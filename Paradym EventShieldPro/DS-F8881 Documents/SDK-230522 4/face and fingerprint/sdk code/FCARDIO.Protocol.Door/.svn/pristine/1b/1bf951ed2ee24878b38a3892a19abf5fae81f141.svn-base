
using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.Data
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuDetail : TemplateData_Base
    {
        /// <summary>
        /// 商品代码
        /// </summary>
        public int MenuCode { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string MenuBarCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal MenuPrice { get; set; }

        public override void SetBytes(IByteBuffer databuf)
        {
            MenuCode = databuf.ReadInt();
            MenuBarCode = Util.StringUtil.GetString(databuf, 40, Encoding.BigEndianUnicode);
            MenuName = Util.StringUtil.GetString(databuf, 16, Encoding.GetEncoding("GB2312"));
            MenuPrice = (decimal)databuf.ReadInt() / (decimal)100;
        }


        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteInt(MenuCode);
            Util.StringUtil.WriteString(databuf, MenuBarCode, 40, Encoding.GetEncoding("GB2312"));
            Util.StringUtil.WriteString(databuf, MenuName, 16, Encoding.GetEncoding("GB2312"));
            databuf.WriteInt((int)(MenuPrice * 100));
            return databuf;
        }

        /// <summary>
        /// 获取每个添加卡类长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 64;
        }

        public override IByteBuffer GetDeleteBytes(IByteBuffer databuf)
        {
            return databuf.WriteInt(MenuCode);
        }

        public override int GetDeleteDataLen()
        {
            throw new NotImplementedException();
        }

        public override void SetFailBytes(IByteBuffer databuf)
        {
            MenuCode = databuf.ReadInt();
        }
    }
}
