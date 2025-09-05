Attribute VB_Name = "ModAst2500"
Option Explicit

'枚举所有已连接的指纹仪
Public Declare Function pisEnumerateDevice Lib "Ast2600N.dll" (ByVal devOrderNumber As Long, ByRef strDevId As Byte, ByRef strDevName As Byte) As Long
'创建一个指纹仪操作上下文对象
Public Declare Function pisCreateContext Lib "Ast2600N.dll" (ByRef lContextID As Long) As Long
'删除创建的指纹仪操作上下文
Public Declare Function pisDestroyContext Lib "Ast2600N.dll" (ByVal lContextID As Long) As Long

'打开指纹仪的链接
Public Declare Function pisOpenDevice Lib "Ast2600N.dll" (ByVal lContextID As Long, ByVal sDevID As String) As Long

'关闭指纹仪的链接
Public Declare Function pisCloseDevice Lib "Ast2600N.dll" (ByVal lContextID As Long) As Long


'获得模块信息,能得到 指纹算法引擎，图像宽度和高度，临时指纹模板大小，正式指纹模板的大小
'imaWidth--图像宽度，imaHeight--图像高度，imaRes----图像精度，featureSize--临时模板尺寸，templateSize--正式模板尺寸
Public Declare Function pisGetInfo Lib "Ast2600N.dll" (ByVal lContextID As Long, ByVal strEngineInfo As String, _
ByRef imaWidth As Long, ByRef imaHeight As Long, ByRef imaRes As Long, ByRef featureSize As Long, ByRef templateSize As Long) As Long


'获取一个指纹图像,bImageBuf 的尺寸大概是 256*256，具体需要调用 pisGetInfo 获取图像尺寸
Public Declare Function pisCapture Lib "Ast2600N.dll" (ByVal lContextID As Long, ByRef bImageBuf As Byte) As Long


'检查一个图像是否为指纹图像，
'bImageBuf---图像缓冲区，需要检查的图像存储空间
'imaWidth--图像宽度，imaHeight--图像高度，imaRes----图像精度
'isCheckFp--检测结果：0 – 非指纹图像，1 – 指纹图像
'fpArea -- 图像中指纹占比，此值越大，在整个图像中指纹图像的区域越大
Public Declare Function pisCheckFp Lib "Ast2600N.dll" (ByVal lContextID As Long, ByRef bImageBuf As Byte, _
ByVal imaWidth As Long, ByVal imaHeight As Long, ByVal imaRes As Long, ByRef isCheckFp As Long, ByRef fpArea As Long) As Long

'根据指纹图像生成一个指纹特征模板（临时指纹模板）
'bImageBuf---图像缓冲区，需要检查的图像存储空间
'imaWidth--图像宽度，imaHeight--图像高度，imaRes----图像精度
'bFeatureData--存储生成指纹特征码的临时模板缓冲区
Public Declare Function pisProcess Lib "Ast2600N.dll" (ByVal lContextID As Long, ByRef bImageBuf As Byte, _
ByVal imaWidth As Long, ByVal imaHeight As Long, ByVal imaRes As Long, ByRef bFeatureData As Byte) As Long


'根据 第三方指纹采集仪 采集的指纹图像生成一个指纹特征模板（临时指纹模板）
'bImageBuf---图像缓冲区，需要检查的图像存储空间
'imaWidth--图像宽度，imaHeight--图像高度，imaRes----图像精度
'bFeatureData--存储生成指纹特征码的临时模板缓冲区
Public Declare Function pisProcessImport Lib "Ast2600N.dll" (ByVal lContextID As Long, ByRef bImageBuf As Byte, _
ByVal imaWidth As Long, ByVal imaHeight As Long, ByVal imaRes As Long, ByRef bFeatureData As Byte) As Long


'根据 三次指纹采集的后生成的指纹特征码，生成一个正式指纹模板
'featureData1，featureData2，featureData3---in 三次成功采集指纹后生成的指纹特征码
'templateData--out 用于存放正式指纹模板的缓冲区
'缓冲区长度需要从 pisGetInfo 函数获取
Public Declare Function pisCreateTemplate Lib "Ast2600N.dll" (ByVal lContextID As Long, _
ByRef featureData1 As Byte, ByRef featureData2 As Byte, ByRef featureData3 As Byte, _
ByRef templateData As Byte) As Long



'创建用于指纹识别的指纹模板集合
'templateID -- 模板ID
'templateData -- 模板缓冲区
Public Declare Function pisAddTptArray Lib "Ast2600N.dll" (ByVal lContextID As Long, _
ByVal templateID As Long, ByRef templateData As Byte) As Long


'比对指纹模板，使用指定模板和模板集合中的所有模板比较
'queryTemplateData -- 需要比对的模板缓冲区
'identifiedTID -- 输出  返回比对成功的模板ID
Public Declare Function pisIdentify Lib "Ast2600N.dll" (ByVal lContextID As Long, _
ByRef queryFeatureData As Byte, ByRef identifiedTID As Long, _
ByRef updatedTemplateData As Byte, ByRef updatedFlag As Long) As Long


'比对指纹模板，使用指定模板和模板集合中的所有模板比较
'queryTemplateData -- 需要比对的模板缓冲区
'identifiedTID -- 输出  返回比对成功的模板ID
Public Declare Function pisIdentifyTpl Lib "Ast2600N.dll" (ByVal lContextID As Long, _
ByRef queryTemplateData As Byte, ByRef identifiedTID As Long) As Long





'控制指纹仪播放语音
'0--请按指纹,1--请重新按指纹,2--请再次按指纹,3--登记成功,谢谢,4--该指纹已有注册,5--请按上一个指纹,6--请读卡
Public Declare Function pisSoundPlay Lib "Ast2600N.dll" (ByVal lContextID As Long, ByVal lSoundIndex As Long) As Long

'控制指纹仪的LED灯
'lLEDKind--LED灯的颜色性质：1--Gree,1--Red,1--BKGrond
'lLampSwitch -- LED灯的开关 0--on,1--off
Public Declare Function pisLedControl Lib "Ast2600N.dll" (ByVal lContextID As Long, ByVal lLEDKind As Long, ByVal lLampSwitch As Long) As Long


'控制指纹仪的读卡功能
'lSwitch-- 开关，1--开始读卡；0--禁止读卡
Public Declare Function pisScanCard Lib "Ast2600N.dll" (ByVal lContextID As Long, ByVal lSwitch As Long) As Long


'从指纹仪中获取都到的卡号
'bCardBuf-- 存储卡号的缓冲区
Public Declare Function pisGetCardNumber Lib "Ast2600N.dll" (ByVal lContextID As Long, ByRef bCardBuf As Byte) As Long


