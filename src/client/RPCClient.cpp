#include "RPCClient.hpp"
#include "DataFormat.hpp"

#include <rpc/client.h>

using namespace std;

static rpc::client* rpc_client = nullptr;

void RPCStartClient(const std::string& addr, unsigned short port)
{
    printf("starting rpc client ...\n");
    rpc_client = new rpc::client(addr, port);
}

void RPCStopClient()
{
    printf("stopping rpc client ...\n");
    delete rpc_client;
    rpc_client = nullptr;
}

int CreateFrame(FrameState frame)
{
    return rpc_client->call("createFrame", frame).as<int>();
}

int SendCommand(Command cmd)
{
    return rpc_client->call("command", cmd).as<int>();
}
