
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Menu
{
    public class ReadAllMenu_Result : TemplateResult_Base
    {

        public List<MenuDetail> MenuDetails { get; set; }

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadAllMenu_Result(List<MenuDetail> DataList)
        {
            this.MenuDetails = DataList;
        }

        public override void Dispose()
        {
            MenuDetails = null;
        }
    }
}
