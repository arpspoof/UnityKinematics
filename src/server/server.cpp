#include "rpc/server.h"
#include "DataFormat.hpp"

using namespace std;

int createFrame(FrameState frameState)
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

int main() 
{
    rpc::server srv(8080);

    srv.bind("createFrame", &createFrame);

    srv.run();

    return 0;
}
