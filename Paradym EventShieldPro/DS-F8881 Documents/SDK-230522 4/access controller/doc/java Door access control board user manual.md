# Instructions for Secondary Development of Access Control Board

## 1. Overview
### 1.1 Introduction
Secondary development is mainly for the interaction between third-party platforms and access control boards. There are two communication methods for commands: TCP and UDP. TCP is the main communication method, while UDP is used for searching devices and modifying the TCP/IP parameters of access control boards.

### 1.2 Basic Terminology


The Communication Connector (ConnectorAllocator) is responsible for sending all commands.

The Communication Connector Event Notification (INConnectorEvent) is responsible for returning the execution results and message notifications of all commands.

The Connector Detail (CommandDetail) includes necessary information for command execution, such as the communication channel for command execution, command authentication information, user additional data, and timeout retry parameters.

### 1.3 Command Invocation Method
#### First, declare the connector method (TCP or UDP)
Declaration of TCP Connector Method

```java
TCPClientDetail tcpClient = new TCPClientDetail("192.168.1.150", 8000);
tcpClient.Timeout = 5000;//connect timeout（ms）
tcpClient.RestartCount = 0;//the time of reconnect
```

"192.168.1.150" is the IP address of the access control board, and 8000 is the TCP port of the access control board.

**Declaration of UDP Connector Method**

```java
UDPDetail udpClient = new UDPDetail("255.255.255.255", 8101, "192.168.1.140", 8100);
udpClient.Timeout = 5000;//连接超时时间（毫秒）
udpClient.RestartCount = 0;//重新连接次数
```

"255.255.255.255" is the fixed address used for searching access control boards. If you need to write TCP/IP parameters, you need to fill in the IP address "192.168.1.150".

Port 8101 is the UDP port of the access control board used mainly for device searching. It is recommended not to change it, otherwise it will be impossible to connect to the device without knowing the IP address.

#### Declaration of the identity information of the control board.

```java
Door8800Identity idt = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8900);
```


"FC-8940H47124309" is the device SN, which is a 16-digit string. The first 8 digits represent the device model (FC-8940H), and the last 8 digits represent the device serial number (47124309). When obtaining the device SN, you can fill it with 16 zeros, i.e., "0000000000000000".

"FFFFFFFF" is the device communication password, which is default to 8 "F"s. The password is 8 digits long, and if the length is insufficient, it will be padded with zeros on the left.

E_ControllerType is the type of the control board, which is divided into advanced board and regular board. For advanced board, choose Door8900, and for regular board, choose Door8800.

#### Creating a (CommandDetail) command detail object.

```java
CommandDetail commandDetail = new CommandDetail();
commandDetail.Connector = tcpClient;
commandDetail.Identity = idt;
```

#### Complete calling example:

```java
public class Test implements INConnectorEvent { //实现通讯连接器事件通知接口
	//声明通讯连接器
	private ConnectorAllocator _Allocator;
	public Test() {
	  	//在构造方法中获取实例（单例）
        _Allocator = ConnectorAllocator.GetAllocator();
        //添加事件通知
        _Allocator.AddListener(this);
    }
    /**
     * Get Command Detail Object
     */
    public CommandDetail getCommandDetail() {
        TCPClientDetail tcpClient = new TCPClientDetail("192.168.1.150", 8000);
        tcpClient.Timeout = 5000;//连接超时时间（毫秒）
        tcpClient.RestartCount = 0;//重新连接次数		
        Door8800Identity idt = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8900);
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Connector = tcpClient;
        commandDetail.Identity = idt;
        return  commandDetail;
    }
    public void openDoor() {
       CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
       OpenDoor_Parameter parameter = new OpenDoor_Parameter(new CommandParameter(commandDetail)); //声明远程开门命令参数对象
        //设置开门参数 1-4 是门号，1是开门 0是不开门
        parameter.Door.SetDoor(1, 1);
        parameter.Door.SetDoor(2, 1);
        parameter.Door.SetDoor(3, 1);
        parameter.Door.SetDoor(4, 0);
        OpenDoor cmd = new OpenDoor(parameter);
         //Add Command to Communication Connector Queue
        _Allocator.AddCommand(cmd);
    }
    /**
 	* 命令执行 succ监听事件
 	* @param cmd  当前执行的命令
 	* @param result 执行命令的Return Result，部分命令是没有Return Result的，返回到此监听事件即为命令 succ。
 	*/
     @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
         if (cmd instanceof OpenDoor) {
              System.out.println("开门 succ");
         }
    }
    /**
     * 当前执行的命令执行进度
     * @param cmd 当前执行的命令
     */
     @Override
    public void CommandProcessEvent(INCommand cmd) {
    	 System.out.println("当前命令:"+cmd.getClass().toString()+",当前进度:"+cmd.getProcessStep()+"/"+cmd.getProcessMax() );
        //当前命令:OpenDoor,当前进度:1/1
    }
    /**
     * 连接失败监听事件
     * @param cmd 当前执行的命令
     * @param isStop  是否是手动停止
     */
     @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        String sCmd=cmd.getClass().toString();
         if (isStop) {
                System.out.println(sCmd+"命令已手动停止!");
            } else {
                System.out.println(sCmd+"网络连接失败!");
            }
        
    }
    /**
     * 连接失败监听事件
     * @param detial 连接器详情对象
     */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail detial) {
        try {         
           System.out.println("网络通道故障:");
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.ConnectorErrorEvent() -- " + e.toString());
        }
    }
      /**
     * 命令超时监听事件
     * @param cmd 当前执行的命令
     */
    @Override
    public void CommandTimeout(INCommand cmd) {          
        System.out.println("命令超时:"+cmd.getClass().toString());
    }
     /**
     * 通讯密码错误监听事件
     * @param cmd 当前执行的命令
     */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {
         System.out.println("通讯密码错误，已失败！");
    }
     /**
     * 校验和错误监听事件
     * @param cmd 当前执行的命令
     */
     @Override
    public void ChecksumErrorEvent(INCommand cmd) {
         System.out.println("命令返回的校验和错误，已失败！");
    }
        /**
     * 数据监控事件
     * @param detial 连接器详情对象
     * @param event  门禁控制板推送过来的数据
     */
    @Override
    public void WatchEvent(ConnectorDetail detial, INData event) {
        try {
            /*
            包含 出入记录、alarm记录、软件开门消息、门磁消息等
            */
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("数据监控:");
            if (event instanceof Door8800WatchTransaction) {
                Door8800WatchTransaction WatchTransaction = (Door8800WatchTransaction) event;
                strBuf.append("，SN：");
                strBuf.append(WatchTransaction.SN);
                strBuf.append("\n");
            } else {
                strBuf.append("，未知事件：");
                strBuf.append(event.getClass().getName());
            }
            System.out.println(strBuf);
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.WatchEvent() -- " + e.toString());
        }
    }
    /**
     * 设备上线（当设备以客户端的方式连接时才会Trigger ）
     * @param client 连接器详情对象
     */
     @Override
    public void ClientOnline(ConnectorDetail client) {
		//需要将ConnectorDetail 类转为TCPClientDetail或者UDPDetail 具体子类对象
    }
    /**
     * 设备离线（当设备以客户端的方式连接时才会Trigger ）
     * @param client  连接器详情对象
     */
    @Override
    public void ClientOffline(ConnectorDetail client) {
        //需要将ConnectorDetail 类转为TCPClientDetail或者UDPDetail 具体子类对象
    }
    
}
```

## 2. Command Details

### 1.System Settings

#### 1.1 Read Device SN

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object，控制器身份填写为16个0 即“0000000000000000”
 ReadSN cmd = new ReadSN(new CommandParameter(commandDetail));//Read 设备SN命令对象
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadSN) {
        	ReadSN_Result snResult = (ReadSN_Result) result;
        	System.out.println("Read SN succ：" + snResult.SN);
        }
    }
```



#### 1.2 Read TCP Parameters

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadTCPSetting cmd = new ReadTCPSetting(new CommandParameter(commandDetail));//Read TCP/IP参数对象
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadSN) {
        	ReadTCPSetting_Result setting = (ReadTCPSetting_Result) result;
            TCPDetail tcp = setting.TCP;
        	System.out.println("控制器MAC地址：" + tcp.GetMAC());
            System.out.println("控制器的IP地址：" + tcp.GetIP());
            System.out.println("子网掩码：" + tcp.GetIPMask());
            System.out.println("网关：" + tcp.GetIPGateway());
            System.out.println("DNS服务器IP：" + tcp.GetDNS());
            System.out.println("自动获得IP：" + tcp.GetAutoIP());//固定只能是静态IP，不可以自动获取
            System.out.println("控制器使用的TCP端口：" + tcp.GetTCPPort());
            System.out.println("控制器使用的UDP端口：" + tcp.GetUDPPort());
            //下面个功能58系列控制板不适用无法作为客户端进行访问服务器
            System.out.println("控制器作为客户端时，目标服务器的端口：" + tcp.GetServerPort());
            System.out.println("控制器作为客户端时，目标服务器的IP,IPv4版本：" + tcp.GetServerIP());
            System.out.println("控制器作为客户端时，目标服务器的域名：" + tcp.GetServerAddr());
        }
    }
```

####1.3 Write TCP Parameters

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 TCPDetail tcp = getTCPDetail();//Write 的tcp 参数需要先Read ，避免参数设置有误
 tcp.SetMAC("00-00-00-00-00-00");//不建议修改
 tcp.SetIP("192.168.1.150");//设置IP地址
 tcp.SetIPMask("255.255.255.0");//设置子网掩码
 tcp.SetIPGateway("192.168.1.1");//设置网关
 tcp.SetAutoIP(false);//自动获得IP.功能未启用，无法设置true
 tcp.SetDNS("233.5.5.5");//设置DNS
 tcp.SetTCPPort(8000);//设置TCP端口（设备命令通讯端口）
 tcp.SetUDPPort(8101);//设置UDP端口（设备搜索端口），不建议修改，如果修改之后忘记了，那将无法搜索到设备
 tcp.SetServerPort(9000);//服务器端口.控制器作为客户端时，目标服务器的端口
 tcp.SetServerIP("192.168.1.30");//服务器IP.控制器作为客户端时，目标服务器的IP,IPv4版本
 tcp.SetServerAddr("www.123.cn");//（tcp.SetServerAddr("192.168.1.30");）服务器的域名（也可以填写IP地址）.控制器作为客户端时，目标服务器的域名(IP地址和域名填写其中一个即可)
 WriteTCPSetting_Parameter parameter = new WriteTCPSetting_Parameter(commandDetail, detail);
 WriteTCPSetting cmd = new WriteTCPSetting(parameter);
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteTCPSetting) {        	
        	System.out.println("Write TCP参数 succ！");
        }
    }
```

#### 1.4 Get Device Firmware Version

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadVersion cmd = new ReadVersion(new CommandParameter(commandDetail));//Read 设备版本号“Ver 07.50”
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadVersion) {    
        	ReadVersion_Result versionResult = (ReadVersion_Result)  result;
        	System.out.println("设备版本号："+versionResult.Version);
        }
    }
```

#### 1.5 Get Device Running Information

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadSystemStatus cmd = new ReadSystemStatus(new CommandParameter(commandDetail));//Read 设备运行信息
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadSystemStatus) {    
        	ReadSystemStatus_Result systemStatus = (ReadSystemStatus_Result)  result;
        	System.out.println("系统运行天数："+systemStatus.RunDay);
            System.out.println("格式化次数："+systemStatus.FormatCount);
            System.out.println("UPS供电status："+systemStatus.UPS);// 0--表示电源取电；1--表示UPS供电
            System.out.println("系统温度："+systemStatus.Temperature);
            System.out.println("上电时间："+systemStatus.StartTime);
            System.out.println("电压："+systemStatus.Voltage);
        }
    }
```

#### 1.6 Read All System Parameters

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadAllSystemSetting cmd = new ReadAllSystemSetting(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadAllSystemSetting) {    
        	ReadAllSystemSetting_Result allSystemSetting = (ReadAllSystemSetting_Result)  result;
        	System.out.println("记录存储方式："+allSystemSetting.RecordMode);//0--是满循环，1--表示满不循环
            System.out.println("读卡器密码键盘启用功能开关："+allSystemSetting.Keyboard);//0-7bit 代表读头的序号 例如 0 是 1号读头
           //门互锁功能开关
            System.out.println("1号门互锁功能："+allSystemSetting.LockInteraction.GetDoor(1));
            System.out.println("2号门互锁功能："+allSystemSetting.LockInteraction.GetDoor(2));
            System.out.println("3号门互锁功能："+allSystemSetting.LockInteraction.GetDoor(3));
            System.out.println("4号门互锁功能："+allSystemSetting.LockInteraction.GetDoor(4));
            //Fire Alarm功能参数
            //0--不启用
            //1--alarm输出，并开所有门，只能软件Release 
            //2--alarm输出，不开所有门，只能软件Release 
            //3--有信号alarm并开门，无信号Release alarm并关门
            //4--有alarm信号时开一次门，就像按钮开门一样
            System.out.println("Fire Alarm功能参数："+allSystemSetting.FireAlarmOption);
            //匪警alarm功能参数
            //0--关闭此功能
            //1--所有门锁定，alarm输出，蜂鸣器不响。不开门，刷卡不能Release ，软件Release ，Release alarm后门的锁定也解锁了。
            //2--alarm输出，不锁定，蜂鸣器响。不开门，刷卡可以Release ，软件可以Release 
            //3--按alarm按钮就alarm，门锁定，并输出，不按时就停止。不开门，按钮停止时就Release ，软件或刷卡不能Release 。按alarm按钮的时候门是处于锁定status的，不按时Release 锁定status。
            System.out.println("匪警alarm功能参数："+allSystemSetting.OpenAlarmOption);
            
            System.out.println("Card Reading Interval Time："+allSystemSetting.ReaderIntervalTime);//最大65535秒。0表示无限制
            //语音播报开关
            //范围 1-80,每个开关true 表示启用，false 表示禁用
            //1-80 含义请查看《语音表》
            System.out.println("语音播报开关："+allSystemSetting.SpeakOpen.GetValue(1));
            System.out.println("读卡器校验："+allSystemSetting.ReaderCheckMode);//0不启用，1启用，2启用校验，但不提示非法数据或线路异常
            System.out.println("主板蜂鸣器："+allSystemSetting.BuzzerMode);//0不启用，1启用
            //Smoke Alarm功能参数
            //0--关闭此功能（默认）
            //1--驱动[Smoke Alarm继电器]，(信号有，就驱动的，信号无，就关闭)
            //2--驱动Smoke Alarm继电器并驱动所有门继电器，主板alarm提示音响(开启后由软件关闭，或重启。)
            //3--驱动Smoke Alarm继电器并锁定所有门，主板alarm提示音响(开启后由软件关闭，或重启。)
            System.out.println("Smoke Alarm功能参数："+allSystemSetting.SmogAlarmOption);
            
            //Limit of People Inside the Door
            System.out.println("Limit of People Inside the Door总人数："+allSystemSetting.EnterDoorLimit.GlobalLimit);
            System.out.println("1号门Limit of People Inside the Door人数："+allSystemSetting.EnterDoorLimit.DoorLimit[0]);
            System.out.println("2号门Limit of People Inside the Door人数："+allSystemSetting.EnterDoorLimit.DoorLimit[1]);
            System.out.println("3号门Limit of People Inside the Door人数："+allSystemSetting.EnterDoorLimit.DoorLimit[2]);
            System.out.println("4号门Limit of People Inside the Door人数："+allSystemSetting.EnterDoorLimit.DoorLimit[3]);
            
            System.out.println("进入门内人数总数："+allSystemSetting.EnterDoorLimit.GlobalEnter);
            System.out.println("1号门进入门内人数总数："+allSystemSetting.EnterDoorLimit.DoorEnter[0]);
            System.out.println("2号门进入门内人数总数："+allSystemSetting.EnterDoorLimit.DoorEnter[1]);
            System.out.println("3号门进入门内人数总数："+allSystemSetting.EnterDoorLimit.DoorEnter[2]);
            System.out.println("4号门进入门内人数总数："+allSystemSetting.EnterDoorLimit.DoorEnter[3]);
            //防盗主机
            System.out.println("防盗主机--启用："+allSystemSetting.TheftAlarmPar.Use);//0--不启用；1--启用
            System.out.println("防盗主机--进入延迟："+allSystemSetting.TheftAlarmPar.InTime);//单位：秒，取值：1-255
            System.out.println("防盗主机--退出延迟："+allSystemSetting.TheftAlarmPar.OutTime);//单位：秒，取值：1-255
            //布防密码/撤防密码 8个数字。空补F。例如密码：23412；表达为：0xFFF23412
            System.out.println("防盗主机--布防密码："+allSystemSetting.TheftAlarmPar.BeginPassword);
            System.out.println("防盗主机--撤防密码："+allSystemSetting.TheftAlarmPar.ClosePassword);
            System.out.println("防盗主机--alarm时长："+allSystemSetting.TheftAlarmPar.AlarmTime);//单位：秒 ,取值：0-65535
            
            System.out.println("防潜回功能参数："+allSystemSetting.CheckInOut);//01--单独每个门检测防潜回；02--整个控制器统一防潜回
            System.out.println("Card Expiration Reminder："+allSystemSetting.CardPeriodSpeak);//0不启用，1启用
            
            System.out.println("定时播报--启用："+allSystemSetting.ReadCardSpeak.Use);//0--不启用；1--启用
            System.out.println("定时播报--消息编号："+allSystemSetting.ReadCardSpeak.MsgIndex);//1--交房租; 2--交管理费;
            System.out.println("定时播报--起始时段："+allSystemSetting.ReadCardSpeak.BeginDate);//年月日时 BCD码，例：0x11120115，表示11年12月1日15点 最大不得超过2099年
            System.out.println("定时播报--结束时段："+allSystemSetting.ReadCardSpeak.EndDate);//最大不得超过2099年
        }
    }
```

