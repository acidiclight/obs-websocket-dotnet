# obs-websocket-dotnet

OBS WebSocket client and command-line interface written in C#/.NET. This project allows you to control OBS Studio using its built-in OBS WebSocket server.

## Why?

Using the OBS UI to do everything is slow. It is common for many users to set up hotkeys instead. However, this is also limiting - you only have so many keys you can press that don't conflict with other system actions, and so many fingers to press them with.

This has lead to the creation of macro boards like the [Elgato Stream Deck](https://www.elgato.com/en/welcome-to-stream-deck) which offer a much more comfortable way to control OBS and other programs. However, particularly with the Stream Deck, a driver and plugin is required. These are often not available on all platforms.

That's where this project comes in. I created it so that I can use my Stream Deck on Linux with an unofficial driver, and use the command-line to control OBS using the Stream Deck. That's the primary use case for this tool. Because it's a command-line interface, it can be scripted to do many more advanced actions than what would be possible with a hotkey.

## End-goal

The end goal of this project is to be able to do everything you can in the OBS Studio UI that you'd normally do while streaming or recording, but through the command-line.

This does not mean it's a full replacement for the OBS UI. For obvious reasons, you should not be able to expect to set up all your scenes and sources through the CLI, since these naturally require a graphical interface. However, once you've set up your scenes, you can control the program.

## Features

As with any command-line tool, for a full list of commands and features, use the `--help` option. But here are some of the main features you'll want to use, and the commands to use them.

 - List all available scenes: `obs-cli scenes list`
 - Switch to a scene: `obs-cli scenes set-current "Left Desktop"`
 - Start recording: `obs-cli recording start`
 - Stop recording: `obs-cli recording stop` - will print the file path to the video that was just recorded
 - Pause/resume recording: `obs-cli recording pause`, `obs-cli recording resume`
 - Get recording status: `obs-cli recording status`
 - Check if you're recording: `obs-cli recording is-active`
 - Check if recording is paused: `obs-cli recording is-paused`
 
## TODO

Features that are planned but not implemented:

 - Streaming controls - start, stop, pause, resume, status, etc.
 - Audio mixer controls - mute, unmute, list, volume control, etc.
 - Controlling the virtual webcam
 - Controlling Studio Mode
 - Filters, transitions, sources
 - Configuration of the CLI through a user config file instead of command-line arguments (e.g, setting the port and password of the WebSocket server)
 - Support for binary communication (currently only JSON transport is supported)
 
## Project Structure

This is really just your bog-standard .NET 7 solution, everything important is under `src/`.

The solution is made of two projects:

 - **OBSWebSocket.Client`: This is the backend client library for communicating with OBS Studio over WebSockets. In the future, I may split it off into a separate repo and turn it into a standalone NuGet package...but I don't know how to do that yet.
 - **OBSWebSocket.CommandLine**: This is the actual `obs-cli` tool and acts as a command-line interface over the backend library. In most cases, this is what you want to actually compile and run.
 
## Building and installation

This is a .NET 7 project, so it must be built with the .NET 7 SDK. This should be easy enough to do, so I'm not going to bore you with instructions on how to do it.

As far as installation goes, you will likely want to publish the **OBSWebSocket.CommandLine** project as a self-contained application. Then move it to the appropriate place for your platform and add `obs-cli` to your PATH. You can now use it globally.

## Setting up OBS itself

By default, the OBS WebSocket server is disabled. To enable it, open up OBS Studio and navigate to Tools -> obs-websocket Settings. A dialog box will open.

From here, enable the "Enable Web Socket Server" checkbox to turn the server on. By default, it will listen on port 4455 but this can be changed in the same settings dialog.

### Authentication

Please note: **OBS must be running for `obs-cli` to work. The CLI is not a replacement for OBS itself.**

It is recommended to set a password on the OBS WebSocket server to prevent unauthorized connections. The `obs-cli` tool supports authentication through the use of the `--password` option, and in the future, a configuration file.

In the OBS WebSocket settings dialog, you can choose to enable authentication and then set a password in the password field. Then, when using `obs-cli`, provide the same password as shown in the example below.

```bash
obs-cli recording start --password "password123"
```

### Communicating to OBS on another computer

Yes. This is a thing you can do. It is recommended to set up your firewall correctly, and **to absolutely set up a password**, but you can use `obs-cli` to control OBS from another computer.

You can use the `--hostname` and `--port` options to do this.

```bash
obs-cli scenes list --hostname 192.168.122.1 --port 4455 --password "super secret password"
```

An example use-case for this would be having `obs-cli` running inside a virtual machine but having OBS running on the host system with a more powerful GPU, but you still want to be able to control OBS from within the VM using a hotkey or other input device. Farfetched, but someone out there will do that.
