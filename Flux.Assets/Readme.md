# Flux.Assets

## What are assets in Flux?

Assets wrapper around files.

They can be a raw data file like:
- Texture
- Mesh
- Audio
- fonts
- etc.

Or they can be a user or engine asset like:
- Texture import settings
- Material
- Mesh import settings
- Item settings
- etc.

Asset can also be a sub asset contained in another asset like:
- Gltf mesh
- Gltf texture
- Gltf material
- Animation clip
- etc.

They can also be virtual assets like:
- Procedural data made at runtime
- Remote assets from a server

An asset always as a .guid file with to contain it guid.

An asset can be linked to another asset by using "." in the file name.
Example:
- Texture.png
- Texture.png.importsettings

##### All of this subject to change.