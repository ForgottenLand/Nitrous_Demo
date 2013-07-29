EZ Replay Manager
#################

Version: 1.41
Author: SoftRare - www.softrare.eu
Genre: Unity3d Extension
Description: This tool allows you to record ANY game for direct replay. Record any game scene and replay it backwards or with double speed! Very funny when replaying a scene with rigidbodies or reviewing game deciding moments. It can even record camera movement.
Legal information: You may only use and change the code if you purchased the whole package in a legal way.
#################

What is this?
_____________
The EZ Replay Manager is an extension for Unity3d which allows you to record whole games or only parts of it, and replay it back while being able to precisely configure what possibilities for configuration the user has. 

What are the features?
______________________
+ Record any game object (also cameras, rigidbodies, characters,...).
+ Specify easily what should be recorded.
+ No programming skills necessary
+ Configure whether the user should be able to interact (starting/ending the recording/replay).
+ Change replay speed, pause, stop, rewind
+ Full heavily commented source code
+ Fully extendable

How to use it?
______________
Usage is extremely easy: If you have an easy scene without game object instantiation you don't even have to be able to program. 

Non-programmers:
++++++++++++++++
1. Just copy the folder "EZReplayManager" from the downloaded package into your project (or export package->import package) and drag the prefab "EZReplayManager" into your scene. 
2. Click on it in the scene hierarchy. It has a list called "Game Objects To Record". Count the game objects in your scene you would like to record. 
3. Put the number you counted in the field named "Size" just below the "Game Objects To Record" list. The list grows.. but it’s still empty, you only expanded the size. 
4. Drag your crucial game objects from the scene into the elements of the list. Leave all other EZReplayManager-parameters as they were. 
(Continue reading at point 5 below..)

If you feel comfortable programming, the steps are equally easy:
++++++++++++++++
1. Just copy the folder "EZReplayManager" from the downloaded package into your project and drag the prefab "EZReplayManager" into your scene.
2. Make a new script, call it "ObjectToRecord.cs" (for example). 
3. In the start-function write the following:
	void Start() {
		EZReplayManager.markObject4recording(gameObject);
	}
4. Drag this script on all prefabs and gameObjects which you would like to have recorded.

5. Start your game.
6. Hit "Start recording" and play your game.
7. Hit "Stop recording" to stop without viewing the replay, and "Replay" to do both instantly.

See a demo at http://www.softrare.eu/ez-replay-manager.html

Advanced Programming How-To
___________________________

EZReplayManager-API details
+++++++++++++++++++++++++++
For comfortable recording and replaying any game, it is important to understand the necessary structure. Fortunately, this is easy as pie:
While your game is running, you can basically mark any game object (also cameras, skies, GUI elements, and of course objects crucial for the game) to be recorded by calling "EZReplayManager.markObject4recording(YOUR_GAME_OBJECT_HERE);". Nothing will happen so far. The only thing you say to the Unity engine and the EZ Replay Manager framework is this: "When I hit record, observe this game object!". So you haven't hit that yet. 

To actually record, call "EZReplayManager.record()". The framework will record the game objects you ordered it to record, for every frame, for every object. At this moment you can still say "EZReplayManager.markObject4recording(YOUR_GAME_OBJECT_HERE);". So this is useful to use on game objects which were created dynamically by calling "GameObject.Instantiate(..)". Give them the above mentioned "ObjectToRecord.cs".

So all this was pretty easy. Now it comes to replaying your game. Keep in mind that between switching the recorder actions such as record, play, and rewind, you should call "EZReplayManager.stop()". This resets some important variables and avoids warnings being displayed. So now call stop, and then 

EZReplayManager.play(0);

The parameter 0 says that the replaying speed should be the recording speed, so no speeding up or slowing down. You can do that by giving a value above 0 (standard between 1 and 5) for speeding up, and below 0 (standard between -1 and -5) for slowing down.

If you like to add some additional parameters that's fine too :) Here is the declaration of the function play:

public static void play(int speed, bool playImmediately, bool backwards, bool exitOnFinished) {
	...
}

So we discussed the first speed parameter. The next ("playImmediately") tells the replay manager to start the replay immediatly when in replay-mode without waiting for some user input. "backwards" results in the first replay being playing in the opposite chronological order.. starting with the end of your recorded game back to the start. While in replay-mode, "exitOnFinished" exits the replay once the replay has been finished.

So, "EZReplayManager.play(-1,false,true,true)" i.e. results in a replay a little slower than normal, the replay won't start immediately, but played backwards by default and the replay mode exits after the replay is finished.

Important variables
+++++++++++++++++++
Dealing with these variables is not absolutely necessary if you are satisfied with the standard behavior of the replay manager. But in some situations it can be helpful. Some of them are also available via the "EZReplayManager" prefab in the Unity3d GUI and can be changed without having to code at all.
1.
	public const bool showErrors = true; //default: true
	public const bool showWarnings = true; //default: true
	public const bool showHints = false; //default: false
	
Declare here what kind of messages you would like to receive in the console. Set them in the code.

2.
	public bool useRecordingGUI = true; //default: true
	public bool useReplayGUI = true; //default: true	
	
