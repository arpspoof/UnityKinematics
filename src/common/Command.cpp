#include "Command.hpp"

AbstractCommandHandler* commandHandler = nullptr;

AbstractCommandHandler* GetCommandHandler()
{
    return commandHandler;
}

void SetCommandHandler(AbstractCommandHandler* handler)
{
    commandHandler = handler;
}