##### 1.6.1 Record Storage Method

**Read Record Storage Method**

```java

 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadRecordMode cmd = new ReadRecordMode(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadRecordMode) {    
        	ReadRecordMode_Result mode = (ReadRecordMode_Result)  result;
        	System.out.println("Read 记录存储方式："+mode.RecordMode);//0--是满循环，1--表示满不循环
        }
    }
```

**Write Record Storage Method**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteRecordMode_Parameter parameter = new WriteRecordMode_Parameter(commandDetail);
 parameter.Mode = 0;//0--是满循环，1--表示满不循环
 WriteRecordMode cmd = new WriteRecordMode(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteRecordMode) {           
        	System.out.println("Write 记录存储方式 succ");
        }
    }
```

##### 1.6.2 Keyboard Parameters

**Read Keyboard Parameters**

```java
    //读卡器密码键盘启用功能开关 
    CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
    ReadKeyboard cmd = new ReadKeyboard(new CommandParameter(commandDetail));
    //Add Command to Communication Connector Queue
    _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadKeyboard) {    
        	ReadKeyboard_Result ret = (ReadKeyboard_Result)  result;
        	//转为bit位
        	byte[] Keyboards = new byte[8];
            for (int i = 7; i >= 0; i--)
            { 
                Keyboards[i] = (byte)(ret.Keyboard & 1);
                ret.Keyboard >>= 1;
            }
           System.out.println("1号读头功能开关："+Keyboards[0]);//1--开启 0--关闭
           System.out.println("2号读头功能开关："+Keyboards[1]);
           System.out.println("3号读头功能开关："+Keyboards[2]);
           System.out.println("4号读头功能开关："+Keyboards[3]);
           System.out.println("5号读头功能开关："+Keyboards[4]);
           System.out.println("6号读头功能开关："+Keyboards[5]);
           System.out.println("7号读头功能开关："+Keyboards[6]);
           System.out.println("8号读头功能开关："+Keyboards[7]);
        }
    }
```

**Write Keyboard Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteKeyboard_Parameter parameter = new WriteKeyboard_Parameter(commandDetail);
 parameter.Keyboard =(short) Integer.parseInt("10000000",2)//1表示开，0表示关
 WriteKeyboard cmd = new WriteKeyboard(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteKeyboard) {    
           System.out.println("Write 键盘参数 succ");
        }
    }
```

##### 1.6.3 Interlock Parameters

**Read Interlock Parameters**

```

 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadLockInteraction cmd = new ReadLockInteraction(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadLockInteraction) {    
           System.out.println("Read Interlock Parameters succ");
           ReadLockInteraction_Result lockInteraction = (ReadLockInteraction_Result) result;
           System.out.println("1号码互锁status："+lockInteraction.DoorPort.GetDoor(1));//1--已启用互锁功能，0--不启用互锁功能
           System.out.println("2号码互锁status："+lockInteraction.DoorPort.GetDoor(2));
           System.out.println("3号码互锁status："+lockInteraction.DoorPort.GetDoor(3));
           System.out.println("4号码互锁status："+lockInteraction.DoorPort.GetDoor(4));
        }
    }
```

**Write Interlock Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteLockInteraction_Parameter parameter = new WriteLockInteraction_Parameter(commandDetail);
 parameter.DoorPort.SetDoor(1,1);//1--已启用互锁功能，0--不启用互锁功能
 parameter.DoorPort.SetDoor(2,0);
 parameter.DoorPort.SetDoor(3,0);
 parameter.DoorPort.SetDoor(4,0);
 WriteLockInteraction cmd = new WriteLockInteraction(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteLockInteraction) {    
           System.out.println("Write Interlock Parameters succ");
        }
    }
```

##### 1.6.4 Fire Alarm Parameters

**Read Fire Alarm Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadFireAlarmOption cmd = new ReadFireAlarmOption(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result;
                                   ) {
        if (cmd instanceof ReadFireAlarmOption) {    
           System.out.println("Read Fire Alarm Parameters succ");
           ReadFireAlarmOption_Result fireAlarmOption = (ReadFireAlarmOption_Result) result;
           //0--不启用
           //1--alarm输出，并开所有门，只能软件Release 
           //2--alarm输出，不开所有门，只能软件Release 
           //3--有信号alarm并开门，无信号Release alarm并关门
           //4--有alarm信号时开一次门，就像按钮开门一样
           System.out.println("Fire Alarm功能参数:"+fireAlarmOption.Option);
        }
    }
```

**Write Fire Alarm Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteFireAlarmOption_Parameter parameter = new WriteFireAlarmOption_Parameter(commandDetail);
 parameter.Option = 0;
 WriteFireAlarmOption cmd = new WriteFireAlarmOption(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteFireAlarmOption) {    
           System.out.println("Write Fire Alarm Parameters succ");
        }
    }
```

##### 1.6.5 Anti-Police Alarm Parameters

**Read Anti-Police Alarm Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadOpenAlarmOption cmd = new ReadOpenAlarmOption(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteFireAlarmOption) {    
           System.out.println("Read Anti-Police Alarm Parameters succ");
           ReadOpenAlarmOption_Result openAlarmOption = (ReadOpenAlarmOption_Result) result;
           //0--关闭此功能
           //1--所有门锁定，alarm输出，蜂鸣器不响。不开门，刷卡不能Release ，软件Release ，Release alarm后门的锁定也解锁了。
           //2--alarm输出，不锁定，蜂鸣器响。不开门，刷卡可以Release ，软件可以Release 
           //3--按alarm按钮就alarm，门锁定，并输出，不按时就停止。不开门，按钮停止时就Release ，软件或刷卡不能Release 。按alarm按钮的时候门是处于锁定status的，不按时Release 锁定status。
            System.out.println("匪警alarm功能参数:"+openAlarmOption.OpenAlarm); 
        }
    }
```

**Write Anti-Police Alarm Parameters**

```java

 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteOpenAlarmOption_Parameter parameter = new WriteOpenAlarmOption_Parameter(commandDetail);
 //0--关闭此功能
 //1--所有门锁定，alarm输出，蜂鸣器不响。不开门，刷卡不能Release ，软件Release ，Release alarm后门的锁定也解锁了。
 //2--alarm输出，不锁定，蜂鸣器响。不开门，刷卡可以Release ，软件可以Release 
 //3--按alarm按钮就alarm，门锁定，并输出，不按时就停止。不开门，按钮停止时就Release ，软件或刷卡不能Release 。按alarm按钮的时候门是处于锁定status的，不按时Release 锁定status。
 parameter.Option = 0;
 WriteOpenAlarmOption cmd = new WriteOpenAlarmOption(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteOpenAlarmOption) {    
           System.out.println("Write Anti-Police Alarm Parameters succ");
        }
    }
```

##### 1.6.6 Smoke Alarm Parameters

**Read Smoke Alarm Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadSmogAlarmOption cmd = new ReadSmogAlarmOption(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteFireAlarmOption) {    
           System.out.println("Read Smoke Alarm Parameters succ");
           ReadSmogAlarmOption_Result smogAlarmOption = (ReadSmogAlarmOption_Result) result;
           //0--关闭此功能
           //1--驱动 [Smoke Alarm继电器]，(信号有，就驱动的，信号无，就关闭)。
           //2--驱动Smoke Alarm继电器并驱动所有门继电器，主板alarm提示音响(开启后由软件关闭，或重启。)
           //3--驱动Smoke Alarm继电器并锁定所有门，主板alarm提示音响(开启后由软件关闭，或重启。) 
            System.out.println("Smoke Alarm Parameters:"+openAlarmOption.Option); 
        }
    }
```

**Write Smoke Alarm Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteSmogAlarmOption_Parameter parameter = new WriteSmogAlarmOption_Parameter(commandDetail);
 //0--关闭此功能
 //1--驱动 [Smoke Alarm继电器]，(信号有，就驱动的，信号无，就关闭)。
 //2--驱动Smoke Alarm继电器并驱动所有门继电器，主板alarm提示音响(开启后由软件关闭，或重启。)
 //3--驱动Smoke Alarm继电器并锁定所有门，主板alarm提示音响(开启后由软件关闭，或重启。) 
 parameter.Option = 0;
 WriteSmogAlarmOption cmd = new WriteSmogAlarmOption(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteSmogAlarmOption) {    
           System.out.println("Write Smoke Alarm Parameters succ");
        }
    }
```

##### 1.6.7 Intelligent Anti-Theft Host Parameters

Normally, this function is turned off, and it can only be turned on by swiping the management card three times in a row (or by using the password to turn it on). After that, the anti-theft monitoring begins. If someone is detected inside, the alarm will be delayed for n minutes (if someone swipes the management card twice within n minutes, the alarm will be canceled, or it can be canceled by using the closing password).

Ways to turn on the function:

1.Swipe the card (with the privilege of anti-theft host card) to turn it on, swipe the card three times.

2.Turn it on with a password.

Ways to turn off the function:

1.Swipe the card (with the privilege of anti-theft host card) to turn it off, swipe the card twice.

2.Turn it off with a password.

After turning it on, enter the anti-theft host function status after using [Enter Delay (Waiting for Personnel to Leave Delay)]. After someone is detected, enter the alarm state after waiting for [Exit Delay (Giving Personnel Time to Close Host Function)].

Pre-deployment status: The prompt during the [Enter Delay] time. This prompt is the buzzer of the card reader.

Pre-alarm prompt: The prompt during the [Exit Delay] time. This prompt is the buzzer of the card reader.


**Read Intelligent Anti-Theft Host Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadTheftAlarmSetting cmd = new ReadTheftAlarmSetting(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadTheftAlarmSetting) {    
           System.out.println("Read Intelligent Anti-Theft Host Parameters succ");
           ReadTheftAlarmSetting_Result ret = (ReadTheftAlarmSetting_Result) result;
           System.out.println("智能防盗主机启用status："+ret.Setting.Use);//false--不启用；true--启用
           System.out.println("智能防盗主机进入延迟："+ret.Setting.InTime);//进入延迟；单位：秒，取值：1-255
           System.out.println("智能防盗主机退出延迟："+ret.Setting.OutTime);//退出延迟；单位：秒，取值：1-255
           System.out.println("智能Anti-Theft Host Deployment 密码："+ret.Setting.BeginPassword);//布防密码；8个数字。空补F。例如密码：23412；表达为：0xFFF23412
           System.out.println("智能防盗主机撤防密码："+ret.Setting.ClosePassword);//撤防密码；8个数字。空补F。例如密码：23412；表达为：0xFFF23412
            System.out.println("智能防盗主机alarm时长："+ret.Setting.AlarmTime);//alarm时长；单位：秒 ,取值：0-65535
        }
    }
```

**Write Intelligent Anti-Theft Host Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteTheftAlarmSetting_Parameter parameter = new WriteTheftAlarmSetting_Parameter(commandDetail);
 parameter.Setting = new TheftAlarmSetting();
 parameter.Setting.Use = false;//false--不启用；true--启用
 parameter.Setting.InTime = 10;//进入延迟；单位：秒，取值：1-255
 parameter.Setting.OutTime = 10;//退出延迟；单位：秒，取值：1-255
 parameter.Setting.BeginPassword = "0xFFF23412";//布防密码；8个数字。空补F。例如密码：23412；表达为：0xFFF23412
 parameter.Setting.ClosePassword = "0xFFF23412";//撤防密码；8个数字。空补F。例如密码：23412；表达为：0xFFF23412
 parameter.Setting.AlarmTime = 10;//alarm时长；单位：秒 ,取值：0-65535
 WriteTheftAlarmSetting cmd = new WriteTheftAlarmSetting(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteTheftAlarmSetting) {    
           System.out.println("Write Intelligent Anti-Theft Host Parameters succ");
        }
    }
```

##### 1.6.8 Set Anti-Return Mode

**Read Set Anti-Return Mode**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadCheckInOut cmd = new ReadCheckInOut(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadCheckInOut) {    
           System.out.println("Read Intelligent Anti-Theft Host Parameters succ");
           ReadCheckInOut_Result ret = (ReadCheckInOut_Result) result;
           System.out.println("防潜回功能参数："+ret.Use);//1--单独每个门检测防潜回；2--整个控制器统一防潜回
        }
    }
```

**Write Set Anti-Return Mode**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteCheckInOut_Parameter parameter = new WriteCheckInOut_Parameter(commandDetail);
 parameter.Mode = 2;//01--单独每个门检测防潜回；02--整个控制器统一防潜回
 WriteCheckInOut cmd = new WriteCheckInOut(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteCheckInOut) {    
           System.out.println("Write Set Anti-Return Mode succ");
        }
    }
```

##### 1.6.9 Card Reading Interval Time

**Read Card Reading Interval Time**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadReaderIntervalTime cmd = new ReadReaderIntervalTime(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadReaderIntervalTime) {    
           System.out.println("Read Card Reading Interval Time succ");
           ReadReaderIntervalTime_Result ret = (ReadReaderIntervalTime_Result) result;
           System.out.println("Card Reading Interval Time:"+ret.IntervalTime);//最大65535秒。0表示无限制
        }
    }
```

**Write Card Reading Interval Time**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteReaderIntervalTime_Parameter parameter = new WriteReaderIntervalTime_Parameter(commandDetail);
 parameter.IntervalTime = 2;//最大65535秒。0表示无限制
 WriteReaderIntervalTime cmd = new WriteReaderIntervalTime(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteReaderIntervalTime) {    
           System.out.println("Write Card Reading Interval Time succ");
        }
    }
```

##### 1.6.10 Voice Broadcast Segment Switch

**Read Voice Broadcast Segment Switch**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadBroadcast cmd = new ReadBroadcast(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadBroadcast) {    
           System.out.println("Read Voice Broadcast Segment Switch succ");
           ReadBroadcast_Result ret = (ReadBroadcast_Result) result;
           for (int i = 1; i <=80; i++) {
             //取值范围 1-80(语音段对照可参考《语音表》)
            System.out.println("Voice Broadcast Segment Switch:"+ret.Broadcast.GetValue(i));// 开关status 开关true 表示启用，false 表示禁用
           }
        }
    }
```

**Write Voice Broadcast Segment Switch**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteBroadcast_Parameter parameter = new WriteBroadcast_Parameter(commandDetail);
 for (int i = 1; i <=80; i++) {
  //取值范围 1-80(语音段对照可参考《语音表》)
  parameter.Broadcast.SetValue(i,false);// 开关status 开关true 表示启用，false 表示禁用
 }
 WriteBroadcast cmd = new WriteBroadcast(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteBroadcast) {    
           System.out.println("读Write Voice Broadcast Segment Switch succ");          
        }
    }
```

