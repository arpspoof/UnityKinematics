#include <iostream>

#include "rpc/client.h"

int main() {
    rpc::client c("localhost", 8080);

    std::cout << "add(2, 3) = ";
    double five = c.call("add", 2, 3).as<double>();
    std::cout << five << std::endl;

    std::cout << "sub(3, 2) = ";
    double one = c.call("sub", 3, 2).as<double>();
    std::cout << one << std::endl;

    std::cout << "mul(5, 0) = ";
    double zero = c.call("mul", five, 0).as<double>();
    std::cout << zero << std::endl;

    std::cout << "div(3, 0) = ";
    double hmm = c.call("div", 3, 0).as<double>();
    std::cout << hmm << std::endl;

    return 0;
}
