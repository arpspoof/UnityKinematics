#include "RPCServer.hpp"

using namespace std;

int main() 
{
    RPCStart(8080);
    while (getchar() != 'q') {;}
    RPCStop();

    return 0;
}
