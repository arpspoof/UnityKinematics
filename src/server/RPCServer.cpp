#include "RPCServer.hpp"

#include <rpc/client.h>
#include <rpc/server.h>
#include <string>

using namespace std;

static rpc::server* rpc_server = nullptr;
static rpc::client* rpc_client = nullptr;

static ActorBuffer<FrameState>* frame_buffer = nullptr;
static ActorBuffer<Command>* cmd_buffer = nullptr;

static void RPCStartCommandSender(const std::string& addr, unsigned short port)
{
    printf("starting command sender ...\n");
    rpc_client = new rpc::client(addr, port);
}

static void RPCStopCommandSender()
{
    printf("stopping command sender ...\n");
    delete rpc_client;
    rpc_client = nullptr;
}

static int createFrame(FrameState frameState)
{
    frame_buffer->Write(frameState);
    return frame_buffer->GetNumOfAvailableElements();
}

static int command(Command cmd)
{
    if (cmd.name == "_sys_connect") {
        string clientAddr = cmd.ps[0];
        unsigned short clientPort = (unsigned short)cmd.pi[0];
        RPCStartCommandSender(clientAddr, clientPort);
        printf("start command sender, client is %s:%d\n", clientAddr.c_str(), clientPort);
        return 0;
    }

    cmd_buffer->Write(cmd);
    return cmd_buffer->GetNumOfAvailableElements();
}

void RPCStartServer(unsigned short port)
{
    printf("starting rpc server ...\n");
    frame_buffer = new ActorBuffer<FrameState>();
    cmd_buffer = new ActorBuffer<Command>();
    rpc_server = new rpc::server(port);
    rpc_server->bind("createFrame", &createFrame);
    rpc_server->bind("command", &command);
    rpc_server->async_run();
}

void RPCStopServer()
{
    RPCStopCommandSender();

    printf("stopping rpc server ...\n");
    rpc_server->stop();
    delete rpc_server;
    delete frame_buffer;
    delete cmd_buffer;
    rpc_server = nullptr;
    frame_buffer = nullptr;
    cmd_buffer = nullptr;
}

ActorBuffer<FrameState>* RPCGetFrameBuffer()
{
    return frame_buffer;
}

ActorBuffer<Command>* RPCGetCommandBuffer()
{
    return cmd_buffer;
}

int SendCommand(Command cmd)
{
    printf("sendign mcd ...\n");
    return rpc_client->call("command", cmd).as<int>();
}
