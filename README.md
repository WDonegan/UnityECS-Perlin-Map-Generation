# UnityECS-Perlin-Map-Generation

This is an experimental project, a sample/demonstration, of 
using perlin noise in layers to create procedural map generation 
data.

I've started by creating a basic UI to adjust the scale and offset of a perlin layer, there 
are 3 hard coded layers and currently you cannot add or remove layers. There is a dropdown to
select each layer to manipulate it. There are also options to link the x and y offsets, as well
as to Band and Outline the map. 

(BAND) My term for creating a contour map of the combined perlin data. 
(OUTLINE) Pretty self explanatory, outlines the various banded regions.
          Note: Doesnt work correctly unless banding is enabled, need to 
          fix in the UI so its not available independently. 
          
The idea here is this is going to create a heightmap, the outline will represend cliffs for 
example, each shade representing a generalized terrain height. This project started during 
the Kenny Game Jam which was about using only Kenny assets, the project evolved and grew 
beyond its intended scope. 
