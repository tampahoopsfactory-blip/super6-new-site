
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Menu
{
    /// <summary>
    /// 添加菜单命令参数
    /// </summary>
    public class WriteMenu_Parameter : TemplateParameter_Base<MenuDetail>
    {
        public WriteMenu_Parameter()
        {

        }

        public WriteMenu_Parameter(List<MenuDetail> list) : base(list)
        {
        }

        protected override bool CheckedParameterItem(MenuDetail Menu)
        {
            if (Menu.MenuPrice < 0 || Menu.MenuPrice > 21474836)
            {
                return false;
            }
            if (Menu.MenuCode < 0 || Menu.MenuCode > int.MaxValue)
            {
                return false;
            }
            if (Menu.MenuName == null || Menu.MenuName.Length > 16)
            {
                return false;
            }
            if (Menu.MenuBarCode == null || Menu.MenuBarCode.Length > 40)
            {
                return false;
            }
            return true;
        }

        protected override bool CheckedDeleteParameterItem(MenuDetail Menu)
        {
            if (Menu.MenuCode < 0 || Menu.MenuCode > int.MaxValue)
            {
                return false;
            }
            return true;
        }

        public override int GetDataLen()
        {
            return 0x40;
        }

        public override int GetDeleteDataLen()
        {
            return 4;
        }

        
    }
}
