**The University of Melbourne**
# COMP30019 – Graphics and Interaction

Final Electronic Submission (project): **4pm, Fri. 6 November**

Do not forget **One member** of your group must submit a text file to the LMS (Canvas) by the due date which includes the commit ID of your final submission.

You can add a link to your Gameplay Video here but you must have already submit it by **4pm, Sun. 25 October**

# Project-2 README

You must modify this `README.md` that describes your application, specifically what it does, how to use it, and how you evaluated and improved it.

Remember that _"this document"_ should be `well written` and formatted **appropriately**. This is just an example of different formating tools available for you. For help with the format you can find a guide [here](https://docs.github.com/en/github/writing-on-github).


**Get ready to complete all the tasks:**

- [x] Read the handout for Project-2 carefully

- [ ] Brief explanation of the game

- [ ] How to use it (especially the user interface aspects)

- [ ] How you modelled objects and entities

- [ ] How you handled the graphics pipeline and camera motion

- [ ] Descriptions of how the shaders work

- [ ] Description of the querying and observational methods used, including: description of the participants (how many, demographics), description of the methodology (which techniques did you use, what did you have participants do, how did you record the data), and feedback gathered.

- [ ] Document the changes made to your game based on the information collected during the evaluation.

- [ ] A statement about any code/APIs you have sourced/used from the internet that is not your own.

- [ ] A description of the contributions made by each member of the group.

## Table of contents
* [Team Members](#team-members)
* [Contributers](#contributers)
* [Explanation of the game](#explanation-of-the-game)
* [Technologies](#technologies)
* [Using Images](#using-images)
* [Code Snipets ](#code-snippets)
* [Brief explaination of the game](#brief-explaination-of-the-game)
* [How to use the game](#how-to-use-the-game)
* [How we modelled objects and entities](#how-we-modelled-objects-and-entities)
* [How we handled the graphics pipeline and camera motion](#how-we-handled-the-graphics-pipeline-and-camera-motion)
* [Description of how the shaders work](#discription-of-how-the-shaders-work)

## Team Members

| Name | Task | State |
| :---         |     :---:      |          ---: |
| Timmy Truong  | Shaders, Video Editing, Interface, Lighting and README     |![90%](https://progress-bar.dev/90)|
| Nathan Rearick    | Movement, Evaluation     |![90%](https://progress-bar.dev/90)|
| Lucien Lu    | Level design, Turrets/Projectiles, AI, Main Menu, Camera, Abilities, Special Movement, Guns, UI      |![95%](https://progress-bar.dev/95)|

## Contributers

| | Contributer | Contribution | Email | Other |
|---|---|---|---|---|
| <p align="center"><img src="kevingaoinsta.PNG"  width="300" ></p> | <b>Kevin Gao</b> | Turret and map assets modelling | kevin.haha@gmail.com | Instagram: https://www.instagram.com/keving_win98se/ |
## Explanation of the game
Our game is a first person shooter (FPS) that....

You can use emojis :+1: but do not over use it, we are looking for professional work. If you would not add them in your job, do not use them here! :shipit:

	
## Technologies
Project is created with:
* Unity 2019.4.3f1
* Ipsum version: 2.33
* Ament library version: 999

## Brief Explanation of the Game ##
You are Marine 8080, a newly recruited private to a special space marine force. A 2.5D combat platformer, you must guide Marine 8080 (the player) from the start to the end of each level in order to activate the radars (Goal objects) until they come face to face with the source of all the trouble. As a game that involves combat, the player begins with 100 health points and will be confronted by a large variety of obstacles (enemies). The player can take damage and die from enemy AI and turret projectiles as well as falling into the void; it is up to the player to survive these hazards. To supplement the player against these hurdles, various powerups and health packs can be picked up to aid the player.
## How to use the game ##
The program is a unity game. Once launched, click “Play” to be faced with various options. “New game” to start from the first level, “Continue” to start on the level where you last left off, “Level select” to open up a menu of each unlocked level (to unlock a level, one must progress to it through the previous levels). Clicking on “Difficulty” will change the difficulty of the game between Easy, Normal and Hard. 
Easy: 50% Damage taken and 150% Damage done. 
Normal: 100% Damage taken and 100% Damage done. 
Hard: 150% Damage taken and 50% Damage done.
There is also an “Options” button which allows the user to delete all saved data, resetting all level unlocks and forces the user to start from scratch, or adjust the volume.

Once the game is begun, the commander will instruct you of the basics. In the heads-up display, on the bottom left, is the player’s current health. In the bottom middle is a list of the player’s unlocked abilities with their respective key to activate on top. In the bottom right, is the current active weapon’s ammo over max ammo. In the top left, are the weapons the player can use where blacked out weapon icons means the weapon hasn’t been unlocked (found) yet. The weapon icons are paried with a number indicating the key to press to switch to the associated weapon.
## How we modelled objects and entities ##
The turret models, platforms and scenery decorations (crates, columns) are created by Kevin Gao through Blender, kevin.haha@gmail.com. Whilst the weapon models are taken from the asset store. As for the levels, everything was made using Unity’s ProBuilder. For the Player model, it was taken from https://vrcmods.com/item/4352-commando, a website for VRChat avatar models. To match our intended style of the game, we recolored the texture map. To top off the models, every model (gun, enemy, player, ProBuilder objects, etc.) one can find in the game is equipped with one of multiple custom shaders.
## How we handled the graphics pipeline and camera motion ##
--- (Timmy write this part thanks)

As for the camera motion, a simple fixed z coordinate (z dimension is the depth for this game) camera is implemented which essentially chases the player and attempts to focus the player in the center of the camera. To make the game feel more smooth, instead of snapping straight to the player, Spherical interpolation is used to give the camera a “falling behind” feeling while keeping up with the player.
## Description of how the shaders work ##
---

## Using Images

You can use images/gif by adding them to a folder in your repo:

<p align="center">
  <img src="Gifs/Q1-1.gif"  width="300" >
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

## Code Snippets 

You can include a code snippet here, but make sure to explain it! 
Do not just copy all your code, only explain the important parts.

```c#
public class firstPersonController : MonoBehaviour
{
    //This function run once when Unity is in Play
     void Start ()
    {
      standMotion();
    }
}
```




