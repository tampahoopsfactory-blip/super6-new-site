using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 注册人员识别信息返回值
    /// </summary>
    public class RegisterIdentificationData_Result: RegisterIdentificationData_Parameter,INCommandResult
    {
        /// <summary>
        /// 状态代码：
        /// 1、已开始注册；2、用户号不存在；3、类型错误或不支持；4、序号已超出范围。5、设备存储空间已满
        /// 101、注册成功；102、用户取消操作 103、注册信息重复
        /// </summary>
        public int Status;

        /// <summary>
        /// 注册类型为 指纹、红外人脸、动态人脸时，返回值存储在这里
        /// 注册类型为 刷卡、密码，时，返回值存储在 PersonDetail 字段中
        /// </summary>
        public IdentificationData ResultData;

        /// <summary>
        /// 当状态为103时，此处指示，注册信息重复的用户号。
        /// </summary>
        public uint UserID ;

        /// <summary>
        /// 创建返回值
        /// </summary>

        public RegisterIdentificationData_Result(Data.Person per, int dtype,int iNum) :base(per, dtype)
        {
            DataNum = iNum;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }
    }
}
