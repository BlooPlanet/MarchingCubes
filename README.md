# MarchingCubes
A simple 'Marching Cube" algorithm to generate complex terrain and other stuff.
<img width="1912" height="1174" alt="Screenshot 2026-10-20 at 19 29 33" src="https://github.com/user-attachments/assets/8fcc26f5-08ea-43ea-bb8f-40847b8b3a0c" />
try it: https://blooplanet.itch.io/marching-cube
```C# Marching Cube API
  	MarchingCube marchingCube = new MarchingCube(width,height,depth,surfaceLevel,initialDensity);
	marchingCube.SetDensity(coordinate,densityValue); // you can set density from this function
	Mesh mesh = marching.ConstructMesh(); // this build mesh and return mesh type from density points
```
Features:
- easy to use interface (api)
- terrain and player example
- cpu based

Local Setup Instruction: first step download project source code and then open downloaded folder with unity project.
Note:you need unity 2022.3 game engine.

Credits:
Sebastian lague: https://www.youtube.com/watch?v=M3iI2l0ltbE .
Graphics Standford Edu: https://graphics.stanford.edu/~mdfisher/MarchingCubes.html .
NVIDIA: https://developer.nvidia.com/gpugems/gpugems3/part-i-geometry/chapter-1-generating-complex-procedural-terrains-using-gpu .
b3agz: https://www.youtube.com/watch?v=dTdn3CC64sc&t=1368s .
Unity 2022.3.
