# OmxPlayerGUI
Graphics User Interface for the Raspberry Pi command line omxplayer audio/video player

# Details
The OmxPlayerGui is designed to play .mp3 and .mp4 files when you click on a file using file manager.

The Raspberry Pi computer comes with an audio/video player called omxplayer. It's a command line program. Pressing keys while it's running causes changes to what the player is doing. For instance, pressing space pauses the audio/video. Pressing the escape key exits the program. The arrow keys advance and rewind playback.

# Language:
The OmxPlayerGui is written in C#.Net using MonoDevelop. Controls are added to the form with the ControlCreator class because MonoDevelop doesn't have a gui for adding controls on a form like Visual Studio has. Visual Studio doesn't run on the Raspberry Pi because Microsoft is afraid. To run the program, type mono OmxPlayerGui.exe.

# Technical details:
The OmxPlayerGui program is a C# Gui program using System.Windows.Forms to display a clickable interface to play a song/video. The program starts the omxplayer in a separate process and attaches to the standard input. Clicking on a button on the OmxPlayerGui sends the appropriate keystrokes to the omxplayer command line program.

The OmxPlayerGui takes the name of a file as a command line parameter. It plays .mp3 and .mp4 files. It's designed so you can double click on a file and the OmxPlayerGui will start playing.
