#include "api/KinematicsClient.hpp"

using namespace std;

static int x = 0;

class DataProvider : public AbstractDataProvider
{
    virtual FrameState GetCurrentState() const override
    {
        ObjectState obj1("asd", Transform(x, 2, 3, 4, 5, 6, 7));
        ObjectState obj2("zxc", Transform(x+1, 12, 13, 14, 15, 16, 17));
        x+=2;

        FrameState frame("test");
        frame.objectStates.push_back(obj1);
        frame.objectStates.push_back(obj2);

        return frame;
    }
};

int main() {
    RPCStartClient("localhost", 8080);

    DataProvider d;
    d.frequency = 10000;
    while(1)
    {
        d.Tick(1000);
        x++;
    }

    return 0;
}
