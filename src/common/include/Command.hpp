#pragma once

#include <rpc/msgpack.hpp>

#include <vector>
#include <string>

struct Command
{
    std::string name;
    std::vector<int> pi;
    std::vector<float> pf;
    std::vector<std::string> ps;
    Command() {}
    Command(std::string name) :name(name) {}
    MSGPACK_DEFINE_ARRAY(name, pi, pf, ps)
};

class AbstractCommandHandler
{
public:
    virtual ~AbstractCommandHandler() {}
    virtual int HandleCommand(Command cmd) = 0;
};

AbstractCommandHandler* GetCommandHandler();
void SetCommandHandler(AbstractCommandHandler* handler);
