using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Command;
namespace DoNetDrive.Protocol.Door.Test
{
    public interface INMain
    {
        
        void AddLog(string s);
        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="s">需要显示的日志</param>
        void AddLog(StringBuilder s);

        /// <summary>
        /// 添加命令日志
        /// </summary>
        /// <param name="e">命令描述符</param>
        /// <param name="txt">命令需要输出的内容</param>
        void AddCmdLog(CommandEventArgs e, string txt);

        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        INCommandDetail GetCommandDetail();

        /// <summary>
        /// 获取协议类型
        /// </summary>
        /// <returns></returns>
        CommandDetailFactory.ControllerType GetProtocolType();

        /// <summary>
        /// 检查协议版本是否为89H
        /// </summary>
        /// <returns></returns>
        bool CheckProtocolTypeIs89H();

        /// <summary>
        /// 将命令加入到分配器开始执行
        /// </summary>
        /// <param name="cmd"></param>
        void AddCommand(INCommand cmd);
    }
}
