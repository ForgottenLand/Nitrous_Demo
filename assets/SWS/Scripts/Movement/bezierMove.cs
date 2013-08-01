/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;


//movement script: HOTween
[AddComponentMenu("Simple Waypoint System/bezierMove")]
public class bezierMove : MonoBehaviour
{
    //which path to call
    public BezierPathManager pathContainer;
    //should this gameobject start to walk on game launch?
    public bool onStart = false;
    //should this gameobject walk to the first waypoint or just spawn there?
    public bool moveToPath = false;
    //close path for building a loop on looptype = loop
    //public bool closePath = false;
    //should this gameobject look to its target point?
    public bool orientToPath = false;

    //lookAhead value used by orientToPath (0-1), 0 means restrict to path
    public float lookAhead = 0;
    //custom object size to add
    public float sizeToAdd = 0;
    //messages to call at a specific position
    [HideInInspector]
    public List<MessageOptions> _messages = new List<MessageOptions>();

    //we have the choice between 2 different move options:
    //time in seconds one node step will take to complete
    //or animation based on speed
    public TimeValue timeValue = TimeValue.speed;
    public enum TimeValue
    {
        time,
        speed
    }
    //time or speed value
    public float speed = 5;

    //animation easetype
    public Holoville.HOTween.EaseType easetype = Holoville.HOTween.EaseType.Linear;
    //custom easetype on HOTween.EaseType.AnimationCurve
    public AnimationCurve AnimEaseType;
    //animation looptype
    public LoopType looptype = LoopType.loop;
    //enum to choose from available looptypes
    public enum LoopType
    {
        none,
        loop,
        pingPong,
    }

    //cache all waypoint positions of requested path
    private Vector3[] waypoints;
    //current position on the path, stored as percentage (0-1)
    private float positionOnPath = -1f;

    //lock axis rotation variable
    public Axis lockAxis = Axis.None;
    //lock position variable
    public Axis lockPosition = Axis.None;

    //animation component
    [HideInInspector]
    public Animation anim;
    //animation to play during walk time
    public AnimationClip walkAnim;
    //animation to play during waiting time
    public AnimationClip idleAnim;
    //whether animations should fade in over a period of time or not
    public bool crossfade = false;


    //--HOTween animation helper variables--
    //active HOTween tween
    public Tweener tween;
    //array of modified waypoint positions for the tween
    private Vector3[] wpPos;
    //parameters for the tween
    private TweenParms tParms;
    //HOTween path plugin for curved movement
    private PlugVector3Path plugPath;


    //checks if gameobject should move on game start
    internal void Start()
    {
        //get animation component from children
        if (!anim)
            anim = gameObject.GetComponentInChildren<Animation>();

        //start moving instantly
        if (onStart)
            StartMove();
    }


    //initialize or update waypoint positions
    internal void InitWaypoints()
    {
        //recreate array used for waypoint positions
        wpPos = new Vector3[waypoints.Length];
        //fill array with original positions and add custom height
        for (int i = 0; i < wpPos.Length; i++)
        {
            wpPos[i] = waypoints[i] + new Vector3(0, sizeToAdd, 0);
        }
    }


    //can be called from an other script also to allow start delay
    public void StartMove()
    {
        //if we start the game and no path Container is set, debug a warning and return
        if (pathContainer == null)
        {
            Debug.LogWarning(gameObject.name + " has no path! Please set Path Container.");
            return;
        }

        //get Transform array with waypoint positions
        waypoints = pathContainer.waypoints;

        //cache original speed for future speed changes
        originSpeed = speed;

        //start moving
        StartCoroutine(Move());
    }


    //start moving depending on settings
    internal IEnumerator Move()
    {
        //if this object should walk to the first waypoint,
        //first start an additional tween
        if (moveToPath)
            yield return StartCoroutine(MoveToPath());
        else
        //if we should not walk to the first waypoint,
        //we set this gameobject position directly to it and launch the tween routine
        {
            //initialize waypoint positions
            InitWaypoints();
            //we also add a defined size to our object height,
            //so our gameobject could "stand" on top of the path.
            transform.position = waypoints[0] + new Vector3(0, sizeToAdd, 0);
            //directly look at the first waypoint at start
            if (orientToPath)
                transform.LookAt(wpPos[1]);
        }

        //we create the tween and start moving to the next waypoint
        CreateTween();
    }


