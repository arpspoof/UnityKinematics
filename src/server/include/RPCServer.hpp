#pragma once

#include "ActorBuffer.hpp"
#include "DataFormat.hpp"
#include "Command.hpp"

void RPCStartServer(unsigned short port, int commandTimeout = 500);
void RPCStopServer();

ActorBuffer<FrameState>* RPCGetFrameBuffer();
ActorBuffer<Command>* RPCGetCommandBuffer();

int SendCommand(Command cmd);
