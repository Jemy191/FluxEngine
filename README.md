# FluxEngine

## ⚠In active developement
Expect breaking change.

This is a simple and ultra modular C# GameEngine/RenderingEngine.  
My goal with this project is to explore how a game made with .Net Common/Best practice perform both in ease of use and speed.  
A secondary goal is to see how decoupled I can make the modules.
My idea is to provide engine agnostic* module that you can plug in any .Net game/engine or even unrealed thing.

I have a lot idea and thing planned but dont expect anything big for a long time😅

#### Some current feature:
- Opengl rendering driven by ecs
- Ecs system
- Ecs behavior system. Similar to Unity GameObject because esc is really not the best for gameplay
- Ecs driven Resource system**

#### Some thing I have planned:
- [ ] Vulkan rendering driven by ecs
- [ ] Assets system***
- [ ] Modular editor

### Thing I made with this Engine
- Minecraft like(Early dev)
- Music player that work by tag(Wip****)

* Exept some low level module like: Rendering, audio, windowing, input or the core game engine module.  
This is due to the engine using Silk.Net for these low level stuff

** More for in engine resource that are use directly by low level stuff like the rendering engine. Resources are different than Assets***.

*** Assets are files on the system and Resource are data that are already loaded.

**** I want to use the OpenAl audio module and Assets module in it. But they do not exist yet.