##### 1.6.11 Card Reader Data Verification

**Read Card Reader Data Verification**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadReaderCheckMode cmd = new ReadReaderCheckMode(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadReaderCheckMode) {    
           System.out.println("Read Voice Broadcast Segment Switch succ");
           ReadReaderCheckMode_Result ret = (ReadReaderCheckMode_Result) result;    
           System.out.println("Voice Broadcast Segment Switch:"+ret.ReaderCheckMode.GetValue(i));//0--不启用，1--启用，2--启用校验，但不提示非法数据或线路异常。         
        }
    }
```

**Write Card Reader Data Verification**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteReaderCheckMode_Parameter parameter = new WriteReaderCheckMode_Parameter(commandDetail);
 parameter.ReaderCheckMode = 0;//0--不启用，1--启用，2--启用校验，但不提示非法数据或线路异常。
 WriteReaderCheckMode cmd = new WriteReaderCheckMode(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteReaderCheckMode) {    
           System.out.println("Write Card Reader Data Verification succ");          
        }
    }
```

##### 1.6.12 Set Main Board Buzzer

**Read Main Board Buzzer**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadBuzzer cmd = new ReadBuzzer(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadBuzzer) {    
           System.out.println("Read Main Board Buzzer para succ");
           ReadBuzzer_Result ret = (ReadBuzzer_Result) result;    
           System.out.println("Main Board Buzzer para:"+ret.Buzzer);//0--不启用，1--启用         
        }
    }
```

**Write Main Board Buzzer**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteBuzzer_Parameter parameter = new WriteBuzzer_Parameter(commandDetail);
 parameter.Buzzer = 0;//0--不启用，1--启用
 WriteBuzzer cmd = new WriteBuzzer(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteBuzzer) {    
           System.out.println("Write 主板蜂鸣器 succ");          
        }
    }
```

##### 1.6.13 Limit of People Inside the Door

**Read Limit of People Inside the Door**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadEnterDoorLimit cmd = new ReadEnterDoorLimit(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadEnterDoorLimit) {    
           System.out.println("Read Limit of People Inside the Door succ");
           ReadEnterDoorLimit_Result ret = (ReadEnterDoorLimit_Result) result;    
             /**
             * Limit of People Inside the Door.<br>
             * 上限值：0--表示不受限制.<br>
             * 全局上限优先级最高，全局上限如果大于 0 则设备使用全局上限.<br>
             * 例如：<br>
             * 全局上限为100,1门上限为50,2门上限为100,。。。。4门上限为1000 <br>
             * 设备将使用全局上限100，即整个主板上进入数不能超过100。<br>
             * 此数据重启后清空。
             */
            System.out.println("Limit of People Inside the Door总人数："+ret.Limit.GlobalLimit);
            System.out.println("1号门Limit of People Inside the Door人数："+ret.Limit.DoorLimit[0]);
            System.out.println("2号门Limit of People Inside the Door人数："+ret.Limit.DoorLimit[1]);
            System.out.println("3号门Limit of People Inside the Door人数："+ret.Limit.DoorLimit[2]);
            System.out.println("4号门Limit of People Inside the Door人数："+ret.Limit.DoorLimit[3]);
            
            System.out.println("进入门内人数总数："+ret.Limit.GlobalEnter);
            System.out.println("1号门进入门内人数总数："+ret.Limit.DoorEnter[0]);
            System.out.println("2号门进入门内人数总数："+ret.Limit.DoorEnter[1]);
            System.out.println("3号门进入门内人数总数："+ret.Limit.DoorEnter[2]);
            System.out.println("4号门进入门内人数总数："+ret.Limit.DoorEnter[3]);       
        }
    }
```

**Write Limit of People Inside the Door**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteEnterDoorLimit_Parameter parameter = new WriteEnterDoorLimit_Parameter(commandDetail);
 parameter.Limit.GlobalLimit = 100;//全局上限人数
 parameter.Limit.DoorLimit[0]=10;//1号门Limit of People Inside the Door人数
 parameter.Limit.DoorLimit[1]=10;//2号门Limit of People Inside the Door人数
 parameter.Limit.DoorLimit[2]=10;//3号门Limit of People Inside the Door人数
 parameter.Limit.DoorLimit[3]=10;//4号门Limit of People Inside the Door人数
 //注意，Write 之前必须先Read 门内人数，否则进入门内的人数会不一致。
 WriteEnterDoorLimit cmd = new WriteEnterDoorLimit(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteBuzzer) {    
           System.out.println("Write Limit of People Inside the Door succ");          
        }
    }
```

##### 1.6.14 Card Expiration Reminder

**Read Card Expiration Reminder**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadCardPeriodSpeak cmd = new ReadCardPeriodSpeak(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadEnterDoorLimit) {    
           System.out.println("Read Card Expiration Reminder succ");
           ReadCardPeriodSpeak_Result ret = (ReadCardPeriodSpeak_Result) result;    
           System.out.println("Card Expiration Reminder启用status："+ret.Use);//true--启用 false--禁用
        }
    }
```

**Write Card Expiration Reminder**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteCardPeriodSpeak_Parameter parameter = new WriteCardPeriodSpeak_Parameter(commandDetail);
 parameter.Use = true;//true--启用 false--禁用
 WriteCardPeriodSpeak cmd = new WriteCardPeriodSpeak(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteBuzzer) {    
           System.out.println("Write Card Expiration Reminder");          
        }
    }
```

##### 1.6.15 Scheduled Card Reading and Voice Message Parameters

**Read Scheduled Card Reading and Voice Message Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 ReadReadCardSpeak cmd = new ReadReadCardSpeak(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadReadCardSpeak) {    
           System.out.println("Read Scheduled Card Reading and Voice Message Parameters succ");
           ReadReadCardSpeak_Result ret = (ReadReadCardSpeak_Result) result;    
           System.out.println("启用status："+ret.SpeakSetting.Use);//true--启用 false--禁用
           System.out.println("消息编号："+ret.SpeakSetting.MsgIndex);//1--交房租; 2--交管理费;
           System.out.println("起始时段："+ret.SpeakSetting.BeginDate);//年月日时 BCD码，例：0x11120115，表示11年12月1日15点 最大不得超过2099年
           System.out.println("结束时段："+ret.SpeakSetting.EndDate);//最大不得超过2099年
        }
    }
```

**Write Scheduled Card Reading and Voice Message Parameters**

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 WriteReadCardSpeak_Parameter parameter = new WriteReadCardSpeak_Parameter(commandDetail);
 parameter.SpeakSetting = new ReadCardSpeak();
 parameter.SpeakSetting.Use = true;//true--启用 false--禁用
 parameter.SpeakSetting.MsgIndex = 1;//1--交房租; 2--交管理费;
 parameter.SpeakSetting.BeginDate = Calendar.getInstance();//年月日时 BCD码，例：0x11120115，表示11年12月1日15点 最大不得超过2099年
 parameter.SpeakSetting.BeginDate.set(2020,9,10);// 2020-10-10;
 parameter.SpeakSetting.EndDate = Calendar.getInstance();
 parameter.SpeakSetting.EndDate.set(2022,9,10);// 2022-10-10;
 WriteReadCardSpeak cmd = new WriteReadCardSpeak(parameter);
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof WriteReadCardSpeak) {    
           System.out.println("Write Scheduled Card Reading and Voice Message Parameters succ");          
        }
    }
```

#### 1.7 Real-Time Monitoring

```java
 CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
 BeginWatch cmd = new BeginWatch(new CommandParameter(commandDetail));
  _Allocator.OpenForciblyConnect(commandDetail.Connector);//设置保持长连接
  //Add Command to Communication Connector Queue
 _Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof BeginWatch) {    
        	System.out.println("开启监控 succ");
        }
    }
```

**Monitoring Return Result**

```java
 @Override
public void WatchEvent(ConnectorDetail detial, INData event){
    
     if (event instanceof Door8800WatchTransaction) { //监控数据的数据结构
          Door8800WatchTransaction watchEvent = (Door8800WatchTransaction) event;
            AbstractTransaction tr = (AbstractTransaction) watchEvent.EventData;
            System.out.println(watchEvent.SN + " 收到监控事件：" + tr.getClass().toString());
            switch (watchEvent.CmdIndex) {
                case 1://认证记录
                    //Door.Access.Door8800.Command.Data
                    //Door.Access.Door89H.Command.Data
                    //CardTransaction类需要区分 89H系列板子使用的是Door.Access.Door89H.Command.Data.CardTransaction
                    CardTransaction card = (CardTransaction) watchEvent.EventData; 
                    break;
                case 2://出门开关信息
                    ButtonTransaction Software=(ButtonTransaction) watchEvent.EventData;
                    break;
                case 3://门磁信息
                    DoorSensorTransaction DoorSensor = (DoorSensorTransaction) watchEvent.EventData;
                    break;
                case 4://软件操作 包括远程开门之类的一些命令
                    SoftwareTransaction softwareTransaction=(SoftwareTransaction) watchEvent.EventData;
                    System.out.println("远程开门消息.................");
                    break;
                case 5://alarm记录
                    AlarmTransaction alarmTransaction= (AlarmTransaction) watchEvent.EventData;
                    break;
                case 6://系统记录
                    SystemTransaction  systemTransaction= (SystemTransaction) watchEvent.EventData;
                    break;
                case 34://保活消息
                    System.out.println("保活包消息.................");
                    break;
                default:
                    DefinedTransaction dt = (DefinedTransaction) watchEvent.EventData;
                    break;
            }   
     }
    
}
```

#### 1.8 Fire Alarm

**Read Fire Alarm status**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadFireAlarmState cmd = new ReadFireAlarmState(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadFireAlarmState) {    
           System.out.println("Read Fire Alarmstatus succ");
           ReadFireAlarmState_Result ret = (ReadFireAlarmState_Result) result;    
           System.out.println("alarmstatus："+ret.FireAlarm);//status：0--未开启alarm；1--已开启alarm
        }
    }
```

**Trigger Fire Alarm**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
SendFireAlarm cmd = new SendFireAlarm(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof SendFireAlarm) {    
        	System.out.println("Trigger Fire Alarm succ");
        }
    }
```

**Release Fire Alarm**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
CloseFireAlarm cmd = new CloseFireAlarm(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof CloseFireAlarm) {    
        	System.out.println("Release Fire Alarm succ");
        }
    }
```

#### 1.9 Smoke Alarm

**Read Smoke Alarm status**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadSmogAlarmState cmd = new ReadSmogAlarmState(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadSmogAlarmState) {    
           System.out.println("Read Smoke Alarmstatus succ");
           ReadSmogAlarmState_Result ret = (ReadSmogAlarmState_Result) result;    
           System.out.println("alarm status："+ret.SmogAlarm);//status：0--未开启alarm；1--已开启alarm
        }
    }
```

**Trigger Smoke Alarm**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
SendSmogAlarm cmd = new SendSmogAlarm(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
  @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof SendSmogAlarm) {    
        	System.out.println("Trigger Smoke Alarm succ");
        }
    }
```

**Release Smoke Alarm**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
CloseSmogAlarm cmd = new CloseSmogAlarm(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof CloseSmogAlarm) {    
        	System.out.println("Release Smoke Alarm succ");
        }
    }
```

#### 1.10 Release alarm

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
CloseAlarm_Parameter parameter = new CloseAlarm_Parameter(commandDetail);
parameter.Door= 1;//需要Release alarm的门，取值范围：1-4
//alarm类型--用于Release alarm
//00--非法卡alarm
//01--门磁alarm
//02--胁迫alarm
//03--开门超时alarm
//04-- Blacklist alarm
//05--匪警alarm
//06--防盗主机alarm
//07--Fire Alarm
//08--Smoke Alarm
//09--关闭电锁出错alarm
//10--防拆alarm
//11--强制关锁alarm
//12--强制开锁alarm
//9-12为一体锁或一体机alarm类型
//9--关闭电锁出错alarm
//10--防拆alarm
//11--强制关锁alarm
//12--强制开锁alarm
parameter.Alarm.SetValue(7 , true);
CloseAlarm cmd = new CloseAlarm(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof CloseSmogAlarm) {    
        	System.out.println("Release alarm succ");
        }
    }
```

#### 1.11 Get device status Information

#####1.11.1 Read Status Information of Each Port

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadWorkStatus cmd = new ReadWorkStatus(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadWorkStatus) {    
           	System.out.println("Read 各端口status信息 succ");
          	ReadWorkStatus_Result ret = (ReadWorkStatus_Result) result;    
           	//门继电器物理status
           	//0--表示COM和NC常闭
           	//1--表示COM和NO常闭    
           	//2--表示继电器无法检测，继电器异常
           	System.out.println("1门继电器物理status:"+ret.RelayState.GetDoor(1));
           	System.out.println("2门继电器物理status:"+ret.RelayState.GetDoor(2));
           	System.out.println("3门继电器物理status:"+ret.RelayState.GetDoor(3));
           	System.out.println("4门继电器物理status:"+ret.RelayState.GetDoor(4));
           	//常开status
           	//0--表示常闭
           	//1--0表示常开
           	System.out.println("1门常开status:"+ret.DoorLongOpenState.GetDoor(1));
           	System.out.println("2门常开status:"+ret.DoorLongOpenState.GetDoor(2));
           	System.out.println("3门常开status:"+ret.DoorLongOpenState.GetDoor(3));
           	System.out.println("4门常开status:"+ret.DoorLongOpenState.GetDoor(4));
           	//门alarmstatus
          	//0--非法刷卡alarm
            //1--门磁alarm
            //2--胁迫alarm
            //3--开门超时alarm
            //4-- Blacklist alarm
            System.out.println("1门alarmstatus:"+ret.DoorAlarmState.GetDoor(1));
           	System.out.println("2门alarmstatus:"+ret.DoorAlarmState.GetDoor(2));
           	System.out.println("3门alarmstatus:"+ret.DoorAlarmState.GetDoor(3));
           	System.out.println("4门alarmstatus:"+ret.DoorAlarmState.GetDoor(4));
            
            //设备alarmstatus
            //0--匪警alarm
            //1--防盗alarm
            //2--Fire Alarm
            //3--Smoke Alarm
            //4--Fire Alarm(命令通知)
            System.out.println("设备alarmstatus:"+ret.AlarmState);
            
            //继电器逻辑status
            //1-4是表示门的继电器
            //0--继电器关
            //1--继电器开
            //2--双稳态
            System.out.println("1门继电器逻辑status:"+ret.LockState.GetDoor(1));
           	System.out.println("2门继电器逻辑status:"+ret.LockState.GetDoor(2));
           	System.out.println("3门继电器逻辑status:"+ret.LockState.GetDoor(3));
           	System.out.println("4门继电器逻辑status:"+ret.LockState.GetDoor(4));
            //5-8是alarm继电器
            //status0表示：COM和NC导通
            //status1表示：COM和NO导通
            System.out.println("消防继电器status:"+ret.LockState.GetDoor(5));//5--消防继电器
           	System.out.println("匪警继电器status:"+ret.LockState.GetDoor(6));//6--匪警继电器
           	System.out.println("Smoke Alarmstatus:"+ret.LockState.GetDoor(7));//7--Smoke Alarm继电器
           	System.out.println("防盗主机alarmstatus:"+ret.LockState.GetDoor(8));//8--防盗主机alarm继电器
            
            //锁定status
            //0--未锁定，1--锁定
            System.out.println("1门锁定status:"+ret.PortLockState.GetDoor(1));
           	System.out.println("2门锁定status:"+ret.PortLockState.GetDoor(2));
           	System.out.println("3门锁定status:"+ret.PortLockState.GetDoor(3));
           	System.out.println("4门锁定status:"+ret.PortLockState.GetDoor(4));
            
             System.out.println("监控status:"+ret.WatchState);//0--未开启监控；1--开启监控
            
            //门内人数
            System.out.println("Limit of People Inside the Door总人数："+ret.EnterTotal.GlobalLimit);
            System.out.println("1号门Limit of People Inside the Door人数："+ret.EnterTotal.DoorLimit[0]);
            System.out.println("2号门Limit of People Inside the Door人数："+ret.EnterTotal.DoorLimit[1]);
            System.out.println("3号门Limit of People Inside the Door人数："+ret.EnterTotal.DoorLimit[2]);
            System.out.println("4号门Limit of People Inside the Door人数："+ret.EnterTotal.DoorLimit[3]);
            
            System.out.println("进入门内人数总数："+ret.EnterTotal.GlobalEnter);
            System.out.println("1号门进入门内人数总数："+ret.EnterTotal.DoorEnter[0]);
            System.out.println("2号门进入门内人数总数："+ret.EnterTotal.DoorEnter[1]);
            System.out.println("3号门进入门内人数总数："+ret.EnterTotal.DoorEnter[2]);
            System.out.println("4号门进入门内人数总数："+ret.EnterTotal.DoorEnter[3]); 
            
            //Anti-Theft Host Deployment status
            //1--延时布防
            //2--已布防
            //3--延时撤防
            //4--未布防
            //5--alarm延时，准备启用alarm
            //6--防盗alarm已启动
             System.out.println("Anti-Theft Host Deployment status:"+ret.TheftState);
        }
    }
```

