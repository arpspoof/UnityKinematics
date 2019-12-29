#include "RPCClient.hpp"
#include "DataFormat.hpp"

#include <rpc/client.h>
#include <rpc/server.h>

using namespace std;

static rpc::client* rpc_client = nullptr;
static rpc::server* rpc_server = nullptr;

static ActorBuffer<Command>* cmd_buffer = nullptr;

static int command(Command cmd)
{
    cmd_buffer->Write(cmd);
    return cmd_buffer->GetNumOfAvailableElements();
}

static void RPCStartCommandListening(unsigned short port)
{
    printf("starting command listener ...\n");
    cmd_buffer = new ActorBuffer<Command>();
    rpc_server = new rpc::server(port);
    rpc_server->bind("command", &command);
    rpc_server->async_run();
}

static void RPCStopCommandListening()
{
    printf("stopping command listener ...\n");
    rpc_server->stop();
    delete rpc_server;
    delete cmd_buffer;
    rpc_server = nullptr;
    cmd_buffer = nullptr;
}

int CreateFrame(FrameState frame)
{
    return rpc_client->call("createFrame", frame).as<int>();
}

int SendCommand(Command cmd)
{
    return rpc_client->call("command", cmd).as<int>();
}

void RPCStartClient(const std::string& serverAddr, unsigned short serverPort,
    const std::string& localAddr, unsigned short commandHandlingPort)
{
    RPCStartCommandListening(commandHandlingPort);

    printf("starting rpc client ...\n");
    rpc_client = new rpc::client(serverAddr, serverPort);

    Command cmd("_sys_connect");
    cmd.ps.push_back(localAddr);
    cmd.pi.push_back(commandHandlingPort);

    SendCommand(cmd);
}

void RPCStopClient()
{
    printf("stopping rpc client ...\n");
    delete rpc_client;
    rpc_client = nullptr;

    RPCStopCommandListening();
}

ActorBuffer<Command>* RPCGetCommandBuffer()
{
    return cmd_buffer;
}
