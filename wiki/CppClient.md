### Notes on C++ Client

Basically, there's no difference with usage of these API functions except language differences. The concepts are the same. However, there are some extra steps needed to set up a C++ client. 
#### Get the API headers and static libraries
After building this project, you should run ```make install``` to generate these headers and libraries. In the install folder (usually in the main repository directory), you'll see two folders, ```include``` and ```lib```. You'll need to add the ```lib``` directory to your compiler linking library path, and link library with name ```client``` to your project. Add the path to the include directory to your compiler include directory definition. In your code, just put this statement to access all API function:
```c++
#include <KinematicsClient.hpp>
```
#### Link rpclib to your client project
In general, you also need to link rpclib to your project. You can refer to our [build instructions](Build.md) on how to get the rpclib libraries.
