#pragma once

#include "Command.hpp"
#include "DataFormat.hpp"

#include <rpc/client.h>

void RPCStartClient(const std::string& addr, unsigned short port);
void RPCStopClient();

int CreateFrame(FrameState frame);
int SendCommand(Command cmd);
