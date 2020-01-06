%{
    #include "RPCServer.hpp"
%}

void RPCStartServer(unsigned short port);
void RPCStopServer();

%template(RPCFrameBuffer) ActorBuffer<FrameState>;
%template(RPCCommandBuffer) ActorBuffer<Command>;

ActorBuffer<FrameState>* RPCGetFrameBuffer();
ActorBuffer<Command>* RPCGetCommandBuffer();

int SendCommand(Command cmd);
