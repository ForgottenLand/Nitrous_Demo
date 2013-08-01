/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;


public class WaypointManager : MonoBehaviour
{

    //these dictionaries store all path names and for each path its manager component with waypoint positions
    //enemies will receive their specific path component
    public static readonly Dictionary<string, PathManager> Paths = new Dictionary<string, PathManager>();
    public static readonly Dictionary<string, BezierPathManager> BezierPaths = new Dictionary<string, BezierPathManager>();

    //execute this before any other Start() or Update() function
    //since we need the data of all paths before we call them
    void Awake()
    {
        //for each child/path of this gameobject, add path to dictionary
        foreach (Transform path in transform)
        {
            AddPath(path.gameObject);
        }

        //http://www.holoville.com/hotween/documentation.html#hotweeninit
        //initialize HOTween immediately instead than having it being
        //automatically initialized when the first Tweener is created.
        HOTween.Init(true, true, true);
        //HOTween's OverwriteManager works in the background, and automatically
        //checks if a running tween needs to be overwritten by a newly started one.
        HOTween.EnableOverwriteManager();
        //If true, shows the eventual paths in use by PlugVector3Path while playing
        //inside Unity's Editor (and if the Editor's Gizmos button is on). 
        HOTween.showPathGizmos = true;
        //exclude less important warnings, e.g. when changing tween parameters
        HOTween.warningLevel = WarningLevel.Important;
    }


    //this adds a path to the dictionary above, so our walker objects can access them
    public static void AddPath(GameObject path)
    {
        //check if path contains the name "Clone" (path was instantiated)
        if (path.name.Contains("Clone"))
        {
            //replace/remove "(Clone)" with an empty character
            path.name = path.name.Replace("(Clone)", "");
        }

        //check if path dictionary already contains this path name
        if (Paths.ContainsKey(path.name) || BezierPaths.ContainsKey(path.name))
        {
            //debug warning and abort
            Debug.LogWarning("Called AddPath() but Scene already contains Path " + path.name + ".");
            return;
        }

        //try to get PathManager component
        PathManager pathMan = path.GetComponentInChildren<PathManager>();

        if (pathMan)
        {
            //add path name and its manager reference to above dictionary to allow indirect access
            Paths.Add(path.name, pathMan);
        }
        else
        {
            //do the same with a bezier path, if it was attached to the path instead
            BezierPathManager bezierPathMan = path.GetComponentInChildren<BezierPathManager>();

            if (bezierPathMan)
                BezierPaths.Add(path.name, bezierPathMan);
            else
            {
                //if no path script could be found, debug warning and abort
                Debug.LogWarning("Called AddPath() but Transform " + path.name + " has no Path Component attached.");
                return;
            }
        }
    }


    //static dictionaries keep their variables between scenes,
    //we don't want that to happen - clear the path dictionary
    //whenever this object gets destroyed (e.g. on scene change)
    void OnDestroy()
    {
        Paths.Clear();
        BezierPaths.Clear();
    }
}

//send message option list for movement scripts
[System.Serializable]
public class MessageOptions
{
    //message name
    public List<string> message = new List<string>();
    //value type enum
    public List<ValueType> type = new List<ValueType>();
    //object value
    public List<UnityEngine.Object> obj = new List<UnityEngine.Object>();
    //text value
    public List<string> text = new List<string>();
    //numerical value
    public List<float> num = new List<float>();
    //vector2 value
    public List<Vector2> vect2 = new List<Vector2>();
    //vector3 value
    public List<Vector3> vect3 = new List<Vector3>();
    //available data types for message parameters
    public enum ValueType
    {
        None,
        Object,
        Text,
        Numeric,
        Vector2,
        Vector3
    }
    //position on the path (only used for bezier paths)
    public float pos;
}
