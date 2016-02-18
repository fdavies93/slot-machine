Goldsmiths, University of London, 2015/16.
MSc CGE + MA CGAD
Intro to Programming, Assignment #2. 

“LEGO SLOT MACHINE”

Team: 
Frank Davies – Lead Programmer
Emanuel Francis – Artist
Yuka Okabe – Artist
Federico Soncini – Director of Content



DESIGN AND GAMEPLAY
The idea of a horizontal slot machine was inspired by a minigame inside Super Mario Bros.3.

![slot_pic_inspiration1](https://cloud.githubusercontent.com/assets/14871171/13149978/8f542416-d65b-11e5-9d66-8326d28a37b4.png)

The idea being that the reels would divide a picture in 3 parts and the victory is achieved when the three match and recompose the image. Link to game inspiration in https://www.youtube.com/watch?v=V9dreY0rUXY at (4:12).

On Yuka’s suggestion we decided to make it Lego Themed, so we selected eight characters that could be split into three horizontal reels. The slot machine was also created in 3D Lego style, in an arcade shape. 

We decided that there would be a single lever, that would start the spinning reels and then stop the single reels. 
We created an in-game store that would allow the player to use nudges to move the reels by one slot. The player gets three nudges at each spin, but each nudge detracts from the victory sum.
There is also a priority among characters. With the Lego Darth Vader a player can win more. 
A strong part of the design is the 8-bit music, which is specifically designed to reflect that mid 80’s Japanese style gaming experience.
On the right side of screen, we created a ministore. The player has different options to maximize the chances of victory or to increase the sum. We maintained an essential and symbolic design for the mini store. 
The fast forward button doesn’t cost any coins but speeds up the reels, making victories more difficult and gives the player a bonus in case of jackpot.
The slow reel has a cost of 6 coins and decreases the spinning velocity, making it easier to reach a jackpot.
The “crazy” reel, on the other hand, randomises reel speeds, it has a cost of [x] coins and could either be an advantage or a disadvantage for the player, depending on whether the outcome of the spinning reel is faster or slower.
The “golden reel” has a cost of [x] and enables the player to win by matching only two reels. 
Finally the “3 rows” button, which has a cost of [x] coins, will allow the player to win using by matching a figure across all the three visible columns. 
The options in the mini-store are all mutual exclusive and are valid for only a single spin. To provide players with a degree of guidance, when one hovers over a button the cost/effect mechanics are explained and displayed on the row above the slot machine reels, which contains minimal written instructions.
The left column displays the total amount coins, the bets (which can be increased or decreased with + and – buttons), and the nudges. Nudges help move the reel by one slot, the cost is coins. The minimum bet is 1 coin.


![slot1](https://cloud.githubusercontent.com/assets/14871171/13149600/ee6f1df4-d659-11e5-9288-6f6329003c25.png)


IMPLEMENTATION

Frank built a basic program for the slot machine in Unity, brought in the art assets created by the artists theam (reels + machine), had changes made to the art based on technical requirements 
The majority of the C# scripts are included in the pullLever.cs file, where all the variables and UI elements are initialized.
The sprites (KARATE, DARTHVADER, STORMTROOPER, NINJA, BATMAN, COWBOY, PIRATE, ROBIN [TYPES/]) are included in an enum container named resultTypes; other enum contain stateTypes (such as READY, PAUSE, SPINNING, etc…) and modifierTypes (such as SLOWREELS, FASTREELS, etc…)

During the implementation some difficulties were experienced in calculating the angles based on a combination of unity's functionality and misalignment of hinge joints, which delayed the project by some time.
Working with the design lead, Frank implemented the system for taking the order of parts on each reel and translating it into a prize value, also nudges and game state variables, implemented various modifiers and bonuses (speed up, slow down, etc.), implemented bets and variable reward winnings, combined in further art assets in group session, coded and designed UI and shop interface, finalised UI and integrated with game state, brought in art assets and finalised with bug testing, fixes and fine tuning.

ARTWORK

The team of Artists, Emanuel and Yuka, worked closely together in creating the artwork for the slot machine.
They designed, modelled and textured the slot machine to look like it was completely built from Lego. Making sure its construction followed the rules of Lego in terms of the sections having the correct proportions and clipping together from the correct sides. The artists also made sure that any slanting angles where made from steps of Lego rather than a straight line and in some case where there is a smooth slant, making sure that the pieces for this exist and that this machine could actually be replicated using real Lego.

The team had to make sure the cylindrical Lego clips were positioned correctly and in the right proportion; it was quite time consuming but in the end result i can be seen that the panels, especially with the lighting hitting off, create a convincing Lego effect.
The artists made sure to leave sufficient space on the slot-machine front for the programmers to utilize the UI elements and scores, along with a realistic cavity proportions for the slot-machine spinning wheels to sit.
The texture had also to follow the correct proportions of yellow and keeping with the strong vivid colours that Lego pieces use. A normal map was used to project the logo detail which sits on every Lego cylindrical clip, (however bearing in mind this might have Copyright issues and can be easily removed or replaced for all clips by making a single edit to the normal map). There is actually a lot more detail, in the slot-machine that does not make it into the camera frame such as arrows formed from protruding Lego and coin deposit near the bottom.
We feel the modelling more than sufficiently serves its purpose within the game and it succeeds in being immediately recognizable as having been built from Lego.

The artists also calculated the proper size of the reel that would suit to the textures we would use. Three textures had to be made for each reel: unlike independent items in traditional slot machine formats (like cherries, BAR, bananas, etc...) the split image style needed extremely precise UV adjustment of each part (head, body, legs).

Particular attention was paid to the sound design. The team felt that in order to convey the aesthetics of an old fashioned, lego style game an 8 bit sound was going to be the best choice. There are currently three different tunes – an intro/outro song, plus a victory sound bite. They sources were found in the free sound library http://ymck.net/dolwnload/magical8bitplug/index.html, then 2 of the existing tunes were arranged into 8 bit game music and exported by Logic X.


