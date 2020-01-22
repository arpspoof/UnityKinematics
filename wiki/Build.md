# Build Instructions

## Linux
### Prerequisite
+ CMake >= 3.15
+ Swig >= 4.0.1
+ rpclib
  + Must build from source
  + Clone from https://github.com/rpclib/rpclib
  + Add ```set(CMAKE_POSITION_INDEPENDENT_CODE ON)``` to the 3rd line of CMakeLists.txt
  + Then follow instructions on http://rpclib.net/compiling/ to build and install
+ mono (Please install from your linux distribution and check if command ```mcs``` works)
+ PythonLibs >= 3.7.4, PythonInterp (For client side Python binding only)

### Build
+ ```git clone git@github.com:arpspoof/UnityKinematics.git```
+ ```mkdir build```
+ ```cd build```
+ ```cmake [Options] ..``` or ```cmake-gui``` (highly recommended), please configure these options:
  + ```CMAKE_BUILD_TYPE``` 
    + Default is ```Release```
  + ```CMAKE_INSTALL_PREFIX``` 
    + Default is ```[repo]/install``` 
    + This is the target folder for ```make install```. You only need to run ```make install``` in case you would like to use a C++ client, see sections below for details.
  + ```RPCLIB_INCLUDE_DIR``` and ```RPCLIB_LIBS_DIR```
    + Defaults are ```/usr/local/include``` and ```/usr/local/lib```. This is the default install path when building rpclib from source.
  + ```BUILD_UNITY_ASSETS```
    + Default is ```ON```
    + Must enable this to build unity rendering support
  + ```BUILD_UNITY_ASSETS_DIR```
    + Default is ```[build]/Assets```
    + Available only when ```BUILD_UNITY_ASSETS``` is ```ON```
    + This is the directory to build assets needed for unity rendering server.
  + ```BUILD_CLIENT_PY_API```
    + Default is ```ON```
    + Enable this to build python client side data provider
  + ```BUILD_CLIENT_PY_API_DIR```
    + Default is ```[build]/pyUnityRenderer```
    + This is the directory to build python client side API
+ ```make```
+ ```make install``` (Only needed for C++ client side)

## Windows
### Prerequisite
+ Visual Studio 2019 >= 16.4.2
+ Swig >= 4.0.1
  + Download Windows binaries from http://www.swig.org/download.html
  + Extract the zip file into a local folder, no further actions needed. Use this path for later CMake configuration.
+ rpclib [from vcpkg]
  + Install vcpkg
    + ```git clone git@github.com:microsoft/vcpkg.git```
    + ```cd vcpkg```
    + ```./bootstrap-vcpkg.bat```
  + Install rpclib
    + ```cd vcpkg```
    + ```.\vcpkg.exe install rpclib --triplet x64-windows```
    + rpclib will be installed to ```[vcpkg]\packages\rpclib_x64-windows```. Use this path for later CMake configuration. 
+ Python >= 3.7 (Make sure the 64-bit version is installed)
  + Python path is usually ```C:/Users/xx/AppData/Local/Programs/Python/Python37```. Use this path for later CMake configuration.

### Build
+ ```git clone git@github.com:arpspoof/UnityKinematics.git```
+ Open Visual Studio and choose **Open Folder** to open this directory
+ Visual Studio will automatically start to configure CMake. Please go through the following steps:
  + Open CMake Settings Editor
  + At the very above, change Configuration name to ```x64-Release``` and Configuration type to ```Release```
  + Open CMake Settings Editor (Available from 'CMake Overview welcome page'), configure the following CMake variables
    + Change ```SWIG_DIR``` to ```[swig]/Lib```
    + Change ```SWIG_EXECUTABLE``` to ```[swig]/swig.exe```
    + Change ```SWIG_VERSION``` to the correct one, like ```4.0.1```
    + Change ```RPCLIB_INCLUDE_DIR``` to ```[rpclib]/include```
    + Change ```RPCLIB_LIBS_DIR``` to ```[rpclib]/lib```
    + Change ```PYTHON_INCLUDE_PATH``` to ```[python]/include```
    + Change ```PYTHON_LIB``` to ```[python]/libs``` (**NOT Lib**)
    + Change ```CSC_PATH``` to the path containing ```csc``` C# compiler. This is usually ```C:/Windows/Microsoft.NET/Framework64/v4.0.30319```
  + save CMakeSettings.json and VS will reconfigure the project
  + Right click on CMakeLists.txt, choose 'Build'
  + If C++ client is needed, also run 'Install'
+ Notes:
  + Make sure no other application is opening anything inside the ```out``` folder.
  + If build fails, try a clean build.
