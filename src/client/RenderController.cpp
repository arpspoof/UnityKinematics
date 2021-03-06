#include "RenderController.hpp"
#include "RPCClient.hpp"

#include <vector>
#include <thread>
#include <chrono>

using namespace std;
using namespace std::chrono;

static bool paused = false;
static int physicalFPS = 60;
static int maxPhysicalFPS = 60;
static double timeAccumulator = 0;
static double wallClockTimeAccumulator = 0;

static AbstractDataProvider* dataProvider;
static AbstractCommandHandler* commandHandler;

static void SendFPSData()
{
    Command cmd("_sys_physical_fps");
    cmd.pi.push_back(physicalFPS);
    cmd.pi.push_back(maxPhysicalFPS);
    cmd.pi.push_back(paused);
    SendCommand(cmd);
}

void InitRenderController(
    const std::string& serverAddr, 
    unsigned short serverPort,
    const std::string& localAddr, 
    unsigned short commandHandlingPort, 
    int maxPhysicalFPS, 
    AbstractDataProvider* dataProvider, 
    AbstractCommandHandler* commandHandler, 
    int rpcTimeout
)
{
    ::maxPhysicalFPS = maxPhysicalFPS;
    ::dataProvider = dataProvider;
    ::commandHandler = commandHandler;

    physicalFPS = min(60, maxPhysicalFPS);

    RPCStartClient(serverAddr, serverPort, localAddr, commandHandlingPort, rpcTimeout);
    SendFPSData();
}

void DisposeRenderController()
{
    RPCStopClient();
}

void Pause(bool setPauseTo)
{
    paused = setPauseTo;
    SendFPSData();
}

void SetPhysicalFPS(int fps)
{
    physicalFPS = fps;
    SendFPSData();
}

static void HandleSystemCommand(const Command& cmd)
{
    const string& name = cmd.name;
    if (name == "_sys_key") {
        const string& key = cmd.ps[0];
        if (key == "Pause") {
            Pause(!paused);
        }
        else if (key == "Physical FPS -") {
            int delta = cmd.pi[0];
            if (delta > 1 && physicalFPS >= 120) delta = 60;
            SetPhysicalFPS(max(physicalFPS - delta, 1));
        }
        else if (key == "Physical FPS +") {
            int delta = cmd.pi[0];
            if (delta > 1 && physicalFPS >= 60) delta = 60;
            SetPhysicalFPS(min(physicalFPS + delta, maxPhysicalFPS));
        }
    }
}

static void PollCommand()
{
    auto cmdBuffer = RPCGetCommandBuffer();
    int n = cmdBuffer->GetNumOfAvailableElements();
    for (int i = 0; i < n; i++) {
        Command cmd = cmdBuffer->ReadAndErase(0);
        if (cmd.name.length() > 5 && cmd.name.substr(0, 5) == "_sys_") HandleSystemCommand(cmd);
        else commandHandler->HandleCommand(cmd);
    }
}

static void SleepFor(int64_t ms)
{
    while (ms >= 10) {
        PollCommand();
        this_thread::sleep_for(milliseconds(10));
        ms -= 10;
    }
}

static void SleepPause()
{
    while (paused) {
        PollCommand();
        this_thread::sleep_for(milliseconds(10));
    }
}

void Tick(float physicalTimeStep, float wallClockTime)
{
    PollCommand();

    if (paused) {
        SleepPause();
    }

    timeAccumulator += physicalTimeStep;
    wallClockTimeAccumulator += wallClockTime;

    if (timeAccumulator >= 1.0 / physicalFPS) {
        FrameState frameState = dataProvider->GetCurrentState();
        frameState.duration = wallClockTimeAccumulator;

        timeAccumulator -= 1.0 / physicalFPS;
        wallClockTimeAccumulator = 0;

        int bufferedFrames = CreateFrame(frameState);
        if (bufferedFrames > 10) {
            int sleepDuration = bufferedFrames * 15;
            SleepFor(sleepDuration);
        }
    }
}

void SendCustomCommand(const Command& cmd)
{
    SendCommand(cmd);
}

void CreatePrimitive(
    const std::string& type, 
    const std::string& groupName,
    const std::string& objectName,
    float param0,
    float param1,
    float param2
)
{
    Command cmd("_sys_create_primitive");
    cmd.ps.push_back(type);
    cmd.ps.push_back(groupName);
    cmd.ps.push_back(objectName);
    cmd.pf.push_back(param0);
    cmd.pf.push_back(param1);
    cmd.pf.push_back(param2);
    SendCommand(cmd);
}
