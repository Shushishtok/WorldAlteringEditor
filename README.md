![Dawn of the Tiberium Age Logo](https://github.com/Rampastring/TSMapEditor/raw/master/dtalogo.png "DTA Logo")

# Dawn of the Tiberium Age Scenario Editor

Work-in-progress scenario editor for Dawn of the Tiberium Age (DTA) https://www.moddb.com/mods/the-dawn-of-the-tiberium-age

## Motivation

The purpose of the project is to develop a new scenario editor for Dawn of the Tiberium Age, 
replacing the old FinalSun map editor developed in the TS/RA2 modding community in the early 2000s.
I think it has been sad to see the ancient map editor restrict the community as much as it has done,
and instead of developing a proper replacement for it (which could be achieved relatively quickly),
the community has instead started writing complex hacks for the old editor by reverse-engineering it,
modifying its executable and injecting code into the editor in the form of custom DLLs.

I personally think it'd be much better in the long term to focus these efforts on building a new editor
instead of modifying one that has no available source and that is based on a very outdated technological base.

To make it familiar for existing mappers, the editor is designed to follow the FinalSun UI design,
but with modernizations and changes to make the editor smoother and more efficient to use.

## Current state of the project

The editor is in an usable state and covers almost everything of what can be done with FinalSun. 
Most basic and commonly used editing tools are included and they're generally more
efficient than the FinalSun equivalents.

There are also some limitations in the editor, particularly on the rendering side of things.
The current map renderer, while functional and usable, is very primitive and does not
properly keep track of areas to refresh, which leads to graphical glitches at times.
The current renderer should be considered more as a proof-of-concept of DTA/TS/RA2 map
rendering done with MonoGame than an actual serious implementation.

As the entire editor is currently work-in-progress, the code-base is so as well.
If you browse the code, you might run into unfinished feature implementations.

## System requirements

This editor uses MonoGame with Rampastring's custom XNAUI library. 
No XNA build is available, so a DirectX11 compatible GPU is required.
VRAM requirements might be steep, as the sprite graphics of the game are currently
converted into full 32-bit ARGB textures prior to drawing. In case of
Dawn of the Tiberium Age, the editor appears to allocate roughly 500 MB of VRAM.

## Downloads

There is currently no publicly hosted download, but the editor will be included with Dawn of the Tiberium Age later on.
Currently, it's easiest to clone the source and compile it yourself using Visual Studio 2017 or newer.
If you want a separate download, you can contact me and ask for one on DTA's Discord server: https://discord.gg/6UtC289

## License

The editor is licensed under the GNU General Public License, version 2.
If you create and publish a derivate, you need to also release your source code for the fork.
Please see LICENSE.txt for more details.

## Why only DTA? What about Tiberian Sun or Red Alert 2?

As DTA runs on the Tiberian Sun game engine, the editor is compatible with the TS/RA2 map format. 
However, because DTA's terrain is flat, the editor does not presently support height levels in
either rendering or input. Voxels are also unspported. Implementing support for those is possible, but it takes some work.

I'm planning to look into supporting height levels later, but I can't currently promise anything regarding the outcome.
Regarding voxels, I don't have any plans of implementing support for them. However, I'll gladly accept contributions and provide assistance
with implementation if someone in the community wants to adopt this editor for TS or RA2/YR and
is willing to write the required code for voxels or height support.

## Screenshot

![Screenshot of the editor](https://github.com/Rampastring/TSMapEditor/raw/master/mapeditor.jpg "Map Editor Screenshot")

## Introduction video

[![Dawn of the Tiberium Age Scenario Editor Introduction](https://github.com/Rampastring/TSMapEditor/raw/master/videopreview.jpg)](https://www.youtube.com/watch?v=jIcr3nCqx7M "Dawn of the Tiberium Age Scenario Editor Introduction")
