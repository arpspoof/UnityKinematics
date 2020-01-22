### Extensions Overview
You may consider extending the client / server system if you want to ..
+ Add customized interaction, like key presses, mouse movements. Note: If the interactions have nothing to do with the client side, implement it just on server side with native Unity API support. 
+ Add customized remote procedure calls. You might want to send something special from client to server or from server to client. 

If you are in these cases, checkout the below **Command System**. Before doing so, make sure your server and client are reachable from each other. I.e., you must provide correct server address and local address as well as ports to enable these functionalities. 

### Command System
#### What is a command
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

#### Sending a command
To send a command, you just need to construct a command object and call ```SendCustomCommand``` on client side or ```CommandSender.Send``` on server side. You're free to choose what kind of parameters you use. However, pay slightly attention to the command name. Don't use command name with prefix **"\_sys\_"** because that's the prefix I'm using internally. Again, this is a small project and I don't want to be too conservative. 

#### Handling an incoming command
Commands will be delivered automatically, you just need to worry about how to handle them. On server side, this is extremely simple, we have prepared everything for you. Just checkout ```Scripts/Customize/CustomCommandHandler.cs``` and you'll know what to do. On client side, we need slightly more amount of work. Recall that in python tutorial [here](https://github.com/arpspoof/UnityKinematics/wiki/Client-Side-Usage-~-Start-Up#a-quick-python-client-tutorial), we already establish a dummy command handler. 
```python
class CommandHandler(AbstractCommandHandler):
    def HandleCommand(self, cmd):
        pass

commandHandler = CommandHandler()
```
What you need to do is just substitute ```pass``` with your own logic. For thread safety, this function will always be called from the main thread so don't worry about synchronizing stuff. 
