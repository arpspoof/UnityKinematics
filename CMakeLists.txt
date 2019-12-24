cmake_minimum_required(VERSION 3.15)

set (CMAKE_CXX_STANDARD 14)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)

project(UnityKinematics)

if(NOT WIN32)
  string(ASCII 27 Esc)
  set(ColourReset "${Esc}[m")
  set(BoldYellow  "${Esc}[1;33m")
endif()
message(NOTICE "${BoldYellow}Important Notice: Please build rpclib using CMAKE_POSITION_INDEPENDENT_CODE${ColourReset}")

if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE "Release" CACHE STRING
        "Choose the type of build, options are: Debug Release RelWithDebInfo MinSizeRel."
        FORCE)
endif()

set(CMAKE_INSTALL_PREFIX "${PROJECT_SOURCE_DIR}/install" CACHE PATH "install prefix" FORCE)

set(RPCLIB_INCLUDE_DIR "/usr/local/include" CACHE PATH "rpclib include directory")
set(RPCLIB_LIBS_DIR "/usr/local/lib" CACHE PATH "rpclib library directory")

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

createLibrary(common ${COMMON_PATH} ${LIBS} ${SYS_LIBS})
createLibrary(client ${CLIENT_PATH} common ${LIBS} ${SYS_LIBS})
createLibrary(server ${SERVER_PATH} common ${LIBS} ${SYS_LIBS})

set_target_properties(client PROPERTIES PUBLIC_HEADER 
    "src/common/include/DataFormat.hpp;src/client/include/api/KinematicsClient.hpp"
)
set_target_properties(server PROPERTIES PUBLIC_HEADER 
    "src/common/include/DataFormat.hpp;src/server/include/api/KinematicsServer.hpp"
)

INSTALL(TARGETS client 
        LIBRARY DESTINATION KinematicsClient/lib
        PUBLIC_HEADER DESTINATION include/KinematicsClient
)
INSTALL(TARGETS server 
        LIBRARY DESTINATION KinematicsServer/lib
        PUBLIC_HEADER DESTINATION include/KinematicsServer
)



set(BUILD_UNITY_API ON CACHE BOOL "Choose whether unity interface should be built")
if (${BUILD_UNITY_API})

set(BUILD_UNITY_API_DIR "${CMAKE_BINARY_DIR}/UnityServerAPI" CACHE PATH "choose where to build unity server API")

find_package(SWIG 4.0.1 REQUIRED)
include(${SWIG_USE_FILE})

set_source_files_properties("${SERVER_PATH}/include/api/KinematicsServer.i" PROPERTIES CPLUSPLUS ON)

swig_add_library(UnityServerAPI 
    LANGUAGE csharp 	
    TYPE MODULE
    OUTPUT_DIR "${BUILD_UNITY_API_DIR}/src"
    OUTFILE_DIR "${CMAKE_BINARY_DIR}/_cstemp"
    SOURCES "${SERVER_PATH}/include/api/KinematicsServer.i"
)

add_dependencies(UnityServerAPI server)

swig_link_libraries(UnityServerAPI server ${LIBS} ${SYS_LIBS})

add_custom_target(build_server_dll ALL
    COMMAND cp -f "${CMAKE_BINARY_DIR}/UnityServerAPI.so" "${BUILD_UNITY_API_DIR}/UnityServerAPI.so"
    COMMAND mcs -out:${BUILD_UNITY_API_DIR}/api.dll -pkg:dotnet -target:library ${BUILD_UNITY_API_DIR}/src/*.cs
)

add_dependencies(build_server_dll UnityServerAPI)

endif()



set(BUILD_TEST_CODE ON CACHE BOOL "Choose whether to build test code")
if (${BUILD_TEST_CODE})

set(TEST_PATH "${SRC_PATH}/test")

add_executable(testserver ${TEST_PATH}/server.cpp)
target_link_libraries(testserver server ${LIBS} ${SYS_LIBS})
target_compile_options(testserver PRIVATE -Wall -Wextra -pedantic -Werror -fPIC)
add_dependencies(testserver server)

add_executable(testclient ${TEST_PATH}/client.cpp)
target_link_libraries(testclient client ${LIBS} ${SYS_LIBS})
target_compile_options(testclient PRIVATE -Wall -Wextra -pedantic -Werror -fPIC)
add_dependencies(testclient client)

endif()
