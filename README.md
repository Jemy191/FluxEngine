# FluxEngine

## ⚠ In active development
Expect breaking change.

This is a simple and ultra modular C# GameEngine/RenderingEngine.  
My goal with this project is to explore how a game made with .Net Common/Best practice perform both in ease of use and speed.  
A secondary goal is to see how decoupled I can make the modules.
My idea is to provide engine agnostic* module that you can plug in any .Net game/engine or even unrelated thing.

I have a lot of idea and thing planned but don't expect anything big for a long time😅

#### Some current feature:
- Opengl rendering driven by ecs
- Ecs system
- Ecs behavior system. Similar to Unity GameObject because esc is really not the best for gameplay
- Ecs driven Resource system**

#### Some things I have planned:
- [ ] Vulkan rendering driven by ecs
- [ ] Assets system***
- [ ] Modular editor

### Thing I made with this Engine(Not yet ready for public eye😅)
- Minecraft like(Early dev)
- Music player that work with tags instead of playlists(Wip****)

> \* Except some low level module like: Rendering, audio, windowing, input or the core game engine module.  
This is due to the engine using Silk.Net for these low level stuff

> ** More for in engine resource that are use directly by low level stuff like the rendering engine. Resources are different from Assets***.  
> Most of the time you will use component to store data and not resource.
> Example of resource: Mesh, Texture, Shader, Sound, etc.

> *** Assets are files on the system that contain data to create resources and Resources are datas that are already loaded.  
> Example of Assets: Prefab, Scene, Level, etc.

> **** I want to use the OpenAl audio module and Assets module in it. But they do not exist yet.
