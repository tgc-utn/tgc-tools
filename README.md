# MeshCreator

Simple 3D scene editor created with C# and DirectX 9

## Features
- You can create a new 3D scene with simple tools
- Ideal tool for a developer (without much knowleadge of 3D modelling) to assembly existing models and create a 3D scene
- You can save your scene and use it later
- Scenes are stored in a custom XML format (easy to read and convert to any another format)
- You can load your scene with [TGC.Viewer](https://github.com/tgc-utn/tgc-viewer) engine.

## Modeling tools
- Create simple geometry: cube, sphere, planes
- Import existing meshes and scenes
- Move, scale and rotate
- Manage large scenes with layers: create new lawers, move objects from layer to another one, hide/show objects inside a layer, etc.
- Change textures of an object
- Adjust texture UV offset and tiling
- Navigate scene with a first person camera
- Group many objects into a single mesh
- Copy and paste objects
- Delete objects
- Edit textures at a triangle level
- Remove individual vertices, edges and triangles from a mesh (only remove, you cannot add new geometry... for now)

## Installation
- Install [.NET Framework](https://www.microsoft.com/net/download/framework) 4.6.1.
- Install [DirectX](http://www.microsoft.com/en-us/download/details.aspx?displaylang=en&id=6812) end-user.
- Download [TGC.Tools](https://github.com/tgc-utn/tgc-tools/releases).
- Execute TGC.Tools.exe

## Usage
- Controls are as much as possible similar to 3Ds MAX
- Select objects: left click on object. You can also create a selection rectangle with left click.
- Pan view: with the mouse middle button
- Rotate camera: LEFT ALT + middle button
- Camera zoom: mouse wheel
- Center camera on one object: Z
- Change editor mode: Q (selection mode), W (translation mode), R (scale mode)
- Select all objects: CTRL + E
- Hide selected objects: H
- Open Object Browser: O (change name, move to another layer, etc)
- Create a new box: B
- Create a new plane: P
- Load an existing mesh: M
- Save scene: LEFT CTRL + S
- Save scene overwritting previous loction: LEFT CTRL + LEFT SHIFT + S
- Delete objects: DELETE
- Copy and paste objects: LEFT CTRL + V will copy and paste all the objects that are currently selected.
- Camera front view: F
- Camera top view: T
- Camera left view: L
- Group objects: G

## How to extend app
The editor currently works with an XML custom format (used by [TGC.Viewer](https://github.com/tgc-utn/tgc-viewer) engine).
It shouldn't be so difficult to change that in order to support another format.
- Mesh importing is done in class: [TgcSceneParser.cs](/TGC.Tools/Utils/TgcSceneLoader/TgcSceneParser.cs)
- Mesh exporting is done in class: [TgcSceneExporter.cs](/TGC.Tools/Utils/TgcSceneLoader/TgcSceneExporter.cs)
