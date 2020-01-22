### Full API List
#### Structures
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
```c++
struct FrameState
{
    std::vector<GroupState> groups;
};
```
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
```c++
class AbstractDataProvider
{
public:
    virtual FrameState GetCurrentState() const = 0;
    virtual ~AbstractDataProvider() {}
};
```
```c++
class AbstractCommandHandler
{
public:
    virtual void HandleCommand(const Command& cmd) const = 0;
    virtual ~AbstractCommandHandler() {}
};
```
#### Methods
```c++
void InitRenderController(
    const std::string& serverAddr, 
    unsigned short serverPort,
    const std::string& localAddr, 
    unsigned short commandHandlingPort, 
    int maxPhysicalFPS, 
    AbstractDataProvider* dataProvider, 
    AbstractCommandHandler* commandHandler
);
```
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
```c++
void Tick(float physicalTimeStep);
```
```c++
void SendCustomCommand(const Command& cmd);
```
```c++
// You must dispose resources before exiting the client
void DisposeRenderController();
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
