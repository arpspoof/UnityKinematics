## Command System

You may consider extending the client / server system if you want to ..
+ Add customized interaction, like key presses, mouse movements. Note: If the interactions have nothing to do with the client side, implement it just on server side with native Unity API support. 
+ Add customized remote procedure calls. You might want to send something special from client to server or from server to client. 

If you are in these cases, checkout the below tutorial for **Command System**. Before doing so, make sure your server and client are reachable from each other. I.e., you must provide correct server address and local address as well as ports to enable these functionalities. 

### What is a command
A command is a special type of message sending from client to server or from server to client so that the receiver can react correspondingly. For example, we use commands for the following purposes:
+ Create Primitives: When a client call ```CreatePrimitive``` function, we send a special command to server which contains all the parameters needed so that server can create the object with correct size and shape.
+ FPS Control: When the user press ```Space``` key to pause on server side, the command is sent from server to client so that the client will fall into sleep until further notice.

The structure of a command is simple:
```c++
struct Command
{
    std::string name;
    std::vector<int> pi;
    std::vector<float> pf;
    std::vector<std::string> ps;
};
```
Here ```name``` is just the name of the command. ```pi```, ```pf```, ```ps``` correspond to three types of parameters, integers, floats and strings, infinite amount for each of these. Therefore, user can define custom function prototype by using these parameter lists.

### Sending a command
To send a command, you just need to construct a command object and call ```SendCustomCommand``` on client side or ```CommandSender.Send``` on server side. You're free to choose what kind of parameters you use. However, pay slightly attention to the command name. Don't use command name with prefix **"\_sys\_"** because that's the prefix I'm using internally. Also note that be careful when you are sending custom coordinates from / to either side. [Coordinate transform](CoordinateTransform.md) may be required.

### Handling an incoming command
Commands will be delivered automatically, you just need to worry about how to handle them. On client side, we just need to implement ```HandleCommand``` function in ```AbstractCommandHandler```. Recall that in our first client side tutorial [here](TutorialBeginner.md#Establish-a-command-handler), we already establish a dummy command handler. 
```python
class CommandHandler(AbstractCommandHandler):
    def HandleCommand(self, cmd):
        pass

commandHandler = CommandHandler()
```
What you need to do is just substitute ```pass``` with your own logic. For thread safety, this function will always be called from the main thread so don't worry about synchronizing stuff. Make sure you are constantly calling ```Tick``` function for this to work properly. 

To handle a custom command on server side, we will use C#'s event system. ```KinematicsServerEvents.OnCommand``` is a C# event which will be raised whenever a **custom** command is available. To use this, first prepare a command handler function which takes a ```Command``` object as the single argument:
```C#
public void OnCommand(Command cmd)
{
    // Your own logic
}
```
Next, in the initialization phase, register this event handler by calling
```C#
KinematicsServerEvents.OnCommand += OnCommand;
```
A good place for initialization work in Unity is ```Start``` function of a component. Or sometimes you may need ```Awake``` instead. Checkout basic Unity tutorials for these concepts. 