    //move to path setting checked
    internal IEnumerator MoveToPath()
    {
        //if moveToPath equals true, we want to start movement from the current position
        //this means that our waypoint position array should start with the current position,
        //and then move to the first.
        //we fill that array with at least with 4 waypoints:
        //the first slot contains the current position, the 2nd and 3rd slot contains a waypoint
        //in the opposite direction of the last one for smoothing purposes,
        //and the last slot finally contains the first waypoint position where the path starts
        wpPos = new Vector3[7];
        wpPos[0] = transform.position;
        wpPos[1] = 2 * waypoints[0] - waypoints[1] + new Vector3(0, sizeToAdd, 0);
        wpPos[2] = 2 * waypoints[0] - waypoints[2] + new Vector3(0, sizeToAdd, 0);
        wpPos[3] = waypoints[0] + new Vector3(0, sizeToAdd, 0);

        //now we smooth out the way to the first waypoint
        List<Vector3> unsmoothedList = new List<Vector3>();
        for (int i = 0; i < 4; i++)
            unsmoothedList.Add(wpPos[i]);
        //store smoothed array
        Vector3[] smoothed = pathContainer.SmoothCurve(unsmoothedList, 1).ToArray();

        //copy smoothed positions to the tween array
        for (int i = 0; i < 4; i++)
            wpPos[i] = smoothed[i];

        //add a few other positions to the path, to avoid speed issues:
        //the object moves to the first waypoint within the time frame (speed setting) defined,
        //e.g. 18 seconds. As this speed should apply to the whole path, we add more waypoints,
        //in order to speed up the movement to the first path point
        wpPos[4] = waypoints[1] + new Vector3(0, sizeToAdd, 0);
        wpPos[5] = pathContainer.points[1].wp.position + new Vector3(0, sizeToAdd, 0);

        if (pathContainer.points.Count > 2)
            wpPos[6] = pathContainer.points[2].wp.position + new Vector3(0, sizeToAdd, 0);
        else
            wpPos[6] = wpPos[5];

        //create HOTween tweener
        CreateTween();

        //move object from current position to the first waypoint
        yield return StartCoroutine(tween.UsePartialPath(-1, 3).WaitForCompletion());
        //disable moveToPath option as we are at the first waypoint now
        moveToPath = false;

        //discard tweener because it was only used for this option
        tween.Kill();
        tween = null;

        //reinitialize original waypoint positions
        InitWaypoints();
    }


    //creates a new HOTween tween which moves us to the next waypoint
    //(defined by passed arguments)
    internal void CreateTween()
    {
        //play walk animation if set
        PlayWalk();

        //prepare HOTween's parameters, you can look them up here
        //http://www.holoville.com/hotween/documentation.html
        ////////////////////////////////////////////////////////////

        //create new HOTween plugin for curved paths
        //pass in array of Vector3 waypoint positions, relative = true
        plugPath = new PlugVector3Path(wpPos, true, PathType.Curved);

        //orients the tween target along the path
        //constrains this game object on one axis
        if (orientToPath || lockAxis != Axis.None)
            plugPath.OrientToPath(lookAhead, lockAxis);

        //lock position axis
        if (lockPosition != Axis.None)
            plugPath.LockPosition(lockPosition);

        //create a smooth path if closePath was true
        /*
        if (closePath && looptype == LoopType.loop && !moveToPath)
            plugPath.ClosePath(true);
         */

        //create TweenParms for storing HOTween's parameters
        tParms = new TweenParms();
        //sets the path plugin as tween position property
        tParms.Prop("position", plugPath);
        //additional tween parameters for partial tweens
        tParms.AutoKill(false);
        tParms.Loops(1);

        //differ between TimeValue like mentioned above at enum TimeValue
        //use speed with linear easing
        if (timeValue == TimeValue.speed)
        {
            tParms.SpeedBased();
            tParms.Ease(EaseType.Linear);
        }
        else
        {
            //use time in seconds and the chosen easetype
            if (easetype == Holoville.HOTween.EaseType.AnimationCurve)
                tParms.Ease(AnimEaseType);
            else
                tParms.Ease(easetype);
        }

        //if we're on the original tween,
        //attach methods to the tween
        if (!moveToPath)
        {
            tParms.OnUpdate(CheckPoint);
            tParms.OnComplete(OnPathComplete);
        }

        //create a new tween, move this gameobject with given arguments
        tween = HOTween.To(transform, originSpeed, tParms);

        //continue new tween with adjusted speed if it was changed before
        if (originSpeed != speed)
            ChangeSpeed(speed);
    }


