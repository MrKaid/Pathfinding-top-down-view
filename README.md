# Pathfinding-top-down-view
An implementation of an A* Search pathfinding algorithm for a top down view object orientated video game


*************THIS IMPLEMENTATION WAS MADE FOR UNITY, HOWEVER, ANY OBJECT BASED GAME ENGINE SUCH AS UNREAL ENGINE SHOULD WORK FINE WITH SOME CHANHES TO PROPRIETARY UNITY CODE*********


SUGGESTIONS FOR PORTING INTO YOUR PROJECT:

-Go through and make note of all Unity proprietary code
-Observe where the CHASER object is mentioned; this is the object that uses AI algorithm and finds the best path to the target(in this case the player)
                              - the target can be set in line 32 of chaserFollowBehavior.cs
-Use line 39 of chaserFollowBehavior.cs to help find if your grid is properly set and if your nodes work as barriers
