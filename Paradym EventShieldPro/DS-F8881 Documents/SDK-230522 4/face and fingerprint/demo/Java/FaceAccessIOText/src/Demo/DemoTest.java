/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Demo;


import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.E_WeekDay;
import Door.Access.Door8800.Command.Data.TimeGroup.TimeSegment;
import Door.Access.Door8800.Command.Data.TimeGroup.WeekTimeGroup;
import Door.Access.Door8800.Command.DateTime.ReadTime;
import Door.Access.Door8800.Command.DateTime.Result.ReadTime_Result;
import Door.Access.Door8800.Door8800Identity;
import Face.AdditionalData.Parameter.ReadFeatureCode_Parameter;
import Face.AdditionalData.Parameter.ReadFile_Parameter;
import Face.AdditionalData.Parameter.ReadPersonAdditionalDetail_Parameter;
import Face.AdditionalData.Parameter.WriteFeatureCode_Parameter;
import Face.AdditionalData.ReadFeatureCode;
import Face.AdditionalData.ReadFile;
import Face.AdditionalData.ReadPersonAdditionalDetail;
import Face.AdditionalData.Result.ReadFeatureCode_Result;
import Face.AdditionalData.WriteFeatureCode;
import Face.Data.IdentificationData;
import Face.Data.Person;
import Face.Data.e_TransactionDatabaseType;
import Face.Door.*;
import Face.Door.Parameter.*;
import Face.Door.Result.ReaderOption_Result;
import Face.Person.*;
import Face.Person.Parameter.*;
import Face.System.*;
import Face.System.Parameter.WriteManageMenuPassword_Parameter;
import Face.System.Parameter.WriteWiegandOutput_Parameter;
import Face.TimeGroup.AddTimeGroup;
import Face.TimeGroup.ClearTimeGroup;
import Face.TimeGroup.Parameter.AddTimeGroup_Parameter;
import Face.TimeGroup.ReadTimeGroup;
import Face.Transaction.*;
import Face.Transaction.Parameter.ClearTransactionDatabase_Parameter;
import Face.Transaction.Parameter.ReadTransactionDatabaseByIndex_Parameter;
import Face.Transaction.Parameter.ReadTransactionDatabase_Parameter;
import Face.Transaction.Parameter.WriteTransactionDatabaseReadIndex_Parameter;

import java.io.DataInputStream;
import java.io.FileInputStream;
import java.util.ArrayList;
import java.util.concurrent.Semaphore;
import java.util.Base64;
import java.util.Date;
/*import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;*/


/**
 * @author F
 */
public class DemoTest implements INConnectorEvent {

    String LocalIp = "192.168.1.130";
    int LocalPort = 8801;
    ConnectorAllocator _Allocator;
    private final Semaphore available = new Semaphore(0, true);
    ///等待
public  boolean  IsOpen;
    public void syn() {
        try {
            available.acquire();
        } catch (Exception e) {
        }

    }
//关闭等待

    public void release() {
      //  _Allocator.Release();
        available.release();
    }

    private CommandDetail getDetail() {
        CommandDetail commandDetail = new CommandDetail();
        /**
         * 此函数超时时间设定长一些
         */
        commandDetail.Timeout = 8000;
        UDPDetail cDetail = new UDPDetail("192.168.1.62", 8101, LocalIp, LocalPort);
        cDetail.Timeout=20000;
        cDetail.RestartCount=0;
        commandDetail.Connector = cDetail;
        /**
         * 设置SN(16位字符)，密码(8位十六进制字符)，设备类型
         */
        Door8800Identity idt = new Door8800Identity("FC-8300T21076163", "FFFFFFFF", E_ControllerType.Face_Fingerprint);
        commandDetail.Identity = idt;
        return commandDetail;
    }

    public void getSn() {
        ReadSn cmd = new ReadSn(new CommandParameter(getDetail()));
        _Allocator.AddCommand((cmd));
    }

