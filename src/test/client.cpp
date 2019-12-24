#include "api/KinematicsClient.hpp"

using namespace std;

static float x = 0;

class DataProvider : public AbstractDataProvider
{
    virtual FrameState GetCurrentState() const override
    {
     /*   ObjectState obj1("asd", x, 2, 3, 4, 5, 6, 7);
        ObjectState obj2("zxc", x+1, 12, 13, 14, 15, 16, 17);
        x+=2;

        FrameState frame("test");
        frame.objectStates.push_back(obj1);
        frame.objectStates.push_back(obj2);

        return frame;*/

        ObjectState obj1("s1", x, x, x, 1, 0, 0, 0);
        ObjectState obj2("c1", -x, -x, -x, 1, 0, 0, 0);
        ObjectState obj3("b1", 0, 0, 0, 1, 0, 0, 0);
        FrameState frame("test");
        frame.objectStates.push_back(obj1);
        frame.objectStates.push_back(obj2);
        frame.objectStates.push_back(obj3);
        x += 0.01f;

        return frame;
    }   
};

int main() {
    RPCStartClient("localhost", 8080);

    DataProvider d;
    d.frequency = 100;

    Command cmd1;
    cmd1.name = "_sys_create_primitive";
    cmd1.pf.push_back(1);
    cmd1.ps.push_back("sphere");
    cmd1.ps.push_back("s1");
    SendCommand(cmd1);

    Command cmd2;
    cmd2.name = "_sys_create_primitive";
    cmd2.pf.push_back(1);
    cmd2.pf.push_back(2);
    cmd2.ps.push_back("capsule");
    cmd2.ps.push_back("c1");
    SendCommand(cmd2);

    Command cmd3;
    cmd3.name = "_sys_create_primitive";
    cmd3.pf.push_back(1);
    cmd3.pf.push_back(2);
    cmd3.pf.push_back(3);
    cmd3.ps.push_back("box");
    cmd3.ps.push_back("b1");
    SendCommand(cmd3);


    while(1)
    {
        d.Tick(100);
    }

    return 0;
}
