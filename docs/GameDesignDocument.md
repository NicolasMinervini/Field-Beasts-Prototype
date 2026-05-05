# Changelog

## Between Testing and Final Physical Prototype

- **Pig’s “Mud Pit” ability now always sets a target’s movement to 1, instead of reducing it by a flat 3 units.**  
  - The idea behind this ability was for the pig to slow the approach of enemy units, ideally while they are within range of longer ranged ally’s attacks. During playtesting, however, we found out very quickly that the Pig could also completely freeze slower units, making them completely useless. This made the action unfun for both players.  
- **Shepherd Dog’s ability to deal 2 extra damage per adjacent ally was reduced to a flat 2 extra damage boost regardless of how many allies are nearby.**  
  - I was concerned while making this ability that it might be too encouraging of “camping” or balling up all your units on a perch and never moving. Not only was I totally right, but it was *also* granting the Shepherd Dog way too much damage with each attack, making it super overpowered. This change kept the vision of the Shepherd Dog working best with a group without being too overwhelming.   
- **Horse now only gains 2 extra movement points after attacking instead of 3**  
  - The Horse is designed to be extremely nimble, excelling at charging in and then backing up to a safe distance to avoid retaliation. I thought it was fine, but my playtesters believed it was too pesky, so I lowered the movement boost.  
- **Rabbit’s nibble now deals 1d4 damage instead of a flat 2 damage**  
  - The flat damage amount was intended to be a reliable alternative to the typical dice rolls for damage, but it instead just felt underwhelming. The fact that this was only one of two attacks which did this made it feel like an out of place addition.  
- **Cow’s headbutt now deals 1d4 damage instead of a flat 2 damage**  
  - This was the same issue as the Rabbit. It just felt boring and unnecessary.  
- **Removed Gobert**  
  - Only ever meant to be a sample card, he had overstayed his welcome.

## Between Physical Prototype and Digital Demo

- **Removed hexagonal grid-based movement, everything now uses exact distances**  
  - My original vision was for the game to be this way, but I had to use a grid in the physical prototype for convenience. Sure, the hexagonal grid didn’t cause any problems, and would probably have been far easier to develop, but I like making myself miserable.  
  - Because of this change, I also have to make general alterations to everything to properly fit this new movement format.

# Game Design Document

# 1.0 \- Introduction

## 1.1 \- Document Scope

	Hello\! This document is designed for you \- yes YOU, Mr. Toegel \- to get an organized overview of my plans for this game demo/prototype as, chances are, I will certainly not get anywhere close to implementing my full vision of what this game could become. Because of that, this document will also be designed for myself, or any unbelievably bored yet capable people who happen upon this project, to continue working towards better realizing this game in the future.

## 1.2 \- Elevator Pitch

	This is a strategy game which uses the idea of unique critters with recognizable abilities from Pokemon and puts it into the more tactical combat format similar to that of Dungeons and Dragons or small scale war games. Its gameplay will focus on trying to contain a high level of tactical depth in the simplest system possible.

# 2.0 \- Game Overview

## 2.1 \- Game Concept

	Each player is given control of a small set of chosen animals, each having unique abilities. The objective of the game is for players to defeat each other’s animals until only one player’s animals remain standing. I want each type of animal to have unique abilities which encourage grouping them together to produce interesting or unexpected strategies. The mark of this game succeeding is if the player feels as if they have used their animals’ abilities to truly outsmart their opponent, or to have at least come up with an odd or absurd enough interaction between them to be entertained.

## 2.2 \- Audience

	Strategy games are hard to get into. It takes a certain level of commitment (or autism) to get involved with the best of the genre. Long story short, I had a lot of fun playing Baldur’s Gate 3, I don’t have the free time to play Dungeons and Dragons, and I am scared of the unnecessarily complex rules of Warhammer. Thus, this game is designed with the layman in mind: those who want a strategy game in a simple and quickly digestible package. The game should be simple enough for the average teenager to fully understand and quick enough that a single match may be started and completed within 30 minutes. Streamlined rules and gameplay should attract this casual audience well.

## 2.3 \- Genre

	This is a pure turn-based strategy game. No buts\! When the fighting starts, you must wait your turn\!

## 2.4 \- Setting

	The game takes place on a farm, except the farmer is nowhere to be found, the animals are freed, and they have all split into different factions fighting for authority over the farmland.

## 2.5 \- World Structure

	The arenas in which the game will take place are freely traversible, without any grids limiting movement. Each animal has a speed value to determine the maximum distance they can move within a single turn. They may also jump up and down platforms to gain advantages on enemies at lower heights, but jumping up elevations incurs extra movement costs depending on how high they are jumping.

## 2.6 \- The Player

	The player views the game from above, taking a role akin to a commander of their animals by panning around the field to get a full view of where everything is. Clicking on units provides all the information about its current health, remaining movement distance this turn, and buttons to prepare each of the animal’s available actions.

## 2.7 \- Core Loop

The main game loop takes place in repeating turns: in a single turn, the active player may separately move all of their creatures and choose one action each of them has available before ending their turn and letting the next player in the turn order do the same.

## 2.8 \- Aesthetics

	I imagine this game having a visual design vaguely inspired by that old animated rendition of George Orwell’s “Animal Farm”, with a cartoonish and expressive design counteracted by dull colors and negative emotional tones. The actions the player takes should be very snappy, movement should be quick and attacking should give immediate and satisfying feedback. The animals might feel miserable, but they shouldn’t be dragging their heels\!

# 3.0 \- Gameplay

## 3.1 \- Objectives

	The main objective of the game is to be the last player standing. This objective is achieved through defeating all of the opponent’s animals.

