cmake_minimum_required(VERSION 3.15)

set (CMAKE_CXX_STANDARD 14)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)

project(UnityKinematics)

if(NOT WIN32)
    string(ASCII 27 Esc)
    set(ColourReset "${Esc}[m")
    set(BoldYellow  "${Esc}[1;33m")
    message(NOTICE "${BoldYellow}Important Notice: Please build rpclib using CMAKE_POSITION_INDEPENDENT_CODE${ColourReset}")
endif()

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
if(WIN32)
    target_compile_options(${varName} PRIVATE -Wall -pedantic -fPIC)
else()
    target_compile_options(${varName} PRIVATE -Wall -Wextra -pedantic -Werror -fPIC)
endif()
endfunction(createLibrary)

function(createExecutable varName varSrcPath)
    file(GLOB_RECURSE TMP_SRC
        "${varSrcPath}/include/*.h"
        "${varSrcPath}/include/*.hpp"
        "${varSrcPath}/*.cpp"
    )
    add_executable(${varName} ${TMP_SRC})
    target_link_libraries(${varName} ${ARGN})
if(WIN32)
    target_compile_options(${varName} PRIVATE -Wall -pedantic -fPIC)
else()
    target_compile_options(${varName} PRIVATE -Wall -Wextra -pedantic -Werror -fPIC)
endif()
endfunction(createExecutable)


set(LIBS rpc)

if(WIN32)
set(SYS_LIBS )
else()
set(SYS_LIBS pthread)
endif()

#createLibrary(common ${COMMON_PATH} ${LIBS} ${SYS_LIBS})
createLibrary(client ${CLIENT_PATH} ${LIBS} ${SYS_LIBS})
createLibrary(server ${SERVER_PATH} ${LIBS} ${SYS_LIBS})

set_target_properties(client PROPERTIES PUBLIC_HEADER 
    "src/common/include/DataFormat.hpp;src/common/include/ActorBuffer.hpp;src/common/include/Command.hpp;src/client/include/api/KinematicsClient.hpp;src/client/include/RenderController.hpp;src/client/include/AbstractCommandHandler.hpp;src/client/include/AbstractDataProvider.hpp"
)

INSTALL(TARGETS client 
        LIBRARY DESTINATION lib
        PUBLIC_HEADER DESTINATION include
)



set(BUILD_UNITY_ASSETS ON CACHE BOOL "Choose whether unity assets should be built")
if (${BUILD_UNITY_ASSETS})

set(BUILD_UNITY_ASSETS_DIR "${CMAKE_BINARY_DIR}/Assets" CACHE PATH "choose where to build unity assets")

find_package(SWIG 4.0.1 REQUIRED)
include(${SWIG_USE_FILE})

set_source_files_properties("${SERVER_PATH}/include/api/KinematicsServer.i" PROPERTIES CPLUSPLUS ON)

swig_add_library(UnityServerAPI 
    LANGUAGE csharp 	
    TYPE MODULE
    OUTPUT_DIR "${CMAKE_BINARY_DIR}/_csbuild"
    OUTFILE_DIR "${CMAKE_BINARY_DIR}/_cstemp"
    SOURCES "${SERVER_PATH}/include/api/KinematicsServer.i"
)

add_dependencies(UnityServerAPI server)

swig_link_libraries(UnityServerAPI server ${LIBS} ${SYS_LIBS})

