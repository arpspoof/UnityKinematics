## Preliminaries
**Unity Recorder** is only the unity plugin needed. Please make sure you've first upgraded your Unity Editor to the newest version (2019+) otherwise things may not work.

**Installation guide**
+ Open your Unity project, go to **Window** -> **Package Manager**.
+ Click on **Advanced**, choose **Show preview packages**, choose Yes.
+ Search for **Unity Recorder**, click **Install**.

**Notes**:

If you see compiler errors after installation, that means your Unity version isn't correct. Please upgrade it to the newest one. Also note that Package Manager may also not work in old versions.

## Configurations
+ Go to **Window** -> **General** -> **Recorder** -> **Recorder Window**.
+ Change the target frame rate to 60. (Or keep default 30 if you like)
+ Change video format to a suitable one. Default is mp4. However, mp4 format may not work on Linux, try WebM instead.
+ Add a file name suffix wildcard to prevent accidental file overwriting. Click on **+Wildcards**, choose **Takes**.
+ Change output resolution to a suitable one. This can be FHD-1080p or even 4k if you like.

## Record a Video
+ Just click on **Start Recorder** and it will start recording. You don't have to go to play mode first. This will automatically start the play mode.
+ If interactions is needed in videos (like rotating camera), you may want to use C# code to control it rather than do this manually while recording. The recording process can be quite slow and hand interactions may not be accurate.
