# FCARDIO.Protocol.Door

#### 介绍
依附于Dotnetty的新版本通讯库，这个项目是其中关于门禁板、人脸机、指纹机、巡更棒、消费机、水控机、电控机等多种设备的协议命令实现



#### 使用说明
- 人脸机通讯步骤
- 1、 初始化通讯分配器
~~~
            mAllocator = ConnectorAllocator.GetAllocator();

            mAllocator.AuthenticationErrorEvent += MAllocator_AuthenticationErrorEvent;

            mAllocator.TransactionMessage += MAllocator_TransactionMessage;

            mAllocator.ConnectorConnectedEvent += mAllocator_ConnectorConnectedEvent;
            mAllocator.ConnectorClosedEvent += mAllocator_ConnectorClosedEvent;
            mAllocator.ConnectorErrorEvent += mAllocator_ConnectorErrorEvent;

            mAllocator.ClientOnline += MAllocator_ClientOnline;
            mAllocator.ClientOffline += MAllocator_ClientOffline;
~~~
- 2、监听UDP
~~~
UDPServerDetail detail = new UDPServerDetail(sLocalIP, port);
mAllocator.OpenConnector(detail);
~~~

- 3、配置远程设备ip和SN信息获得 INCommandDetail 对象
~~~
            var cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.UDPClient, '设备IP', 设备端口号,
                CommandDetailFactory.ControllerType.Door89H, '设备SN', '设备通讯密码');

            if (connectType == CommandDetailFactory.ConnectType.UDPClient)
            {
                var dtl = cmdDtl.Connector as Core.Connector.TCPClient.TCPClientDetail;
                dtl.LocalAddr = mServerIP;//本机电脑IP
                dtl.LocalPort = mServerPort;//本地电脑监听端口
            }
~~~
- 4、 创建操作命令
~~~
     ReadSN cmd = new ReadSN(cmdDtl);
     并增加处理命令执行完毕后的委托
     cmdDtl.CommandTimeout += CmdDtl_CommandTimeout;
     cmdDtl.CommandCompleteEvent += CmdDtl_CommandCompleteEvent;
     cmdDtl.CommandErrorEvent += CmdDtl_CommandErrorEvent;
~~~

- 5、将命令加入通讯分配器队列

~~~
     mAllocator.AddCommand(cmd);
~~~