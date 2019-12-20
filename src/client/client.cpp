#include "rpc/client.h"
#include "DataFormat.hpp"

#include <thread>
#include <future>
#include <chrono>

using namespace std;

static rpc::client* c;
static int x = 0;

static void keepsend()
{
    while(1)
    {
        ObjectState obj1("asd", Transform(x, 2, 3, 4, 5, 6, 7));
        ObjectState obj2("zxc", Transform(x+1, 12, 13, 14, 15, 16, 17));
        x+=2;

        FrameState frame("test");
        frame.objectStates.push_back(obj1);
        frame.objectStates.push_back(obj2);

        c->call("createFrame", frame).as<int>();
        this_thread::sleep_for(chrono::milliseconds(800));
    }
}

int main() {
    c = new rpc::client("localhost", 8080);

    auto fut = async(keepsend);
    fut.wait();

    return 0;
}
