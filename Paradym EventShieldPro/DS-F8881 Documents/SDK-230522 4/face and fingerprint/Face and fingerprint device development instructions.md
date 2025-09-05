# 1、Description

**Introduction**


The secondary development is mainly for third-party interaction with facial recognition devices. There are two communication methods for commands: TCP and UDP. UDP communication is mainly used within the local area network. By searching for devices in the local area network, commands can be sent through active connection communication using the device's local area network IP, port, SN, and communication password. TCP requires opening a service listener on the upper computer side, and the device will connect to the upper computer as a client. The upper computer needs to establish a connection with the device before issuing commands. TCP is usually used for wide area networks (UDP can also achieve the same function).

**Keywords**

| Keywords             |        name           | Explanation                                                         |
| ------------------ | ------------------------| ------------------------------------------------------------ |
| ConnectorAllocator | Communication Connector | Based on the command pattern. Used to coordinate the relationship between the connection channel, command, and caller. It pushes the command to the specified connection channel and monitors the operation of the connection channel. The command needs to be loaded through AddCommand or AddCommandAsync. |
| INCommand          | Command Object          | Encapsulation class of commands. It includes specific command packaging, data analysis, numerical judgment, and other related logic in the protocol. |
| INCommandDetail    | Command Details         | The necessary information during command execution, including the connector channel for command execution, command authentication information, user additional data, timeout retry parameters, and so on. These information can help the command executor accurately execute the command and handle any exceptions during command execution. |

**ConnectorAllocator Callback event**

| Event                    | name                       | Explanation                                                         |
| ------------------------ | ---------------------------| ------------------------------------------------------------ |
| CommandCompleteEvent     | Command completed          | When a command is successfully executed, an event will be triggered, and the result can be obtained in the CommandEventArgs parameter |
| CommandErrorEvent        | Connection error           | Generally, it could be due to connection handshake failure, non-existent serial port, non-existent USB, lack of file writing permission, etc. It could also be due to the user calling the Stop command to forcefully terminate the execution of the command. |
| CommandProcessEvent      | Command progress           | When a command starts executing, it will trigger continuous progress reports to update the status of the command execution.               |
| CommandTimeout           | Command timeout            | When a command execution exceeds the maximum wait time and retry attempts, it will trigger a timeout event.             |
| AuthenticationErrorEvent | Authentication of identity | When the communication password is incorrect, it will trigger an authentication error event.                                       |
| TransactionMessage       | Transaction message        | Data pushed by the device will be received here, such as real-time facial recognition records, device heartbeat, door magnetic records, etc. |
| ConnectorConnectedEvent  | Connection established     | It will trigger when the connection channel is successfully established.                                  |
| ConnectorClosedEvent     | Connection closed          | It will trigger when the connection channel is closed.                                     |
| ConnectorErrorEvent      | Connection error           | It will trigger when an error occurs on the connection channel, indicating that the device does not exist or cannot be connected to.    |
| ClientOnline             | Device online              | It will trigger when the device connects to the host as a client.                       |
| ClientOffline            | Device offline             | It will trigger when the device is disconnected from the host due to some reason.                   |

```c#
using DevComponents.DotNetBar;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.Fingerprint.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    internal class ConnectorAllocatorTestClass
    {
        ConnectorAllocator _connectorAllocator;
        public ConnectorAllocatorTestClass()
        {
            //Get the connection channel object (singleton).
            _connectorAllocator = ConnectorAllocator.GetAllocator();

            _connectorAllocator.CommandCompleteEvent += CommandCompleteEvent;
            _connectorAllocator.CommandErrorEvent += CommandErrorEvent;
            _connectorAllocator.CommandProcessEvent += CommandProcessEvent;
            _connectorAllocator.CommandTimeout += CommandTimeout;
            _connectorAllocator.AuthenticationErrorEvent += AuthenticationErrorEvent;
            _connectorAllocator.TransactionMessage += TransactionMessage;
            _connectorAllocator.ConnectorConnectedEvent += ConnectorConnectedEvent;
            _connectorAllocator.ConnectorClosedEvent += ConnectorClosedEvent;
            _connectorAllocator.ConnectorErrorEvent += ConnectorErrorEvent;
            _connectorAllocator.ClientOnline += ClientOnline;
            _connectorAllocator.ClientOffline += ClientOffline;
        }

        private void ClientOffline(object sender, Core.Connector.ServerEventArgs e)
        {
            Console.WriteLine("Device offline");
        }

        private void ClientOnline(object sender, Core.Connector.ServerEventArgs e)
        {
            Console.WriteLine("Device online");
        }
        /// <summary>
        /// Connection error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connector"></param>
        private void ConnectorErrorEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            Console.WriteLine("Connection error");
        }
        /// <summary>
        /// Connection close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connector"></param>
        private void ConnectorClosedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            Console.WriteLine("Connection close");
        }
        /// <summary>
        /// Connection succ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connector"></param>
        private void ConnectorConnectedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            Console.WriteLine("Connection succ");
        }

        /// <summary>
        /// Data received event.
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TransactionMessage(Core.Connector.INConnectorDetail connector, Core.Data.INData EventData)
        {
            Door8800Transaction transaction = EventData as Door8800Transaction;

            switch (transaction.CmdIndex)
            {
                case 1:
                    //Face recognition record.
                    var cardTransaction = EventData as CardTransaction;
                    break;
                case 2:
                    //Door magnetic record.
                    var doorSensor = EventData as DoorSensorTransaction;
                    break;
                case 3:
                    //System record.
                    var systemTransaction= EventData as SystemTransaction;
                    break;
                case 4:
                    //Temperature record.
                    var bodyTemperature = EventData as BodyTemperatureTransaction;
                    break;
                case 0xA0:
                    //Connection test message. A SendConnectTestResponse command needs to be replied.
                    break;
                case 0x22:
                    //Heartbeat message.
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Authentication verification failed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{commandName}Authentication verification failed.");
        }

        /// <summary>
        /// Command timeout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandTimeout(object sender, CommandEventArgs e)
        {
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{commandName}Command timeout.");
        }

        /// <summary>
        /// Command progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandProcessEvent(object sender, CommandEventArgs e)
        {
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{commandName}Current progress of the command{e.Command.getProcessMax()}/{e.Command.getProcessStep()}");
        }

        /// <summary>
        /// Command failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandErrorEvent(object sender, CommandEventArgs e)
        {
            OnlineAccess.OnlineAccessCommandDetail fcDtl = e.CommandDetail as OnlineAccess.OnlineAccessCommandDetail;
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{fcDtl.SN} Device Execute {commandName} Command failed");
        }
        /// <summary>
        /// Command completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            var result = e.Result;//Return result
        }
    }
}

```

**Example of invocation**

