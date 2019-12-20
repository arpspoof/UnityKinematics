#pragma once

#include <string>
#include <rpc/client.h>

void RPCStartClient(const std::string& addr, unsigned short port);
void RPCStopClient();

rpc::client* RPCGetClient();