## 3.2 \- Progression

	While the core plan of this game is a simple and short 2 player game with no overall progression, if this project were to continue I would implement progression within a singleplayer campaign. This campaign would be in the form of many matches increasing in complexity and difficulty while following a general plot. Each fight would have context as to why it is occurring, and completion will allow the player to strengthen their team of animals and unlock progress in a story about conquering the whole farm.

## 3.3 \- Play Flow

	The general flow of the game should be designed to keep matches short. It should start well balanced, should “snowball” in favor of the player who begins to gain the greater advantage. It shouldn’t be impossible for the losing player to resist and possibly win, but the game should avoid giving benefits to them that would draw out the match or undo progress made by the other player to succeed.

## 3.4 \- Difficulty

	Difficulty, if implemented, should mainly be in the form of match complexity. “Easier” modes should give less animals to each player to use, while “harder” modes give the players more animals to manage.

# 4.0 \- Mechanics

## 4.1 \- Rules

	During a player’s turn, they can have each of their animals move and do an action. When they choose to end their turn, control is given over to the next player in the turn order. Actions are given linear increases to their range when targeting areas at lower heights than them (i.e. 1 extra meter of range against targets 1 meter below, 2 extra meters against 2 meters below, etc), and jumping to higher ground is given an extra movement cost equal to that of the height difference. Jumping to lower areas and attacking higher targets do not have any extra costs or range bonuses.  
	A possible additional rule is that each player could be given a set of objects, barriers, or platforms to place around their half of the game arena before the fight starts, so that they may try and build an environment which benefits their intended strategy.

## 4.2 \- Game Universe

	Managing the health and movement of animals is done by the computer. When a unit moves, the player should only see the calculated length of the path, the amount of movement distance the animal has this turn, and whether or not the selected target position is within accessible range.  
	For a potential singleplayer mode, computer-controlled opponent animals will each have their own sets of logic to determine what actions are used, which are executed when it is their turn. Each animal will have a priority order of actions it will try to take, with each action having a function to check if it should be used or if the computer-controlled animal should try the next action in line.

## 4.3 \- Physics

	This game does not need any special physics. The environments are static and the characters are not influenced by gravity and other forces.

## 4.5 \- Character Movement

The player’s camera view can be panned around the arena however they wish. The animal characters being controlled are fixed to a navmesh generated to fit the match arena. When the player moves an animal to a valid position, the animal will move along the shortest possible path generated by AI navigation to that target position.

## 4.6 \- Player Interaction

	The player mainly interacts with the game world through commanding their animals. Specifically, each animal has the ability to walk or jump to chosen positions, and has a list of actions (such as attacks and healing/buffing abilities) it can use on other animals, any position in range, or itself.

### 4.6.1 \- Game Menus

	The main game interface should include a hotbar of all the selected animal’s actions and movement options, a meter displaying the amount of their movement speed that hasn’t yet been used yet, and the animal’s maximum and current health. There should also be a list displaying icons of all the player’s controllable animals, which can be clicked to automatically select and center on that animal. The top of the screen should display the turn order of every team/player, highlighting the player whose turn it currently is. The hotbar of available actions should be buttons that can be clicked to prepare the actions so the player can select them before clicking a chosen target.	

# 5.0 \- Graphics and Audio

## 5.1 \- Visual System

	The game should follow a cartoon-like design, with the animals designed with exaggerated proportions to be more expressive, but this should be counteracted by dull and dreary colors. The general objective is to create a tone that isn’t serious, but also doesn’t try to be positive or happy. While the game should not be averse to showing violence, blood and gore should be as minimal as possible.

### 5.1.1 \- Landscape

	The game’s environment should be a farm, obstacles should be wooden fences, hay bales, and other farming equipment. Everything should be muted, worn down, and rusty. The condition of the land should seem dire and in disarray.

## 5.2 \- Interface

The interface should look improvised and haphazardly. Animal icons should be on wooden panels with colors peeling off, meters should look like old rusted analog gauges, and text fonts should be rough and imperfect.

## 5.3 \- Audio Look and Feel

	Music should be present at times, but quiet to give room for ambience. The audio and music of the game should try to serve the narrative that the environment is decrepit and wartorn: ambience should be dominated by blowing winds and occasional squeaking of loose pieces of metal, the voices of the animals (I mean animal sounds, not actual dialogue) should sound harsh or aggressive, and the minimal music which plays should give a domineering and militaristic feeling.

# 6.0 \- Story/Narrative

	As stated earlier, the primary objective for a simple two-player game would largely omit any overarching story or plot. But if I were to continue this project to my ideal end of a singleplayer story-based mode, I would focus the plot around the player leading a militaristic, authoritarian animal faction seizing control against other opposing groups. Maybe it could be a sort of “bad guys win” type of story, or maybe the story progresses in a way meant to speak some warning about pointless fighting.

# 7.0 \- Characters

	The characters should each be different farm animals, with stats and abilities relating to the type of animal they are. The rooster character should be aggressive and brave, but also rather fragile. The bull should be slow, have a lot of health, and have abilities designed for it to focus on challenging a specific opponent. The shepherd dog should be strongest when working with a group. The cartoonish design of the game could help allow players to suspend their disbelief with unrealistic or even slightly goofy abilities.   
Importantly, I want the pigs to be the leaders and commanders of the other animals, as another bit of inspiration from “Animal Farm”. In combat, the pig should be designed to focus on applying buffs to allies and debuffs to enemies from a safe distance, rather than dealing actual damage.

# 8.0 \- Game World / Locations / Levels

	All the game's levels and environments should take place within the farm. Possible combat arenas the gameplay takes place in should be themed as various parts of the farm such as a cramped maze within a cornfield, a grassy meadow with large hay bales spread about, the indoors of a barn with second story platforms and traversable wooden beams up above, or a muddy animal pen with broken fences and troughs to take cover behind.