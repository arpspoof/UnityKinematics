#include "rpc/client.h"
#include "DataFormat.hpp"

using namespace std;

int main() {
    rpc::client c("localhost", 8080);

    ObjectState obj1("asd", Transform(1, 2, 3, 4, 5, 6, 7));
    ObjectState obj2("zxc", Transform(11, 12, 13, 14, 15, 16, 17));

    FrameState frame("test");
    frame.objectStates.push_back(obj1);
    frame.objectStates.push_back(obj2);

    int result = c.call("createFrame", frame).as<int>();
    printf("result = %d\n", result);

    return 0;
}