    public DemoTest() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.UDPBind(LocalIp, LocalPort);

    }

    public void ReadPersonDatabaseDetail() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadPersonDatabaseDetail cmd = new ReadPersonDatabaseDetail(par);
        _Allocator.AddCommand(cmd);
    }

    public void OpenDoor_CheckNum() {
        //  CommandParameter parameter=new CommandParameter(getDetail());
        Remote_CheckNum_Parameter par = new Remote_CheckNum_Parameter(getDetail(), (byte) 1);
        OpenDoor_CheckNum cmd = new OpenDoor_CheckNum(par);
        _Allocator.AddCommand(cmd);
    }

    public void OpenDoor() {
        
        CommandParameter par = new CommandParameter(getDetail());
        OpenDoor cmd = new OpenDoor(par);
        _Allocator.AddCommand(cmd);
        syn();
    }

    public void HoldDoor() {
        CommandParameter par = new CommandParameter(getDetail());
        HoldDoor cmd = new HoldDoor(par);
        _Allocator.AddCommand(cmd);
    }

    public void CloseDoor() {
        CommandParameter par = new CommandParameter(getDetail());
        CloseDoor cmd = new CloseDoor(par);
        _Allocator.AddCommand(cmd);
    }

    public void LockDoor() {
        CommandParameter par = new CommandParameter(getDetail());
        LockDoor cmd = new LockDoor(par);
        _Allocator.AddCommand(cmd);
    }

    public void UnlockDoor() {
        CommandParameter par = new CommandParameter(getDetail());
        UnlockDoor cmd = new UnlockDoor(par);
        _Allocator.AddCommand(cmd);
    }

    public void ReadReaderOption() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadReaderOption cmd = new ReadReaderOption(par);
        _Allocator.AddCommand(cmd);
    }

    public void WriteReaderOption() {

        ReaderOption_Parameter par = new ReaderOption_Parameter(getDetail(), (byte) 1);
        WriteReaderOption cmd = new WriteReaderOption(par);
        _Allocator.AddCommand(cmd);
    }

    public void readRelayOption() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadRelayOption cmd = new ReadRelayOption(par);
        _Allocator.AddCommand(cmd);
    }

    public void writeRelayOption() {
        RelayOption_Parameter par = new RelayOption_Parameter(getDetail(), true);
        WriteRelayOption cmd = new WriteRelayOption(par);
        _Allocator.AddCommand(cmd);

    }

    public void readUnlockingTime() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadUnlockingTime cmd = new ReadUnlockingTime(par);
        _Allocator.AddCommand(cmd);
    }

    public void writeUnlockingTime() {
        UnlockingTime_Parameter par = new UnlockingTime_Parameter(getDetail(), 5);
        WriteUnlockingTime cmd = new WriteUnlockingTime(par);
        _Allocator.AddCommand(cmd);
    }

    public void readExemptionVerificationOpen() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadExemptionVerificationOpen cmd = new ReadExemptionVerificationOpen(par);
        _Allocator.AddCommand(cmd);
    }

    public void writeExemptionVerificationOpen() {
        ExemptionVerificationOpen_Parameter par = new ExemptionVerificationOpen_Parameter(getDetail(), false, true, (byte) 1);
        WriteExemptionVerificationOpen cmd = new WriteExemptionVerificationOpen(par);
        _Allocator.AddCommand(cmd);
    }

    public void readVoiceBroadcastSetting() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadVoiceBroadcastSetting cmd = new ReadVoiceBroadcastSetting(par);
        _Allocator.AddCommand(cmd);
    }

    public void writeVoiceBroadcastSetting() {
        VoiceBroadcastSetting_Parameter parameter = new VoiceBroadcastSetting_Parameter(getDetail(), false);
        WriteVoiceBroadcastSetting cmd = new WriteVoiceBroadcastSetting(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readReaderIntervalTime() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadReaderIntervalTime cmd = new ReadReaderIntervalTime(par);
        _Allocator.AddCommand(cmd);
    }

    public void writeReaderIntervalTime() {
        ReaderIntervalTime_Parameter par = new ReaderIntervalTime_Parameter(getDetail(), true, (byte) 2, 30);
        WriteReaderIntervalTime cmd = new WriteReaderIntervalTime(par);
        _Allocator.AddCommand(cmd);
    }

    public void readExpirationPrompt() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadExpirationPrompt cmd = new ReadExpirationPrompt(par);
        _Allocator.AddCommand(cmd);
    }

    public void writeExpirationPrompt() {
        ExpirationPrompt_Parameter parameter = new ExpirationPrompt_Parameter(getDetail(), true, (byte) 10);
        WriteExpirationPrompt cmd = new WriteExpirationPrompt(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readPersonDataBase() {


        CommandParameter parameter = new CommandParameter(getDetail());
        ReadPersonDataBase cmd = new ReadPersonDataBase(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void addPerson() {

        ArrayList<Person> PersonList = new ArrayList<Person>();
        Person person = new Person();
        person.PName = "测试555";
        person.UserCode = 100023;
        PersonList.add(person);

        Person person2 = new Person();
        person2.PName = "测试555444";
        person2.UserCode = 100024;

        PersonList.add(person2);
        Person_Parameter parameter = new Person_Parameter(getDetail(), PersonList);
        AddPerson cmd = new AddPerson(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void deletePerson() {
        ArrayList<Integer> userCodeList = new ArrayList<Integer>();
        userCodeList.add(100023);
        userCodeList.add(100024);
        DeletePerson_Parameter parameter = new DeletePerson_Parameter(getDetail(), userCodeList);
        DeletePerson cmd = new DeletePerson(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void registerIdentificationData() {
        Person person = new Person();
        person.PName = "测试111111";
        person.UserCode = 1234556;
        RegisterIdentificationData_Parameter parameter = new RegisterIdentificationData_Parameter(getDetail(), person, 3);
        RegisterIdentificationData cmd = new RegisterIdentificationData(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void addPersonAndImage() {
       // for (int i=0;i<10;i++) {
            IdentificationData[] data = new IdentificationData[1];
            Person person = new Person();
            person.PName = "陈"+1;
            person.UserCode = 1000;
            byte[] data1 =  ReadImage("D:\\图片\\103 - 副本.jpg");

            data[0] = new IdentificationData(1, 1, data1);
            // data[1] = new IdentificationData(2, 2, data2);
            AddPersonAndImage_Parameter parameter = new AddPersonAndImage_Parameter(getDetail(), person, data);
            AddPersonAndImage cmd = new AddPersonAndImage(parameter);
            _Allocator.AddCommand(cmd);
       // }
    }

    public void readFile() {
        ReadFile_Parameter parameter = new ReadFile_Parameter(getDetail(), 1, 3, 1);
        ReadFile cmd = new ReadFile(parameter);
        _Allocator.AddCommand(cmd);
    }

    private byte[] ReadImage(String path) {
        try {
            DataInputStream stream = new DataInputStream(new FileInputStream(path));
            byte[] file = new byte[stream.available()];
            stream.read(file);
            return file;

        } catch (Exception ex) {

        }
        return null;
    }

    public void readFeatureCode() {
        ReadFeatureCode_Parameter parameter = new ReadFeatureCode_Parameter(getDetail(), 10000, 2, 0);
        ReadFeatureCode cmd = new ReadFeatureCode(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readPersonDetail() {
        ReadPersonDetail_Parameter parameter = new ReadPersonDetail_Parameter(getDetail(), 230);
        ReadPersonDetail cmd = new ReadPersonDetail(parameter);
        _Allocator.AddCommand(cmd);
    }


    public void readTime(){
        CommandParameter parameter=new CommandParameter(getDetail());
        ReadTime cmd=new ReadTime(parameter);
        _Allocator.AddCommand(cmd);
    }
    public void readManageMenuPassword(){
        CommandParameter parameter=new CommandParameter(getDetail());
        ReadManageMenuPassword cmd=new ReadManageMenuPassword(parameter);
        _Allocator.AddCommand(cmd);
    }

    public  void writeManageMenuPassword(){
        WriteManageMenuPassword_Parameter parameter=new WriteManageMenuPassword_Parameter(getDetail(),"1234");
        WriteManageMenuPassword cmd=new WriteManageMenuPassword(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void writeFeatureCode() {

        //   ReadFeatureCode_Result feature= (ReadFeatureCode_Result)result;
        byte[] data1 = Base64.getDecoder().decode("cDg3ArAgEkFWWIyUneS/AgwBAwcOBQkNAgMCCgQIBgsGAA4Fin6PlKV1pVZbi1iBinyElGC9AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIKcpXF+V8aNadWgR5CTxci/yAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAICAAAB4EZGZEYGCAgqKpW0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////////////////////////////////////////////////////////D/z//////0cA9P////9vAAD4////fwAAAP////8CAADg////LwAAAPj//78AAAAA////C0AAAPD//z8AAAAA////DwAAAND//z8AAAAA/f//CwAAAMD//78CAAAA/P//DwAAAMD///8AAAAA/P//LyoAAMD///+jAgAA+P//vyoAgIr///9vAACo/P///wsAgOr/////AnCC/v///7+qq+D/////r6qq//////+bqv7/AAAYGAABAO6b1szgqhvMo+YA+gbtCPJN7+bqPf8HLisyEQAZYDBi");
        WriteFeatureCode_Parameter parameter = new WriteFeatureCode_Parameter(getDetail(), 10000, 2, 0, data1);

        WriteFeatureCode cmd1 = new WriteFeatureCode(parameter);
        _Allocator.AddCommand(cmd1);
    }

    public void readTransactionDatabaseDetail() {
        CommandParameter parameter = new CommandParameter(getDetail());
        ReadTransactionDatabaseDetail cmd = new ReadTransactionDatabaseDetail(parameter);
        _Allocator.AddCommand(cmd);
    }


    public void writeTransactionDatabaseReadIndex() {
        WriteTransactionDatabaseReadIndex_Parameter parameter = new WriteTransactionDatabaseReadIndex_Parameter(getDetail(), e_TransactionDatabaseType.OnSystemTransaction);
        WriteTransactionDatabaseReadIndex cmd = new WriteTransactionDatabaseReadIndex(parameter);
        _Allocator.AddCommand(cmd);

    }

    public void clearTransactionDatabase() {
        ClearTransactionDatabase_Parameter parameter = new ClearTransactionDatabase_Parameter(getDetail(), e_TransactionDatabaseType.OnDoorSensorTransaction);
        ClearTransactionDatabase cmd = new ClearTransactionDatabase(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readTransactionDatabaseByIndex() {
        ReadTransactionDatabaseByIndex_Parameter parameter = new ReadTransactionDatabaseByIndex_Parameter(getDetail(), e_TransactionDatabaseType.OnCardTransaction);
        parameter.Quantity = 100;
        parameter.ReadIndex = 90;
        ReadTransactionDatabaseByIndex cmd = new ReadTransactionDatabaseByIndex(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readTransactionDatabase() {
        ReadTransactionDatabase_Parameter parameter = new ReadTransactionDatabase_Parameter(getDetail(), e_TransactionDatabaseType.OnCardTransaction);
        // parameter.PacketSize=60;
        parameter.Quantity = 10;
        ReadTransactionDatabase cmd = new ReadTransactionDatabase(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readPersonAdditionalDetail() {
        ReadPersonAdditionalDetail_Parameter parameter = new ReadPersonAdditionalDetail_Parameter(getDetail(), 230);
        ReadPersonAdditionalDetail cmd = new ReadPersonAdditionalDetail(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readWiegandOutput() {
        CommandParameter parameter = new CommandParameter(getDetail());
        ReadWiegandOutput cmd = new ReadWiegandOutput(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void writeWiegandOutput() {
        WriteWiegandOutput_Parameter parameter = new WriteWiegandOutput_Parameter(getDetail(), (byte) 1, (byte) 1, (byte) 1, (byte) 2);
        WriteWiegandOutput cmd = new WriteWiegandOutput(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readTimeGroup() {
        CommandParameter parameter = new CommandParameter(getDetail());
        ReadTimeGroup cmd = new ReadTimeGroup(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void clearTimeGroup() {
        CommandParameter parameter = new CommandParameter(getDetail());
        ClearTimeGroup cmd = new ClearTimeGroup(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void addTimeGroup() {
        ArrayList<WeekTimeGroup> list = new ArrayList<>();
        for (int i = 0; i < 64; i++) {
            WeekTimeGroup group = new WeekTimeGroup(8);
            TimeSegment t = group.GetItem(E_WeekDay.Monday).GetItem(0);
            t.SetBeginTime(0, 0);
            t.SetEndTime(23, 59);
            list.add(group);
        }

        TimeSegment item = list.get(0).GetItem(E_WeekDay.Monday).GetItem(0);
        item.SetBeginTime(8, 0);
        item.SetEndTime(20, 0);
        AddTimeGroup_Parameter parameter = new AddTimeGroup_Parameter(getDetail(), list);
        AddTimeGroup cmd = new AddTimeGroup(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void readSystemStatus(){
        ReadSystemStatus cmd=new ReadSystemStatus(new CommandParameter(getDetail()));
        _Allocator.AddCommand(cmd);
    }
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {

       // ReadTime_Result time= (ReadTime_Result) result;
      //  System.out.println(time.ControllerDate.getTime());
        //release();
         Date date = new Date();
         System.out.println("开门成功"+date.toString());
        // ReadPersonDatabaseDetail_Result detail = (ReadPersonDatabaseDetail_Result) result;
/*
        if (result instanceof ReaderOption_Result) {
            ReaderOption_Result result1 = (ReaderOption_Result) result;
            System.out.println(result1.ReaderOption);
        }
*/

      /*  ObjectMapper mapper=new ObjectMapper();
        try {
            System.out.println("成功"+mapper.writerWithDefaultPrettyPrinter().writeValueAsString(result));
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }*/

    }


    @Override
    public void CommandProcessEvent(INCommand cmd) {
        System.out.println("执行进度..");
    }

    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        throw new UnsupportedOperationException("2"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {
        throw new UnsupportedOperationException("3"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void CommandTimeout(INCommand cmd) {
        throw new UnsupportedOperationException("4"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        throw new UnsupportedOperationException("5"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
        throw new UnsupportedOperationException("6"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {
        throw new UnsupportedOperationException("7"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOnline(ConnectorDetail client) {
        throw new UnsupportedOperationException("8"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOffline(ConnectorDetail client) throws UnsupportedOperationException {
        throw new UnsupportedOperationException("9"); //To change body of generated methods, choose Tools | Templates.
    }
}
