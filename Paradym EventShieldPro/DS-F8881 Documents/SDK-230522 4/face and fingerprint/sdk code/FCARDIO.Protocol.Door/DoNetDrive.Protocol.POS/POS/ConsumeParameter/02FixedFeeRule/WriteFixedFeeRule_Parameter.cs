using DotNetty.Buffers;
using DoNetDrive.Protocol.POS.Data;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.FixedFeeRule
{
    /// <summary>
    /// 设置定额扣费规则命令参数
    /// </summary>
    public class WriteFixedFeeRule_Parameter : AbstractParameter
    {

        public List<FixedFeeRuleDetail> DataList;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteFixedFeeRule_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="DataList"></param>
        public WriteFixedFeeRule_Parameter(List<FixedFeeRuleDetail> DataList)
        {
            this.DataList = DataList;
            if (!checkedParameter())
            {
                throw new ArgumentException("DataList Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DataList == null || DataList.Count == 0 || DataList.Count > 8)
            {
                return false;
            }
            foreach (var item in DataList)
            {
                if (item.FixedFee < 0)
                    return false;
                if (item.ConsumptionLimits < 0)
                    return false;
                if (item.MealTimeName != null && item.MealTimeName.Length > 10)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            DataList = null;
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            foreach (var item in DataList)
            {
                databuf = item.GetBytes(databuf);
            }

            return databuf;
        }

        /// <summary>
        /// 获取写入参数长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 200;
        }

        /// <summary>
        /// 没有实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            DataList = new List<FixedFeeRuleDetail>();
            for (int i = 0; i < 8; i++)
            {
                FixedFeeRuleDetail fixedFeeRuleDetail = new FixedFeeRuleDetail();
                fixedFeeRuleDetail.SetBytes(databuf);
                DataList.Add(fixedFeeRuleDetail);
            }
        }
    }
}
