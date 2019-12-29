#include "AbstractDataProvider.hpp"
#include "RPCClient.hpp"

#include <thread>
#include <cmath>

using namespace std;
using namespace std::chrono;

AbstractDataProvider::AbstractDataProvider(int physicalFPS) 
    :physicalFPS(physicalFPS), frameCounter(-1), skipRate(max(1, physicalFPS / 60)), paused(false)
{
}

AbstractDataProvider::~AbstractDataProvider()
{
}

void AbstractDataProvider::SetPhysicalFPS(int physicalFPS)
{
    this->physicalFPS = physicalFPS;
    skipRate = max(1, physicalFPS / 60);
}

void AbstractDataProvider::Tick()
{
    PollCommand();

    if (paused) {
        Pause();
    }

    frameCounter++;
    if (frameCounter % skipRate != 0) return;

    int bufferedFrames = CreateFrame(GetCurrentState());
    if (bufferedFrames > 10) {
        int sleepDuration = bufferedFrames * 15;
        Sleep(sleepDuration);
    }
}

void AbstractDataProvider::PollCommand()
{
    auto cmdBuffer = RPCGetCommandBuffer();
    int n = cmdBuffer->GetNumOfAvailableElements();
    for (int i = 0; i < n; i++) {
        Command cmd = cmdBuffer->ReadAndErase(0);
        if (cmd.name.length() > 5 && cmd.name.substr(0, 5) == "_sys_") HandleSystemCommand(cmd);
        else HandleCommand(cmd);
    }
}

void AbstractDataProvider::HandleSystemCommand(Command& cmd)
{
    string& name = cmd.name;
    if (name == "_sys_key") {
        string& key = cmd.ps[0];
        if (key == "Space") {
            paused = !paused;
        }
        else if (key == "LeftArrow") {
            if (skipRate > 1) {
                skipRate--;
            }
        }
        else if (key == "RightArrow") {
            skipRate++;
        }
    }
}

void AbstractDataProvider::Sleep(int64_t ms)
{
    while (ms >= 10) {
        PollCommand();
        this_thread::sleep_for(milliseconds(10));
        ms -= 10;
    }
}

void AbstractDataProvider::Pause()
{
    while (paused) {
        PollCommand();
        this_thread::sleep_for(milliseconds(10));
    }
}