#####1.11.2 Read Anti-Theft Host Deployment status

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadTheftAlarmState cmd = new ReadTheftAlarmState(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadTheftAlarmState) {    
           System.out.println("Read Anti-Theft Host Deployment status succ");
           ReadTheftAlarmState_Result ret = (ReadTheftAlarmState_Result) result;               
             //Anti-Theft Host Deployment status
            //1--延时布防
            //2--已布防
            //3--延时撤防
            //4--未布防
            //5--alarm延时，准备启用alarm
            //6--防盗alarm已启动
           System.out.println("Anti-Theft Host Deployment status："+ret.TheftState);//Anti-Theft Host Deployment status
            System.out.println("alarmstatus："+ret.TheftAlarm);//0--未alarm，1--已alarm
        }
    }
```

#### 1.12 Initialize Device

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
FormatController cmd = new FormatController(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
 @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof FormatController) {    
        	System.out.println("Initialize Device succ");
        }
    }
```

#### 1.13 Search Device

```java
public class SearchEquptDemo implements INConnectorEvent { //实现通讯连接器事件通知接口
    //声明通讯连接器

    private ConnectorAllocator _Allocator;
    //本机IP地址
    private String localIp = "192.168.1.97";
    //本机端口
    private int localPort = 9000;

    public SearchEquptDemo() {
        //在构造方法中获取实例（单例）
        _Allocator = ConnectorAllocator.GetAllocator();
        //添加事件通知
        _Allocator.AddListener(this);
        //绑定UDP
        _Allocator.UDPBind(localIp, localPort);
    }

    /**
     * Get Command Detail Object
     */
    public CommandDetail getCommandDetail() {
        UDPDetail udp = new UDPDetail("255.255.255.255", 8101, localIp, localPort);//IP地址，端口(默认8000)
        Door8800Identity idt = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8900);
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Connector = udp;
        commandDetail.Identity = idt;
        commandDetail.Timeout = 10000;//Search Device时间可以设置长一下
        commandDetail.RestartCount = 1;
        return commandDetail;
    }

    public void search() {
        CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object      
        SearchEquptOnNetNum cmd = new SearchEquptOnNetNum(new CommandParameter(commandDetail));
        //Add Command to Communication Connector Queue
        _Allocator.AddCommand(cmd);
    }
    SearchEquptOnNetNum_Result SearchResult;//搜索完成时Return Result

    /**
     * 命令执行 succ监听事件
     *
     * @param cmd 当前执行的命令
     * @param result 执行命令的Return Result，部分命令是没有Return Result的，返回到此监听事件即为命令 succ。
     */
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
            //搜索结果返回至CommandTimeout
    }

    /**
     * 命令超时监听事件
     *
     * @param cmd 当前执行的命令
     */
    @Override
    public void CommandTimeout(INCommand cmd) {
        if (cmd instanceof SearchEquptOnNetNum) {
            SearchEquptOnNetNum_Result result = (SearchEquptOnNetNum_Result) cmd.getCommandResult();
            if (result != null) {
                int size = result.ResultList.size();
                for (int i = 0; i < size; i++) {
                    SearchEquptOnNetNum_Result.SearchResult searchResult = result.ResultList.get(i);
                    System.out.println("SN=" + searchResult.SN + ",IP=" + searchResult.TCP.GetIP());
                }
            }
        }
        System.out.println("搜索完成");
    }

    /**
     * 当前执行的命令执行进度
     *
     * @param cmd 当前执行的命令
     */
    @Override
    public void CommandProcessEvent(INCommand cmd) {
        System.out.println("当前命令:" + cmd.getClass().toString() + ",当前进度:" + cmd.getProcessStep() + "/" + cmd.getProcessMax());
        //当前命令:OpenDoor,当前进度:1/1
    }

    /**
     * 连接失败监听事件
     *
     * @param cmd 当前执行的命令
     * @param isStop 是否是手动停止
     */
    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        String sCmd = cmd.getClass().toString();
        if (isStop) {
            System.out.println(sCmd + "命令已手动停止!");
        } else {
            System.out.println(sCmd + "网络连接失败!");
        }

    }

    /**
     * 连接失败监听事件
     *
     * @param detial 连接器详情对象
     */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail detial) {
        try {
            System.out.println("网络通道故障:");
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.ConnectorErrorEvent() -- " + e.toString());
        }
    }

    /**
     * 通讯密码错误监听事件
     *
     * @param cmd 当前执行的命令
     */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        System.out.println("通讯密码错误，已失败！");
    }

    /**
     * 校验和错误监听事件
     *
     * @param cmd 当前执行的命令
     */
    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
        System.out.println("命令返回的校验和错误，已失败！");
    }

    /**
     * 数据监控事件
     *
     * @param detial 连接器详情对象
     * @param event 门禁控制板推送过来的数据
     */
    @Override
    public void WatchEvent(ConnectorDetail detial, INData event) {
        try {
            /*
            包含 出入记录、alarm记录、软件开门消息、门磁消息等
             */
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("数据监控:");
            if (event instanceof Door8800WatchTransaction) {
                Door8800WatchTransaction WatchTransaction = (Door8800WatchTransaction) event;
                strBuf.append("，SN：");
                strBuf.append(WatchTransaction.SN);
                strBuf.append("\n");
            } else {
                strBuf.append("，未知事件：");
                strBuf.append(event.getClass().getName());
            }
            System.out.println(strBuf);
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.WatchEvent() -- " + e.toString());
        }
    }

    /**
     * 设备上线（当设备以客户端的方式连接时才会Trigger ）
     *
     * @param client 连接器详情对象
     */
    @Override
    public void ClientOnline(ConnectorDetail client) {
        //需要将ConnectorDetail 类转为TCPClientDetail或者UDPDetail 具体子类对象
    }

    /**
     * 设备离线（当设备以客户端的方式连接时才会Trigger ）
     *
     * @param client 连接器详情对象
     */
    @Override
    public void ClientOffline(ConnectorDetail client) {
        //需要将ConnectorDetail 类转为TCPClientDetail或者UDPDetail 具体子类对象
    }

}
```

#### 1.14 Controller Client Keep-Alive Interval

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadKeepAliveInterval cmd = new ReadKeepAliveInterval(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadKeepAliveInterval) {
        ReadKeepAliveInterval_Result ret = (ReadKeepAliveInterval_Result) result;
        System.out.println("保活间隔时间："+ret.IntervalTime);//取值范围：0-3600,0--关闭功能;        
    }    
}
```

####1.15 Anti-Theft Alarm Deployment and Disarmament

**Deployment**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
SetTheftFortify cmd = new SetTheftFortify(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof SetTheftFortify) {       
        System.out.println("防盗alarm布防 succ");       
    }    
}
```

**Disarmament**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
SetTheftDisarming cmd = new SetTheftDisarming(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof SetTheftDisarming) {       
        System.out.println("防盗alarm撤防 succ");       
    }    
}
```

#### 1.16  Blacklist alarm

**Read  Blacklist alarm function Switch**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadBalcklistAlarmOption cmd = new ReadBalcklistAlarmOption(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadBalcklistAlarmOption) {    
        ReadBalcklistAlarmOption_Result ret = (ReadBalcklistAlarmOption_Result) result;
        System.out.println(" Blacklist alarm功能开关status："+ret.Use);  //true--开启 false--关闭
    }    
}
```

**Write  Blacklist alarm function Switch**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteBalcklistAlarmOption_Parameter parameter = new WriteBalcklistAlarmOption_Parameter(commandDetail);
parameter.Use = true;//true--开启 false--关闭
WriteBalcklistAlarmOption cmd = new WriteBalcklistAlarmOption(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteBalcklistAlarmOption) {          
        System.out.println("Write  Blacklist alarm功能开关 succ"); 
    }    
}
```

#### 1.17 Anti-Probing Function

**Read Anti-Probing Function**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadExploreLockMode cmd = new ReadExploreLockMode(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadExploreLockMode) {          
        ReadExploreLockMode_Result ret = (ReadExploreLockMode_Result) result;
        System.out.println("Anti-Probing Function开关status："+ret.Use);  //true--开启 false--关闭
    }    
}
```

**Write Anti-Probing Function**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteExploreLockMode_Parameter parameter = new WriteExploreLockMode_Parameter(commandDetail);
parameter.Use = true;//true--开启 false--关闭
WriteExploreLockMode cmd = new WriteExploreLockMode(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteExploreLockMode) {          
        System.out.println("Write Anti-Probing Function开关 succ"); 
    }    
}
```

#### 1.18 485 Line Reverse Connection Detection Switch

**Read 485 Line Reverse Connection Detection Switch**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadCheck485Line cmd = new ReadCheck485Line(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadCheck485Line) {          
        ReadCheck485Line_Result ret = (ReadCheck485Line_Result) result;
        System.out.println("Anti-Probing Function开关status："+ret.Use);  //true--开启 false--关闭
    }    
}
```

**Write 485 Line Reverse Connection Detection Switch**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteCheck485Line_Parameter parameter = new WriteCheck485Line_Parameter(commandDetail);
parameter.Use = true;//true--开启 false--关闭
WriteCheck485Line cmd = new WriteCheck485Line(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteCheck485Line) {          
        System.out.println("Write 485Line Reverse Connection Detection Switch succ"); 
    }    
}
```

#### 1.19 Time for Expiration Reminder

**Read Time for Expiration Reminder**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadCardDeadlineTipDay cmd = new ReadCardDeadlineTipDay(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadCardDeadlineTipDay) {          
        ReadCardDeadlineTipDay_Result ret = (ReadCardDeadlineTipDay_Result) result;
        System.out.println("Time for Expiration Reminder："+ret.Day);  //取值范围: 1-255。0--表示关闭;默认值是30天
    }    
}
```

**Write Time for Expiration Reminder**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteCardDeadlineTipDay_Parameter parameter = new WriteCardDeadlineTipDay_Parameter(commandDetail);
parameter.Day = 30;//取值范围: 1-255。0--表示关闭;默认值是30天
WriteCardDeadlineTipDay cmd = new WriteCardDeadlineTipDay(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteCardDeadlineTipDay) {          
        System.out.println("Write Time for Expiration Reminder succ"); 
    }    
}
```

###2.Door Settings

#### 2.1 Card Reader Byte

**Read Card Reader Byte**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadReaderOption cmd = new ReadReaderOption(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadReaderOption) {          
        ReadReaderOption_Result ret = (ReadReaderOption_Result) result;
        //1--韦根26(三字节)
        //2--韦根34(四字节)
        //3--韦根26(二字节)
        //4--禁用
        //5--8字节身份证读卡器 (89H类型控制板)
        //6--二维码读卡器（兼容8字节身份证）(89H类型控制板)
        System.out.println("1门Card Reader Byte:"+ret.door.GetDoor(1));
        System.out.println("2门Card Reader Byte:"+ret.door.GetDoor(2));
        System.out.println("3门Card Reader Byte:"+ret.door.GetDoor(3));
        System.out.println("4门Card Reader Byte:"+ret.door.GetDoor(4));
    }    
}
```

**Write Card Reader Byte**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteReaderOption_Parameter parameter = new WriteReaderOption_Parameter(commandDetail);
//1--韦根26(三字节)
//2--韦根34(四字节)
//3--韦根26(二字节)
//4--禁用
//5--8字节身份证读卡器 (89H类型控制板)
//6--二维码读卡器（兼容8字节身份证）(89H类型控制板)
parameter.door.SetDoor(1,2);
parameter.door.SetDoor(2,2);
parameter.door.SetDoor(3,2);
parameter.door.SetDoor(4,2);
WriteReaderOption cmd = new WriteReaderOption(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteReaderOption) {          
        System.out.println("Write Card Reader Byte succ"); 
    }    
}
```

####2.2 Relay Parameters

**Read Relay Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadRelayOption cmd = new ReadRelayOption(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadRelayOption) {          
        ReadRelayOption_Result ret = (ReadRelayOption_Result) result;
        //1--不输出（默认）COM NC
        //2--输出COM NO
        //3--读卡切换输出status（当读到合法卡后自动自动切换到当前相反的status。）例如卷闸门。
        System.out.println("1门Relay Parameters:"+ret.door.GetDoor(1));
        System.out.println("2门Relay Parameters:"+ret.door.GetDoor(2));
        System.out.println("3门Relay Parameters:"+ret.door.GetDoor(3));
        System.out.println("4门Relay Parameters:"+ret.door.GetDoor(4));
    }    
}
```

**Write Relay Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteRelayOption_Parameter parameter = new WriteRelayOption_Parameter(commandDetail);
//1--不输出（默认）COM NC
//2--输出COM NO
//3--读卡切换输出status（当读到合法卡后自动自动切换到当前相反的status。）例如卷闸门。 刷一下卡之后门就为开启status，会一直持续，再次刷卡是会为关闭status
parameter.door.SetDoor(1,1);
parameter.door.SetDoor(2,1);
parameter.door.SetDoor(3,1);
parameter.door.SetDoor(4,1);
WriteRelayOption cmd = new WriteRelayOption(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteRelayOption) {          
        System.out.println("Write Relay Parameters succ"); 
    }    
}
```

####2.3 Open/Close Door

**Remote Door Opening**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
OpenDoor_Parameter parameter = new OpenDoor_Parameter(commandDetail);
//1表示开门，0表示不开门
parameter.door.SetDoor(1,0);
parameter.door.SetDoor(2,0);
parameter.door.SetDoor(3,0);
parameter.door.SetDoor(4,1);
parameter.IsCheckNum = false;//开门指令是否带有验证序号
parameter.CheckNum = 123;//开门指令附带的验证序号(验证码是为了避免多次Trigger 相同的开门指令。)
OpenDoor cmd = new OpenDoor(parameter);
//Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof OpenDoor) {          
        System.out.println("远程开门 succ"); 
    }    
}
```

**Remote Door Closing**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
RemoteDoor_Parameter parameter = new RemoteDoor_Parameter(commandDetail);
//1表示操作，0表示不操作
parameter.door.SetDoor(1,1);
parameter.door.SetDoor(2,1);
parameter.door.SetDoor(3,1);
parameter.door.SetDoor(4,1);
CloseDoor cmd = new CloseDoor(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof CloseDoor) {          
        System.out.println("远程关门 succ"); 
    }    
}
```

#### 2.4 Door Always Open

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
RemoteDoor_Parameter parameter = new RemoteDoor_Parameter(commandDetail);
//1表示操作，0表示不操作
parameter.door.SetDoor(1,1);
parameter.door.SetDoor(2,1);
parameter.door.SetDoor(3,1);
parameter.door.SetDoor(4,1);
HoldDoor cmd = new HoldDoor(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof HoldDoor) {          
        System.out.println("门常开设置 succ"); 
    }    
}
```

#### 2.5 Lock Door

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
RemoteDoor_Parameter parameter = new RemoteDoor_Parameter(commandDetail);
//1表示操作，0表示不操作
parameter.door.SetDoor(1,1);
parameter.door.SetDoor(2,1);
parameter.door.SetDoor(3,1);
parameter.door.SetDoor(4,1);
LockDoor cmd = new LockDoor(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof LockDoor) {          
        System.out.println("锁定门 succ"); 
    }    
}
```

#### 2.6 UnLock Door

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
RemoteDoor_Parameter parameter = new RemoteDoor_Parameter(commandDetail);
//1表示操作，0表示不操作
parameter.door.SetDoor(1,1);
parameter.door.SetDoor(2,1);
parameter.door.SetDoor(3,1);
parameter.door.SetDoor(4,1);
UnlockDoor cmd = new UnlockDoor(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof UnlockDoor) {          
        System.out.println("解锁门 succ"); 
    }    
}
```

