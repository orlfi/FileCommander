# FileCommander

FileCommander is an entry-level console two-panel file manager for .NET Core.

![Sample app](https://raw.githubusercontent.com/orlfi/FileCommander/master/screenshots/sample.gif)

## Features
* Navigate through the file structure separately in each panel
* Display of file size and creation time
* Changing the disk
* File selection
* Launch programs associated with the file type
* Copy / move files with the display of the operation progress and the ability to cancel
* Delete files and directories
* Error handling of file operations with the output of messages in a separate window
* Creating directories
* Rename files and directories
* Support for entering console commands in the command line:
  * **help** - open the help window
  * **cd** - change directory
  * **cp** - copy files and directories
  * **mv** - move files and directories
  * **rm** - deleting files and directories
* Navigate through the team history
* Save the last state when exiting
* Ability to set a color scheme using a theme file theme.json in in the program directory
* Logging critical errors in a file errors.txt

## Running and Building

* Windows - Build and run using the .NET SDK command line tools (`dotnet build` in the root directory). Run `FileCommander` with `dotnet ./FileCommander/bin/Debug/netcoreapp3.1/FileCommander.dll` or by directly executing `./FileCommander/bin/Debug/netcoreapp3.1/FileCommander.exe`.
* Windows - Open `FileCommander.sln` with Visual Studio 2019.

## Licence

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License version 3 as published by the Free Software Foundation.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details. 
