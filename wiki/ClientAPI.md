### Client API List
#### Structures
[Explanations for ```ObjectState```](TutorialBeginner.md#Describing-state-of-an-object)
```c++
struct ObjectState
{
    std::string objectName;
    float x, y, z;
    float qw, qx, qy, qz;
    ObjectState() {}
    ObjectState(std::string objectName, float x, float y, float z, float qw, float qx, float qy, float qz) 
        :objectName(objectName), x(x), y(y), z(z), qw(qw), qx(qx), qy(qy), qz(qz) {}
    ObjectState(std::string objectName) 
        :objectName(objectName), x(0), y(0), z(0), qw(1), qx(0), qy(0), qz(0) {}
};
```
[Explanations for ```GroupState```](TutorialBeginner.md#Groups-and-state-of-a-group)
```c++
struct GroupState
{
    std::string groupName;
    std::vector<ObjectState> objectStates;
    GroupState() {}
    GroupState(std::string groupName) :groupName(groupName) {}
    GroupState(std::string groupName, int nObj) :groupName(groupName), objectStates(nObj) {}
};
```
[Explanations for ```FrameState```](TutorialBeginner.md#Frames-and-state-of-a-frame)
```c++
struct FrameState
{
    std::vector<GroupState> groups;
};
```
[Explanations for ```Command```](CommandSystem.md)
```c++
struct Command
{
    std::string name;
    std::vector<int> pi;
    std::vector<float> pf;
    std::vector<std::string> ps;
    Command() {}
    Command(std::string name) :name(name) {}
};
```
#### Abstract classes
[Explanations for ```AbstractDataProvider```](TutorialBeginner.md#Establish-a-data-provider)
```c++
class AbstractDataProvider
{
public:
    virtual FrameState GetCurrentState() const = 0;
    virtual ~AbstractDataProvider() {}
};
```
[Explanations for ```AbstractCommandHandler```](CommandSystem.md)
```c++
class AbstractCommandHandler
{
public:
    virtual void HandleCommand(const Command& cmd) const = 0;
    virtual ~AbstractCommandHandler() {}
};
```
#### Methods
[Explanations for ```InitRenderController```](TutorialBeginner.md#initialize-the-client)
```c++
void InitRenderController(
    const std::string& serverAddr, 
    unsigned short serverPort,
    const std::string& localAddr, 
    unsigned short commandHandlingPort, 
    int maxPhysicalFPS, 
    AbstractDataProvider* dataProvider, 
    AbstractCommandHandler* commandHandler, 
    int rpcTimeout = 2000
);
```
[Explanations for ```CreatePrimitive```](TutorialBeginner.md#Create-primitive-objects-from-the-client)
```c++
void CreatePrimitive(
    const std::string& type, 
    const std::string& groupName,
    const std::string& objectName,
    float param0,
    float param1,
    float param2
);
```
Explanations for ```Tick``` [part 1](TutorialBeginner.md#Establish-a-data-provider), [part 2](TutorialBeginner.md#Driving-the-time-forward)
```c++
void Tick(float physicalTimeStep);
```
[Explanations for ```SendCustomCommand```](CommandSystem.md#Sending-a-command)
```c++
void SendCustomCommand(const Command& cmd);
```
```c++
// Call this if you would like to pause the client and server in your client code
void Pause(bool setPauseTo = true);
```
```c++
// Physical FPS determines how many physical frames in one physical second should be rendered.
// This is usually controlled automatically.
// Call this if you would like to take over the control on client side.
void SetPhysicalFPS(int fps);
```
```c++
// You must dispose resources before exiting the client
void DisposeRenderController();
```
