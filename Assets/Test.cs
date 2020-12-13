
/*
 * AIR RAID
 * 
 Game Controls
 WASD to move
 Left Shift to increase speed

HJKL-YUI are the different ability keys
H - Shoot
J - Drop Bomb

You have an energy meter, boost and abilities cost energy
When you run out of energy, you will tailspin until energy replensihes

Enemies spawn as eggs which drop into the scene
The eggs will hatch into one of several enemy types, including the "Egg Launcher" which will create more eggs

The goal of the game is to destroy all the enemies before they overwhelm and destroy the city




====================================================================================
                         EEEE  X   X  AAAA  MM   MM                                                                             
                         E      X X   A  A  M M M M                                                                                         
                         EEE     X    AAAA  M  M  M                                                                                 
                         E      X X   A  A  M     M                                                  
                         EEEE  X   X  A  A  M     M                                                  
====================================================================================

The Final examination is the extension, research and fixes of the game Air Raid included



This exam will be impossible without using breakpoints, Ctrl+F, and Find All references. To see everywhere in the code where a variable or class is used, you can use "Find All References". This project is very
large and that can easily feel overwhelming. Have a strategy. A good method is to first take out a piece of paper, look at the high level flow and try to draw out a little bit of how things are connected, what features exist, which
classes are responsible for what in vague terms. Then, read the questions and start getting more specific. Just like in the job field, There is more code than what you are asked to fix/understand. Which means it's important to
know how to focus on the parts of the code that matter. If a question asks about particles, dont randomly click classes and look through Enemy (unless its related to enemy shooting). Instead, go to the folder which contains most of the important
scripts you need. Then try to find out how those scripts all relate to each other, draw on paper the heirarchy, and then you can figure out just that part of the code how it works in isolation.
Having paper to doodle on will be important, plan for the fact you will be tired in 5 hours from now, and your memory will be full. Take notes as you discover things for your own sake.


=================================================================================Research Questions============================================================ 30%
For the research questions, imagine being asked an interview question, or leaving documentation behind for the next developer to inherit this code. Write enough details to satisfy someone's
belief that you understand the code.

0) This code uses Top Down Architecture, and therefore has a class which initializes and updates all the Managers, which class is that?
    - The top-most starting point of the game is the MainEntry.cs script which initializes the GameFlow.cs. GameFlow.cs does handle all the Manager Initialization and Updates

1) Describe how the buildings are all added to the building manager
    - When the BuildingManager is initialized, it counts all the children in the Tent/Building/Wall/Tower and Castle Parent (defined in the gl script) 
      and adds them to the "allBuildings" list using the total count as the "totalNumOfBuildings"

2) Is the input for the player calculated in PlayerController? If yes, where, if not, where and how does player recieve it?
    - The inputs keys themselves are handled in the InputManager.cs script and the abilities triggered by the InputManager is handled by the AbilityManager.cs script
    - The PlayerController Refresh and PhysicsRefresh function take an InputPkg as a parameter and when a key is pressed, it will pass along the InputPkg to the ability manager
    - The AbilityManager will then handle the key pressed correctly depending if the key is FirstPressed/Held/Released and will also update the player abilities

3) Where are the places that the player's death are called? How can a player die?
    - ShipDestroyed() in the PlayerController Refresh function - if the Player's HP is less-than or equal to 0, the ship is then destroyed
    - ShipDestroyed() in the PlayerController OnCollisionEnter() function, if the ship collides with anything, it is destroyed immediately.

4) What are the two ways (not including the error way) of spawning an enemy egg? (where in code are they called)
    - Eggs can be spawned at the beginning of the game, SpawnInitialSkyEggs() function in EnemyManager.cs, which "spawns" an egg on each Starting Plane of the map.
    - An egg can also be spawned/shot by the EggSpitter enemy (EggSpitter.cs) in its SpawnEgg() function.

5) When an enemy dies, are they destroyed right away, or put on a stack to be removed later?
    - When the EnemyManager.EnemyDied() function is called, the enemy is put on a stack to be removed later.

6) Where is the players max energy, max speed, max hp, and all the player variables stored and set?
    - In the PlayerManager.cs, there is a class called PlayerStats which handles everything related to the player's stats.

7) Draw out the enemy heirarchy, Which enemies inherit from which? What is the main responsibilities for each of the classes
    - The Enemy.cs class is the top-most class for enemies, it inherits from MonoBehavior and implements the IHittable interface.
        - This class is handling the enemy HP, Energy, its "alive" state and his grow in size according to a timer.

    - The RootedEnemy.cs script then inherit from Enemy.cs
        - This script contains the RootSystem for the enemy and also manages the handling when it comes to checking/setting if the enemy isRooted.
        - This script also handles the "root growing" time and the energy gained by the enemy when its root becomes bigger
        - It also handles the "linking" between the roots and the enemy. It will create the roots if needed and link the enemy itself to its roots to be able
          to manage the root growing/energy gain between the enemy and the root.
        - All the rooted enemies that inherits from RootedEnemy needs to be properly rooted before they can behave.

    - The MobileEnemy.cs script inherit from Enemy.cs
        - This script simply initializes the enemy (energy, isAlive state)
        - It also ensures that the NavMeshAgent component is loaded so that the enemy will be able to navigate on the NavMesh

    - The EggSpitter.cs enemy inherits from the RootedEnemy.cs script
        - This class handles the max number of eggs the enemy can spawn, the energy needed to spawn an egg (portion of the RootedEnemy's energy is transferred to the egg itself when
          its shot so the EggSpitter will get smaller after shooting an egg, requiring it to "rebuild" some energy/get bigger before it can shoot one again.
        - This class also handles the velocity at which an egg will be shot, according to a random mix/max threshold
        - Also handles the cooldown time for an egg to be shot again, random min/max threshold

    - The Egg.cs enemy inherits from RootedEnemy.cs
        - The script handles the eggs spawned when the game starts and the eggs shot by the EggSpitter
        - It handles the energy required for an egg to hatch and the maximum of time that an egg can take before it hatches

    - The AATurret.cs is the last enemy that inherits from RootedEnemy.cs
        - This script handles the "stats" of the turret such as the minimum amount of energy required to fire, the fire rate, the turret range and the projectile speed.
        - The AATurret.cs will fire according to a timer, if the player is in range and if the turret itself has enough energy to fire.
        - The damage caused by the projectile is also affected by the energy that was used to fire it.
        - The amount of energy used when firing a projectile is decided at random between 10% and 80% of the current enemy energy.
        - It is also not 100% sure that the projectile will hit the player as there is some "randomness" for the accuracy.
        - The projectile is also made to explode when it is near the player so even if the hit is not direct, it will still affect the player.        

    - The Crawler.cs inherits from MobileEnemy.cs
        - This script handles the enemy attack speed, the velocity at which it's projectile is launched, the energy cost for its attack and its state (attackMode or wanderMode).
        - Upon initialization, the enemy will be assigned a random building from the building list of the BuildingManager, it will be set to "attackMode = false (wanderMode)" and will start moving toward it.
        - Once it reaches the building's position, it will check it if still exists and if it has enough energy to shoot it, if it does, he will be set to "attackMode = true" and start attacking.
        - In the event that the building is destroyed before it can go to it and destroy it, it will find another target to destroy and go back to "attackMode = false" and start moving toward the new building.

8) Taking a look at the projectile class, I dont see any logic for "if enemy, Enemy.HitByBullet, if building, Building.HitByBullet..." so how are bullets damaging everything?
    - The way the Projectiles' damage is handled is by checking if the target implements the "IHittable" interface.
    - If the target that is hit implements that interface, it will take damage.
    - If the target does not implement that interface, and the target's layer is one of the following: Enemy, Building, Floor or wall,
      a function called "HitNonTarget" will be called instead which instead, will create a small explosion particle but will not cause any damage.    

9) The enemy creature known as "AATurret" shoots a projectile at players. What is the name of that projectile (That class name that does the projectile logic)
    - The logic for the accuracy and lifespan of the projectile is in a function called "ShootAtPlayer".

10) Explain how an egg hatches, how long does it take and how does it decide which enemy to spawn?
    - The egg hatching logic is in a function called "HatchEgg" in Egg.cs
    - An egg requires a minimum of 10 energy in order to hatch, and the energy increases periodically according to its root system.
    - The maximum amount of time an egg can take to spawn is 90 seconds
    - During the first minute of the game, all eggs will hatch an egg spitter
    - For all the other types of enemies that are hatched, spawning is decided according to a random chance
    - After the first minute of game, there is a 50% chance that an egg will hatch a Crawler, 30% chance for a AATurret and 20% for the egg to hatch an EggSpitter

11) How does a plane stall?
    - The plane can stall when it uses all its energy, causing the player to loose control and the plane to "nose-dive" toward the ground.

12) No buildings in the scene have building components, but they do in runtime, how are buildings setup?
    - When the game starts, the BuildingManager will take all the Buildings in the scene (according to the answer provided in question 2) and will Add the "Building" Component (script)
      to all buildings in its list using the ExtractAllBuildingsFromParent function.
    - Once the Building script has been added, it will "Initialize" the building and set its health according the the HP parameter that was passed when calling the
      ExtractAllBuildingsFromParent function in the BuildingManager Initialize function.

==================================================================================Bug Fix=================================================================== 20%
0) DONE -- bool "DEBUG_preventRootSpawning" in RootSystem.cs

There is a system of roots that grows out from certain types of enemies, such as the spitter or egg. These roots branch and spread across the land, the larger the root system, the more
powerful the creatures become, This is all controled in the RootSystem class. Although it is a neat feature, unfortunatly at this moment it is very unperformant and lacks some features we want.

You have permission from the developer who created it to temporarily disable the system. Find a safe way using a static bool in an intelligent place to disable the system. Have it that when
the bool is set to True, the system is disabled and no roots are created for any enemy. Try to be as least intrusive to the code as possible.

After testing your bug fix and ensuring that it works. Set the bool to false so the roots stay in the game (will need them for balancing). But post here where the bool is/how to toggle the system off.
If the system is on, enemies will not grow, so give all enemies a flat rate of growth if the system is disabled for testing purposes

1) DONE - The UI for the player energy and health bar are broken, in both scale and function, fix them

================================================================================Balances==================================================================== 20%
0) DONE - An enemy's energy has an affect on the size. Increase the ratio of energy to size by 33%   (for example if value was 1, set to 1.33)
1) DONE - Change the rate of fire of the basic machine gun ability to .1f   (so it shoots 10x a second)
2) DONE - Change the energy cost of the bomb drop ability to 35
3) DONE (Added 6th plane) The enemy currently spawns randomly in 1 of 4 regions, add a 5th region for it to be able to spawn from




================================================================================Feature===================================================================== 30%
Add only ONE of the following features of your choice.  (You do NOT have to do all of them, just chose one)
It is advised first to take a look at how much work you think each feature would take and which you feel most comfortable with, and then go for that one.

UI Feature (DONE)
On the HUD,there is a white square, it represents the abilities cooldown, however it is not active or completed. Build a system that shows the cooldown of your abilities with a League Of Legends
or RPG style cooldown circle. For the image, you can just show the key that corresponds to the ability or put a string name.
Any system that already currently exists is probably incomplete and can be discarded if you wish. Forever, you need to maintain some form of generic-ness through a proper UI or HUD Manager.
Try not to hardcode too much logic directly in the each child of ability.

Ability Feature
Add a new Ability, Fire a rocket that travels foward slowly and then explodes with a large AOE. 
    -It should fire 2 rockets in one shot, one from each wing\
    - Rocket has a particle or line renderer fire trial.
    -The rocket starts slower but accelerates over time
    -Makes a sound on explosion
    -Should cost at least 30 energy to fire
    -Have a rate of fire of 1 shot per 3 seconds

Enemy Feature
Add a new type of enemy, a circle blimp that floats into the sky until it reaches a specific height, and then wanders randomly in a Random.UnitCircle direction. 
After a long random peroid of time (20~30 seconds) and if it has over a certain threshold of energy, it will split, creating another one of itself. (but dividing it's energy in 2)
It inherits the energy properly from the egg
It gains a certain amount of energy per second while alive
If it crashes into the player or a building, it should kill them and it self
It can be shot and killed just like any other enemy











 */
