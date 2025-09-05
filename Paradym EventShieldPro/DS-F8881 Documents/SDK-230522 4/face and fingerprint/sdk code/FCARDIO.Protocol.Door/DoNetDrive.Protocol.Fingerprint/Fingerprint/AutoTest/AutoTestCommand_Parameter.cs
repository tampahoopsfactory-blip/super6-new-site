using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AutoTest
{
    /// <summary>
    /// 自动测试参数
    /// </summary>
    public class AutoTestCommand_Parameter : AbstractParameter
    {
        /// <summary>
        /// 自动测试类型：
        /// <para>1  -- 测试屏幕 屏幕显示依次：白屏、黑屏、红屏、黄屏、蓝屏，彩虹格图 ，不同图像间的切换使用键盘按任意键或点击屏幕（触屏）</para>
        /// <para>2  -- LED指示灯测试指令：指纹机快速切换绿色LED与红色LED交换互闪 1秒钟（0.1秒切换一个颜色，直续1秒时间）</para>
        /// <para>3  -- 声音测试指令：指纹机连续发出“谢谢”、“谢谢”、“谢谢” （3次）</para>
        /// <para>4  -- 按键测试指令：屏幕显示需要测试的按键 要求必须指定按键按三次才算通过，接着要求按下一个按键，所有按键都测试过才算完成。 </para>
        /// <para>5  -- 防拆测试指令：指纹机屏幕提示防拆测试，并且等待防拆按键的按下与放开3次</para>
        /// <para>6  -- 读卡测试指令：指纹机屏幕显示请读两次卡，并且等待人为读两次卡（卡号固定为12345678）</para>
        /// <para>7  -- U盘检查测试指令：指纹机检查U盘读写正常，并自动导入u盘中的人事资料</para>
        /// <para>8  -- 按指纹测试指令：指纹机屏幕显示请按相同指纹三次，并且等待人为按下3次同一个指纹（相同用户号的人相同指纹序号验证三次）</para>
        /// <para>9  -- WG输入测试指令：指纹机收到WG输入的12345678卡号，连续正确两次</para>
        /// <para>10 -- WG输出测试指令：指纹机通过WG输出的12345678卡号，连续输出两次</para>
        /// <para>11 -- 触摸屏幕检测，显示涂色区域，要求将整个屏幕都涂抹一次才算成功</para>
        /// <para>12 -- WIFI测试（自动搜索WIFI信息号，能搜索到就算成功）</para>
        /// </summary>
        public int Mode;

        /// <summary>
        /// 使用自动测试类型初始化实例
        /// </summary>
        /// <param name="_Mode">自动测试类型</param>
        public AutoTestCommand_Parameter(int _Mode)
        {
            Mode = _Mode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode > 12 || Mode < 1)
            {
                return false;
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
        /// 对自动测试类型进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Mode);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x01;
        }

        /// <summary>
        /// 对自动测试类型进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Mode = databuf.ReadByte();
        }
    }
}
