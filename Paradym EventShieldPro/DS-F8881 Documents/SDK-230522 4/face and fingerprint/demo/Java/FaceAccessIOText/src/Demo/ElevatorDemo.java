package Demo;

import Door.Access.Command.*;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.E_WeekDay;
import Door.Access.Door8800.Command.Data.TimeGroup.DayTimeGroup;
import Door.Access.Door8800.Command.Data.TimeGroup.TimeSegment;
import Door.Access.Door8800.Door8800Identity;
import Face.Elevator.Person.*;
import Face.Elevator.System.RelayType.ReadRelayType;
import Face.Elevator.System.RelayType.ReadRelayType_Result;
import Face.Elevator.System.RelayType.WriteRelayType;
import Face.Elevator.System.RelayType.WriteRelayType_Parameter;
import Face.Elevator.System.ReleaseTime.ReadReleaseTime;
import Face.Elevator.System.ReleaseTime.ReadReleaseTime_Result;
import Face.Elevator.System.ReleaseTime.WriteReleaseTime;
import Face.Elevator.System.ReleaseTime.WriteReleaseTime_Parameter;
import Face.Elevator.System.Remote.*;
import Face.Elevator.System.TimingOpen.*;
import Face.Elevator.System.WorkType.ReadWorkType;
import Face.Elevator.System.WorkType.ReadWorkType_Result;
import Face.Elevator.System.WorkType.WriteWorkType;
import Face.Elevator.System.WorkType.WriteWorkType_Parameter;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.Arrays;

public class ElevatorDemo implements INConnectorEvent {
    ConnectorAllocator _Allocator;
    String LocalIp = "192.168.1.97";
    int LocalPort = 8801;

    private CommandDetail getDetail() {
        CommandDetail commandDetail = new CommandDetail();
        /**
         * 此函数超时时间设定长一些
         */
        commandDetail.Timeout = 8000;
        UDPDetail cDetail = new UDPDetail("192.168.1.132", 8686, LocalIp, LocalPort);
        commandDetail.Connector = cDetail;
        /**
         * 设置SN(16位字符)，密码(8位十六进制字符)，设备类型
         */
        Door8800Identity idt = new Door8800Identity("FC-8400T22050715", "FFFFFFFF", E_ControllerType.Face_Fingerprint);
        commandDetail.Identity = idt;
        return commandDetail;
    }

