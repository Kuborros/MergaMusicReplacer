# Merga Music Replacer 1.2.1

A mod for the game [Freedom Planet 2](https://freedomplanet2.com/) which allows you to replace the music with custom tracks during each phase of the final boss fight.  

## Installation:
To install the mod extract the downloaded zip file contents from the releases tab into the main game directory.  
If asked, agree to merge the BepInEx folders.  

## Usage:
Run the game once after installing so the configuration file gets created.  
put three music tracks that are in a supported format in to the `mod_overrides` folder and open the configuration file in *./BepinEx/config/com.kuborro.plugins.fp2.mergamusic.cfg* with your preferred text editor.  
replace the track names in the configuration file with the names of your tracks in `mod_overrides` **including the file extension**!  
![example_of_mods](https://user-images.githubusercontent.com/33236735/194930971-f8581181-3e5d-45e5-ad35-e0d1b8f8837d.png)

## Supported formats:
MPEG layer 3 (.mp3)  
Ogg Vorbis (.ogg)  
Microsoft Wave (.wav)  
Audio Interchange File Format (.aiff / .aif)  
Ultimate Soundtracker module (.mod)  
Impulse Tracker module (.it)  
Scream Tracker module (.s3m)  
FastTracker 2 module (.xm)  

## Prerequisites:
The mod requires [BepinEx 5](https://github.com/BepInEx/BepInEx) to function. You can download it here:
* [Direct Download](https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x86_5.4.21.0.zip)  

Extract the downloaded zip file in to the main game directory.  

## Building:
Follow the BepinEx guide for setting up Visual Studio found [here](https://docs.bepinex.dev/master/index.html).  
Open the solution in Visual Studio and build the project.
