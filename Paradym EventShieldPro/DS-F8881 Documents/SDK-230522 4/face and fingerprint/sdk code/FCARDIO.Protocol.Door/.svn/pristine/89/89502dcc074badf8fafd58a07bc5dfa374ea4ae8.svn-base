# DoNetDrive.Protocol.USB.CardReader

## 介绍

用于定义适用于USB发卡(502MS模块)的设备命令，含命令协议文档中的所有章节
USB设备需要安装驱动，驱动安装后再设备管理器中可看到 COM 端口号，使用此端口号进行通讯。



## 软件架构
基于 netstandard2.0 ；



## 使用说明

~~~ c#
var mAllocator = ConnectorAllocator.GetAllocator();


//导入 串口通讯库
var defFactory = mAllocator.ConnectorFactory as DefaultConnectorFactory;
defFactory.ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, DoNetDrive.Connector.COM.SerialPortFactory.GetInstance());
			
var cmdDtl = CommandDetailFactory.CreateDetail(new SerialPortDetail((byte)port, iBaudrate), 
				CommandDetailFactory.ControllerType.USBDrive_CardReader,
                 "0", null);
var cmd = new DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN.ReadSN(cmdDtl);
try
{
    await mAllocator.AddCommandAsync(cmd);
    var snResult = cmd.getResult() as DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN.SN_Result;
    Console.WriteLine(snResult.SN);

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

//释放
mAllocator.Dispose();
~~~



## 版本记录



### Ver 1.17

增加EMID卡类型



### Ver 1.18

> 更新依赖关系  
- DoNetDrive.Core Version="2.9.0"