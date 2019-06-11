#ifndef __BUZZARD_H__
#define __BUZZARD_H__

#define null NULL

#include <stdio.h>
#include <stdint.h>
#include <string.h>

#include <stdbool.h>

typedef int8_t             int8;
typedef uint8_t            uint8;
typedef int16_t            int16;
typedef uint16_t           uint16;
typedef int32_t            int32;
typedef uint32_t           uint32;
typedef long long          int64;
typedef unsigned long long uint64;
typedef const char*        string;

#if defined( _MSC_VER )
  #define BUZZARD_API __declspec(dllexport)
  #define BUZZARD_CC __stdcall
#else
  #define BUZZARD_API
  #define BUZZARD_CC
#endif

#endif
