/****************************************************************
 *  Program : FPDATACONVPRO.H                                   *
 *  Purpose : DataConv in PC                                    *
 *  Compile : Microsoft Visual C++ 6.0 (Windows XP)             *
 *  Version : PEFIS FINGER 1.0                                  *
 *  Copyright (C) 2013  Amnokgang Technology Development Corp.  *
 *  All Rights Reserved.                                        *
 ****************************************************************/

#ifndef _FPDATACONVPRO_H_
#define _FPDATACONVPRO_H_

#define FPCONV_API __declspec(dllexport) __stdcall

#define		_FpOK		  				0
#define		DATA_VER_70					0x70
#define		DATA_VER_80					0x80
#define		DATA_VER_89					0x89
#define		MAX_FTR_LEN					(2048)

#ifdef __cplusplus
extern "C" {
#endif

void	FPCONV_API FPCONV_Init( void ) ;
long	FPCONV_API FPCONV_GetConvDLLModel( char *Model ) ;
long	FPCONV_API FPCONV_GetFpDataValidity( void* apFpDataBuffer ) ;
long	FPCONV_API FPCONV_GetFpDataVersionAndSize( void* apFpDataBuffer, unsigned long* apnVersion, unsigned long* apnSize ) ;
long	FPCONV_API FPCONV_GetFpDataSize( unsigned long anFpDataVersion, unsigned long* apnFpDataSize ) ;
long	FPCONV_API FPCONV_Convert( unsigned long anSrcVer, void* apSrcFpData, unsigned long anDestVer, void* apDestFpData ) ;

long	FPCONV_API FPCONV_ISOToPEFIS( void* apSrcFpData, int nSrcDataSize, void* apDestFpData, int *nDestDataSize ) ;
long	FPCONV_API FPCONV_PEFISToISO( void* apSrcFpData, int nSrcDataSize, void* apDestFpData, int *nDestDataSize ) ;

#ifdef __cplusplus
}
#endif

#endif // _FPDATACONVPRO_H_
