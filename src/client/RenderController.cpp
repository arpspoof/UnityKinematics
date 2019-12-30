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
static double timeAccumulator = 20000; // INF

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
    AbstractCommandHandler* commandHandler
)
{
    ::maxPhysicalFPS = maxPhysicalFPS;
    ::dataProvider = dataProvider;
    ::commandHandler = commandHandler;

    physicalFPS = min(60, maxPhysicalFPS);

    RPCStartClient(serverAddr, serverPort, localAddr, commandHandlingPort);
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
        if (key == "Space") {
            Pause(!paused);
        }
        else if (key == "LeftArrow") {
            SetPhysicalFPS(max(physicalFPS - cmd.pi[0], 1));
        }
        else if (key == "RightArrow") {
            SetPhysicalFPS(min(physicalFPS + cmd.pi[0], maxPhysicalFPS));
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

static void Sleep(int64_t ms)
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

void Tick(float physicalTimeStep)
{
    PollCommand();

    if (paused) {
        SleepPause();
    }

    timeAccumulator += physicalTimeStep;
    if (timeAccumulator >= 1.0 / physicalFPS) {
        timeAccumulator = 0;
        FrameState frameState = dataProvider->GetCurrentState();
        int bufferedFrames = CreateFrame(frameState);
        if (bufferedFrames > 10) {
            int sleepDuration = bufferedFrames * 15;
            Sleep(sleepDuration);
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
