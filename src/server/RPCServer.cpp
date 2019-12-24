#include "RPCServer.hpp"
#include <rpc/server.h>

using namespace std;

static rpc::server* rpc_server = nullptr;
static ActorBuffer<FrameState>* frame_buffer = nullptr;
static ActorBuffer<Command>* cmd_buffer = nullptr;

static int createFrame(FrameState frameState)
{
    frame_buffer->Write(frameState);
    return frame_buffer->GetNumOfAvailableElements();
}

static int command(Command cmd)
{
    cmd_buffer->Write(cmd);
    return cmd_buffer->GetNumOfAvailableElements();
}

void RPCStart(unsigned short port)
{
    printf("starting rpc server ...\n");
    frame_buffer = new ActorBuffer<FrameState>();
    cmd_buffer = new ActorBuffer<Command>();
    rpc_server = new rpc::server(port);
    rpc_server->bind("createFrame", &createFrame);
    rpc_server->bind("command", &command);
    rpc_server->async_run();
}

void RPCStop()
{
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