    //invokes messages at the defined positions
    public void CheckPoint()
    {
        //store current position for later comparison
        float oldPosition = positionOnPath;
        //set new position (percentage value)
        positionOnPath = tween.fullElapsed / tween.fullDuration;

        //check action key collisions, trigger events for them
        for (int i = 0; i < _messages.Count; i++)
        {
            //if this gameobject has passed a message position, trigger message
            if (oldPosition < _messages[i].pos && positionOnPath >= _messages[i].pos && oldPosition != positionOnPath)
            {
                SendMessages(i);
            }
        }
    }


    //inserts a delay at the current position
    public IEnumerator Wait(float value)
    {
        //pause tween while waiting
        Pause();

        //wait at the current waypoint position
        //own implementation of a WaitForSeconds() coroutine
        float timer = Time.time + value;
        while (Time.time < timer)
            yield return null;

        //don't play the walking animation at the end of the path
        if (positionOnPath < 1 && positionOnPath != -1)
            Resume();
    }


    //execute messages at the current position
    internal void SendMessages(int key)
    {
        //loop through messages for this message
        for (int i = 0; i < _messages[key].message.Count; i++)
        {
            //if no message name was defined, abort further execution
            if (_messages[key].message[i] == "")
                continue;
            //else store MessageOption at this waypoint
            MessageOptions mess = _messages[key];
            //differ between various data types and pass in the corresponding value
            switch (mess.type[i])
            {
                case MessageOptions.ValueType.None:
                    SendMessage(mess.message[i], SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageOptions.ValueType.Object:
                    SendMessage(mess.message[i], mess.obj[i], SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageOptions.ValueType.Text:
                    SendMessage(mess.message[i], mess.text[i], SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageOptions.ValueType.Numeric:
                    SendMessage(mess.message[i], mess.num[i], SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageOptions.ValueType.Vector2:
                    SendMessage(mess.message[i], mess.vect2[i], SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageOptions.ValueType.Vector3:
                    SendMessage(mess.message[i], mess.vect3[i], SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }


    //add new message slot with default values to an existing message option
    internal MessageOptions AddMessageToOption(MessageOptions opt)
    {
        opt.message.Add("");
        opt.type.Add(MessageOptions.ValueType.None);
        opt.obj.Add(null);
        opt.text.Add(null);
        opt.num.Add(0);
        opt.vect2.Add(Vector2.zero);
        opt.vect3.Add(Vector3.zero);
        return opt;
    }


    //called by HOTween (via tween.OnComplete) at the end of the path
    internal void OnPathComplete()
    {
        ReachedEnd();
    }


    //we reached the end of the path
    internal void ReachedEnd()
    {
        //reset calculated position used for invoking messages
        positionOnPath = -1;

        //we differ between all looptypes, because each one has a specific property
        switch (looptype)
        {
            //LoopType.none means there will be no repeat,
            //so we just discard the tweener and return
            case LoopType.none:

                tween.Kill();
                tween = null;
                PlayIdle();
                break;

            //in a loop we simply start from the beginning
            case LoopType.loop:

                Stop();
                StartMove();
                break;

            //on LoopType.pingPong, we revert the tween,
            //let it run through (backwards) and then restart from the beginning (forwards)
            //so our object basically moves back and forth
            case LoopType.pingPong:

                //cache old waypoint positions
                Vector3[] cachePos = new Vector3[wpPos.Length];
                System.Array.Copy(wpPos, cachePos, wpPos.Length);
                //invert waypoint positions
                for (int i = 0; i < wpPos.Length; i++)
                {
                    wpPos[i] = cachePos[wpPos.Length - 1 - i];
                }

                //cache old messages and their positions
                MessageOptions[] cacheOpt = new MessageOptions[_messages.Count];
                _messages.CopyTo(cacheOpt);
                //invert messages
                for (int i = 0; i < _messages.Count; i++)
                {
                    _messages[i].pos = 1 - _messages[i].pos;
                    _messages[i] = cacheOpt[cacheOpt.Length - 1 - i];
                }

                //create a new reversed tween
                CreateTween();
                break;
        }
    }


    //play idle animation if set
    internal void PlayIdle()
    {
        //if an idle animation is attached and set,
        //and if crossfade is checked, fade walk animation out and fade idle in
        //else play it instantly
        if (idleAnim)
        {
            if (crossfade)
                anim.CrossFade(idleAnim.name, 0.2f);
            else
                anim.Play(idleAnim.name);
        }
    }


    //play walk animation if set
    internal void PlayWalk()
    {
        //if a walk animation is attached to this walker object and set,
        //fade idle animation out (crossfade = true) and fade walk anim in,
        //or play it instantly (crossfade = false)
        if (walkAnim)
        {
            if (crossfade)
                anim.CrossFade(walkAnim.name, 0.2f);
            else
                anim.Play(walkAnim.name);
        }
    }


    //method to change the current path of this walker object
    public void SetPath(BezierPathManager newPath)
    {
        //disable any running movement methods
        Stop();
        //set new path container
        pathContainer = newPath;
        //restart/continue movement on new path
        StartMove();
    }


    //disables any running movement methods
    public void Stop()
    {
        //exit waypoint coroutine
        StopAllCoroutines();
        //destroy current HOTween movement component
        if (tween != null)
            tween.Kill();
        plugPath = null;
        tween = null;
        //play idle animation if set
        PlayIdle();
    }


    //stops movement of our walker object and sets it back to first waypoint 
    public void Reset()
    {
        //disable any running movement methods
        Stop();
        //position this walker at our first waypoint, with our additional height
        if (pathContainer)
            transform.position = waypoints[0] + new Vector3(0, sizeToAdd, 0);
    }


    //pauses the current tween and tries to play the idle animation
    public void Pause()
    {
        //pause the tweener of this object and play idle animation
        if (tween != null)
            tween.Pause();
        PlayIdle();
    }


    //resumes the current tween and tries to play the walk animation
    public void Resume()
    {
        //resume tweener
        if(tween != null)
            tween.Play();
        //play walk animation
        PlayWalk();
    }


    //cache original speed at start
    private float originSpeed;
    //change running tween speed
    //manipulates HOTween's tween timeScale value
    public void ChangeSpeed(float value)
    {
        //calulate new timeScale value based on original speed
        float newValue;
        if (timeValue == TimeValue.speed)
            newValue = value / originSpeed;
        else
            newValue = originSpeed / value;
        //set speed, change HOTween timescale percentually to 'newValue'
        speed = value;
        tween.timeScale = newValue;
    }


    //get message option at a specific waypoint,
    //auto adds messages if desired messageID is greater than zero
    public MessageOptions GetMessageOption(int index, int messageID)
    {
        //in case message options weren't used before 
        //adds additional messages if required
        for (int i = _messages.Count; i <= index; i++)
            AddMessageToOption(new MessageOptions());

        //get message option at waypoint
        MessageOptions opt = _messages[index];

        //adds additional messages if required
        for (int i = opt.message.Count; i <= messageID; i++)
            AddMessageToOption(opt);

        //returns message option
        return opt;
    }


    //draw spheres for each message on the path for visibility
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        if (tween == null || moveToPath) 
            return;

        for (int i = 0; i < _messages.Count; i++)
        {
            Gizmos.DrawSphere(tween.GetPointOnPath(_messages[i].pos), 0.2f);
        }
    }
}