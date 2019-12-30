%module(directors="1") pyUnityRenderer

%{
    #include "api/KinematicsClient.hpp"
%}

%include "DataFormat.i"
%include "Command.i"
%include "ActorBuffer.i"
%include "AbstractCommandHandler.i"
%include "AbstractDataProvider.i"
%include "RenderController.i"