```c#
using DevComponents.DotNetBar;
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    internal class TestClass
    {
        ConnectorAllocator _connectorAllocator;
        string _serverIP;
        int _serverPort;
        public TestClass()
        {
            //Get the connection channel object (singleton).
            _connectorAllocator = ConnectorAllocator.GetAllocator();
            //Local IP address
            _serverIP = "192.168.1.120";
            //Listening port
            _serverPort = 9000;

            //Open UDP listening
            UDPServerDetail serverDetail = new UDPServerDetail(_serverIP, _serverPort);
            _connectorAllocator.OpenForciblyConnect(serverDetail);
        }
        /// <summary>
        /// Get command details
        /// </summary>
        /// <returns></returns>
        public INCommandDetail GetCommandDetail()
        {
            /**
             * CommandDetailFactory.ConnectType.UDPClient Communication mode
             * 192.168.1.150 Local network address of the device
             * 8101 UDP port of device 
             * CommandDetailFactory.ControllerType.A33_Face device type
             * 0000000000000000 16-digit device SN
             * FFFFFFFF Communication password of the device
             */
            var cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.UDPClient, "192.168.1.150", 8101,
                CommandDetailFactory.ControllerType.A33_Face, "0000000000000000", "FFFFFFFF");

            //In UDP mode, you need to add the local IP and local listening port, while in TCP mode, you do not need to
            var dtl = cmdDtl.Connector as UDPClientDetail;
            dtl.LocalAddr = _serverIP;
            dtl.LocalPort = _serverPort;
            //Timeout period (longer timeout period is needed for commands that require more time to execute)
            cmdDtl.Timeout = 600;
            //Retry attempts in case of timeout
            cmdDtl.RestartCount = 3;
            return cmdDtl;
        }
        /// <summary>
        ///  async  await Call method
        /// </summary>
        /// <returns></returns>
        public async Task GetDeviceSnAsync()
        {
            var cmdDetail = GetCommandDetail();
            //Create a command object to read the SN
            INCommand cmd = new ReadSN(cmdDetail);
           
            await _connectorAllocator.AddCommandAsync(cmd);
            var result = cmd.getResult() as SN_Result;
            var sn = Encoding.ASCII.GetString(result.SNBuf);
        }
        /// <summary>
        /// Event callback method
        /// </summary>
        public void GetDeviceSn()
        {
            var cmdDetail = GetCommandDetail();
            //Create a command object to read the SN
            INCommand cmd = new ReadSN(cmdDetail);
            //Event callback method
            cmdDetail.CommandCompleteEvent += ReadSN_CommandCompleteEvent;
            _connectorAllocator.AddCommand(cmd);
        }
        /// <summary>
        /// Callback for reading SN.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadSN_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            var result = e.Result as SN_Result;
            var sn = Encoding.ASCII.GetString(result.SNBuf);
        }
    }
}

```



# 2、System parameters

## 1>.Device SN

Device SN is a unique identifier in the device, with a length of 16 digits. It is an important identification for communication, and if the SN is incorrect, communication with the device will not be possible.

The SN information will be carried every time a command is sent, and the device SN is composed of the first 8 digits and the last 8 digits. The first 8 digits represent the device model, and the last 8 digits represent the serial number.

**Notes:**

If the device SN is not known, '0000000000000000' can be used as a substitute for the device SN.

**READ SN**