#### 2.7 Card Reading Authentication Method

**Read Card Reading Authentication Method**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadReaderWorkSetting cmd = new ReadReaderWorkSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadReaderWorkSetting) {          
       ReadReaderWorkSetting_Result ret = (ReadReaderWorkSetting_Result) result;
        System.out.println(ret.DoorNum+"号门 星期一认证方式"); 
        DayTimeGroup_ReaderWork day1 = ret.ReaderWork.GetItem(E_WeekDay.Monday);//获取星期一的认证方式
        TimeSegment_ReaderWork time1 = day1.GetItem(1);//1-8时段
        short[] beginTime = new  short[2];
        time1.GetBeginTime(beginTime);
        short[] endTime = new  short[2];
        time1.GetEndTime(endTime);
        System.out.println("时段1开始时间："+beginTime[0]+":"+beginTime[1]);
        System.out.println("时段1结束时间："+endTime[0]+":"+endTime[1]);
       	System.out.println("仅读卡："+times.GetWorkType(ReaderWorkType.OnlyCard));
        System.out.println("仅密码："+times.GetWorkType(ReaderWorkType.OnlyPassword));
        System.out.println("读卡加密码："+times.GetWorkType(ReaderWorkType.CardAndPassword));
        System.out.println("手动输入卡号+密码："+times.GetWorkType(ReaderWorkType.InputCardAndPassword));
    }    
}
```

**Write Card Reading Authentication Method**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

WriteReaderWorkSetting_Parameter parameter = new WriteReaderWorkSetting_Parameter(commandDetail,
                                                                                  1);//取值范围1-4号门
DayTimeGroup_ReaderWork day1 = ret.ReaderWork.GetItem(E_WeekDay.Monday);//获取星期一的认证方式
TimeSegment_ReaderWork time1 = day1.GetItem(1);//1-8时段
time1.SetBeginTime(9,0);
time1.SetEndTime(18,0);
time1.SetWorkType(ReaderWorkType.OnlyCard,true);//设置仅读卡
time1.SetWorkType(ReaderWorkType.OnlyPassword,true);//设置仅密码
time1.SetWorkType(ReaderWorkType.CardAndPassword,true);//设置读卡加密码
WriteReaderWorkSetting cmd = new WriteReaderWorkSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteReaderWorkSetting) {          
        System.out.println("Write Card Reading Authentication Method succ"); 
    }    
}
```



#### 2.8 Door Operating Mode

**Read Door Operating Mode**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadDoorWorkSetting cmd = new ReadDoorWorkSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadDoorWorkSetting) {          
       ReadDoorWorkSetting_Result ret = (ReadDoorWorkSetting_Result) result;
        System.out.println("号门:"+ret.DoorNum); 
      	System.out.println("Door Operating Mode是否启用："+ret.Use);
        if(ret.Use){
            /**
            *1--普通
            *2--多卡
            *3--首卡
            *4--常开
            */
             System.out.println("Door Operating Mode："+ret.DoorWorkType);
            /**
            *1--合法卡在时段内即可常开;
            *2--授权中标记为常开卡的在指定时段内刷卡即可常开;
            *3--自动开关，到时间自动开关门
            */
        	 System.out.println("常开工作模式："+ret.HoldDoorOption);
            if(ret.DoorWorkType == 4 || ret.DoorWorkType == 4){
                E_WeekDay[] vDay = E_WeekDay.values();
        		for (int i = 0; i < 7; i++) {
            	    DayTimeGroup day = ret.timeGroup.GetItem(vDay);
                    System.out.println("时段:"+vDay); 
                    for (int d = 0; d < 8; d++) {
                        int num=d+1;
                        TimeSegment timeSegment = day.GetItem(d);
                        short[]mBeginTime = new short[2]; 
                        short[] mEndTime = new short[2]; 
                        timeSegment.GetBeginTime(mBeginTime);
                        timeSegment.GetEndTime(mEndTime);
                        System.out.println("开始时间"+num+"="+mBeginTime[0]+":"+mBeginTime[1]); 
                        System.out.println("结束时间"+num+"="+mEndTime[0]+":"+mEndTime[1]); 
                    }
       			 }
            }
        }
       
    }    
}
```

**Write Door Operating Mode**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

WriteDoorWorkSetting_Parameter parameter = new WriteDoorWorkSetting_Parameter(commandDetail);
parameter.Use = true;//Door Operating Mode是否启用
parameter.DoorNum = 1;//门号 取值：1-4
parameter.DoorWorkType = 4 //Door Operating Mode 1--普通;2--多卡;3--首卡;4--常开
parameter.HoldDoorOption = 3 //常开工作模式选项(工作模式常开方式有效)，1--合法卡在时段内即可常开；2--授权中标记为常开卡的在指定时段内刷卡即可常开；3--自动开关，到时间自动开关门
  //周时段 用于在工作模式设定为首卡，常开时，在时段内会生效
TimeSegment timeSegment = parameter.timeGroup.GetItem(E_WeekDay.Monday).GetItem(0);  
timeSegment.SetBeginTime(8,00);//08:00 -- 18:00 常开时间
timeSegment.SetEndTime(18,00);
WriteDoorWorkSetting cmd = new WriteDoorWorkSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteDoorWorkSetting) {          
        System.out.println("Write Door Operating Mode succ"); 
    }    
}
```

#### 2.9 Timed Door Locking

**Read Timed Door Locking**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadAutoLockedSetting cmd = new ReadAutoLockedSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadAutoLockedSetting) {    
        ReadAutoLockedSetting_Result ret = (ReadAutoLockedSetting_Result) result;      
        System.out.println("门号："+ret.DoorNum); 
         System.out.println("是否启用定时锁定功能："+ret.Use); 
        if(ret.Use){
             E_WeekDay[] vDay = E_WeekDay.values();
        		for (int i = 0; i < 7; i++) {
            	    DayTimeGroup day = ret.timeGroup.GetItem(vDay);
                    System.out.println("时段:"+vDay); 
                    for (int d = 0; d < 8; d++) {
                        int num=d+1;
                        TimeSegment timeSegment = day.GetItem(d);
                        short[]mBeginTime = new short[2]; 
                        short[] mEndTime = new short[2]; 
                        timeSegment.GetBeginTime(mBeginTime);
                        timeSegment.GetEndTime(mEndTime);
                        System.out.println("开始时间"+num+"="+mBeginTime[0]+":"+mBeginTime[1]); 
                        System.out.println("结束时间"+num+"="+mEndTime[0]+":"+mEndTime[1]); 
                    }
       			 }
        }
    }    
}
```

**Write Timed Door Locking**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

WriteAutoLockedSetting_Parameter parameter = new WriteAutoLockedSetting_Parameter(commandDetail);
parameter.Use = true;//是否启用定时锁定功能 ，功能开启后，在时段内的时候门将进入自动锁定status
parameter.DoorNum = 1;//门号 取值：1-4
  //周时段 定时锁定时段 
TimeSegment timeSegment = parameter.timeGroup.GetItem(E_WeekDay.Monday).GetItem(0);  
timeSegment.SetBeginTime(8,00);//08:00 -- 18:00 锁定时间
timeSegment.SetEndTime(18,00);
WriteAutoLockedSetting cmd = new WriteAutoLockedSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteAutoLockedSetting) {          
        System.out.println("Write Timed Door Locking succ"); 
    }    
}
```

#### 2.10 Output Duration when Unlocking

**Read Output Duration when Unlocking**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadRelayReleaseTime cmd = new ReadRelayReleaseTime	(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadRelayReleaseTime) {        
        ReadRelayReleaseTime_Result ret = (ReadRelayReleaseTime_Result) result;
        System.out.println("门号："+ret.Door); 
        System.out.println("保持输出时间："+ret.Door); //继电器开锁后释放时间 取值范围：0-65535；0表示0.5秒。单位：秒；
    }
}
```

**Write Output Duration when Unlocking**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
//1=门号；5=开锁保持时间5秒
WriteRelayReleaseTime_Parameter parameter = new WriteRelayReleaseTime_Parameter(commandDetail,1,5);//取值范围1-4号门
WriteRelayReleaseTime cmd = new WriteRelayReleaseTime(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteAutoLockedSetting) {          
        System.out.println("Write 开锁保持 succ"); 
    }    
}
```

#### 2.11 Card Reading Interval for Repeated Card Reading

**Read Card Reading Interval for Repeated Card Reading**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadReaderInterval cmd = new ReadReaderInterval(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadReaderInterval) {        
        ReadReaderInterval_Result ret = (ReadReaderInterval_Result) result;
        System.out.println("门号："+ret.Door); 
        System.out.println("启用Card Reading Interval for Repeated Card Reading功能："+ret.Use); 
        //1--记录读卡，不开门，有提示
        //2--不记录读卡，不开门，有提示
        //3--不做响应，无提示
        System.out.println("重复读卡时的记录响应模式："+ret.RecordOption); 
    }
}
```

**Write Card Reading Interval for Repeated Card Reading**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
//1=门号；true=是否Card Reading Interval for Repeated Card Reading功能;2--不记录读卡，不开门，有提示
WriteReaderInterval_Parameter parameter = new WriteReaderInterval_Parameter(commandDetail,1,true,2);//取值范围1-4号门
WriteReaderInterval cmd = new WriteReaderInterval(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteReaderInterval) {          
        System.out.println("Write Card Reading Interval for Repeated Card Reading succ"); 
    }    
}
```

#### 2.12 Illegal Card Reading alarm

**Read Illegal Card Reading alarm**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadInvalidCardAlarmOption cmd = new ReadInvalidCardAlarmOption(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadInvalidCardAlarmOption) {        
        ReadInvalidCardAlarmOption_Result ret = (ReadInvalidCardAlarmOption_Result) result;
        System.out.println("门号："+ret.DoorNum); 
        System.out.println("启用Illegal Card Reading alarm功能："+ret.Use); 
    }
}
```

**Write Illegal Card Reading alarm**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
//1=门号；true=是否Card Reading Interval for Repeated Card Reading功能;2--不记录读卡，不开门，有提示
WriteInvalidCardAlarmOption_Parameter parameter = new WriteInvalidCardAlarmOption_Parameter(commandDetail,1,true,2);//取值范围1-4号门
WriteInvalidCardAlarmOption cmd = new WriteInvalidCardAlarmOption(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteReaderInterval) {          
        System.out.println("Write Card Reading Interval for Repeated Card Reading succ"); 
    }    
}
```

#### 2.13 **Duress Alarm Password**

**Read Duress Alarm Password**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadAlarmPassword cmd = new ReadAlarmPassword(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadAlarmPassword) {        
        ReadAlarmPassword_Result ret = (ReadAlarmPassword_Result) result;
        System.out.println("门号："+ret.DoorNum); 
        System.out.println("启用胁迫alarm功能："+ret.Use); 
        System.out.println("Duress Alarm Password："+ret.Password); //Duress Alarm Password，最大长度8个字符，由数字组成。
         System.out.println("alarm选项："+ret.AlarmOption); //alarm选项:1--不开门，alarm输出;2--开门，alarm输出;3--锁定门，alarm，只能软件解锁
    }
}
```

**Write Duress Alarm Password**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
//1=门号；true=是否Card Reading Interval for Repeated Card Reading功能;2--不记录读卡，不开门，有提示
WriteAlarmPassword_Parameter parameter = new WriteAlarmPassword_Parameter(commandDetail,
                                                                          1,//门号
                                                                          true,//是否启用胁迫alarm
                                                                          "12345678"//Duress Alarm Password，最大长度8个字符，由数字组成
                                                                          2);//alarm选项:1--不开门，alarm输出;2--开门，alarm输出;3--锁定门，alarm，只能软件解锁
WriteAlarmPassword cmd = new WriteAlarmPassword(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteAlarmPassword) {          
        System.out.println("Write Duress Alarm Password succ"); 
    }    
}
```

#### 2.14 Anti-Return Parameters

**Read Anti-Return Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadAntiPassback cmd = new ReadAntiPassback(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadAntiPassback) {        
        ReadAntiPassback_Result ret = (ReadAntiPassback_Result) result;
        System.out.println("门号："+ret.DoorNum); //门端口在控制板中的索引号，取值：1-4
        System.out.println("启用防潜返功能："+ret.Use);  //是否启用防潜返功能 true 或 false
    }
}
```

**Write Anti-Return Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
//1=门号；true=是否Card Reading Interval for Repeated Card Reading功能;2--不记录读卡，不开门，有提示
WriteAntiPassback_Parameter parameter = new WriteAntiPassback_Parameter(commandDetail,
                                                                          1,//门号
                                                                          true);//是否启用胁迫alarm
WriteAntiPassback cmd = new WriteAntiPassback(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteAntiPassback) {          
        System.out.println("Write Anti-Return Parameters succ"); 
    }    
}
```

#### 2.15 Door Opening Timeout Alarm Function

**Read Door Opening Timeout Alarm Function**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadOvertimeAlarmSetting cmd = new ReadOvertimeAlarmSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadOvertimeAlarmSetting) {        
        ReadOvertimeAlarmSetting_Result ret = (ReadOvertimeAlarmSetting_Result) result;
        System.out.println("门号："+ret.DoorNum); //门端口在控制板中的索引号，取值：1-4
        System.out.println("启用Door Opening Timeout Alarm Function："+ret.Use);  //是否启用Door Opening Timeout Alarm Function true 或 false
        System.out.println("超时时间："+ret.Overtime);//超时时间，指门磁一直打开的时间,取值范围：0-65535,0表示关闭；单位秒
        System.out.println("在开门超时后，alarm输出："+ret.Alarm);// 在开门超时后，是否alarm输出 true 或 false
    }
}
```

**Write Door Opening Timeout Alarm Function**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
//1=门号；true=是否Card Reading Interval for Repeated Card Reading功能;2--不记录读卡，不开门，有提示
WriteOvertimeAlarmSetting_Parameter parameter = new WriteOvertimeAlarmSetting_Parameter(commandDetail,
                                                                          1,//门号
                                                                          true,//是否启用胁迫alarm
                                                                          10,//超时时间,指门磁一直打开的时间,取值范围：0-65535,0表示关闭；单位秒
                                                                          true);//在开门超时后，是否alarm输出
WriteOvertimeAlarmSetting cmd = new WriteOvertimeAlarmSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteOvertimeAlarmSetting) {          
        System.out.println("Write Door Opening Timeout Alarm Function succ"); 
    }    
}
```

#### 2.16 Exit Switch Parameters

**Read Exit Switch Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadPushButtonSetting cmd = new ReadPushButtonSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadPushButtonSetting) {        
        ReadPushButtonSetting_Result ret = (ReadPushButtonSetting_Result) result;
        System.out.println("号门:"+ret.DoorNum); //门端口在控制板中的索引号，取值：1-4
      	System.out.println("启用出门按钮功能："+ret.Use);//是否启用出门按钮功能 true 或 false
        System.out.println("启用出门按钮常开功能："+ret.NormallyOpen);//出门按钮按下5秒后门进入常开status，再次按5秒退出常开
	    System.out.println("出门按钮的使用时段:");
        E_WeekDay[] vDay = E_WeekDay.values();
        for (int i = 0; i < 7; i++) {
            DayTimeGroup day = ret.timeGroup.GetItem(vDay);
            System.out.println("时段:"+vDay); 
            for (int d = 0; d < 8; d++) {
                 int num=d+1;
                 TimeSegment timeSegment = day.GetItem(d);
                 short[]mBeginTime = new short[2]; 
                 short[] mEndTime = new short[2]; 
                 timeSegment.GetBeginTime(mBeginTime);
                 timeSegment.GetEndTime(mEndTime);
                 System.out.println("开始时间"+num+"="+mBeginTime[0]+":"+mBeginTime[1]); 
                 System.out.println("结束时间"+num+"="+mEndTime[0]+":"+mEndTime[1]); 
            }
       	}
    }
}
```

**Write Exit Switch Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

WritePushButtonSetting_Parameter parameter = new WritePushButtonSetting_Parameter(commandDetail,
                                                                                 1,//门号 取值：1-4
                                                                                 true,//是否启用出门按钮功能
                                                                                 true);//是否启用出门按钮常开功能,出门按钮按下5秒后门进入常开status，再次按5秒退出常开

  //出门按钮的使用时段
TimeSegment timeSegment = parameter.timeGroup.GetItem(E_WeekDay.Monday).GetItem(0);  
timeSegment.SetBeginTime(8,00);//08:00 -- 18:00 使用出门按钮时间
timeSegment.SetEndTime(18,00);
WritePushButtonSetting cmd = new WritePushButtonSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WritePushButtonSetting) {          
        System.out.println("Write 门开关参数 succ"); 
    }    
}
```

