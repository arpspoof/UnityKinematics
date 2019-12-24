%module(directors="1") UnityServerAPI

%{
    #include "api/KinematicsServer.hpp"
%}

%include "DataFormat.i"
%include "Command.i"
%include "ActorBuffer.i"
%include "RPCServer.i"