    public ElevatorDemo() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.UDPBind(LocalIp, LocalPort);

    }

    private ArrayList<Byte> GetRelays() {
        ArrayList<Byte> relays = new ArrayList<>(64);
        for (int i = 0; i < 64; i++) {
            relays.add((byte) 0);
        }
        return relays;
    }

    /**
     * 电梯开门
     */
    public void openRelayTest() {
        CommandDetail commandDetail = getDetail();
        ArrayList<Byte> relays = GetRelays();
        relays.set(0, (byte) 1);
        RemoteRelay_Parameter parameter = new RemoteRelay_Parameter(commandDetail, relays);
        OpenRelay cmd = new OpenRelay(parameter);
        _Allocator.AddCommand(cmd);
        closeRelayTest();
    }

    /**
     * 电梯关门
     */
    public void closeRelayTest() {
        CommandDetail commandDetail = getDetail();
        ArrayList<Byte> relays = GetRelays();
        relays.set(0, (byte) 1);
        RemoteRelay_Parameter parameter = new RemoteRelay_Parameter(commandDetail, relays);
        CloseRelay cmd = new CloseRelay(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 锁定梯控
     */
    public void lockRelayTest() {
        CommandDetail commandDetail = getDetail();
        ArrayList<Byte> relays = GetRelays();
        for (int i = 0; i < relays.size(); i++) {
            relays.set(i, (byte) 1);
        }

        RemoteRelay_Parameter parameter = new RemoteRelay_Parameter(commandDetail, relays);
        LockRelay cmd = new LockRelay(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 电梯解锁
     */
    public void unlockRelayTest() {
        CommandDetail commandDetail = getDetail();
        ArrayList<Byte> relays = GetRelays();
        for (int i = 0; i < relays.size(); i++) {
            relays.set(i, (byte) 1);
        }

        RemoteRelay_Parameter parameter = new RemoteRelay_Parameter(commandDetail, relays);
        UnlockRelay cmd = new UnlockRelay(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 电梯常开
     */
    public void holdRelayTest() {
        CommandDetail commandDetail = getDetail();
        ArrayList<Byte> relays = GetRelays();
        for (int i = 0; i < relays.size(); i++) {
            relays.set(i, (byte) 1);
        }

        RemoteRelay_Parameter parameter = new RemoteRelay_Parameter(commandDetail, relays);
        HoldRelay cmd = new HoldRelay(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 开门保持时间
     */
    public void ReadReleaseTimeTest() {
        CommandDetail commandDetail = getDetail();
        ReadReleaseTime cmd = new ReadReleaseTime(new CommandParameter(commandDetail));
        _Allocator.AddCommand(cmd);
    }

    /**
     * 设置开门保持时间
     */
    public void WriteReleaseTimeTest() {
        CommandDetail commandDetail = getDetail();
        ArrayList<Integer> releaseTimes = new ArrayList<>();
        for (int i = 0; i < 64; i++) {
            releaseTimes.add(3);
        }

        WriteReleaseTime cmd = new WriteReleaseTime(new WriteReleaseTime_Parameter(commandDetail, releaseTimes));
        _Allocator.AddCommand(cmd);
    }

    /**
     * 设置电梯常开
     */
    public void ReadTimingOpenTest() {

        CommandDetail commandDetail = getDetail();
        ReadTimingOpen cmd = new ReadTimingOpen(new ReadTimingOpen_Parameter(commandDetail, (byte) 1));
        _Allocator.AddCommand(cmd);
    }

    /**
     *设置定时常开命令
     */
    public  void WriteTimingOpenTest(){
        CommandDetail commandDetail = getDetail();
        WriteTimingOpen_Parameter parameter =new WriteTimingOpen_Parameter(commandDetail);
        parameter.Port=1;//楼号 1-64
        parameter.Use=1;//是否启用常开
        parameter.WorkType=1;//工作模式

        for(E_WeekDay e :E_WeekDay.values()){
                TimeSegment ts = parameter.WeekTimeGroup.GetItem(e).GetItem(0); //设置一周的常开时段
                ts.SetBeginTime(0,0);//这里设置8个时段中的第一个时段，有需要可以设置多个
                ts.SetEndTime(23,49);
        }
        WriteTimingOpen cmd = new WriteTimingOpen(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 获取电梯模块启用状态
     */
    public  void ReadWorkTypeTest(){
        CommandDetail commandDetail = getDetail();
        ReadWorkType cmd = new ReadWorkType(new CommandParameter(commandDetail));
        _Allocator.AddCommand(cmd);
    }

    /**
     * 设置电梯模块启用状态
     */
    public  void WriteWorkTypeTest(){
        CommandDetail commandDetail = getDetail();
        WriteWorkType_Parameter parameter=new WriteWorkType_Parameter(commandDetail,
                0);//0--禁用电梯模式 1--启动电梯模式
        WriteWorkType cmd = new WriteWorkType(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 电梯继电器板的64个继电器输出类型的参数
     *  1、COM_NC常闭（默认值）
     *  2、COM_NO常闭
     */
    public  void ReadRelayTypeTest(){
        CommandDetail commandDetail = getDetail();
        ReadRelayType cmd = new ReadRelayType(new CommandParameter(commandDetail));
        _Allocator.AddCommand(cmd);
    }

    /**
     * 写入电梯继电器板的64个继电器输出类型的参数
     */
    public  void WriteRelayTypeTest(){
        CommandDetail commandDetail = getDetail();
        ArrayList<Byte> relays=new ArrayList<>(64);
        for(int i=0;i<64;i++){
            relays.add((byte)1);// 1、COM_NC常闭（默认值） 2、COM_NO常闭
        }
        WriteRelayType_Parameter parameter=new WriteRelayType_Parameter(commandDetail,relays);
        WriteRelayType cmd = new WriteRelayType(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 读取人员电梯扩展权限
     */
    public  void ReadPersonElevatorAccessTest(){
        CommandDetail commandDetail = getDetail();
        ReadPersonElevatorAccess_Parameter  parameter=new ReadPersonElevatorAccess_Parameter(commandDetail,1);//1 是用户编号
        ReadPersonElevatorAccess cmd=new ReadPersonElevatorAccess(parameter);
        _Allocator.AddCommand(cmd);
    }
    public  void WritePersonElevatorAccessTest(){
        CommandDetail commandDetail = getDetail();
        commandDetail.Timeout=5000;
        WritePersonElevatorAccess_Parameter parameter=new WritePersonElevatorAccess_Parameter(commandDetail);
        parameter.UserCode = 1;
        for(int i=0;i<64;i++){
            parameter.RelayAccesss.add((byte)1);//0表示无权限，1表示有权限
        }
        WritePersonElevatorAccess cmd=new WritePersonElevatorAccess(parameter);
        _Allocator.AddCommand(cmd);
    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof CloseRelay) {
            System.out.println("关门成功");
        } else if (cmd instanceof OpenRelay) {
            System.out.println("开门成功");
        } else if (cmd instanceof ReadReleaseTime) {
            ReadReleaseTime_Result res = (ReadReleaseTime_Result) result;
            for (int b : res.ReleaseTimes) {
                System.out.println("开锁保持时长：" + b);
            }

        } else if (cmd instanceof ReadTimingOpen) {
            ReadTimingOpen_Result res = (ReadTimingOpen_Result) result;
            System.out.println("端口：" + res.Port);
            System.out.println("是否启用：" + res.Use);
            System.out.println("常开工作模式：" + res.WorkType);
            ShowDayTimeGroup(res.WeekTimeGroup.GetItem(E_WeekDay.Monday));

        }else if(cmd instanceof  ReadWorkType){
            ReadWorkType_Result res=(ReadWorkType_Result) result;
            System.out.println("是否电梯模式启用：" + res.WorkType);
        }else if(cmd instanceof  ReadRelayType){
            ReadRelayType_Result res=(ReadRelayType_Result) result;
            for(int i=0;i<64;i++){
               System.out.println((i+1)+":"+res.RelayTypes.get(i));// 1、COM_NC常闭（默认值） 2、COM_NO常闭
            }
        }else  if(cmd instanceof  ReadPersonElevatorAccess){
            ReadPersonElevatorAccess_Result res= (ReadPersonElevatorAccess_Result) result;

            System.out.println("用户编号："+res.UserCode);
            System.out.println("状态："+res.Status);//1--表示成功；0--表示用户号未登记；2--表示不支持此功能
            if(res.Status==1){
                for(int i=0;i<64;i++){
                    System.out.println((i+1)+":"+res.RelayAccesss.get(i));//0表示无权限，1表示有权限
                }
            }
        }else if(cmd instanceof  WritePersonElevatorAccess){
            WritePersonElevatorAccess_Result res= (WritePersonElevatorAccess_Result)result;

            System.out.println("用户编号："+res.UserCode);
            System.out.println("状态："+res.Status);//1--表示成功；0--表示用户号未登记；2--表示不支持此功能
        }
    }

    private void ShowDayTimeGroup(DayTimeGroup dayTimeGroup) {

        for (int i = 0; i < 8; i++) {
            TimeSegment ts = dayTimeGroup.GetItem(i);
            short[] mBeginTime = new short[2];
            short[] mEndTime = new short[2];
            ts.GetBeginTime(mBeginTime);
            ts.GetEndTime(mEndTime);
            System.out.print(mBeginTime[0] + ":" + mBeginTime[1]);
            System.out.print("\t");
            System.out.println(mEndTime[0] + ":" + mEndTime[1]);
        }

    }

    @Override
    public void CommandProcessEvent(INCommand cmd) {

    }

    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {

    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {

    }

    @Override
    public void CommandTimeout(INCommand cmd) {

    }

    @Override
    public void PasswordErrorEvent(INCommand cmd) {

    }

    @Override
    public void ChecksumErrorEvent(INCommand cmd) {

    }

    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {

    }

    @Override
    public void ClientOnline(ConnectorDetail client) {

    }

    @Override
    public void ClientOffline(ConnectorDetail client) {

    }
}
