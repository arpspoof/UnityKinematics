#include "api/KinematicsServer.hpp"

#include <future>
#include <thread>
#include <chrono> 

using namespace std;

static void printFrame(FrameState frameState)
{
    printf("receive frame, nObj = %ld:\n", frameState.objectStates.size());
    printf("session name = %s\n", frameState.sessionName.c_str());
    for (auto& obj : frameState.objectStates) {
        printf("obj: p = %f\n", obj.x);
    }
}

static void check()
{
    auto buffer = RPCGetFrameBuffer();
    while(1)
    {
        int n = buffer->GetNumOfAvailableElements();
     /*   for (int i = 0; i < n; i++) {
            printf("frame (%d/%d)\n", i, n);
            printFrame(buffer->ReadAndErase(0));
        }*/
        if (n > 0) {
            printf("frame (%d/%d)\n", 0, 1);
            printFrame(buffer->ReadAndErase(0));
            Command cmd("argrdhg");
            SendCommand(cmd);
        }
        this_thread::sleep_for(chrono::milliseconds(500));
    }
}

int main() 
{
    RPCStartServer(8080);

    auto fut = async(check);
    fut.wait();

    while (getchar() != 'q') {;}
    RPCStopServer();

    return 0;
}