#### 2.17 Door Magnetic Alarm Parameters

**Read Door Magnetic Alarm Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadSensorAlarmSetting cmd = new ReadSensorAlarmSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadSensorAlarmSetting) {        
        ReadSensorAlarmSetting_Result ret = (ReadSensorAlarmSetting_Result) result;
        System.out.println("号门:"+ret.DoorNum); //门端口在控制板中的索引号，取值：1-4
      	System.out.println("启用门磁alarm功能："+ret.Use);//是否启用门磁alarm功能 true 或 false
        System.out.println("启用出门按钮常开功能："+ret.NormallyOpen);//出门按钮按下5秒后门进入常开status，再次按5秒退出常开
	    System.out.println("门磁alarm不alarm时段:");//注意：这里的时段规定的是不alarm时段，即在功能开启后，如果在时段内，门磁随意打开不会alarm
        E_WeekDay[] vDay = E_WeekDay.values();
        for (int i = 0; i < 7; i++) {
            DayTimeGroup day = ret.timeGroup.GetItem(vDay);
            System.out.println("时段:"+vDay); 
            for (int d = 0; d < 8; d++) {
                 int num=d+1;
                 TimeSegment timeSegment = day.GetItem(d);
                 short[]mBeginTime = new short[2]; 
                 short[] mEndTime = new short[2]; 
                 timeSegment.GetBeginTime(mBeginTime);
                 timeSegment.GetEndTime(mEndTime);
                 System.out.println("开始时间"+num+"="+mBeginTime[0]+":"+mBeginTime[1]); 
                 System.out.println("结束时间"+num+"="+mEndTime[0]+":"+mEndTime[1]); 
            }
       	}
    }
}
```

**Write Door Magnetic Alarm Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

WriteSensorAlarmSetting_Parameter parameter = new WriteSensorAlarmSetting_Parameter(commandDetail,
                                                                                 1,//门号 取值：1-4
                                                                                 true);//是否启用门磁alarm功能

  //门磁alarm不alarm时段
TimeSegment timeSegment = parameter.timeGroup.GetItem(E_WeekDay.Monday).GetItem(0);  
//注意：这里的时段规定的是不alarm时段，即在功能开启后，如果在时段内，门磁随意打开不会alarm
timeSegment.SetBeginTime(8,00);//08:00 -- 18:00 
timeSegment.SetEndTime(18,00);
WriteSensorAlarmSetting cmd = new WriteSensorAlarmSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WritePushButtonSetting) {          
        System.out.println("Write Door Magnetic Alarm Parameters succ"); 
    }    
}
```

#### 2.18 All-Card Door Opening Parameters

**Read All-Card Door Opening Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
DoorPort_Parameter parameter = new DoorPort_Parameter(commandDetail,1);//取值范围1-4号门
ReadAnyCardSetting cmd = new ReadAnyCardSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadAnyCardSetting) {        
        ReadAnyCardSetting_Result ret = (ReadAnyCardSetting_Result) result;
        System.out.println("号门:"+ret.DoorNum); //门端口在控制板中的索引号，取值：1-4
      	System.out.println("启用全卡开门功能："+ret.Use);//是否启用全卡开门功能 true 或 false
        System.out.println("启用在刷卡开门后保存卡片权限："+ret.AutoSave);//是否启用在刷卡开门后保存卡片权限，保存后，以后关闭全卡功能，此卡也能开门
	    System.out.println("刷卡注册时段:"+ret.AutoSaveTimeGroupIndex);//取值范围：1-64,当 AutoSave = true; 时，可设定在哪个时段刷卡才会进行注册
    }
}
```

**Write All-Card Door Opening Parameters**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object

WriteAnyCardSetting_Parameter parameter = new WriteAnyCardSetting_Parameter(commandDetail,
                                                                            1,//门号 取值：1-4
                                                                            true,//是否启用全卡开门功能
                                                                            true,//是否启用在刷卡开门后保存卡片权限,保存后，以后关闭全卡功能，此卡也能开门
                                                                            1);//取值范围：1-64 ,当 AutoSave = true; 时，可设定在哪个时段刷卡才会进行注册

WriteAnyCardSetting cmd = new WriteAnyCardSetting(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WritePushButtonSetting) {          
        System.out.println("Write All-Card Door Opening Parameters succ"); 
    }    
}
```

### 3.Door Opening Card Settings

####**3.1 Get Controller Card Data**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
/**
*Type of Card Database to be Read
*1--Sort Card Area
*2--Non-Sort Card Area
*3--All Area
*/
ReadCardDataBase_Parameter parameter = new ReadCardDataBase_Parameter(commandDetail,3);
//Please note that regular controllers use the class methods under the Door.Access.Door8800.Command.Card package.
//Advanced controllers use the class methods under the Door.Access.Door89H.Command.Card package.
ReadCardDataBase cmd = new ReadCardDataBase(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof ReadCardDataBase) { 
        //Please note that regular controllers use the class methods under the Door.Access.Door8800.Command.Card package.
		//Advanced controllers use the class methods under the Door.Access.Door89H.Command.Card package.
        ReadCardDataBase_Result ret = (ReadCardDataBase_Result) result;
        System.out.println("卡片区域类型："+ret.CardType);
        System.out.println("Read 到的卡片数量："+ret.DataBaseSize);
        System.out.println("卡片信息：");
        for (int i = 0; i < ret.DataBaseSize; i++) {
			CardDetail card = ret.CardList.get(i);
           	System.out.println("卡号："+card.GetCardData());//取值范围  普通控制器 0x1-0xFFFFFFFF 高级控制器 0x1-0xFFFFFFFFFFFFFFFF
            System.out.println("卡密码："+card.Password);//无密码不填。密码是4-8位的数字
            System.out.println("截止日期："+card.Expiry);//最大2089年12月31日
            System.out.println("1门Door Opening Time Slot："+card.GetTimeGroup(1));
            System.out.println("2门Door Opening Time Slot："+card.GetTimeGroup(2));
            System.out.println("3门Door Opening Time Slot："+card.GetTimeGroup(3));
            System.out.println("4门Door Opening Time Slot："+card.GetTimeGroup(4));
            System.out.println("有效次数："+card.OpenTimes);// 取值范围：0-65535
            System.out.println("卡片status："+card.CardStatus);//0--正常status；1--挂失；2-- Blacklist 
            System.out.println("节假日限制："+card.HolidayUse);//true--受限；false--不受限
			System.out.println("1门是否有权限："+card.GetDoor(1));//true 有权限，false 无权限
            System.out.println("2门是否有权限："+card.GetDoor(2));
            System.out.println("3门是否有权限："+card.GetDoor(3));
            System.out.println("4门是否有权限："+card.GetDoor(4));            
            System.out.println("节假日开关status："+card.GetHolidayValue(1));//取值范围 1-30
            System.out.println("普通卡："+card.IsNormal());
            System.out.println("首卡："+card.IsPrivilege());
            System.out.println("常开："+card.IsTiming());
            System.out.println("巡更："+card.IsGuardTour());
            System.out.println("防盗设置卡："+card.IsAlarmSetting());
     	}
    }    
}
```

####**3.2 Read Controller Card**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
/**
*Type of Card Database to be Read
*1--Sort Card Area
*2--Non-Sort Card Area
*3--All Area
*/
ReadCardDetail_Parameter parameter = new ReadCardDetail_Parameter(commandDetail,"1938495734");
//请注意普通控制器使用Door.Access.Door8800.Command.Card 包下的类方法
//高级控制器使用Door.Access.Door89H.Command.Card 包下的类方法
ReadCardDetail cmd = new ReadCardDetail(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof ReadCardDetail) { 
        //请注意普通控制器使用Door.Access.Door8800.Command.Card 包下的类方法
        //高级控制器使用Door.Access.Door89H.Command.Card 包下的类方法
        ReadCardDetail_Result ret = (ReadCardDetail_Result) result;
        System.out.println("卡片是否存在："+ret.IsReady);
        if(ret.IsReady){
            System.out.println("卡片信息：");  
            //请注意普通控制器使用Door.Access.Door8800.Command.Data 包下的类
            //高级控制器使用Door.Access.Door89H.Command.Data 包下的类
            CardDetail card = ret.Card;
            System.out.println("卡号："+card.GetCardData());//取值范围  普通控制器 0x1-0xFFFFFFFF 高级控制器 0x1-0xFFFFFFFFFFFFFFFF
            System.out.println("卡密码："+card.Password);//无密码不填。密码是4-8位的数字
            System.out.println("截止日期："+card.Expiry);//最大2089年12月31日
            System.out.println("1门Door Opening Time Slot："+card.GetTimeGroup(1));
            System.out.println("2门Door Opening Time Slot："+card.GetTimeGroup(2));
            System.out.println("3门Door Opening Time Slot："+card.GetTimeGroup(3));
            System.out.println("4门Door Opening Time Slot："+card.GetTimeGroup(4));
            System.out.println("有效次数："+card.OpenTimes);// 取值范围：0-65535
            System.out.println("卡片status："+card.CardStatus);//0--正常status；1--挂失；2-- Blacklist 
            System.out.println("节假日限制："+card.HolidayUse);//true--受限；false--不受限
            System.out.println("1门是否有权限："+card.GetDoor(1));//true 有权限，false 无权限
            System.out.println("2门是否有权限："+card.GetDoor(2));
            System.out.println("3门是否有权限："+card.GetDoor(3));
            System.out.println("4门是否有权限："+card.GetDoor(4));            
            System.out.println("节假日开关status："+card.GetHolidayValue(1));//取值范围 1-30
            System.out.println("普通卡："+card.IsNormal());
            System.out.println("首卡："+card.IsPrivilege());
            System.out.println("常开："+card.IsTiming());
            System.out.println("巡更："+card.IsGuardTour());
            System.out.println("防盗设置卡："+card.IsAlarmSetting());
        }
      
    }    
}
```

####**3.3 Delete Card**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
String[] cardDatas = new String[1];//卡号数组
cardDatas[0] = "1938495734";
DeleteCard_Parameter parameter =  new DeleteCard_Parameter(commandDetail , cardDatas)
DeleteCard cmd = new DeleteCard(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof DeleteCard) {            
        System.out.println("Delete Card succ");
    }    
}
```

#### 3.5 Write Card Number to Non-Sort Area

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ArrayList<CardDetail> cardList = new ArrayList<CardDetail>();
HolidayTest holiday = new HolidayTest();
holiday.Test();
CardDetail card = new CardDetail();
card.SetCardData("1938495734");
card.Expiry = Calendar.getInstance();
card.Expiry.set(2023, 9, 10);//2023-10-10
card.SetTimeGroup(1, 1);//门号，取值范围：1-4 ,Door Opening Time Slot号，取值范围：1-64
card.SetTimeGroup(2, 1);
card.SetTimeGroup(3, 1);
card.SetTimeGroup(4, 1);
card.OpenTimes = 65535;//开门次数 取值范围：0-65535 65535表示无限制次数，0表示开门次数耗尽
card.CardStatus = 0;//正常卡号
card.HolidayUse = false;//节假日受限
//门号，取值范围：1-4 true 有权限，false 无权限
card.SetDoor(1, true);
card.SetDoor(2, true);
card.SetDoor(3, true);
card.SetDoor(4, true);
card.SetNormal();//设置为普通卡
cardList.add(card);
WriteCardListBySequence_Parameter parameter =  new WriteCardListBySequence_Parameter(commandDetail , cardList)
//控制器非排序区Write 少量卡时效率高
//但是当区域内存储卡片超过5000时，Write 卡片效率会逐渐降低
//建议：当Write 卡数量大于2000张时，应清空所有卡，然后将所有卡片上传到排序区
//请注意普通控制器使用Door.Access.Door8800.Command.Card 包下的类方法
//高级控制器使用Door.Access.Door89H.Command.Card 包下的类方法
WriteCardListBySequence cmd = new WriteCardListBySequence(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof WriteCardListBySequence) {            
          WriteCardListBySequence_Result ret = (WriteCardListBySequence_Result) result;
            if (ret.OverflowCount > 0)
                System.out.println("卡号溢出 数量" + ret.OverflowCount);
            else if (ret.FailTotal > 0) {
                System.out.println("上传失败卡号数量：" + ret.FailTotal);
            } else {
                System.out.println("上传 succ");
            }
    }    
}
```

#### 3.6 Write Card Number to Sort Area

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ArrayList<CardDetail> cardList = new ArrayList<CardDetail>();
HolidayTest holiday = new HolidayTest();
holiday.Test();
CardDetail card = new CardDetail();
card.SetCardData("1938495734");
card.Expiry = Calendar.getInstance();
card.Expiry.set(2023, 9, 10);//2023-10-10
card.SetTimeGroup(1, 1);//门号，取值范围：1-4 ,Door Opening Time Slot号，取值范围：1-64
card.SetTimeGroup(2, 1);
card.SetTimeGroup(3, 1);
card.SetTimeGroup(4, 1);
card.OpenTimes = 65535;//开门次数 取值范围：0-65535 65535表示无限制次数，0表示开门次数耗尽
card.CardStatus = 0;//正常卡号
card.HolidayUse = false;//节假日受限
//门号，取值范围：1-4 true 有权限，false 无权限
card.SetDoor(1, true);
card.SetDoor(2, true);
card.SetDoor(3, true);
card.SetDoor(4, true);
card.SetNormal();//设置为普通卡
cardList.add(card);
//注意 添加卡片的时候需要按照卡号的大小顺序从小到大进行排序，否则会导致卡片不可识别
WriteCardListBySort_Parameter parameter =  new WriteCardListBySort_Parameter(commandDetail , cardList)
//控制器非排序区Write 少量卡时效率高
//但是当区域内存储卡片超过5000时，Write 卡片效率会逐渐降低
//建议：当Write 卡数量大于2000张时，应清空所有卡，然后将所有卡片上传到排序区
//请注意普通控制器使用Door.Access.Door8800.Command.Card 包下的类方法
//高级控制器使用Door.Access.Door89H.Command.Card 包下的类方法
WriteCardListBySort cmd = new WriteCardListBySort(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof WriteCardListBySort) {            
        System.out.println("Write 排序区卡片 succ");
    }    
}
```

#### 3.7 Read Controller Card Storage Details

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadCardDatabaseDetail cmd = new ReadCardDatabaseDetail(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof ReadCardDatabaseDetail) {          
        ReadCardDatabaseDetail_Result ret = (ReadCardDatabaseDetail_Result) result;
        System.out.println("排序区容量上限："+ret.SortDataBaseSize);
        System.out.println("排序区已使用数量："+ret.SortCardSize);
        System.out.println("非排序区容量上限："+ret.SequenceDataBaseSize);
        System.out.println("非排序区已使用数量："+ret.SequenceCardSize);
    }    
}
```

#### 3.8 Clear All Cards in Controller

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
/**
*Type of Card Database to be Read
*1--Sort Card Area
*2--Non-Sort Card Area
*3--All Area
*/
ClearCardDataBase_Parameter parameter = new ClearCardDataBase_Parameter(commandDetail,3);
ClearCardDataBase cmd = new ClearCardDataBase(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof ClearCardDataBase) {             
        System.out.println("Clear All Cards in Controller succ");
    }    
}
```



### 4.Time and Date

#### 4.1 Read Device Time

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadTime cmd = new ReadTime(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof ReadTime) {          
        ReadTime_Result ret = (ReadTime_Result) result;
        System.out.println("Device Time："+ret.ControllerDate);
    }    
}
```

