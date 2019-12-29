#pragma once

#include "Command.hpp"
#include "DataFormat.hpp"
#include "Command.hpp"
#include "ActorBuffer.hpp"

void RPCStartClient(const std::string& serverAddr, unsigned short serverPort,
    const std::string& localAddr, unsigned short commandHandlingPort);
void RPCStopClient();

int CreateFrame(FrameState frame);
int SendCommand(Command cmd);

// non-API
ActorBuffer<Command>* RPCGetCommandBuffer();
