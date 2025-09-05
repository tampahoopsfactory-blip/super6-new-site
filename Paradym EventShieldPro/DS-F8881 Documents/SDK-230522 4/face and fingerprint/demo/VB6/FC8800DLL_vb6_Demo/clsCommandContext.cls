VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsCommandContext"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Public LocalIP As String
Public LocalPort As Long

Public SearchCount As Long

Public Users As DriveCardLists






Public Records As DriveRecordLists
Public Record As DriveRecord


'Command Operation Index
Public CommandOperateIndex As Long

'Device fingerprinting algorithm version number
Public FingerprintVer As Long

'Fingerprint index number
Public FPIndex As Long

'Fingerprint file index number
Public FPFileIndex As Long



'Palm vein  index number
Public PalmVeinIndex As Long


'Palm vein file index number
Public PalmVeinFileIndex As Long




'Current progress of the operation
Public CurrentProgress As String

