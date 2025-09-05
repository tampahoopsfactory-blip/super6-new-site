using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm
{
    public class SetFireAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用消防报警报警功能，如果此功能关闭则表示遇到消防报警输入端子短路时不报警
        /// </summary>
        public bool Use = true;

        /// <summary>
        /// 提供给 继承类 使用
        /// </summary>
        public SetFireAlarm_Parameter() { }

        /// <summary>
        /// 参数初始化实例
        /// </summary>
        /// <param name="use">是否启用消防报警报警功能</param>
        public SetFireAlarm_Parameter(bool use)
        {
            this.Use = use;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
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
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(Use);
            return databuf;
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
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Use = databuf.ReadBoolean();
        }

    }
}
