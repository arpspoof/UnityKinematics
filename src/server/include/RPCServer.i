%{
    #include "RPCServer.hpp"
%}

void RPCStart(unsigned short port);
void RPCStop();

FrameBuffer* RPCGetBuffer();
