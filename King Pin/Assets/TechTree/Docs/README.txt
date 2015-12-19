Tech Tree Tool
--------------

API Documentation
-----------------
http://www.differentmethods.com/docs/TechTree/html/files.html

Video Intro
-----------
http://www.youtube.com/watch?v=i5SxZsYW890


Video Tutorial
--------------
http://www.youtube.com/watch?v=NrK8FL-H5bc


Support
-------
support@differentmethods.com


General Usage Notes
-------------------

Colors are used to identify resources and groups. Make sure you set these to
a distinct color when you create them.

Every Resource, Group and Blueprint is identified by a unique string ID. You
must make sure the ID is unique and clear. It is used in your code to fetch
blueprints from the model. It is helpful to name the ID after the item it 
belongs to.

Green buttons are additive operations, Eg to add a connection between blueprints.
Red buttons are destructive operations, Eg to remove blueprints / groups. Yellow
buttons show more information, or toggle visibility on and off.

**IMPORTANT** 
The Test level included has a generic game interface which you can use to test
your tech tree models. This interface is the most basic implementation needed
to use a tech tree model in a game.


Getting Started
---------------

1. Choose Window -> TechTree Manager. A new window will open. I recommend that you 
maximise or dock this window, as the tree can become large!

2. Click the object field at the top right of the window, and choose the 
HomeworldTechTree from the Assets. The window should now load the initial 
Resources screen.

3. To create your own TechTree, right click in the project window and choose
Assets->Create->TechTree Model. 

You will want to put the new asset in it's own
folder, as during editing, many more assets will be created alongside the main 
asset.

NB: The above has changed, all assets are now created and stored inside the main
blueprint model asset. The example homeworld tech tree however still uses the old
format.


Resources
---------
These are the variables that the player must manage in the game, and use to
create units, research technology etc. They could include "Research Points",
Credits, Energy, Wood, Gold, Stone Etc.

Resources have an initial quantity (qty) and a max possible blueprint cost
(MaxCost). The MaxCost is the maximum value that any blueprint might require to
be built. This value is only used to provide a helpful scale in the Costs editor.

The Regen flag is used to make a resource automatically increase its quantity
at game time. The amount it increases is controlled by the Rate value.

If a resource is Capped, then the most quantity of that resource ever available
will be the the Max value.


Blueprints
----------
These are the things that can be built, researched or constructed in the
game. They could include soldiers, factories or spaceships. Most blueprints
have prerequisite blueprints, which must be built by the player during the
game.

Some blueprints have a "Mutex", which means only one of the dependent blueprints
can be built by the player. This can force them to choose between two branches
of technology, and helps adds strategic depth to the game.

Some blueprints are a Factory. This means they are used to build other blueprints.
(Only if they have been built themselves!) Most games only have 1-3 factory 
blueprints. For example, you can have a Factory for building air force units, and
another for building land units, and perhaps one for building structures, or
researching new technology.

You will always need at least one factory blueprint already built for the
player at the start of the game. From this initial factory, the player can start
building other blueprints during the game. You enable a factory to be in this
state by checking the "Pre-built" flag for the blueprint.

Usually research type blueprints are only built once, however unit or structure
type blueprints can be built multiple times. You will need to set the 
"Allow Multiple" flag on the blueprint to enable this.

All blueprints have a GameObject and a Sprite field. You use this to attach
game assets to a blueprint. For example, you might attach a soldier model to
the soldier blueprint. You can then Instantiate this model at runtime when
the soldier blueprint is built.


Costs
-----

When a blueprint is built, it can consume resources. It also has a Min Time value,
which is the minimum time it would take to build the blueprint, if enough resources
were available.

If a resource is consumed, and there is no more of that resource available,
the build process will pause until more of that resource becomes available.

Time and Resource Costs can be modified in the Blueprint inspector, or in the
Costs window using slider controls.


API Overview
------------

Once you have built a tech tree, you can use it in a game with the 
BlueprintModelController component. Assign this component to a GameObject, 
then assign the model you created with the Tech Tree Editor to the BMC field.

You access the Tech Tree information using this component, with one instance
of the component per player.

When the BMC starts, it will check all blueprints and instantiate any that are marked
to be pre-built. If the blueprint does not have a gameobject assigned, it will
create an empty gameobject. It then adds a TechTreeUnit component to the new
object, and if the blueprint is a factory, it will also add a TechTreeFactory
component to the new object. Finally, an "OnSpawn" message is sent to the new
gameobject, with a single parameter which is the factory that created the unit.
NB: The factory parameter will be null for all prebuilt units.

TechTreeFactory components have a public array of blueprints, which you can
iterate through and use in your game code. The .CanBeBuilt property can be used
to see if the blueprint can be built into a unit. To start a build process, the 
component has a Build(string ID) method which will return a BlueprintBuildRequest
object. You can use this object to track the status of the build. When the build
is completed, the unit will be instantiated and the "OnSpawn" message will be sent
to the new object.

The Blueprint BuildRequest object is used to track the state and progress of a 
blueprint that is being built. It has a status property, which is used to determining
if the request was successful or not. It has a blueprint property, which is
the blueprint which is being built. You can also use the percentComplete field
to monitor progress of the build. The BuildRequest object also has a Cancel()
method, which terminates the build and returns the resources.

