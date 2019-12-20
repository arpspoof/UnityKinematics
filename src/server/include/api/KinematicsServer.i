%module(directors="1") UnityServerAPI

%{
    #include "api/KinematicsServer.hpp"
%}

%include "DataFormat.i"
%include "FrameBuffer.i"
%include "RPCServer.i"