#### 4.1.2 Write Device Time

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteTime cmd = new WriteTime(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result;) {
    if (cmd instanceof WriteTime) {          
        System.out.println("Write Device Time succ"); 
    }    
}
```

#### 4.1.3 Write Custom Time

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
WriteTimeDefine_Parameter parameter = new WriteTimeDefine_Parameter(commandDetail，Calendar.getInstance());
WriteTimeDefine cmd = new WriteTimeDefine(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteTimeDefine) {          
        System.out.println("Write Custom Time succ"); 
    }    
}
```

###5.Event Log

####5.1 Read Record (Storage) Pointer Information

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
CommandParameter parameter = new CommandParameter(commandDetail);
ReadTransactionDatabaseDetail cmd = new ReadTransactionDatabaseDetail(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadTransactionDatabaseDetail) {          
      ReadTransactionDatabaseDetail_Result ret = (ReadTransactionDatabaseDetail_Result)  result;
        var cardTransaction = ret.DatabaseDetail.CardTransactionDetail;
        System.out.println("读卡记录："); 
        printTransactionDetail(cardTransaction);
        
       var buttonTransaction = ret.DatabaseDetail.ButtonTransactionDetail;
        System.out.println("按钮记录："); 
        printTransactionDetail(buttonTransaction);
        
        var doorSensorTransaction = ret.DatabaseDetail.DoorSensorTransactionDetail;
        System.out.println("门磁记录："); 
        printTransactionDetail(doorSensorTransaction);
        
        var softwareTransaction = ret.DatabaseDetail.SoftwareTransactionDetail;
        System.out.println("远程记录："); 
        printTransactionDetail(softwareTransaction);
        
        var alarmTransaction = ret.DatabaseDetail.AlarmTransactionDetail;
        System.out.println("alarm记录："); 
        printTransactionDetail(alarmTransaction);
        
        var systemTransaction = ret.DatabaseDetail.SystemTransactionDetail;
        System.out.println("系统记录："); 
        printTransactionDetail(systemTransaction);

    }    
}
private void printTransactionDetail( TransactionDetail detail){
    System.out.print("记录容量："+detail.DataBaseMaxSize);
    System.out.print("\t"); 
    System.out.print("新纪录数："+detail.NewSzie());
    System.out.print("\t"); 
    System.out.print("记录尾号："+detail.WriteIndex);
    System.out.print("\t"); 
    System.out.print("上传断点："+detail.ReadIndex);
    System.out.print("\t");
    System.out.print("循环标志："+detail.IsCircle);
} 
```



#### 5.2  Clear All Records

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
CommandParameter parameter = new CommandParameter(commandDetail);
TransactionDatabaseEmpty cmd = new TransactionDatabaseEmpty(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof TransactionDatabaseEmpty) {          
        System.out.println("Clear All Records succ"); 
    }    
}
```

#### 5.3 Clear Records

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
  /**
     * Record Enumeration Type
     * 1 -- Card Reading Record
     * 2 -- Exit Switch Record
     * 3 -- Door Magnetic Record
     * 4 -- Software Operation Record
     * 5 -- Alarm Record
     * 6 -- System Record
     */
ClearTransactionDatabase_Parameter parameter = new ClearTransactionDatabase_Parameter(commandDetail,e_TransactionDatabaseType.OnCardTransaction);
ClearTransactionDatabase cmd = new ClearTransactionDatabase(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ClearTransactionDatabase) {          
        System.out.println("Clear Records succ"); 
    }    
}
```

#### 5.4 Update Record (Storage) Pointer

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
  /**
     * Record Enumeration Type
     * 1 -- Card Reading Record
     * 2 -- Exit Switch Record
     * 3 -- Door Magnetic Record
     * 4 -- Software Operation Record
     * 5 -- Alarm Record
     * 6 -- System Record
     */
WriteTransactionDatabaseReadIndex_Parameter parameter = new WriteTransactionDatabaseReadIndex_Parameter(commandDetail,e_TransactionDatabaseType.OnCardTransaction);
parameter.ReadIndex = 60; //表示当前Read 记录的所在位置，如果与记录尾号相等，那么表示记录已经Read 完毕
parameter.IsCircle = true;//当记录存储数量超过容量大小时，是否循环覆盖
WriteTransactionDatabaseReadIndex cmd = new WriteTransactionDatabaseReadIndex(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteTransactionDatabaseReadIndex) {          
        System.out.println("Update Record (Storage) Pointer succ"); 
    }    
}
```

#### 5.5 Update Record Tail Number

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
  /**
     * Record Enumeration Type
     * 1 -- Card Reading Record
     * 2 -- Exit Switch Record
     * 3 -- Door Magnetic Record
     * 4 -- Software Operation Record
     * 5 -- Alarm Record
     * 6 -- System Record
     */
WriteTransactionDatabaseWriteIndex_Parameter parameter = new WriteTransactionDatabaseWriteIndex_Parameter(commandDetail,e_TransactionDatabaseType.OnCardTransaction);
parameter.WriteIndex = 60; //记录尾号的位置，（注意不可随意更改，避免出现无效记录）
WriteTransactionDatabaseWriteIndex cmd = new WriteTransactionDatabaseWriteIndex(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WriteTransactionDatabaseWriteIndex) {          
        System.out.println("Update Record Tail Number succ"); 
    }    
}
```

#### 5.6 Read New Record

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
  /**
     * Record Enumeration Type
     * 1 -- Card Reading Record
     * 2 -- Exit Switch Record
     * 3 -- Door Magnetic Record
     * 4 -- Software Operation Record
     * 5 -- Alarm Record
     * 6 -- System Record
     */
ReadTransactionDatabase_Parameter parameter = new ReadTransactionDatabase_Parameter(commandDetail,e_TransactionDatabaseType.OnCardTransaction);
parameter.Quantity = 500; //Read 数量 0-160000,0表示都取所有新记录
parameter.PacketSize = 60; //每次Read 数量 1-300 (一次最好不要Read 太多60左右是比较好的)
ReadTransactionDatabase cmd = new ReadTransactionDatabase(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadTransactionDatabase) {  
        ReadTransactionDatabase_Result ret = (ReadTransactionDatabase_Result) result;
        
        System.out.println("记录类型："+ret.DatabaseType); 
        System.out.println("Read 数量："+ret.Quantity); 
        System.out.println("剩余新记录数量："+ret.readable); 
        int size = ret.TransactionList.size();
         for (int i = 0; i < size; i++) {
             //记录类型根据 查询是的枚举类型进行判断
             //读卡记录：CardTransaction---读卡记录需要判断控制器的类型 如果是89H类型控制板请使用89H包下的记录类型
             //出门开关：ButtonTransaction
             //门磁记录：DoorSensorTransaction
             //远程操作记录：SoftwareTransaction
             //alarm记录：AlarmTransaction
             //系统记录：SystemTransaction
            CardTransaction transaction =(CardTransaction) ret.TransactionList.get(i); 
             if(!transaction.IsNull()){
                 System.out.println("记录序号："+transaction.SerialNumber); 
                 System.out.println("卡号十六进制："+transaction.CardDataHex); 
                 System.out.println("卡号："+transaction.CardData); 
                 System.out.println("读卡器号："+transaction.Reader); 
                 System.out.println("门号："+transaction.DoorNum()); 
                 System.out.println("进口方向："+transaction.IsEnter()); 
                 System.out.println("出口方向："+transaction.IsExit());  
                 System.out.println("记录时间："+transaction.TransactionDate());  
                 System.out.println("记录类型："+transaction.TransactionType());  //详情查询文档末尾的记录代码
                 System.out.println("记录代码："+transaction.TransactionCode());  
             }
        }
    }    
}
```

#### 5.7 Read Record with Specified Index Range

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
  /**
     * Record Enumeration Type
     * 1 -- Card Reading Record
     * 2 -- Exit Switch Record
     * 3 -- Door Magnetic Record
     * 4 -- Software Operation Record
     * 5 -- Alarm Record
     * 6 -- System Record
     */
ReadTransactionDatabaseByIndex_Parameter parameter = new ReadTransactionDatabaseByIndex_Parameter(commandDetail,e_TransactionDatabaseType.OnCardTransaction);
parameter.ReadIndex = 1; //记录的序号（索引号），既是记录断点，填写你需要采集的记录开始位置，不能大于记录尾号
parameter.Quantity = 60; //每次Read 数量 1-300 (一次最好不要Read 太多60左右是比较好的)
ReadTransactionDatabaseByIndex cmd = new ReadTransactionDatabaseByIndex(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadTransactionDatabaseByIndex) {  
        ReadTransactionDatabaseByIndex_Result ret = (ReadTransactionDatabaseByIndex_Result) result;
        
        System.out.println("记录类型："+ret.DatabaseType); 
        System.out.println("Read 数量："+ret.Quantity); 
        System.out.println("记录索引号："+ret.ReadIndex); 
        int size = ret.TransactionList.size();
         for (int i = 0; i < size; i++) {
             //记录类型根据 查询是的枚举类型进行判断
             //读卡记录：CardTransaction---读卡记录需要判断控制器的类型 如果是89H类型控制板请使用89H包下的记录类型
             //出门开关：ButtonTransaction
             //门磁记录：DoorSensorTransaction
             //远程操作记录：SoftwareTransaction
             //alarm记录：AlarmTransaction
             //系统记录：SystemTransaction
            CardTransaction transaction =(CardTransaction) ret.TransactionList.get(i); 
             if(!transaction.IsNull()){
                 System.out.println("记录序号："+transaction.SerialNumber); 
                 System.out.println("卡号十六进制："+transaction.CardDataHex); 
                 System.out.println("卡号："+transaction.CardData); 
                 System.out.println("读卡器号："+transaction.Reader); 
                 System.out.println("门号："+transaction.DoorNum()); 
                 System.out.println("进口方向："+transaction.IsEnter()); 
                 System.out.println("出口方向："+transaction.IsExit());  
                 System.out.println("记录时间："+transaction.TransactionDate());  
                 System.out.println("记录类型："+transaction.TransactionType());  //详情查询文档末尾的记录代码
                 System.out.println("记录代码："+transaction.TransactionCode());  
             }
        }
    }    
}
```



### 6.Door Opening Password

#### 6.1 Read Password Capacity Information

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadPasswordDataBaseDetail cmd = new ReadPasswordDataBaseDetail(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadPasswordDataBaseDetail) {  
        ReadPasswordDataBaseDetail_Result ret = (ReadPasswordDataBaseDetail_Result) result;
        System.out.println("密码容量："+ret.Capacity); 
        System.out.println("已存数量："+ret.UseNumber); 
    }    
}
```

#### 6.2 Clear All Passwords

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ClearPasswordDateBase cmd = new ClearPasswordDateBase(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ClearPasswordDateBase) {     
        System.out.println("Clear All Door Opening Password succ"); 
    }    
}
```

#### 6.3 Read All Passwords

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadPasswordDataBase cmd = new ReadPasswordDataBase(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadPasswordDataBase) {     
         ReadPasswordDataBase_Result ret = (ReadPasswordDataBase_Result) result;
         System.out.println("密码数量："+ret.DataBaseSize); 
         int size = ret.PasswordDetails.size();
        for( int i = 0; i < size; i++){
            //8800和89H的类型需要进行区分，根据控制板的型号选择引用相应的包
            PasswordDetail detail = (PasswordDetail) ret.PasswordDetails.get(i);
             System.out.println("1号门权限："+ret.GetDoor(1)); 
             System.out.println("2号门权限："+ret.GetDoor(2)); 
             System.out.println("3号门权限："+ret.GetDoor(3)); 
             System.out.println("4号门权限："+ret.GetDoor(4)); 
             System.out.println("密码："+ret.Password); 
            
            //89H系列控制板独有参数
             System.out.println("开门次数："+ret.OpenTimes); 
             System.out.println("有效期："+ret.Expiry); 
        }
    }    
}
```

#### 6.4 Add Passwords

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ArrayList<PasswordDetail> passwordDetails = new ArrayList<PasswordDetail>();
PasswordDetail detail = new PasswordDetail();
detail.SetDoor(1,true); //设置对应门的开门权限
detail.SetDoor(2,true);
detail.SetDoor(3,true);
detail.SetDoor(4,true);
detail.Password = "12345678"; //Door Opening Password
//89H系列独有参数
detail.OpenTimes = 100; //开门次数
detail.Expiry = Calendar.getInstance();
detail.Expiry.set(2024, 10 - 1, 28, 10, 10, 10);//设置有效时间
WritePassword_Parameter parameter = new WritePassword_Parameter(commandDetail,passwordDetails)
WritePassword cmd = new WritePassword(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof WritePassword) {     
        WritePassword_Result ret = (WritePassword_Result) result;
   		if(ret.OverflowCount > 0){
             System.out.println("添加密码 密码数量已超过上线："+ret.OverflowCount); 
        } else{
            
		 System.out.println("添加密码 succ"); 
          System.out.println("添加密码 错误数量："+ret.ErrorCount); 
            if(ret.ErrorCount > 0)
             System.out.println("添加密码 错误错误详情："+ret.ErrorDetails); 
        }
       
    }    
}
```

#### 6.5 Delete Passwords

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
String[] PasswordsStrings = new String[1] ;
PasswordsStrings[0]="12345678";
WritePassword_Parameter parameter = new WritePassword_Parameter(commandDetail,PasswordsStrings)
DeletePassword cmd = new DeletePassword(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof DeletePassword) {     
        System.out.println("del Door Opening Password succ"); 
    }    
}
```

#### 

### 7.Door Opening Time Slot

####7.1 **Read Door Opening Time Slot**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadTimeGroup cmd = new ReadTimeGroup(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadTimeGroup) {    
        ReadTimeGroup_Result ret = (ReadTimeGroup_Result) result;
        System.out.println("返回的Door Opening Time Slot数量："+ret.DataBaseSize);
        for (int i = 0; i < ret.DataBaseSize; i++) {
            WeekTimeGroup timeGroup =  ret.List.get(i);
            System.out.println("时段索引号："+timeGroup.GetIndex());//索引号 1-64
            DayTimeGroup dayMondayGroup =  timeGroup.GetItem(E_WeekDay.Monday);
            System.out.println("星期一时段：");
            printDayTimeGroup(daySundayGroup);
            
            DayTimeGroup dayTuesdayGroup =  timeGroup.GetItem(E_WeekDay.Tuesday);
            System.out.println("星期二时段：");
            printDayTimeGroup(daySundayGroup);
            
            DayTimeGroup dayWednesdayGroup =  timeGroup.GetItem(E_WeekDay.Wednesday);
            System.out.println("星期三时段：");
            printDayTimeGroup(daySundayGroup);
            
            DayTimeGroup dayThursdayGroup =  timeGroup.GetItem(E_WeekDay.Thursday);
            System.out.println("星期四时段：");
            printDayTimeGroup(daySundayGroup);
            
            DayTimeGroup dayFridayGroup =  timeGroup.GetItem(E_WeekDay.Friday);
            System.out.println("星期五时段：");
            printDayTimeGroup(daySundayGroup);
            
            DayTimeGroup daySaturdayGroup =  timeGroup.GetItem(E_WeekDay.Saturday);
            System.out.println("星期六时段：");
            printDayTimeGroup(daySundayGroup);
            
            DayTimeGroup daySundayGroup =  timeGroup.GetItem(E_WeekDay.Sunday);
            System.out.println("星期日时段：");
            printDayTimeGroup(daySundayGroup);
        }
    }    
}
public void printDayTimeGroup(DayTimeGroup dayGroup){
     for (int i = 0; i < 8; i++) {//每天一共8个小时段
         TimeSegment time = dayGroup.GetItem(i);
         int group=i+1;
         short[] beginTime = new short[2];
         short[] endTime = new short[2];
         time.GetBeginTime(beginTime);
         time.GetEndTime(endTime);
         System.out.println("开始时间"+group+"："+beginTime[0]+":"+beginTime[1]);
         System.out.println("结束时间"+group+"："+endTime[0]+":"+endTime[1]);
     }
}

