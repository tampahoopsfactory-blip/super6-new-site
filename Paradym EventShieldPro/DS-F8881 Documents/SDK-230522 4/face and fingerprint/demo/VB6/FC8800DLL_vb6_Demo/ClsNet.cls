VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ClsNet"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private Const MAX_ADAPTER_NAME_LENGTH = 260
Private Const MAX_ADAPTER_ADDRESS_LENGTH = 8
Private Const MAX_ADAPTER_DESCRIPTION_LENGTH = 132
Private Const ERROR_BUFFER_OVERFLOW = 111
Private Const MIB_IF_TYPE_ETHERNET = 6
Private Const MIB_IF_TYPE_TOKENRING = 9
Private Const MIB_IF_TYPE_FDDI = 15
Private Const MIB_IF_TYPE_PPP = 23
Private Const MIB_IF_TYPE_LOOPBACK = 24
Private Const MIB_IF_TYPE_SLIP = 28

Private Type IP_ADDR_STRING
            Next As Long
            IPAddress As String * 16
            IpMask As String * 16
            Context As Long
End Type

Private Type IP_ADAPTER_INFO
            Next As Long
            ComboIndex As Long
            AdapterName As String * MAX_ADAPTER_NAME_LENGTH
            Description As String * MAX_ADAPTER_DESCRIPTION_LENGTH
            AddressLength As Long
            Address(7) As Byte
            Index As Long
            Type As Long
            DhcpEnabled As Long
            CurrentIpAddress As Long
            IpAddressList As IP_ADDR_STRING
            GatewayList As IP_ADDR_STRING
            DhcpServer As IP_ADDR_STRING
            HaveWins As Byte
            PrimaryWinsServer As IP_ADDR_STRING
            SecondaryWinsServer As IP_ADDR_STRING
            LeaseObtained As Long
            LeaseExpires As Long
End Type


Private Declare Function GetAdaptersInfo Lib "IPHlpApi.dll" _
       (IpAdapterInfo As Any, pOutBufLen As Long) As Long
Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
       (Destination As Any, Source As Any, ByVal Length As Long)
      

Public Function MacAddress(ByRef sMAC() As String) As Boolean
 
    Dim error As Long
    Dim AdapterInfoSize As Long
    Dim i As Integer
    Dim PhysicalAddress  As String
    Dim AdapterInfo As IP_ADAPTER_INFO
    Dim pAdapt As Long
    Dim Buffer2 As IP_ADAPTER_INFO
    Dim AdapterInfoBuffer() As Byte
    Dim CurrentMAC As String
   
    MacAddress = False
    AdapterInfoSize = 0
    error = GetAdaptersInfo(ByVal 0&, AdapterInfoSize)
    If error <> 0 Then
        If error <> ERROR_BUFFER_OVERFLOW Then
           Exit Function
        End If
    End If
    ReDim AdapterInfoBuffer(AdapterInfoSize - 1)

    error = GetAdaptersInfo(AdapterInfoBuffer(0), AdapterInfoSize)
    If error <> 0 Then
       MsgBox "GetAdaptersInfo failed with error " & error
       Exit Function
    End If
    Dim IPInfoSize As Long
    IPInfoSize = Len(AdapterInfo)
    CopyMemory AdapterInfo, AdapterInfoBuffer(0), IPInfoSize
    pAdapt = AdapterInfo.Next
    If AdapterInfo.AddressLength > 0 Then pAdapt = 1
    Do While pAdapt <> 0
        CopyMemory Buffer2, AdapterInfo, IPInfoSize
        
        PhysicalAddress = ""
        For i = 0 To Buffer2.AddressLength - 1
            PhysicalAddress = PhysicalAddress & Format(Hex(Buffer2.Address(i)), "00")
            If i < Buffer2.AddressLength - 1 Then
                PhysicalAddress = PhysicalAddress & "-"
            End If
        Next
        CurrentMAC = CurrentMAC & PhysicalAddress & vbNewLine
       
        pAdapt = Buffer2.Next
        If pAdapt <> 0 Then
            CopyMemory AdapterInfo, ByVal pAdapt, IPInfoSize
        End If
    Loop
    If Len(CurrentMAC) > 0 Then
        sMAC = Split(CurrentMAC, vbNewLine)
        MacAddress = True
    Else
        MacAddress = False
    End If
    
End Function


Public Function IPAddress(ByRef sIPList() As String) As Boolean
 
    Dim error As Long
    Dim AdapterInfoSize As Long
    Dim i As Integer
    Dim PhysicalAddress  As String
    Dim AdapterInfo As IP_ADAPTER_INFO
    Dim pAdapt As Long
    Dim Buffer2 As IP_ADAPTER_INFO
    Dim AdapterInfoBuffer() As Byte
    Dim sCurrentIP As String
    Dim IPInfoSize As Integer
    IPInfoSize = Len(AdapterInfo)
    
    IPAddress = False
    AdapterInfoSize = 0
    error = GetAdaptersInfo(ByVal 0&, AdapterInfoSize)
    If error <> 0 Then
        If error <> ERROR_BUFFER_OVERFLOW Then
           Exit Function
        End If
    End If
    ReDim AdapterInfoBuffer(AdapterInfoSize - 1)

    error = GetAdaptersInfo(AdapterInfoBuffer(0), AdapterInfoSize)
    If error <> 0 Then
       MsgBox "GetAdaptersInfo failed with error " & error
       Exit Function
    End If
    IPAddress = False
    CopyMemory AdapterInfo, AdapterInfoBuffer(0), IPInfoSize
    pAdapt = AdapterInfo.Next
    If AdapterInfo.AddressLength > 0 Then pAdapt = 1
    Do While pAdapt <> 0
        CopyMemory Buffer2, AdapterInfo, IPInfoSize

        PhysicalAddress = Buffer2.IpAddressList.IPAddress
        PhysicalAddress = Replace(PhysicalAddress, Chr(0), vbNullString)
        sCurrentIP = sCurrentIP & PhysicalAddress & vbNewLine

        pAdapt = Buffer2.Next
        If pAdapt <> 0 Then
            CopyMemory AdapterInfo, ByVal pAdapt, IPInfoSize
        End If
    Loop
    If Len(sCurrentIP) > 0 Then
        sIPList = Split(sCurrentIP, vbNewLine)
        IPAddress = True
    Else
        IPAddress = False
    End If
    
End Function