if(WIN32)
set(CSC_PATH "C:/Windows/Microsoft.NET/Framework64/v4.0.30319" CACHE PATH "Path to Windows C# compiler (csc.exe)")
set(CSC ${CSC_PATH}/csc.exe)
string(REPLACE / \\ BUILD_UNITY_ASSETS_DIR_RV ${BUILD_UNITY_ASSETS_DIR})
string(REPLACE / \\ SRC_PATH_RV ${SRC_PATH})
string(REPLACE / \\ CMAKE_BINARY_DIR_RV ${CMAKE_BINARY_DIR})
add_custom_target(build_server_dll ALL
    COMMAND cd ${CMAKE_BINARY_DIR}/_csbuild
    COMMAND ${CSC} -out:api.dll -target:library *.cs
    COMMAND (if exist ${BUILD_UNITY_ASSETS_DIR_RV} rd /s /q ${BUILD_UNITY_ASSETS_DIR_RV})
    COMMAND mkdir ${BUILD_UNITY_ASSETS_DIR_RV}\\Scripts\\API
    COMMAND xcopy /Y /s ${SRC_PATH_RV}\\unity\\*.* ${BUILD_UNITY_ASSETS_DIR_RV}
    COMMAND xcopy /Y /s ${CMAKE_BINARY_DIR_RV}\\_csbuild\\api.dll ${BUILD_UNITY_ASSETS_DIR_RV}\\Scripts\\API
    COMMAND xcopy /Y ${CMAKE_BINARY_DIR_RV}\\UnityServerAPI.dll ${BUILD_UNITY_ASSETS_DIR_RV}\\Scripts\\API
)
else()
add_custom_target(build_server_dll ALL
    COMMAND mcs -out:${CMAKE_BINARY_DIR}/_csbuild/api.dll -pkg:dotnet -target:library ${CMAKE_BINARY_DIR}/_csbuild/*.cs
    COMMAND mkdir -p ${BUILD_UNITY_ASSETS_DIR}
    COMMAND cp -rf ${SRC_PATH}/unity/* ${BUILD_UNITY_ASSETS_DIR}
    COMMAND mkdir -p ${BUILD_UNITY_ASSETS_DIR}/Scripts/API
    COMMAND cp -f ${CMAKE_BINARY_DIR}/UnityServerAPI.so ${CMAKE_BINARY_DIR}/_csbuild/api.dll ${BUILD_UNITY_ASSETS_DIR}/Scripts/API/
)
endif()

add_dependencies(build_server_dll UnityServerAPI)

endif()



set(BUILD_CLIENT_PY_API ON CACHE BOOL "Choose whether to build python binding of unity renderer client API")
if (${BUILD_CLIENT_PY_API})

set(BUILD_CLIENT_PY_API_DIR "${CMAKE_BINARY_DIR}/pyUnityRenderer" CACHE PATH "choose where to build python client API")

if(WIN32)
set(PYTHON_INCLUDE_PATH "" CACHE PATH "Python include dir, usually C:/Users/xx/AppData/Local/Programs/Python/Python37/include")
set(PYTHON_LIB "" CACHE PATH "Python lib dir, usually C:/Users/xx/AppData/Local/Programs/Python/Python37/libs")
else()
find_package(PythonLibs 3.7.4 REQUIRED)
find_package(PythonInterp ${PYTHONLIBS_VERSION_STRING} REQUIRED)
endif()

include_directories(${PYTHON_INCLUDE_PATH})
link_directories(${PYTHON_LIB})

find_package(SWIG 4.0.1 REQUIRED)
include(${SWIG_USE_FILE})

set_source_files_properties("${CLIENT_PATH}/include/api/KinematicsClient.i" PROPERTIES CPLUSPLUS ON)

swig_add_library(pyUnityRenderer 
    LANGUAGE python
    OUTPUT_DIR "${BUILD_CLIENT_PY_API_DIR}"
    OUTFILE_DIR "${CMAKE_BINARY_DIR}/_pytemp"
    SOURCES "${CLIENT_PATH}/include/api/KinematicsClient.i"
)

add_dependencies(pyUnityRenderer client)

swig_link_libraries(pyUnityRenderer client ${LIBS} ${SYS_LIBS})

if(WIN32)
string(REPLACE / \\ CMAKE_BINARY_DIR_RV ${CMAKE_BINARY_DIR})
string(REPLACE / \\ BUILD_CLIENT_PY_API_DIR_RV ${BUILD_CLIENT_PY_API_DIR})
add_custom_target(copy_client_dll ALL
    COMMAND xcopy /Y ${CMAKE_BINARY_DIR_RV}\\_pyUnityRenderer.pyd ${BUILD_CLIENT_PY_API_DIR_RV}
)
else()
add_custom_target(copy_client_dll ALL
    COMMAND cp -f ${CMAKE_BINARY_DIR}/_pyUnityRenderer.so ${BUILD_CLIENT_PY_API_DIR}
)
endif()

add_dependencies(copy_client_dll pyUnityRenderer)

endif()
