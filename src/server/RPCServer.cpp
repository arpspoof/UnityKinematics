#include "RPCServer.hpp"
#include "DataFormat.hpp"

#include <rpc/server.h>

using namespace std;

static rpc::server* rpc_server = nullptr;

static int createFrame(FrameState frameState)
{
    printf("receive frame, nObj = %ld:\n", frameState.objectStates.size());
    printf("session name = %s\n", frameState.sessionName.c_str());
    for (auto& obj : frameState.objectStates) {
        printf("obj: name = %s, p = %f,%f,%f, q = %f,%f,%f,%f\n", obj.objectName.c_str(), 
            obj.transform.p.x, obj.transform.p.y, obj.transform.p.z,
            obj.transform.q.w, obj.transform.q.x, obj.transform.q.y, obj.transform.q.z);
    }
    return 0;
}

void rpc_start(unsigned short port)
{
    rpc_server = new rpc::server(port);
    rpc_server->bind("createFrame", &createFrame);
    rpc_server->run();
}

void rpc_stop()
{

}
