using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Menu
{
    public class ReadMenuDetail_Result : INCommandResult
    {
        /// <summary>
        /// 菜单是否存在
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// 菜单的详情
        /// </summary>
        public Data.MenuDetail MenuDetail;


        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="isReady">菜单是否存在</param>
        /// <param name="MenuDetail">MenuDetail 保存菜单详情的实体</param>
        public ReadMenuDetail_Result(bool isReady, Data.MenuDetail MenuDetail)
        {
            IsReady = isReady;
            this.MenuDetail = MenuDetail;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            MenuDetail = null;
        }
    }
}
