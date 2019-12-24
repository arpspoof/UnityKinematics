#include "RPCServer.hpp"
#include "DataFormat.hpp"
#include "Command.hpp"

#include <rpc/server.h>

using namespace std;

static rpc::server* rpc_server = nullptr;
static FrameBuffer* rpc_buffer = nullptr;

static int createFrame(FrameState frameState)
{
    rpc_buffer->Write(frameState);
    return rpc_buffer->GetNumOfAvailableElements();
}

static int command(Command cmd)
{
    auto handler = GetCommandHandler();
    if (handler) {
        handler->HandleCommand(cmd);
    }
    return 0;
}

void RPCStart(unsigned short port)
{
    printf("starting rpc server ...\n");
    rpc_buffer = new FrameBuffer();
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
    delete rpc_buffer;
    rpc_server = nullptr;
    rpc_buffer = nullptr;
}

FrameBuffer* RPCGetBuffer()
{
    return rpc_buffer;
}
