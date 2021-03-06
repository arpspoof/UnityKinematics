#pragma once

#include "Command.hpp"
#include "DataFormat.hpp"
#include "Command.hpp"
#include "ActorBuffer.hpp"

#include <vector>

void RPCStartClient(const std::string& serverAddr, unsigned short serverPort,
    const std::string& localAddr, unsigned short commandHandlingPort, int rpcTimeout);
void RPCStopClient();

int CreateFrame(FrameState frame);
int SendCommand(const Command& cmd);

// non-API
ActorBuffer<Command>* RPCGetCommandBuffer();
