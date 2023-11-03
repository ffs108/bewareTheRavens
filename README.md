___________________________________________________________
Assignment 2 - ISTA 425
___________________________________________________________
+	Daniel Shevelev	> Procedural Bg / Raven hitbox			          
+	Koan Zhang > Sweep & Prune
+	Lin Shao > Wizard hitbox						          
+	Francisco Figueroa > Infinite Parallax				          
___________________________________________________________


In this assignment we have implemented the following:

*Infinite Parallax Scrolling 
	The wizard may move as much left or right as they wish (barring colliding
enemy or fireball) with the background smoothingly accompanying the player endlessly.
This is done through shifting the layers in relative to the player movement along the
positive and negative x player axis whenever players reaches a positional threshold.
This is done seamlessly with the background being generated outside the view of the player.

*Wizard Dies when hit
	The wizard dies when its hitbox (AABB) of itself and that of another game object,
be that a raven, fireball, or even raven corpse interact within bounds with one another.
When these parameters are met the wizard begins its death animation along with its death
sound effect. Inputs to a dead wizard are also halted and are not processed by the game.

*Wizard Jump Physics
	The wizard is able to leap in the air in a realistic(as far as wizarding videogames
go) fashion and fall back to the ground at a constant rate of speed. The animation for the
leap mechanic is also functional with the wizard responding to spacebar input to begin
both the acent in game and animation, before peaking and falling back down due to gravity.

*Sweep and prune algorithm
	Implemented and it checks each and every fireball, raven and the player collision.
If a collision occurs then it is printed to the console. The same collision is printed
numerous times because the sweep and prune algorithm works on every frame and while the
objects are moving through space and colliding they are likely to overlap for many frames
so that one collision keeps getting printed. Updating the master list seems a little 
inefficient and would run into problems if we had more objects but given only ravens and 
fireballs the algorithm runs fine. Uses AABB Intersection Tests to see if a collision
actually happened.

___________________________________________________________

*Fireballs kill Ravens
	In the script, the first thing I do is the code that ravens dies after being
hit by a fireball. I did this using a combination of the collider component and code
on the raven prefab and the fireball prefab.Then I used the gravity calculation formula to
let ravens fall to the ground according to a parabolic curve after death, and then
disappear. The codes that implement these functions are line 8, line 9, line 12 to 15,
line 32, and line 38 to 42. Finally, I added an empty GameObject to the scene called
"DestroyRaven" and made it the child of the Player Camera object. Then combined with
the code from line 24 to line 27 in the script, the effect of ravens being destroyed
after flying out of the screen is realized.

*Procedural Background was developed
	Additional sky, mist and forest layers were created. The sky now has a sunset, light
and night mode. The mist has a fog, poisonous and scary red mode, the tree has a flipped x
axis mode and a regular mode. The start of the game these modes are randomized, and the
randomization occurs when the player has traveled a x amount of distance regardless of
direction. The x variable is able to change in the inspector. The bigger the x the more
the player has to move for a new procedurally generated background.
