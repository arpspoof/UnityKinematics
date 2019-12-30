#pragma once

#include "Command.hpp"

class AbstractCommandHandler
{
public:
    virtual void HandleCommand(const Command& cmd) const = 0;
};