To read the unique ID of the device from the machine

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadSN(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as SN_Result;
var sn = Encoding.ASCII.GetString(result.SNBuf);
```

**Write SN**

To modify the unique ID of the device

```c#
//Command details
var cmdDetail = GetCommandDetail();
var sn = "0000000000000000";
var par = new SN_Parameter(sn)
//Create a command object
var cmd = new WriteSN(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed
```

## 2>.Communication password

The communication password is used to verify the communication during the transmission between the host and the device. If the communication password is incorrect, the command execution will fail.

**NOTE:**

The password can only be a decimal number, with a maximum length of 8 digits, except for the default communication password.

**Read the communication password**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadConnectPassword(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as Password_Result;
var password = result.Password;
```

**Write the communication password**

This command will write a new password to the device and overwrite the original password.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//password
var password = "12345678";
//Command parameters
var par = new Password_Parameter(password);
//Create a command object
var cmd = new WriteConnectPassword(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

**Restore the default communication password**

This command will reset the password to 8 digits of "F"  "FFFFFFFF"

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ResetConnectPassword(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 3>.Network parameters

**NOTE**

When the network parameters are set to automatically obtain IP, setting the IP address, DNS, etc. will be invalid.

**TCPDetail**

| Attribute name| type   | Description             |
| ----------- --| ------ | ----------------------- |
| mIP           | string | Device IP address       |
| mMAC          | string | Device mac address      |
| mIPMask       | string | IP mask                 |
| mIPGateway    | string | gateway                 |
| mDNS          | string | DNS                     |
| mDNSBackup    | string | Backup DNS              |
| mTCPPort      | int    | TCP port (not to facial)|
| mUDPPort      | int    | UDP port                |
| mServerPort   | int    | server port             |
| mServerIP     | string | server address          |
| mServerAddr   | string | Server domain           |
| mAutoIP       | bool   | DHCP                    |

**Read Network parameters**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadTCPSetting(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadTCPSetting_Result;
var tcpDetail = result.TCP;
```

**Write Network parameters**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var tcpDetail = new TCPDetail();//Generally, the device parameters are read first, and then modified and written.
var par = new WriteTCPSetting_Parameter(tcpDetail)
//Create a command object
var cmd = new WriteTCPSetting(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 4>.Get the device version number

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadVersion(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadVersion_Result;
var version = result.Version;//version
var fingerprintVersion = result.FingerprintVersion;//fingerprint version
```

## 5>.Get the device running information

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadSystemRunStatus(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadSystemRunStatus_Result;
var RunDay = result.RunDay; //Device running days
var FormatCount = result.FormatCount; //Format count
var RestartCount = result.RestartCount; //Watchdog reset count
var StartTime = result.StartTime; //Power-on time
```

## 6>.Record storage method

**Read Record storage method**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadRecordMode(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadRecordMode_Result;
var mode = result.Mode; //Record storage method: 0 Full cycle，1 Full not cycle
```

**Write Record storage method**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var mode = 0;//Record storage method 0 Full cycle，1 Full not cycle
var par = new WriteRecordMode_Parameter(mode)
//Create a command object
var cmd = new WriteRecordMode(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 7>.Real-time monitoring

**Open Real-time monitoring**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new BeginWatch(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

**Close Real-time monitoring**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new CloseWatch(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

**Get the monitoring status**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadWatchState(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var watchState = cmd.WatchState; //1.Open 0.close
```

## 8>.Initialize the device

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new FormatController(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 9>.Search device

```C#
using DoNetDrive.Core;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public class SearchTest
    {
        ConnectorAllocator _connectorAllocator;
        string _serverIP;
        int _serverPort;
        /// <summary>
        /// Random number for automatic search.
        /// </summary>
        private int mSearchID;
        /// <summary>
        /// Search times
        /// </summary>
        private int mSearchCount = 0;
        /// <summary>
        /// Random number generator
        /// </summary>
        public static Random CodeRandom = new Random();
        /// <summary>
        /// Device list
        /// </summary>
        List<string> mSNList = new List<string>();
        /// <summary>
        /// Device port
        /// </summary>
        int mDrivePort = 8101;
        /// <summary>
        /// Get a random number
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNum() => CodeRandom.Next(1, 65530);
        public SearchTest()
        {
            //Get the connection channel object (singleton)
            _connectorAllocator = ConnectorAllocator.GetAllocator();
            _serverIP = "192.168.1.120";
            _serverPort = 9000;

            //Open UDP listening
            UDPServerDetail serverDetail = new UDPServerDetail(_serverIP, _serverPort);
            _connectorAllocator.OpenForciblyConnect(serverDetail);
        }
        /// <summary>
        /// search Command details
        /// </summary>
        /// <returns></returns>
        private OnlineAccessCommandDetail SearchCommandDetail()
        {
            string sDestIP = "255.255.255.255";
            string sSearchSN = "0000000000000000", sPassword = "FFFFFFFF";
            var oUDPDtl = new UDPClientDetail(sDestIP, mDrivePort,
                _serverIP, _serverPort);
            var dtl = new OnlineAccessCommandDetail(oUDPDtl, sSearchSN, sPassword)
            {
                Timeout = 2000,
                RestartCount = 3,
                UserData = null
            };
            return dtl;
        }
        /// <summary>
        /// Search method
        /// </summary>
        public void BeginSearch()
        {
            mSearchID = GetRandomNum();
            mSearchCount++;
            if (mSearchCount == 5)
            {
                //Exit search if the search times exceed 5
                return;
            }
            var cmdDtl = SearchCommandDetail();
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;//Broadcast search
            var searchCmd = new SearchControltor(cmdDtl, searchPar);
            _connectorAllocator.AddCommand(searchCmd);

            cmdDtl.CommandCompleteEvent += Search_CommandCompleteEvent;
            cmdDtl.CommandTimeout += Search_CommandTimeout;
            cmdDtl.CommandErrorEvent += Search_CommandErrorEvent;
        }
        /// <summary>
        /// Retry search if there is a command error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_CommandErrorEvent(object sender, Core.Command.CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            BeginSearch();
        }
        /// <summary>
        /// etry search if there is a command timeout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_CommandTimeout(object sender, Core.Command.CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            BeginSearch();
        }
        /// <summary>
        /// If the search is successful, return the device information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_CommandCompleteEvent(object sender, Core.Command.CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            SearchControltor_Result rst = e.Result as SearchControltor_Result;
            if (rst != null)
            {
                if (!mSNList.Contains(rst.SN))
                {
                    mSNList.Add(rst.SN);
                }
            }
        }
    }
}

```

## 10>.Client keep-alive interval

**Read the keep-alive interval**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadKeepAliveInterval(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadKeepAliveInterval_Result;
var intervalTime = result.IntervalTime; //Keep-alive interval time
```

**写入保活间隔**

```C#
//Command details
var cmdDetail = GetCommandDetail();
var intervalTime = 30;//Keep-alive interval time
var par = new WriteKeepAliveInterval_Parameter(intervalTime);
//Create a command object
var cmd = new WriteKeepAliveInterval(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 11>.Local identity

**Read Local identity**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadLocalIdentity(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadLocalIdentity_Result;
var localName = result.LocalName; //device name
var door = result.Door; //door no
var inOut = result.InOut; //0.in  1.out
```

**Write Local identity**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create command parameters
var par = new WriteLocalIdentity_Parameter(1,"device name",0);
//Create a command object
var cmd = new WriteLocalIdentity(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 12>.Wiegand output

**Read Wiegand output**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadLocalIdentity(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadLocalIdentity_Result;
var readCardByte = result.ReadCardByte; //Card read byte: 1. Wiegand 26 (three bytes) 2. Wiegand 34 (four bytes) 3. Wiegand 26 (two bytes) 4. Wiegand 66 (eight bytes) 5. Disabled.
var wgOutputSwitch = result.WGOutputSwitch; //Wiegand output function switch: 1. Enable 2. Disable.
var wgByteSort = result.WGByteSort; //Wiegand byte order: 1. High byte first, low byte last. 2. Low byte first, high byte last.
var outputType = result.OutputType; //Output data type: 1. User ID 2. Card number.
```

**Write Wiegand output**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create command parameters
var par = new WriteWiegandOutput_Parameter(1,//Card read byte
                                           1,//Wiegand output function switch
                                           1,//Wiegand byte order
                                           1);//Output data type
//Create a command object
var cmd = new WriteWiegandOutput(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 13>.Face and fingerprint comparison threshold

**Read comparison threshold**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadComparisonThreshold(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadComparisonThreshold_Result;
var faceComparisonThreshold = result.FaceComparisonThreshold; // face comparison threshold：Effective only for all-in-one machines, it takes effect from 1 to 100 when used with fingerprint and facial recognition readers. For dynamic facial recognition machines, it only applies to the facial comparison threshold, with a value range of 1-6. 
var fingerprintComparisonThreshold = result.FingerprintComparisonThreshold; // fingerprint comparison threshold: 1-100
```

**Write comparison threshold**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create command parameters
var par = new WriteComparisonThreshold_Parameter(3,//face comparison threshold
                                           80);//fingerprint comparison threshold
//Create a command object
var cmd = new WriteComparisonThreshold(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 14>.Management password

** Read menu password**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadManageMenuPassword(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadManageMenuPassword_Result;
var password = result.Password; //menu password* 
```

**Write menu password**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var password = "12345678";
//Create command parameters
var par = new WriteManageMenuPassword_Parameter(password);
//Create a command object
var cmd = new WriteManageMenuPassword(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 15>.OEM information

**Read OEM information**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadOEM(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as OEM_Result;
var manufacturer =  result.Detail.Manufacturer; //Manufacturer
var webAddr =  result.Detail.WebAddr; //web
var deliveryDate =  result.Detail.DeliveryDate; //Production date
```

**Write OEM information**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//OEM信息
var oem = new OEMDetail{
    Manufacturer = "Manufacturer",
    WebAddr = "www.123.com",
    DeliveryDate = DateTime.Now
};
//Create command parameters
var par = new OEM_Parameter(password);
//Create a command object
var cmd = new WriteOEM(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 16>.Facial recognition machine liveness detection

**Read liveness detection**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceBioassay(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceBioassay_Result;
var bioassayType =  result.BioassayType; //liveness detection Type：0--close；1-- Infrared detection；2-- Infrared + color 
```

**Write liveness detection**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//liveness detection Type
var bioassayType = 1;
//Create command parameters
var par = new WriteFaceBioassay_Parameter(bioassayType);
//Create a command object
var cmd = new WriteFaceBioassay(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 17>.Facial recognition machine recognition distance

**Read Facial recognition machine recognition distance**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceIdentifyRange(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceIdentifyRange_Result;
var identifyRange =  result.IdentifyRange; //recognition distance Type：1--Short （0.2-0.5米）；2--Medium (0.2-1.5米）；3--long（0.2-1.5米以上） 
```

**Write Facial recognition machine recognition distance**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//活recognition distance Type
var identifyRange = 1;
//Create command parameters
var par = new WriteFaceIdentifyRange_Parameter(identifyRange);
//Create a command object
var cmd = new WriteFaceIdentifyRange(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 18>.Set machine language

**Read machine language**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadDriveLanguage(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadDriveLanguage_Result;
var language =  result.Language; //language：1--Chinese；2--English；3--Traditional Chinese
```

**Write machine language**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//language type
var language = 1;
//Create command parameters
var par = new WriteDriveLanguage_Parameter(language);
//Create a command object
var cmd = new WriteDriveLanguage(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 19>.Set machine volume

**Read volume**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadDriveVolume(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadDriveVolume_Result;
var volume =  result.Volume; //device volume：0-10；0--close；10--max
```

**Write volume**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//volume
var volume = 10;
//Create command parameters
var par = new WriteDriveVolume_Parameter(volume);
//Create a command object
var cmd = new WriteDriveVolume(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 20>.Fill light mode

**Read Fill light mode**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceLEDMode(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceLEDMode_Result;
var ledMode =  result.LEDMode; //Fill light mode  1--Always on；2--Turn on when personnel is detected；0--Always off
```

**Write Fill light mode**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Fill light mode
var ledMode = 1;
//Create command parameters
var par = new WriteFaceLEDMode_Parameter(ledMode);
//Create a command object
var cmd = new WriteFaceLEDMode(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 21>.Mask recognition switch

**Read Mask recognition switch**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceMouthmufflePar(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceMouthmufflePar_Result;
var mouthmuffle =  result.Mouthmuffle; //Mask recognition switch 0--disable；1--enable
```

**Write Mask recognition switch**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Mask recognition switch
var mouthmuffle = 1;
//Create command parameters
var par = new WriteFaceMouthmufflePar_Parameter(mouthmuffle);
//Create a command object
var cmd = new WriteFaceMouthmufflePar(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 22>.Temperature measurement and format

**Read Temperature measurement and format**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceBodyTemperaturePar(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceBodyTemperaturePar_Result;
var bodyTemperaturePar =  result.BodyTemperaturePar; //Temperature measurement and format 0--Disable；1--Celsius (default)；2--Fahrenheit
```

**Write Temperature measurement and format**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var bodyTemperaturePar = 1;
//Create command parameters
var par = new WriteFaceBodyTemperaturePar_Parameter(mouthmuffle);
//Create a command object
var cmd = new WriteFaceBodyTemperaturePar(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 23>.Set temperature alarm threshold

**Read temperature alarm threshold**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceBodyTemperatureAlarmPar(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceBodyTemperatureAlarmPar_Result;
var alarmPar = ((double)result.AlarmPar/(double)10).ToString("0.0"); //temperature alarm threshold 350-500 Stabilize using integer storage, divide by 10 when displaying, for example 37.5 is stored as 375
```

**Write temperature alarm threshold**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//temperature alarm threshold
var alarmPar = 375;
//Create command parameters
var par = new WriteBodyTemperatureAlarmPar_Parameter(alarmPar);
//Create a command object
var cmd = new WriteFaceBodyTemperatureAlarmPar(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 24>.Temperature detection information display switch

**Read Temperature detection information display switch**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadFaceBodyTemperatureShowPar(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadFaceBodyTemperatureShowPar_Result;
var isShow = result.IsShow; //Temperature detection information display switch 0--Disable；1--enable
```

**Write Temperature detection information display switch**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Temperature detection information display switch
var isShow = 1;
//Create command parameters
var par = new WriteFaceBodyTemperatureShowPar_Parameter(isShow);
//Create a command object
var cmd = new WriteFaceBodyTemperatureShowPar(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 25>.Short message after legal verification

**Read Short message**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadShortMessage(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadShortMessage_Result;
var message = result.Message; //Short message displayed after legal verification, 30 characters
```

**Write Short message**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var message = "Short message displayed";
//Create command parameters
var par = new WriteShortMessage_Parameter(message);
//Create a command object
var cmd = new WriteShortMessage(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 26>.Client network parameters

### 1.Modify network server parameters

**Read network server parameters**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadNetworkServerDetail(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadNetworkServerDetail_Result;
var serverIP = result.ServerIP; //server IP
var serverPort = result.ServerPort; //server port
var serverDomain = result.ServerDomain; //server domain
```

**Wrtie network server parameters**

```C#
//Command details
var cmdDetail = GetCommandDetail();
var serverIP = "113.65.205.192";
var serverPort = 9003;
//Create command parameters
var par = new WriteNetworkServerDetail_Parameter(serverPort, serverIP);
par.ServerDomain="WWW.123.com";
//Create a command object
var cmd = new WriteNetworkServerDetail(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



### 2.Client mode communication method

**Read communication method**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadClientWorkMode(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//Get the return result. If it is not a correct return, an exception may occur, and exception handling is required
var result = cmd.getResult() as ReadClientWorkMode_Result;
var clientModel = result.ClientModel; //communication method 0--Disable;1--UDP;2--TCP Client;3--TCP Client + TLS ;4--MQTT（TCP Client）;5--MQTT（TCP Client） + TLS ;
```

**Write communication method**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var clientModel = 2;
//Create command parameters
var par = new WriteClientWorkMode_Parameter(clientModel);
//Create a command object
var cmd = new WriteClientWorkMode(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



### 3.Send keep-alive packet immediately

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new RequireSendKeepalivePacket(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as RequireSendKeepalivePacket_Result;
var resultStatus = result.ResultStatus;//Keep-alive packet status return value
		/// 1--send succ
        /// 2--Server Parameter not set
        /// 3--Server Parameter error
        /// 4--Server Connection failed （TCP）
        /// 5--No response from server
        /// 6--Network parameter setting error
        /// 7--Ethernet cable not connected
        /// 8--Wifi not connected
```



### 4.Get server connection status

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadClientStatus(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadClientStatus_Result;
var clientModel = result.ClientModel;//Client mode communication method 0--Disable;1--UDP;2--TCP Client;3--TCP Client + TLS ;4--MQTT（TCP Client）;5--MQTT（TCP Client） + TLS ;
var serverIP = result.ServerIP;//server IP
var lastKeepaliveTime = result.LastKeepaliveTime;//Time of last keep-alive packet sent
var connectStatus = result.ConnectStatus;//Connection status 0--TCP Client not connect；1--TCP Client connected；2--UDP Client Disconnected status  255--Disable
```



### 5.Reconnect to server immediately

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new RequireConnectServer(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as RequireConnectServer_Result;
var resultStatus = result.ResultStatus;
		//Keep-alive packet status return value 
		/// 1--Reconnected (for UDP, it means a keep-alive packet has been sent)
		/// 2--Server Parameter not set
        /// 3--Server Parameter error
        /// 4--Server Connection failed （TCP）
        /// 5--No response from server
        /// 6--Network parameter setting error
        /// 7--Ethernet cable not connected
        /// 8--Wifi not connected

```



## 27>.Restart device

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new RequireRestart(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 28>.Authentication mode

**Read Authentication mode**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadAuthenticationMode(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadAuthenticationMode_Result;
var authenticationMode = result.AuthenticationMode;//Authentication mode：1-5； 1、Standard(default) ；2、Facial recognition + password；3、Card + facial recognition; 4. Multiple-person attendance; 5. Identity verification
```
**Write Authentication mode**

```C#
//Command details
var cmdDetail = GetCommandDetail();
var authenticationMode = 1;
//Create command parameters
var par = new WriteAuthenticationMode_Parameter(clientModel);
//Create a command object
var cmd = new WriteAuthenticationMode(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 29>.Record photo saving switch

**Read switch**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadSaveRecordImage(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadSaveRecordImage_Result;
var saveImageSwitch = result.SaveImageSwitch;//savin photo true/false
```

**Write switch**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//true/false
var saveImageSwitch = true;
//Create command parameters
var par = new WriteSaveRecordImage_Parameter(saveImageSwitch);
//Create a command object
var cmd = new WriteSaveRecordImage(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 30>.Photosensitive mode

**Read Photosensitive mode**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadLightPattern(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadLightPattern_Result;
var lightPattern = result.LightPattern;//Photosensitive mode:1、Standard (default) 2、Brightening  3、Darkening
```

**Write Photosensitive mode**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Photosensitive mode:1、Standard (default) 2、Brightening  3、Darkening
var lightPattern = 1;
//Create command parameters
var par = new WriteLightPattern_Parameter(lightPattern);
//Create a command object
var cmd = new WriteLightPattern(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



# 3、Time and date

**Read device time**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadTime(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadTime_Result;
var controllerDate = result.ControllerDate;//device time
```

**Write device time**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new WriteTime(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

**Write custom time**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//custom time
var controllerDate = DateTime.Now;
//Create command parameters
var par = new WriteCustomTime_Parameter(controllerDate);
//Create a command object
var cmd = new WriteCustomTime(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

**Broadcast write time**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new WriteCustomTime(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



# 4、Access control parameters

## 1>.Card number byte

**Read Card number byte**

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadReaderOption(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReaderOption_Result;
var readerOption = result.ReaderOption;// 1 - WG26(3byte)2 - WG34(4byte) 3 - WG26(2byte) 4 - Disable 5 - 8byte
```

**Write Card number byte**

```C#
//Command details
var cmdDetail = GetCommandDetail();
var readerOption = 1;
//Create command parameters
var par = new ReaderOption_Parameter(readerOption);
//Create a command object
var cmd = new WriteReaderOption(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 2>.Door relay (bistable)

**Read bistable**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadRelayOption(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as RelayOption_Result;
var isSupport = result.IsSupport;//Whether the relay supports bistable, true means support, false means not support
```

**Write bistable**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isSupport = true;
//Create command parameters
var par = new RelayOption_Parameter(isSupport);
//Create a command object
var cmd = new WriteRelayOption(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 3>.Remote door opening

**Without verification code**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new OpenDoor(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

**With verification code**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Verification code. The purpose of the verification code is to prevent repeated door opening.
var checkNum = 123;
//Create command parameters
var par = new Remote_CheckNum_Parameter(isSupport);
//Create a command object
var cmd = new OpenDoor_CheckNum(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 4>.Remote door closing

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new CloseDoor(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 5>. Set door to normally open

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new HoldDoor(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 6>.Remote door locking

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new LockDoor(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 7>.Remote door unlocking

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new UnlockDoor(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 8>.Timed normally open parameters

**Read Timed normally open parameters**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadDoorWorkSetting(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as DoorWorkSetting_Result;
var use = result.Use;//true Enable，false  Disable
var doorTriggerMode = result.DoorTriggerMode;//Door Constant Open Trigger Mode:1.Legitimate cardholders can keep the door constantly open within the specified time period. 2.Authorized cards marked as constant open cards can keep the door constantly open by swiping during the designated time period.3.Automatic switch, the door will automatically open or close at the designated time.
var weekTimeGroup = result.weekTimeGroup;//Door Working Mode Time Period, divided into 7 days with 8 time periods per day.

for (int i = 0; i < 7; i++){ //Includes time periods within 7 days
    var day = result.weekTimeGroup.GetItem(i);//Get the time periods for one day.
    for (int j = 0; j < 8; j++){
        var item = day.GetItem(j);//Loop to get the 8 time periods for a day.
        var beginTime = item.GetBeginTime();//start time
        var endTime = item.GetEndTime();//end time
    }
}
```

**Write Timed normally open parameters**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Is scheduled constant open enabled
var use = true;
//Door Constant Open Trigger Mode.
var doorTriggerMode = 3;

var weekTimeGroup = new WeekTimeGroup(8);

var day = result.weekTimeGroup.GetItem(0);//Get the time periods for a specific day, indexed from 0 to 6, with 0 representing Monday.

var item = day.GetItem(0);//Get the first sub-segment of the 8 time periods, indexed from 0 to 7.

item.SetBeginTime(0,0);//Indicates that the start time is from midnight at 00:00.
item.SetEndTime(23,59)//Indicates that the end time is at 23:59.

//Create command parameters
var par = new WriteDoorWorkSetting_Parameter(use, doorTriggerMode, weekTimeGroup);
//Create a command object
var cmd = new WriteDoorWorkSetting(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 9>.Lock opening time output duration.

**Read Lock opening time output duration.**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadUnlockingTime(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadUnlockingTime_Result;
var releaseTime = result.ReleaseTime;//Lock opening time output duration.
```

**Write Lock opening time output duration.**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Lock opening time output duration.
ushort releaseTime = 3;
//Create command parameters obj
var par = new WriteUnlockingTime_Parameter(releaseTime);
//Create a command object
var cmd = new WriteUnlockingTime(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 10>.Free access to open the door without verification.

Read Free access to open the door without verification.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadExemptionVerificationOpen(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadExemptionVerificationOpen_Result;
var isUseExemptionVerification = result.IsUseExemptionVerification;// true enable，false Disable
```

Write Free access to open the door without verification.

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isUseExemptionVerification = true;
//Create command parameters obj
var par = new WriteExemptionVerificationOpen_Parameter(isUseExemptionVerification, false, 1);
//Create a command object
var cmd = new WriteExemptionVerificationOpen(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 11>.Voice broadcast function

**Read Voice broadcast function**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadVoiceBroadcastSetting(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as VoiceBroadcastSetting_Result;
var use = result.Use;// true Enable，false Disable
```

**Write Voice broadcast function**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var use = true;
//Create command parameters obj
var par = new WriteVoiceBroadcastSetting_Parameter(use);
//Create a command object
var cmd = new WriteVoiceBroadcastSetting(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 12>.Interval between repeated verification permissions.

**Read Interval between repeated verification permissions.**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadReaderIntervalTime(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadReaderIntervalTime_Result;
var intervalTime = result.IntervalTime;//Card reading interval time, in seconds, with a maximum of 65535 seconds. 0 indicates no limit.
var mode = result.Mode;//Detection mode 1 - Record card reading without opening the door, with prompt.; 2 - Do not record card reading, do not open the door, with prompt.; 3 - No response, no prompt.;
```

**Write Interval between repeated verification permissions.**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var intervalTime = true;
var mode = 2;
//Create command parameters obj
var par = new WriteReaderIntervalTime_Parameter(true, intervalTime, mode);
//Create a command object
var cmd = new WriteReaderIntervalTime(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



# 5、Alarm parameters

## 1>.Fire alarm

**Manually trigger fire alarm**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new WriteSendFireAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 2>.Blacklist alarm

**Read Blacklist alarm**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadBlacklistAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadBlacklistAlarm_Result;
var isAlarm = result.IsAlarm;//Is the blacklist alarm function enabled? If this function is disabled, it means that there will be no alarm when a blacklisted card is swiped, only a record will be made.
```

**Write Blacklist alarm**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isAlarm = true;
//Create a command object 
var par = new WriteBlacklistAlarm_Parameter(isAlarm);
//Create a command object
var cmd = new WriteSendFireAlarm(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 3>.Tamper alarm function.

Read Tamper alarm function.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadAntiDisassemblyAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadAntiDisassemblyAlarm_Result;
var isUse = result.IsUse;//Is the tamper alarm function enabled? When the tamper switch is disconnected, the control board will record the tamper alarm. This status will be displayed when retrieving the status. When the tamper switch is short-circuited, the alarm will be automatically closed. This function cannot be turned off unless the tamper function is disabled.
```

Write Tamper alarm function.

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isUse = true;
//Create a command object
var par = new WriteAntiDisassemblyAlarm_Parameter(isUse);
//Create a command object
var cmd = new WriteAntiDisassemblyAlarm(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 4>.Illegal validation alarm.

**Read Illegal validation alarm.**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadIllegalVerificationAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadIllegalVerificationAlarm_Result;
var isUse = result.IsUse;//Is the illegal validation alarm enabled
var times = result.Times;// Illegal authentication count: 0-255; 0 means that the alarm will be triggered after reading the card once.
```

**Write Illegal validation alarm**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isUse = true;

var times = 0;
//Create a command object
var par = new WriteIllegalVerificationAlarm_Parameter(isUse,times);
//Create a command object
var cmd = new WriteIllegalVerificationAlarm(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 5>.Duress alarm password.

**Read Duress alarm password**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadAlarmPassword(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadAlarmPassword_Result;
var use = result.Use;//
var alarmOption = result.AlarmOption;// Alarm options: 1 - Do not open the door, output alarm; 2 - Open the door, output alarm; 3 - Lock the door, alarm, can only be unlocked by software.
var password = result.Password;// Duress alarm password, with a maximum length of 8 characters composed of numbers.
```

**Write Duress alarm password**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var use = true;
var alarmOption = 0;
var password = 0;
//Create a command object
var par = new WriteAlarmPassword_Parameter(isUse, password, alarmOption);
//Create a command object
var cmd = new WriteAlarmPassword(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 6>.Door opening timeout alarm parameters

**Read Door opening timeout alarm parameters**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadOpenDoorTimeoutAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadOpenDoorTimeoutAlarm_Result;
var isUse = result.IsUse;
var allowTime = result.AllowTime;// The time allowed for door opening is 1-65535 seconds, 0 - indicates that it is not enabled.
var relayOutput = result.RelayOutput;// Relay output, false - do not output relay; true - output relay (alarm relay).
```

**Write Door opening timeout alarm parameters**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isUse = true;
var allowTime = 30;
var relayOutput = true;
//Create a command object
var par = new WriteOpenDoorTimeoutAlarm_Parameter(isUse, allowTime, relayOutput);
//Create a command object
var cmd = new WriteOpenDoorTimeoutAlarm(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 7>.Door magnetic alarm function

**Read Door magnetic alarm function**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadGateMagneticAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadGateMagneticAlarm_Result;
var isUse = result.IsUse;//Is the door magnetic alarm enabled?
var weekTimeGroup = result.WeekTimeGroup;// Time periods when door magnetic alarm check is not enabled.
for (int i = 0; i < 7; i++){ //Includes time periods within 7 days.
    var day = result.weekTimeGroup.GetItem(i);//Get the time periods for one day.
    for (int j = 0; j < 8; j++){
        var item = day.GetItem(j);//Loop to get the 8 time periods for a day.
        var beginTime = item.GetBeginTime();
        var endTime = item.GetEndTime();
    }
}
```

**Write Door magnetic alarm function**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Is the door magnetic alarm enabled?
var use = true;

var weekTimeGroup = new WeekTimeGroup(8);

var day = result.weekTimeGroup.GetItem(0);//Get the time periods for a specific day, indexed from 0 to 6, with 0 representing Monday.

var item = day.GetItem(0);//Get the first sub-segment of the 8 time periods, indexed from 0 to 7.

item.SetBeginTime(0,0);//Indicates that the start time is from midnight at 00:00.
item.SetEndTime(23,59)//Indicates that the end time is at 23:59.

//Create command parameters
var par = new WriteGateMagneticAlarm_Parameter(use, doorTriggerMode, weekTimeGroup);
//Create a command object
var cmd = new WriteGateMagneticAlarm(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```

## 8>.Switch to release alarm for legitimate validation

**Read Switch to release alarm for legitimate validation**

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadLegalVerificationCloseAlarm(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadLegalVerificationCloseAlarm_Result;
var isUse = result.IsUse;
```

**Write Switch to release alarm for legitimate validation**

```c#
//Command details
var cmdDetail = GetCommandDetail();
var isUse = true;
//Create a command object
var par = new WriteLegalVerificationCloseAlarm_Parameter(isUse);
//Create a command object
var cmd = new WriteLegalVerificationCloseAlarm(cmdDetail, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//If there is no return result and no exception occurs, it indicates success. If there is an exception, it indicates failure.
```

## 9>.Release alarm.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//There are 7 types of alarms. 1 means to release the alarm, and 0 means to ignore it.
byte[] list = new byte[7];
list[0] = 1;//Illegal authentication alarm.
list[1] = 0;//Door magnetic alarm.
list[2] = 0;//Duress alarm.
list[3] = 0;//Door opening timeout alarm.
list[4] = 0;//Blacklist alarm.
list[5] = 0;//Tamper alarm.
list[6] = 0;//Fire alarm.
//Create a command object
var par = new CloseAlarm_Parameter(list);
//Create a command object
var cmd = new CloseAlarm(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//If there is no return result and no exception occurs, it indicates success. If there is an exception, it indicates failure.
```

# 6、Holiday

## 1>.Read holiday capacity information.

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadHolidayDetail(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadHolidayDetail_Result;
var detail = result.Detail;//Holiday details stored in the controller.
var capacity = detail.Capacity;//Maximum capacity that can be stored in the controller.
var count = detail.Count;//Number of items already stored in the controller.
```



## 2>.Clear all holidays

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ClearHoliday(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 3>.Read all holidays

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadAllHoliday(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
var result = cmd.getResult() as ReadAllHoliday_Result;
var holidays = result.Holidays;//Holiday list
foreach (HolidayDetail item in result.Holidays){
    var holidayType = item.HolidayType;//Holiday type
    var index = item.Index;//Index number of the holiday
    var holiday = item.Holiday;//Holiday date  
    var yearLoop = item.YearLoop;//Recurring annually
}
```

**Holiday Detail**

| 属性        | type     | Explanation                                                         |
| ----------- | -------- | ------------------------------------------------------------ |
| Index       | byte     | Index number of the holiday 1-30                                          |
| Holiday     | DateTime | Holiday date                                                   |
| HolidayType | byte     | Holiday types: 1、Morning  (00:00:00  -  11:59:59)<br/>2、Afternoon  (12:00:00  -  23:59:59)<br/> 3、All day (00:00:00  -  23:59:59) |
| YearLoop    | bool     | Recurring annually      |

## 4>.Add Holiday

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Holiday list 
var holidayDetails = new List<HolidayDetail>();
//Holiday detail
var holidayDetail = new HolidayDetail();
holidayDetail.Index = 1;
holidayDetail.Holiday = DateTime.Now;
holidayDetail.HolidayType = 3;
holidayDetail.YearLoop = false;
holidayDetails.Add(holidayDetail);
//Create a command object
var par = new AddHoliday_Parameter(holidayDetails);
//Create a command object
var cmd = new AddHoliday(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//If there is no return result and no exception occurs, it indicates success. If there is an exception, it indicates failure.
```



## 5>.Delete Holiday

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Holiday list 
var holidayDetails = new List<HolidayDetail>();
//Holiday detail
var holidayDetail = new HolidayDetail();
holidayDetail.Index = 1;//Delete a holiday based on the index number.
holidayDetails.Add(holidayDetail);
//Create a command object
var par = new DeleteHoliday_Parameter(holidayDetails);
//Create a command object
var cmd = new DeleteHoliday(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//If there is no return result and no exception occurs, it indicates success. If there is an exception, it indicates failure.
```



# 7、Door opening time period.

## 1>.clear all Door opening time period

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ClearTimeGroup(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 2>.read all Door opening time period.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadTimeGroup(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);

var result = cmd.getResult() as ReadTimeGroup_Result;
var ListWeekTimeGroup = result.ListWeekTimeGroup;//There are 64 sets of door opening time periods. Each set contains the time periods for a week, with 8 time segments per day.
 for (int i = 0; i < ListWeekTimeGroup.Count; i++){
     var weekTimeGroup = ListWeekTimeGroup[i];
     var index= weekTimeGroup.GetIndex();//Get the index number of the week time period list, indexed from 1 to 64.
     for (int j = 0; j < 7; j++){
         var day = weekTimeGroup.GetItem(j);//Get the time periods of a day
         var segmentCount = day.GetSegmentCount();//Get the number of time periods included in a day.
          for (int k = 0; k < segmentCount; k++){
              var segment = day.GetItem(k);//Time period
              var beginTime = segment.GetBeginTime();//Start time of the time period.
              var endTime = segment.GetEndTime();//end time of the time period.
          }
     }
 }
```



## 3>.Write Door opening time period

```c#
//Command details
var cmdDetail = GetCommandDetail();
var weekTimeGroups = new List<WeekTimeGroup>()();
var weekTimeGroup = new WeekTimeGroup(8);
weekTimeGroup.SetIndex(1);//Set the index number in the weekly time period list to 1-64, and the index number cannot be repeated.
var day = weekTimeGroup.GetItem(0);//Get the time periods of Monday
var segment = day.GetItem(0);//Get the first time period
segment.SetBeginTime(0,0);//00:00
segment.SetEndTime(23,59);//23:59
weekTimeGroups.Add(weekTimeGroup);
//Create a command object
var par = new AddTimeGroup_Parameter(weekTimeGroups);
//Create a command object
var cmd = new AddTimeGroup(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



# 8、Personnel file

## 1>.Read personnel file storage information

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadPersonDatabaseDetail(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);

var result = cmd.getResult() as ReadPersonDatabaseDetail_Result;
var sortDataBaseSize = result.SortDataBaseSize;//Maximum capacity
var sortPersonSize = result.SortPersonSize;//The number of personnel already stored
var sortFingerprintDataBaseSize = result.SortFingerprintDataBaseSize;//Maximum capacity of fingerprint feature code
var sortFingerprintSize = result.SortFingerprintSize;//The number of fingerprints already stored
var sortFaceDataBaseSize = result.SortFaceDataBaseSize;//Maximum capacity of face feature code.
var sortFaceSize = result.SortFaceSize;//The number of faces already stored
```



## 2>.Clear all Read personnel file storage information

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ClearPersonDataBase(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//There is no return result. If no exception occurs, it means the operation is successful. If an exception occurs, it means the operation has failed.
```



## 3>.Read all personnel file storage information

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadPersonDataBase(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//返回结果
var result = cmd.getResult() as ReadPersonDataBase_Result;
var DataBaseSize = result.DataBaseSize;//The number of personnel detected/recognized.
var PersonList = result.PersonList;//The list of personnel detected/recognized.
```

**Person**

| name        | type     | Explanation                                                         |
| ----------- | -------- | ------------------------------------------------------------ |
| UserCode    | uint     | User ID                                                       |
| CardData    | UInt64   | Card number, value range 0x1-0xFFFFFFFF                                |
| Password    | string   | Card password, leave it blank if there is no password. The password is a 4-8 digit number.                         |
| Expiry      | DateTime | Expiration date, maximum December 31, 2089.                               |
| TimeGroup   | int      | Access time range, value range: index number from 1 to 64.                              |
| OpenTimes   | ushort   | Valid access count, value range: 0-65535; 0 means the access count has been used up, and 65535 means unlimited access. |
| Identity    | int      | User identity: 0 - normal user, 1 - administrator.                      |
| CardType    | int      | Card type: 0 - normal card, 1 - always open card.                           |
| CardStatus  | int      | Card status: 0 - normal status, 1 - lost, 2 - blacklisted, 3 - deleted.        |
| EnterStatus | int      | Access mark: 1 - valid for entry, 2 - valid for exit, 3 or 0 - valid for both entry and exit              |
| PName       | string   | Personnel name                                                   |
| PCode       | string   | Personnel ID                                                    |
| Dept        | string   | Personnel department                                                     |
| Job         | string   | Personnel position                                                     |



## 4>.Query personnel details

```C#
//Command details
var cmdDetail = GetCommandDetail();
var userCode = 20;
//Create a command object
var par = new ReadPersonDetail_Parameter(userCode);
//Create a command object
var cmd = new ReadPersonDetail(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);

var result = cmd.getResult() as ReadPersonDetail_Result;
var isReady = result.IsReady;//Whether the personnel exists or not.
var person  = result.Person;//Personnel details
```



## 5>.Add personnel

```c#
//Command details
var cmdDetail = GetCommandDetail();
//User list
var persons = new List<Person>();
//user info
var person = new Person();
person.UserCode = 12345;
person.PName = "test";
persons.Add(person);
//Create a command object
var par = new AddPerson_Parameter(persons);
//Create a command object
var cmd = new AddPerson(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
```



## 6>.delete personnel

```c#
//Command details
var cmdDetail = GetCommandDetail();
//user list
var persons = new List<Person>();
//user info
var person = new Person();
person.UserCode = 12345;//The user number is a unique identifier. When deleting personnel, the user number must be provided.
persons.Add(person);
//Create a command object
var par = new DeletePerson_Parameter(persons);
//Create a command object
var cmd = new DeletePerson(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
```



## 7>.Notify the device to enter data entry mode.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//usre info
var person = new Person();
person.UserCode = 12345;//user id
person.PName = "test";//name
//Create a command object
var par = new RegisterIdentificationData_Parameter(person,3);
//Create a command object
var cmd = new RegisterIdentificationData(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
//返回结果
var result = cmd.getResult() as RegisterIdentificationData_Result;
var Status = result.Status;//Status codes:1.Registration has started. 2.User number does not exist. 3.Type error or not supported. 4.The index number is out of range. 5.Device storage space is full. 101.Registration successful. 102. User canceled operation. 103.Duplicate registration information.
var userID =  result.UserID;//When the status is 103, this indicates the user number of the duplicated registration information
var ResultData =  result.ResultData;//Return the face or fingerprint feature registered from the device.

```

**IdentificationData（Personnel recognition information）**

| Attribute| Type   | Explanation                                                         |
| -------- | ------ | ------------------------------------------------------------ |
| DataType | int    | Data type, value range: 1-4; 1 - personnel photo, 2 - fingerprint feature code, 3 - infrared face feature code, 4 - dynamic face feature code. |
| DataNum  | int    | Data index. When the data type is 2, the value range is 0-9.                    |
| DataBuf  | byte[] | Data buffer                                                   |



# 9、Record operation

## 1>.Read record pointer information

```C#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var cmd = new ReadTransactionDatabaseDetail(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);

var result = cmd.getResult() as ReadTransactionDatabaseDetail_Result;
var Status = result.Status;//Status codes:1.Registration has started. 2.User number does not exist. 3.Type error or not supported. 4.The index number is out of range. 5.Device storage space is full. 101.Registration successful. 102. User canceled operation. 103.Duplicate registration information.
var databaseDetail =  result.DatabaseDetail;//Details of the record database
var CardTransactionDetail = databaseDetail.CardTransactionDetail;//Recognition record
var DoorSensorTransactionDetail = databaseDetail.DoorSensorTransactionDetail;//Door magnetic record
var SystemTransactionDetail = databaseDetail.SystemTransactionDetail;//System record
var BodyTemperatureTransactionDetail = databaseDetail.BodyTemperatureTransactionDetail;//Temperature record
```

**TransactionDetail**

| Attribute       | Type | Explanation     |
| --------------- | ---- | --------------- |
| DataBaseMaxSize | long | Record capacity|
| WriteIndex      | long | Record the last digits |
| ReadIndex       | long | Upload breakpoint |
| IsCircle        | bool | Loop marker. |

## 2>.Clear all records

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Create a command object
var par = new ClearTransactionDatabase_Parameter();
//Create a command object
var cmd = new ClearTransactionDatabase(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
```



## 3>.Clear record

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Types of records that need to be cleared, including:OnCardTransaction、OnDoorSensorTransaction、OnSystemTransaction、OnBodyTemperatureTransaction
var type = e_TransactionDatabaseType.OnCardTransaction;
//Create a command object
var par = new ClearTransactionDatabase_Parameter(type);
//Create a command object
var cmd = new ClearTransactionDatabase(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
```



## 4>.Update record pointer

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Types of records that need to update the pointer, including:OnCardTransaction、OnDoorSensorTransaction、OnSystemTransaction、OnBodyTemperatureTransaction
var type = e_TransactionDatabaseType.OnCardTransaction;
//pointer index
var readIndex = 1;
//Create a command object
var par = new WriteTransactionDatabaseReadIndex_Parameter(type, readIndex);
//Create a command object
var cmd = new WriteTransactionDatabaseReadIndex(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
```



## 5>.Update record last digits

```c#
//Command details
var cmdDetail = GetCommandDetail();
//Types of records that need to update the pointer, including:OnCardTransaction、OnDoorSensorTransaction、OnSystemTransaction、OnBodyTemperatureTransaction
var type = e_TransactionDatabaseType.OnCardTransaction;
//Record last digits index.
var writeIndex = 100;
//Create a command object
var par = new WriteTransactionDatabaseWriteIndex_Parameter(type, readIndex);
//Create a command object
var cmd = new WriteTransactionDatabaseWriteIndex(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);
```



## 6>.Read record -- read by record number.

```c#
//Command details
var cmdDetail = GetCommandDetail();
//including:OnCardTransaction、OnDoorSensorTransaction、OnSystemTransaction、OnBodyTemperatureTransaction
var type = (int)e_TransactionDatabaseType.OnCardTransaction;
//Record start index.
var readIndex = 100;
//Number of records to read.
var quantity = 30;
//Create a command object
var par = new ReadTransactionDatabaseByIndex_Parameter(type, readIndex, quantity);
//Create a command object
var cmd = new ReadTransactionDatabaseByIndex(cmdDtl, par);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);

var result = cmd.getResult() as ReadTransactionDatabaseByIndex_Result;

var TransactionType = result.TransactionType;//Record type
var ReadIndex = result.ReadIndex;//Record start index.
var Quantity = result.Quantity;
var TransactionList = result.TransactionList;//Record list, the returned record content is determined based on the record type.

```



# 10、Real-time monitoring msg

Real-time monitoring Messages are divided into UDP and TCP protocol types, which can be selected in the device's server settings.

The usage process can be divided into three steps:

```c#
        //Global definition
        ConnectorAllocator _connectorAllocator = ConnectorAllocator.GetAllocator();
        string mServerIP = "192.168.1.110"; 
        int mServerPort = 9001;
        ConcurrentDictionary<string, INConnectorDetail> ClientConnectorList = new();//Used to save the connection information of the device.
        ConcurrentDictionary<string, string> DeviceList = new();//Used to save and manage the connection information of the device.
		/// <summary>
        /// Constructor method
        /// </summary>
 		public ListenerDemo()
        {
            _connectorAllocator.ClientOnline += _connectorAllocator_ClientOnline;
            _connectorAllocator.ClientOffline += _connectorAllocator_ClientOffline;
            _connectorAllocator.TransactionMessage += _connectorAllocator_TransactionMessage;
        }
```



## 1>、Start listening

**NOTE**

Both TCP and UDP listening methods can be used simultaneously.。

**UDP Listening example**

```c#
 		/// <summary>
        /// listening UDP
        /// </summary>
        public void ListenerUDP()
        {
            var udp = new UDPServerDetail(mServerIP, mServerPort); //Creat UDPServer obj
            _connectorAllocator.OpenForciblyConnect(udp);//Start UDP Server
        }
```

**TCP Listening example**

```c#
        /// <summary>
        /// Listening TCP
        /// </summary>
        public void ListenerTCP()
        {
            var tcp = new TCPServerDetail(mServerIP, mServerPort);
            _connectorAllocator.OpenForciblyConnect(tcp);//Start TCP Server
        }
```

## 2>、Device online/offline listening

Add or remove connection objects when the device goes online or offline.

```C#
        /// <summary>
        /// Device online/offline listening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connectorAllocator_ClientOffline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            var key = inc.GetKey();
            if (ClientConnectorList.ContainsKey(key))
            {
                //Remove connection object.
                ClientConnectorList.Remove(key, out _);
                Console.WriteLine("device offline");
            }
        }
        /// <summary>
        /// device online listening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void _connectorAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            var connectorType = inc.GetConnectorType();
            var key = inc.GetKey();
            if (connectorType == ConnectorType.TCPServerClient || connectorType == ConnectorType.UDPClient)
            {
                var fC8800Request = new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
                inc.RemoveRequestHandle(typeof(Door8800RequestHandle));//Remove first to prevent adding when it already exists.
                inc.AddRequestHandle(fC8800Request);
                if (!ClientConnectorList.ContainsKey(key))
                {
                    ClientConnectorList.TryAdd(key, inc.GetConnectorDetail());//Add connection object.
                }
            }
        }
```

**Parser processing factory.**

```c#
		/// <summary>
        /// Parser processing factory.
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="cmdIndex"></param>
        /// <param name="cmdPar"></param>
        /// <returns></returns>
        private static AbstractTransaction RequestHandleFactory(string sn, byte cmdIndex, byte cmdPar)
        {
            if (cmdIndex >= 1 && cmdIndex <= 4)
            {    //Record message         
                return ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
            }
            if (cmdIndex == 0x22)
            {
                //Heartbeat message
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.KeepaliveTransaction();
            }

            if (cmdIndex == 0xA0)
            {
                //Connection test message
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.ConnectMessageTransaction();
            }
            return null;
        }
```



## 3>、Receive message push event

```c#
/// <summary>
/// Receive message push event（When the device has a message to push, it will respond to this event）
/// </summary>
private static void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData){
      		CheckOpenForciblyConnect(connector);//Check if the long connection is open
            Door8800Transaction fcTrn = EventData as Door8800Transaction;
            var SN = fcTrn.SN;
            AddDevice(connector, SN);//Add device
            switch (fcTrn.CmdIndex)
            {
                case 0x01:
                    //0x01 Recognize record 
                     var cardTransaction = EventData as CardTransaction;
                    break;
                case 0x04:
                    //0x04 Body temperature record
                    var bodyTemperatureTransaction = EventData as BodyTemperatureTransaction;
                    break;
                case 0x02:
                    //0x02 Door magnetic record
                    var doorSensorTransaction = EventData as DoorSensorTransaction;
                    break;
                case 0x03:
                    //0x03 System record
                    var systemTransaction = EventData as SystemTransaction;
                    break;
                case 0x22:
                case 0xA0:
                    //Keep-alive message and connection test message
                    //Reply response
                    var sndConntmsg = new SendConnectTestResponse(GetCommandDetail(SN));
                    _connectorAllocator.AddCommand(sndConntmsg);
                    break;
                default:
                    break;
            }
}
```



**Check if the long connection is open**

```c#
 /// <summary>
        /// Check if the long connection is open
        /// </summary>
        /// <param name="connector"></param>
        private void CheckOpenForciblyConnect(INConnectorDetail connector)
        {
            var conn = _connectorAllocator.GetConnector(connector);
            if (conn != null)
            {
                if (!conn.IsForciblyConnect())
                {
                    conn.OpenForciblyConnect();//Save long connection
                }
            }
        }
```

**Add device**

```c#
 		/// <summary>
        /// Add device
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="SN"></param>
        private void AddDevice(INConnectorDetail connector, string SN)
        {
            var key = connector.GetKey();
            if (ClientConnectorList.ContainsKey(key))
            {
                if (!DeviceList.ContainsKey(SN))
                {
                    DeviceList.TryAdd(SN, key);
                }
                else
                {
                    DeviceList[SN] = key;
                }
            }
        }
```

**Get Command details**

```c#
 /// <summary>
        /// Get Command details
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public INCommandDetail GetCommandDetail(string sn)
        {
            if (DeviceList.ContainsKey(sn))
            {
                var key = DeviceList[sn];
                if (ClientConnectorList.ContainsKey(key))
                {
                    var cnt = ClientConnectorList[key];
                    var result = new OnlineAccessCommandDetail(cnt, sn, "ffffffff");
                    return result;
                }
            }
            return default;
        }
```

# 11、Remote upgrade

**Load software key**

```c#
public Dictionary<string, EquptAESKey> LoadAesKey(){
var typeFile = "softwarekey.txt"; //software key
var sTypes = File.ReadAllText(typeFile, Encoding.UTF8).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
var mAesKey = new Dictionary<string, EquptAESKey>();
foreach (var sTypeLine in sTypes)
{
  var sTypeDetails = sTypeLine.Split(',');
  var oKey = new EquptAESKey(sTypeDetails[0], sTypeDetails[1], sTypeDetails[2].ToInt32());
  mAesKey.Add(oKey.Name, oKey);
}
}
```

**Verify firmware**

```c#
public bool CheckSoftware(byte[] bSurFile ){
      var mAesKey = LoadAesKey();
      var oItem = mAesKey["FC8300"];
      var sKey = Encoding.ASCII.GetString(bSurFile, 0, 16);
      if (!sKey.Equals(oItem.Key))
      {
       //Key information verification failed        
       return false;
      }
      var iFileLen = bSurFile.Length; //firmware length
      int iSoftwareSize = iFileLen - 26;
      uint iFileCRC32 = bSurFile.Copy(iFileLen - 4, 4).ToInt32();
      byte[] bSoftWareData = bSurFile.Copy(22, iSoftwareSize);
      uint itmpCRC32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(bSoftWareData, 0, (uint)iSoftwareSize);
      if (itmpCRC32 != iFileCRC32)
      {
        //crc check failed       
         return false;
      }
    return true;
}
```

**Face recognition device upgrade**

```c#

var bSurFile = File.ReadAllBytes(@"D:\FC8300_502.RCbin");//Read firmware file
if(!CheckSoftware(bSurFile)){//Verify parameters
    return;
}
var iFileLen = bSurFile.Length; //Firmware length
int iSoftwareSize = iFileLen - 26;
byte[] bSoftWareData = bSurFile.Copy(22, iSoftwareSize);
byte[] iCRCBuf = bSurFile.Copy(18, 4);
uint iSoftwareCRC32 = iCRCBuf.ToInt32();
//Create firmware upgrade command parameter object
var par = new Software.UpdateSoftware_Parameter(bSoftWareData, iSoftwareCRC32);
//If upgrading fingerprint device is required, the command object needs to be changed to:UpdateSoftware_FP
//Create face recognition device firmware upgrade command object.
var cmd = new UpdateSoftware(cmdDetail);
//Add to the connection allocator
await _connectorAllocator.AddCommandAsync(cmd);

var result = cmd.getResult() as UpdateSoftware_Result;

var Success =  result.Success;//Write result 1--verification successful 0--verification failed.
var SkipPacketCount = result.SkipPacketCount; //Number of skipped packets.

```

