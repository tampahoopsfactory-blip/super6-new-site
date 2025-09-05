/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Demo;
import Door.Access.Command.CommandDetail;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Door8800.Command.System.Parameter.SearchEquptOnNetNum_Parameter;
import Door.Access.Door8800.Command.System.SearchEquptOnNetNum;
import java.util.Scanner;
/**
 *
 * @author F
 */
public class JavaDemo {
           public static void main(String[] args) {
        // TODO code application logic here
        //门相关操作
        ConnectorAllocator a = ConnectorAllocator.GetAllocator();
         // DataMonitor dm=new  DataMonitor(a);
          //dm.OpenMonitor();
        //数据监控
       // DataMonitor dm = new DataMonitor(a);
        //dm.OpenMonitor();
       // dm.syn();
     //   OpenDoor od = new OpenDoor(a);
      //  Scanner input=new Scanner(System.in);
     //          while (true) {   
    //               System.out.println("请输入门号1-4");
    //                int doorNum=input.nextInt();
     //               if(doorNum>4||doorNum<1){
    //                    System.out.println("门号不能大于4或小于1");
     //               }else{
    //                   od.openDoor(doorNum);  
   //                 }                    
 //              }
      //TcpServerTest test=new  TcpServerTest();
     // test.syn();
    //   od.deletePassword();
     //   od.readPasswordDataBase();
     //  od.syn();
        ReadSn sn=new   ReadSn();
        sn.GetSn();
        sn.syn();
       // od.CloseDoor();
        //卡片相关操作
      // UploadCard uc = new UploadCard(a);
       // uc.UploadCardList();
       // uc.UploadSortCardList();
       // uc.GetCard();
      //  uc.syn();
        //时间相关操作
//        SetDateTime sdt = new SetDateTime(a);
//        sdt.Set();
//        sdt.SetCustomize();
//        sdt.Get();
        
      //  System.out.println("123");
       // byte[] b = new byte[10];
        //System.in.read();
        //a.Release();

    }
}
