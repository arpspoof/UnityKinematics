#pragma once

#include "Command.hpp"
#include "DataFormat.hpp"

#include <chrono>

class AbstractDataProvider
{
    int physicalFPS;
    int frameCounter;
    int skipRate;

    bool paused;

public:
    void SetPhysicalFPS(int physicalFPS);
    void Tick();

    AbstractDataProvider(int physicalFPS = 60);
    virtual ~AbstractDataProvider();

private:
    void PollCommand();
    void HandleSystemCommand(Command& cmd);

    void Sleep(int64_t ms);
    void Pause();

public: // Abstract
    virtual FrameState GetCurrentState() const = 0;
    virtual void HandleCommand(Command& cmd) const = 0;
};
