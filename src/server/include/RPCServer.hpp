#pragma once

#include "ActorBuffer.hpp"
#include "DataFormat.hpp"
#include "Command.hpp"

void RPCStart(unsigned short port);
void RPCStop();

ActorBuffer<FrameState>* RPCGetFrameBuffer();
ActorBuffer<Command>* RPCGetCommandBuffer();
