using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System.Collections.Generic;
using System.Text;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置第三方平台推送功能的参数
    /// </summary>
    public class WriteThirdpartyAPI_Parameter : AbstractParameter
    {
        /// <summary>
        /// 回车换行符
        /// </summary>
        public static string NewLine = "\r\n";
        /// <summary>
        /// UTF8编码
        /// </summary>
        public static Encoding UTF8 = Encoding.UTF8;

        /// <summary>
        /// 第三方平台类型：
        /// 0--禁用
        /// 1--云筑网
        /// 2--安徽省阜阳市
        /// 3--湖南省
        /// </summary>
        public int PlatformType;

        public readonly Dictionary<string, string> PlatformDictionary;


        /// <summary>
        /// 构建一个空的第三方平台推送功能实例,默认禁用所有
        /// </summary>
        public WriteThirdpartyAPI_Parameter()
        {
            PlatformDictionary = new Dictionary<string, string>();
            PlatformType = 0;
        }

        /// <summary>
        /// 创建设置第三方平台推送功能的参数
        /// </summary>
        /// <param name="itype">第三方平台类型</param>
        public WriteThirdpartyAPI_Parameter(int iType, Dictionary<string, string> par)
        {
            PlatformDictionary = new Dictionary<string, string>(par);
            PlatformType = iType;
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
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            string sendStr = GetConfigString();

            return UTF8.GetByteCount(sendStr);
        }

        /// <summary>
        /// 获取键值对字符串方式的值
        /// </summary>
        /// <returns></returns>
        public string GetConfigString()
        {
            StringBuilder builder = new StringBuilder(1024);
            builder.Append("PlatformType: ").Append(PlatformType).Append(NewLine);

            if (PlatformDictionary != null)
            {
                if (PlatformDictionary.ContainsKey("PlatformType")) PlatformDictionary.Remove("PlatformType");

                foreach (KeyValuePair<string, string> pair in PlatformDictionary)
                {
                    builder.Append(pair.Key).Append(": ").Append(pair.Value).Append(NewLine);
                }
            }
            builder.Append(NewLine);
            return builder.ToString();
        }
        /// <summary>
        /// 使用键值对的字符串方式配置
        /// </summary>
        /// <param name="sConfigs"></param>
        public void SetConfigString(string sConfigs)
        {
            string[] lines = sConfigs.Split(new string[] { NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                try
                {
                    int iSplitCharIndex = line.IndexOf(':');
                    if(iSplitCharIndex> 0)
                    {

                        string key = line.Substring(0, iSplitCharIndex);
                        string value = line.Substring(iSplitCharIndex+1);
                        if (value.StartsWith(" ")) value = value.Substring(1);
                        if (key.ToLower() == "platformtype")
                        {
                            PlatformType = int.Parse(value.Trim());
                        }
                        else
                        {
                            if (PlatformDictionary.ContainsKey(key))
                            {
                                PlatformDictionary[key] = value;
                            }
                            else
                            {
                                PlatformDictionary.Add(key, value);
                            }
                        }
                    }
                   

                }
                catch (System.Exception ex)
                {

                }

            }
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
            string sendStr = GetConfigString();
            databuf.WriteString(sendStr, UTF8);

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            PlatformDictionary.Clear();
            string par = databuf.ReadString(databuf.ReadableBytes, UTF8);
            SetConfigString(par);
        }
    }
}
