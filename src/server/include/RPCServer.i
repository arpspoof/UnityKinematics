%{
    #include "RPCServer.hpp"
%}

void RPCStart(unsigned short port);
void RPCStop();

%template(FrameBuffer) ActorBuffer<FrameState>;
%template(CommandBuffer) ActorBuffer<Command>;

ActorBuffer<FrameState>* RPCGetFrameBuffer();
ActorBuffer<Command>* RPCGetCommandBuffer();
