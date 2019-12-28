%{
    #include "RPCServer.hpp"
%}

void RPCStartServer(unsigned short port);
void RPCStopServer();

%template(FrameBuffer) ActorBuffer<FrameState>;
%template(CommandBuffer) ActorBuffer<Command>;

ActorBuffer<FrameState>* RPCGetFrameBuffer();
ActorBuffer<Command>* RPCGetCommandBuffer();

int SendCommand(Command cmd);
