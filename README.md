# GameOfLife
This is a simple Library and Game to showcase it. It was mainly a educational project for myself, if you want to use it for some reason, please go ahead.

## The Lib
The GameOfLifeLib is programmed to be able to easily be used without the game itself with low coupling.
It is able to run decently sized boards/patterns and does so one iteration at the time. I also has some basic functionality to load and save(soon.) files in plaintext format. Is is easily extendable to more file types.

## The Game
The Game client is implemented in MongoGames and is mainly to showcase the lib.
![Example of loading a glider and starting the game.](https://github.com/VGdin/GameOfLife/blob/master/Example.gif)

The game has some basic config available in the Config.cs file on compilation.

The game can be manouvered either by keybindings or entering commands (first enter this mode by pressing dot). These can also be modified in the IputHandler.cs

The default key-bindings are:
- Arrow keys = move selection
- space = flip cell at selection
- hjkl = move camera
- y/u = Zoom in/out
- i/o = increase/decrease update rate
- p = pause
- c = clear board
- dot = enter console
- esc = exit console
- enter = (in console) apply command

The default commands are:
- Exit = exit console
- clear = clear board
- goto X Y = place the selection at given coordinates X Y
- center = center camera on selection
- LOAD filename = load pattern at selection, top right placed at selection.


N.B. I do not guarantee any parts of the code and it is provided as is.