Use this to set whether you want to display the graphical user interfaces that the user playing your game can record, stop, replay, rewind, speed up and speed down all by himself without you having to implement the replay managers API.

3.
	public float recordingInterval = 0.05f;  //default: 0.05f 
	private const int maxSpeedSliderValue = 5; //default: 5
	private const int minSpeedSliderValue = -5; //default: -5
	
These values have immediate influence on the recorders performance. "recordingInterval" is the rate by which the manager records your game. 0.05 means 20 frames per second (fps). So the manager will record 20 states per second per object. If this is too high of an update rate (meaning your value is too low actually) (it can happen if you record many complex objects simultaneously, with a small value) you will notice it by the replay going slower than your recording (your game) actually went. If the value is too low of an update rate(meaning you should actually lower it) you will notice this by your replay being played choppy. Find the right value for your game by adjusting it. Try 0.04 (25 fps) if your replay is choppy and lower it until you are satisfied.

The other values determine how much the game can be speeded up ("maxSpeedSliderValue") and slowed down ("minSpeedSliderValue") in replay mode.

4.
	public List<GameObject> gameObjectsToRecord = new List<GameObject>();
	public List<string> componentsAndScriptsToKeepAtReplay = new List<string>(); 
	
The first list is meant to help newbie’s who are not able to program yet. You can drag and drop your game objects in design time here to have them marked for recording in the real game.

The second can be of importance if you have important scripts on your game objects which you don't want them to lose even if the game objects are currently in replay mode. Normally all scripts except for some vital are being deleted in this mode, but you can write the name of the scripts and components here which you want to keep while a game object is in replay mode. For excluding "CharacterController" (not recommended) call 

	componentsAndScriptsToKeepAtReplay.add("CharacterController");

Don't worry, your original game objects won't be touched at all, the replay manager will not delete any scripts on them, just on their replay-clone counterparts. 

5.

	public const int ACTION_RECORD = 0;
	public const int ACTION_PLAY = 1;
	public const int ACTION_PAUSED = 2;
	public const int ACTION_STOPPED = 3;
	public const int MODE_LIVE = 0;
	public const int MODE_REPLAY = 1; 
	
	private static int currentMode = MODE_LIVE;
	private static int currentAction = ACTION_STOPPED;	

currentMode and currentAction are important values. currentMode can have two values:  MODE_LIVE and MODE_REPLAY. MODE_LIVE is the mode where your game is going on, whereas MODE_REPLAY describes the mode when the replay is being played back.

currentAction describes the pretty self explanatory states in which the recorder can be in.. his values can be ACTION_RECORD, ACTION_PLAY, ACTION_PAUSED and ACTION_STOPPED.

Do not give those two variables other values than explained here. It will very likely result in unwanted behavior.

To ask the values from outside use these two functions:

	if (EZReplayManager.getCurrentAction() == EZReplayManager.ACTION_PLAY)
		//...your code here...
	}	
	
	if (EZReplayManager.getCurrentMode() == EZReplayManager.MODE_LIVE)
		//...your code here...
	}	
	
6.

	private static int recorderPosition = 0;
	
This variable also has big impact on the component. It describes at what recorder position the component currently is. It can be requested via this function from the outside of the class:

	EZReplayManager.getCurrentPosition();
	
Extending the EZ Replay Manager
+++++++++++++++++++++++++++++++
The extension is very generic, should work on all games, but of course it is also easily extendable: Until now only the information for position, rotation and whether particles are emitted are being saved. So if your game object does something fancy, like .. I don't know, i.e. being destroyed in a long lasting explosion, which slowly evaporates it. You should extend the file "SavedState.cs" with the code of the value for evaporation. 

First declare 2 instance variables 

	private bool evaporates;
	private float evaporationValue;

The first (constructor) determines whether the evaporation takes place, the second, at what stage the evaporation is "right now". Let’s say responsible for the evaporation is a script called "Evaporator". Add in the constructor:

	if(go.GetComponent<Evaporator>()) {
		this.evaporates = go.GetComponent<Evaporator>().evaporates;
		this.evaporationValue = go.GetComponent<Evaporator>().evaporationValue;
	}

So now the evaporation value is saved for every frame for every game object which has a component "Evaporator". In these examples it has two values, which can be received and set: "evaporates" says whether the evaporation has been started, "evaporationValue" returns the value (for example 0f for no evaporation, and 1f for total destruction). To be able to see this in replay add the following line to your code (in some Start()-function or better in Unity3d GUI on the EZReplayManager prefab).

	componentsAndScriptsToKeepAtReplay.add("Evaporator");

Now back to "SavedState.cs": In the function "synchronizeProperties" we determine what happens when the game is being played back to the user. Add the following:

	if (this.evaporates) 
		go.GetComponent<Evaporator>().evaporationValue = this.evaporationValue;
	else if ( go.GetComponent<Evaporator>() ) 
		go.GetComponent<Evaporator>().evaporates = false;
		
So if in this particular frame the evaporation was set to exist, we set the value like the one which was saved. If not we switch the evaporation off.
