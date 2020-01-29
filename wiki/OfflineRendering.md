## Offline Rendering

Sometimes it may be useful to render data offline which means the Unity server side can work alone to render something without the connection with a client. You may consider use these functionalities if

+ You would like accurate timing for video recording.
+ You are playing exactly the same motion once and once again.

----
### Data Recorder

+ Script: SystemPlugins/DataRecorder.cs
+ Required component: None

#### Introduction
In order to render offline data, we have to create a offline data file first. This is done via DataRecorder plugin.

#### Parameters
+ *pathToFile* : Path to the file to store the result.
+ *autoStart* : Automatically start the recorder when entering play mode.
+ *autoStopAndWrite* : Automatically save current data to file when exiting play mode.

#### API
Please refer to the [API list](ServerAPI.md#Offline-Rendering) for this plugin.

#### Create data file by recording online data
+ Enable *autoStart* and *autoStopAndWrite*.
+ Enter play mode, start the client.
+ After the client sending all required data, exit the play mode.
+ Data will be recorded to *pathToFile*.

#### Manually create a data file
+ Disable *autoStart* and *autoStopAndWrite*.
+ In your script, manually call ```StartRecording``` before adding any data.
+ In your script, manually call ```StopRecordingAndWriteToFile``` after adding all data.
+ In your script, call ```RecordFrame``` to add a new frame to your data file.
+ In your script, call ```RecordCommand``` to add a new command to your data file.

----
### Data Playback

+ Script: SystemPlugins/DataPlayback.cs
+ Required component: None

#### Introduction
This plugin renders a data file offline.

#### Parameters
+ *pathToFile* : Path to the data file obtained via DataRecorder.

#### Usage
Start the play mode and data from the data file will be read and rendered.
