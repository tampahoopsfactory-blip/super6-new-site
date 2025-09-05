/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Demo;

import Door.Access.Connector.ConnectorAllocator;

import java.math.BigInteger;

/**
 * @author F
 */
public class JavaDemo {
    public static void main(String[] args) throws InterruptedException {

        //   BigInteger MaxCardData=new  BigInteger("FFFFFFFFFF",16);
        //   BigInteger cardData=new BigInteger("1245115");
        //   if(MaxCardData.compareTo(cardData)== -1){
        //        System.out.println("超过最大卡号限制");
        //     }else{
        //         System.out.println("正常卡号");
        //      }

        //    BigInteger bigInteger=new BigInteger("FFFFFFFFFF",16);
        //   System.out.println("Demo.JavaDemo.main()"+bigInteger);
        //   byte [] bs=bigInteger.toString(16).getBytes();
        //    System.out.println("Demo.JavaDemo.main()"+bs.length);

        // for (int i = 0; i < bs.length; i++) {
        //      System.out.println(bs[i]);
        //   }
        // TODO code application logic here
        //门相关操作
        ConnectorAllocator a = ConnectorAllocator.GetAllocator();
        // TimeGroup group=new    TimeGroup(a);
        //  group.SetTimeGroup();
        //  group.syn();
        //数据监控
        // DataMonitor dm = new DataMonitor(a);
        //  dm.OpenMonitor();
        //  ReadRecord readRecord=new ReadRecord(a);
        //  readRecord.Read();
        // DataMonitor dm=new  DataMonitor(a);
        // dm.OpenMonitor();
        OpenDoor od = new OpenDoor(a);
         od.deletePassword();
        //od.readPushButtonSetting();
          od.syn();
        //Thread.sleep(10000);
        //od.open();
        // OpenDoor od2 = new OpenDoor(a);
        // od2.open();
        //  od2.syn();
        //卡片相关操作
       // UploadCard uc = new UploadCard(a);
     //   uc.readCardDataBase();
     //   uc.syn();
        //   uc.UploadSortCardList();
        //   uc.ClearCards();
        //时间相关操作
        //   SetDateTime sdt = new SetDateTime(a);
        //   sdt.Set();
        //   sdt.SetCustomize();
        //  sdt.Get();
        // ReadRecord  readRecord=new  ReadRecord(a);
        //  readRecord.ReadPasswordDetail();
        // readRecord.syn();
        // System.out.println("");
        //  byte[] b = new byte[10];
        //System.in.read();
        //a.Release();

    }
}