```

####7.2 **Write Door Opening Time Slot**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ArrayList<WeekTimeGroup> List = new ArrayList<WeekTimeGroup>();//时段分组集合，最大64
WeekTimeGroup timeGroup = new WeekTimeGroup(8);
timeGroup.SetIndex(1);//索引号 1-64 最大64个时段分组
DayTimeGroup dayMondayGroup =  timeGroup.GetItem(E_WeekDay.Monday); //获取星期一的时段分组
TimeSegment segment1 = dayMondayGroup.GetItem(0);
//可以设置8个时段，现在只设置8个时段中的第一个时段
segment1.SetBeginTime(9,0);//09:00 上午9点开始
segment1.SetEndTime(18,00);//18:00 下午18点结束
List.add(timeGroup);
AddTimeGroup_Parameter parameter = new AddTimeGroup_Parameter(commandDetail,List);
AddTimeGroup cmd = new AddTimeGroup(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof AddTimeGroup) {          
        System.out.println("Write Door Opening Time Slot succ"); 
    }    
}
```

####7.3 **Clear Door Opening Time Slot**

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ClearTimeGroup cmd = new ClearTimeGroup(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ClearTimeGroup) {          
        System.out.println("清空Door Opening Time Slot succ"); 
    }    
}
```

### 8. Holiday

#### 8.1 Read Holiday Capacity Information

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadHolidayDetail cmd = new ReadHolidayDetail(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadHolidayDetail) {       
        ReadHolidayDetail_Result ret = (ReadHolidayDetail_Result) result;
        System.out.println("存储的最大容量"+ret.Detail.Capacity); 
        System.out.println("已存储的数量"+ret.Detail.Count); 
    }    
}
```

#### 8.2 Clear all holidays.

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ClearHoliday cmd = new ClearHoliday(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ClearHoliday) {          
        System.out.println("Clear all holidays. succ"); 
    }    
}
```

#### 8.3 Read all holidays.



```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ReadAllHoliday cmd = new ReadAllHoliday(new CommandParameter(commandDetail));
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof ReadAllHoliday) {       
        ReadAllHoliday_Result ret = (ReadAllHoliday_Result) result;
        int size =  ret.Holidays.size();
         for (int i = 0; i < size; i++) {
            HolidayDetail detail =  ret.Holidays.get(i);
            System.out.println("节假日的索引号"+detail.Index);
            System.out.println("节假日日期"+detail.Holiday); 
             /**
             * 节假日类型 
             * 1、上午 (00:00:00 - 11:59:59)<br>
             * 2、下午 (12:00:00 - 23:59:59)<br>
             * 3、全天 (00:00:00 - 23:59:59)
             */
            System.out.println("节假日类型"+detail.HolidayType); 
            System.out.println("表示，是否每年循环使用"+detail.YearLoop); 
        }
    }    
}
```

#### 8.4 Add holidays.

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ArrayList<HolidayDetail> Holidays = new ArrayList<HolidayDetail>() ;
HolidayDetail detail = new HolidayDetail();
detail.Index = 1;//节假日的索引号 1-30,最大支持30个节假日
detail.Holiday = Calendar.getInstance();//节假日时间
detail.Holiday.set(2022,10-1,1);//设置十月1号进行节假日限制
 /**
 * 节假日类型 
 * 1、上午 (00:00:00 - 11:59:59)<br>
 * 2、下午 (12:00:00 - 23:59:59)<br>
 * 3、全天 (00:00:00 - 23:59:59)
 */            
detail.HolidayType = 3;//设置全天不开门
detail.YearLoop = true;//表示每年是否十月1号都进行节假日限制
AddHoliday_Parameter parameter = new AddHoliday_Parameter(commandDetail,Holidays);
AddHoliday cmd = new AddHoliday(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof AddHoliday) {          
        System.out.println("添加节假日 succ"); 
    }    
}
```



#### 8.5 Del holidays.

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
ArrayList<Integer> indexList = new ArrayList<Integer>();
indexList.add(1);//节假日索引号列表1-30；
DeleteHoliday_Parameter parameter = new DeleteHoliday_Parameter(commandDetail,indexList)
DeleteHoliday cmd = new DeleteHoliday(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof DeleteHoliday) {          
        System.out.println("删除节假日 succ"); 
    }    
}
```

### 9.  Firmware upgrade.

```java
CommandDetail commandDetail = getCommandDetail();//Get Command Detail Object
File faceFile = new File("D:\\E\\应用软件包\\FC89HV750_F.RCBin");
byte[] faceData = Files.readAllBytes(faceFile.toPath());
UpdateSoftware_Parameter parameter = new UpdateSoftware_Parameter(commandDetail, faceData);
UpdateSoftware cmd = new UpdateSoftware(parameter);
  //Add Command to Communication Connector Queue
_Allocator.AddCommand(cmd);
```

**Return Result**

```java
@Override    
public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
    if (cmd instanceof UpdateSoftware) {          
         UpdateSoftware_Result ret=(UpdateSoftware_Result)result;
         System.out.println("固件Write Return Result："+ret.Success);//Return Result 1--校验 succ 0--校验失败 255--Write 失败
    }    
}
```

### 10.The general process for using a WAN (Wide Area Network)

#### 10.1 Set TCP/IP para


#### 10.2 Code Example

```java
public class TcpServerTest implements INConnectorEvent {

    ConnectorAllocator _Allocator;
    private String LocalIP;

    private int LocalPort;
    /**
    *用于保存连接到服务器的设备
    *需要执行命令时根据设备SN进行获取设备连接信息，具体使用参考“开启监控方法”
    */
    HashMap<String, TCPServerClientDetail> TcpDictionary = new HashMap<String, TCPServerClientDetail>();
    
    public TcpServerTest() {
        _Allocator = ConnectorAllocator.GetAllocator();//获取通道对象
        _Allocator.AddListener(this);//添加监听
        try {
            InetAddress addr = InetAddress.getLocalHost();//获取本机IP
            LocalIP = addr.getHostAddress();
            LocalPort = 9000;
            _Allocator.Listen(LocalIP, LocalPort); //绑定TCP监听IP和端口            
        } catch (UnknownHostException ex) {
            Logger.getLogger(TcpServerTest.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * 开启监控
     */
    private void beginWatch(String sn, TCPServerClientDetail detail) {
         CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 5000;//此函数超时时间设定长一些
        commandDetail.RestartCount = 0;
        commandDetail.Connector = detail;
        Door8800Identity idt = new Door8800Identity(sn, "FFFFFFFF", E_ControllerType.Door8900);//设备SN(16位字符)，密码(8位十六进制字符)，设备类型
        commandDetail.Identity = idt;
        BeginWatch cmd = new BeginWatch(new CommandParameter(commandDetail));
        _Allocator.AddCommand(cmd);
    }
    /**
     * 命令执行 succ回调
     *
     * @param cmd cmd是执行命令的命令对象
     * @param result 是命令执行后返回的结果
     */
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        //命令执行 succ之后会执行
        if (cmd instanceof BeginWatch) {
            System.out.println("开启设备监控 succ");
            writeKeepAliveInterval();
        }
    }

    /**
     * 命令执行进度回调
     *
     * @param cmd
     */
    @Override
    public void CommandProcessEvent(INCommand cmd) {
        System.out.println("命令执行进度:" + cmd.getProcessMax() + "/" + cmd.getProcessStep());
    }

    /**
     * 连接出错
     *
     * @param cmd
     * @param isStop 是否是用户停止时发生的
     */
    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
      System.out.println("连接出错："+cmd);
    }

    /**
     * 连接出错
     *
     * @param detail
     */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {
		System.out.println("连接出错："+detail);
    }

    /**
     * 命令超时
     *
     * @param cmd
     */
    @Override
    public void CommandTimeout(INCommand cmd) {
		System.out.println("命令超时："+cmd);
    }

    /**
     * 密码错误
     *
     * @param cmd
     */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        System.out.println("通讯密码错误");
    }

    /**
     * 通讯校验和错误
     *
     * @param cmd
     */
    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
        System.out.println("通讯校验和错误");
    }

    /**
     * 数据监控
     *
     * @param detail
     * @param event
     */
    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {
        if (event instanceof BytesData) {
            authentication(detail, event);
        } else {
            Transaction(detail, event);
        }
    }

    /**
     * 身份验证
     *
     * @param detail
     * @param event
     */
    private void authentication(ConnectorDetail detail, INData event) {
        BytesData b = (BytesData) event;
        TCPServerClientDetail cd = (TCPServerClientDetail) detail;
        ByteBuf dBuf = b.GetBytes();
        if (dBuf.getByte(0) == 0x7e) {
            Door8800Decompile decompile = new Door8800Decompile();

            ArrayList<INPacketModel> oRetPack = new ArrayList<INPacketModel>(10);
            if (decompile.Decompile(dBuf, oRetPack)) {
                Door8800PacketModel m = (Door8800PacketModel) oRetPack.get(0);
                String snString = m.GetSN();             
                TcpDictionary.put(snString, cd);//保存连接到服务器的设备
                _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Door8900));
                System.out.println("添加控制板解析器");
                System.out.println("客户端ID:" + cd.ClientID + "(" + m.GetSN() + ")，收到数据包：" + ByteBufUtil.hexDump(dBuf));
               
            }

        }
    }

    /**
     * 监控消息处理
     *
     * @param detail
     * @param event
     */
    private void Transaction(ConnectorDetail detail, INData event) {
        if (event instanceof Door8800WatchTransaction) {
            Door8800WatchTransaction watchEvent = (Door8800WatchTransaction) event;
            AbstractTransaction tr = (AbstractTransaction) watchEvent.EventData;
            System.out.println(watchEvent.SN + " 收到监控事件：" + tr.getClass().toString());
            switch (watchEvent.CmdIndex) {
                case 1://认证记录
                    CardTransaction card = (CardTransaction) watchEvent.EventData;
                    break;
                case 2://出门开关信息
                    ButtonTransaction Software=(ButtonTransaction) watchEvent.EventData;
                    break;
                case 3://门磁信息
                    DoorSensorTransaction DoorSensor = (DoorSensorTransaction) watchEvent.EventData;
                    break;
                case 4://软件操作 包括远程开门之类的一些命令
                    SoftwareTransaction softwareTransaction=(SoftwareTransaction) watchEvent.EventData;
                    System.out.println("远程开门消息.................");
                    break;
                case 5://alarm记录
                    AlarmTransaction alarmTransaction= (AlarmTransaction) watchEvent.EventData;
                    break;
                case 6://系统记录
                    SystemTransaction  systemTransaction= (SystemTransaction) watchEvent.EventData;
                    break;
                case 34://保活消息
                    System.out.println("保活包消息.................");
                    break;
                default:
                    DefinedTransaction dt = (DefinedTransaction) watchEvent.EventData;
                    break;
            }
        } else {
            System.out.println("testio.FCardIO.FCardIOTest.WatchEvent() -- 未知消息");
        }
    }


    @Override
    public void ClientOnline(ConnectorDetail client) {
       System.out.println("设备上线："+client);
    }

    @Override
    public void ClientOffline(ConnectorDetail client) {
        System.out.println("设备离线："+client);
    }
```



### Record code (TransactionType):

Value	Explanation
1	Card reading record
2	Door opening/closing record
3	Door magnetic record
4	Software operation record
5	Alarm record
6	System record

### card reading records status（TransactionCode）

Value	Explanation
1	Valid card opening the door
2	Password opening the door - card number is the password
3	Card with password
4	Manual input of card with password
5	First card opening the door
6	Door always open - in always open mode, swiping the card enters always open status
7	Multiple card opening the door - triggered after verifying the combination of cards
8	Card swiped repeatedly
9	Expiry date has passed
10	Door opening time slot has expired
11	Invalid on holidays
12	Unregistered card
13	Patrol card - does not open the door
14	Detection lock
15	No valid times left
16	Anti-passback
17	Wrong password - card number is the wrong password
18	Password plus card mode - wrong password - card number is the card number
19	Door opens when locked (swiping card) or (swiping card with password)
20	Door opens when locked (password opening)
21	First card did not open the door
22	Lost card
23	Blacklisted card
24	Maximum capacity inside the door reached, entry is prohibited
25	Anti-theft arming status turned on (set by card)
26	Anti-theft arming status turned off (set by card)
27	Anti-theft arming status turned on (set by password)
28	Anti-theft arming status turned off (set by password)
29	Door opens when interlocked (swiping card) or (swiping card with password)
30	Door opens when interlocked (password opening)
31	All cards open the door
32	Multiple card opening the door - waiting for the next card
33	Multiple card opening the door - combination error
34	Invalid swiping of card outside of first card time period
35	Invalid password outside of first card time period
36	Swiping card opening the door is disabled in verification mode
37	Password opening the door is disabled in verification mode
38	Card has been swiped inside the door, waiting for card swiping outside (inside and outside card verification)
39	Card has been swiped outside the door, waiting for card swiping inside (inside and outside card verification)
40	Please swipe management card (prompted after enabling management card function) (elevator board)
41	Please swipe ordinary card (prompted after enabling management card function) (elevator board)
42	Password opening the door is prohibited when the first card is not swiped.
43	Controller has expired - card swiping
44	Controller has expired - password entry
45	Valid card opening the door - expiry date is approaching
46	Refused to open the door - area anti-passback lost connection to the host
47	Refused to open the door - area interlock lost connection to the host
48	Area anti-passback - refused to open the door
49	Area interlock - refused to open the door due to a door not being closed properly
50	Door opening password valid times expired
51	Door opening password expiry date has passed
52	QR code has expired

### Exit Switch Record of Status (TransactionCode)

Value	Explanation
1	Legal door open
2	Door opening time slot expired
3	Button pressed while locked
4	Control board expired
5	Button pressed during interlock (door not opened)

### Door Magnetic Record of Status (TransactionCode)

Value	Explanation
1	Door open
2	Door close
3	Enter door magnetic alarm status
4	Exit door magnetic alarm status
5	Door not closed properly

### Software Operation Record of Status (TransactionCode)

Value	Explanation
1	Software door open
2	Software door close
3	Software always open
4	Control board automatically enters always open mode
5	Control board automatically closes the door
6	Long press door open button always open
7	Long press door open button always closed
8	Software lock
9	Software release lock
10	Control board timing lock - automatically lock at the set time
11	Control board timing lock - automatically release lock at the set time
12	Alarm - lock
13	Alarm - release lock
14	Remote door open during interlock

### Alarm Record of Status (TransactionCode)

Value	Explanation
1	    Door magnet alarm
2	    Intruder alarm
3	    Fire alarm
4	    Illegal card swipe alarm
5	    Duress alarm
6	    Fire alarm (command notification)
7	    Smoke alarm
8	    Anti-theft alarm
9	    Blacklist alarm
10	    Door open timeout alarm
0x11	Door magnet alarm cancelled
0x12	Intruder alarm cancelled
0x13	Fire alarm cancelled
0x14	Illegal card swipe alarm cancelled
0x15	Duress alarm cancelled
0x17	Smoke alarm cancelled
0x18	Anti-theft alarm cancelled
0x19	Blacklist alarm cancelled
0x1A	Door open timeout alarm cancelled
0x21	Door magnet alarm cancelled (command notification)
0x22	Intruder alarm cancelled (command notification)
0x23	Fire alarm cancelled (command notification)
0x24	Illegal card swipe alarm cancelled (command notification)
0x25	Duress alarm cancelled (command notification)
0x27	Smoke alarm cancelled (command notification)
0x28	Anti-theft alarm closed (software closed)
0x29	Blacklist alarm closed (software closed)
0x2A	Door open timeout alarm closed
0xB	    Control board anti-tamper alarm
0x1B	Control board anti-tamper alarm cancelled
0xC	    Card reader anti-tamper alarm
0x1C	Card reader anti-tamper alarm cancelled

### System Record of Status (TransactionCode)

Value	Explanation
1	System power on
2	System error reset (watchdog)
3	Device formatting record
4	System high temperature record, temperature greater than >75
5	System UPS power record
6	Temperature sensor damaged, temperature greater than >100
7	Voltage too low, less than <09V
8	Voltage too high, greater than >14V
9	Card reader reversed connection
10	Card reader circuit not connected properly
11	Unrecognized card reader
12	Voltage returned to normal, less than 14V, greater than 9V
13	Network cable disconnected
14	Network cable plugged in

