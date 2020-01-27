#### FPS Control
In the top left corner of the screen, you can see something like 'Render: 60 RF/s' which is the current rendering frequency. You can adjust this by keys ```ArrowUp```(Increase) and ```ArrowDown```(Decrease). 

When client side data is available, you can also see something like 'Physical: 60/3000 PF/s' which is the physical data frequency. Here 3000 means one physical second will correspond to 3000 physical frames. And 60 means that only 60 of the total 3000 physical frames will be rendered (uniformly spaced selected frames). If more than 60 physical frames are rendered per physical second, the animation will look 'slowed down' or 'more detailed' like. You can adjust this by keys ```ArrowRight```(Increase, slow down) and ```ArrowLeft```(Decrease, speed up). You can perform more aggressive adjustments by holding ```RightCtrl``` at the same time.

You can also pause the animation when client side data is available by pressing key ```Space```. 

**Note**: FPS control involves network interaction between the server and the client, if there is no client please do not use these buttons otherwise the server will crash. If the client has disconnected, these buttons have no effects but will slow down the server for a while. 



#### Clear the scene
If you want to remove everything left in the scene, you can press key ```R```. This will remove all client side data objects. 

**Note**: Do not use this when client is still sending data, just waste bandwidth...
