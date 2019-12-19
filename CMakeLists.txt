cmake_minimum_required(VERSION 3.15)

set (CMAKE_CXX_STANDARD 14)

project(UnityKinematics)


if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE "Release" CACHE STRING
        "Choose the type of build, options are: Debug Release RelWithDebInfo MinSizeRel."
        FORCE)
endif()


set(RPCLIB_INCLUDE_DIR "" CACHE PATH "rpclib include directory, usually /usr/local/include on linux")
set(RPCLIB_LIBS_DIR "" CACHE PATH "rpclib library directory, usually /usr/local/lib on linux")

if (RPCLIB_INCLUDE_DIR STREQUAL "")
    message(FATAL_ERROR "No rpclib include directory specified")
endif()

if (RPCLIB_LIBS_DIR STREQUAL "")
    message(FATAL_ERROR "No rpclib library directory specified")
endif()


include_directories(SYSTEM "${RPCLIB_INCLUDE_DIR}")
link_directories("${RPCLIB_LIBS_DIR}")


set(SRC_PATH "${PROJECT_SOURCE_DIR}/src")
set(COMMON_PATH "${SRC_PATH}/common")
set(CLIENT_PATH "${SRC_PATH}/client")
set(SERVER_PATH "${SRC_PATH}/server")

include_directories("${COMMON_PATH}/include")
include_directories("${CLIENT_PATH}/include")
include_directories("${SERVER_PATH}/include")


if (${CMAKE_BUILD_TYPE} MATCHES "Debug")
    add_compile_definitions(_DEBUG)
else()
    add_compile_definitions(NDEBUG)
endif()


function(createLibrary varName varSrcPath)
    file(GLOB_RECURSE TMP_SRC
        "${varSrcPath}/include/*.h"
        "${varSrcPath}/include/*.hpp"
        "${varSrcPath}/*.cpp"
    )
    add_library(${varName} ${TMP_SRC})
    target_link_libraries(${varName} ${ARGN})
    target_compile_options(${varName} PRIVATE -Wall -Wextra -pedantic -Werror -fPIC)
endfunction(createLibrary)

function(createExecutable varName varSrcPath)
    file(GLOB_RECURSE TMP_SRC
        "${varSrcPath}/include/*.h"
        "${varSrcPath}/include/*.hpp"
        "${varSrcPath}/*.cpp"
    )
    add_executable(${varName} ${TMP_SRC})
    target_link_libraries(${varName} ${ARGN})
    target_compile_options(${varName} PRIVATE -Wall -Wextra -pedantic -Werror -fPIC)
endfunction(createExecutable)


set(LIBS librpc.a)
set(SYS_LIBS pthread)

createExecutable(client ${CLIENT_PATH} ${LIBS} ${SYS_LIBS})
createExecutable(server ${SERVER_PATH} ${LIBS} ${SYS_LIBS})

