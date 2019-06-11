cmake_minimum_required(VERSION 2.8)

project(Buzzard)

if (${CMAKE_SYSTEM_NAME} STREQUAL "Darwin")
  set(CMAKE_MACOSX_RPATH 1)
endif()

set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS_DEBUG} -g -Wall")
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS_DEBUG} -g -Wall")

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/inc)

set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)
set(SRCS)
set(SRCS ${SRCS} ${SRC_DIR}/image.c)

add_library(Buzzard-shared SHARED ${SRCS})
SET_TARGET_PROPERTIES(Buzzard-shared PROPERTIES OUTPUT_NAME Buzzard)

add_library(Buzzard-static STATIC ${SRCS})
SET_TARGET_PROPERTIES(Buzzard-static PROPERTIES OUTPUT_NAME Buzzard)
