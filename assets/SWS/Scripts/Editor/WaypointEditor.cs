/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//path start/finalize manager
[CustomEditor(typeof(WaypointManager))]
public class WaypointEditor : Editor
{
    private WaypointManager script;     //manager reference
    private bool placing = false;   //if we are placing new waypoints in editor
    private GameObject path; //new path gameobject
    private string pathName = "";    //new path name
    private PathManager pathMan; //Path Manager reference for editing waypoints
    private BezierPathManager bezierPathMan; //BezierPath Manager reference for editing waypoints
    private List<GameObject> wpList = new List<GameObject>();   //temporary list for editor created waypoints in a path
    private List<BezierPoint> bwpList = new List<BezierPoint>();   //temporary list of BezierPoints for created waypoints

    //scene view input
    public void OnSceneGUI()
    {
        //if left mouse button was clicked, in combination with alt key and placing is true, enable waypoint editing
        //(we clicked to start a new path)
        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.P && placing)
        {
            //create a ray to get where we clicked on terrain/ground/objects in scene view and pass in mouse position
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            //ray hit something
            if (Physics.Raycast(worldRay, out hitInfo))
            {
                //call this method when you've used an event.
                //the event's type will be set to EventType.Used,
                //causing other GUI elements to ignore it
                Event.current.Use();

                //place a waypoint at clicked point
                if (pathMan)
                    PlaceWaypoint(hitInfo.point);
                else if (bezierPathMan)
                    PlaceBezierPoint(hitInfo.point);
            }
        }
    }

    //inspector input
    public override void OnInspectorGUI()
    {
        //show default variables of script "WaypointManager"
        DrawDefaultInspector();
        //get WaypointManager.cs reference
        script = (WaypointManager)target;

        //make the default styles used by EditorGUI look like controls
        EditorGUIUtility.LookLikeControls();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        //draw path text label
        GUILayout.Label("Enter Path Name: ", EditorStyles.boldLabel, GUILayout.Height(15));
        //display text field for creating a path with that name
        pathName = EditorGUILayout.TextField(pathName, GUILayout.Height(15));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //create new waypoint path button
        if (!placing && GUILayout.Button("Start Waypoint Path", GUILayout.Height(40)))
        {
            //no path name defined, abort with short editor warning
            if (pathName == "")
            {
                Debug.LogWarning("no path name defined");
                return;
            }

            //path name already given, abort with short editor warning
            if (script.transform.FindChild(pathName) != null)
            {
                Debug.LogWarning("path name already given");
                return;
            }

            //create a new container transform which will hold all new waypoints
            path = new GameObject(pathName);
            //attach PathManager component to this new waypoint container
            pathMan = path.AddComponent<PathManager>();
            //create waypoint array instance of PathManager
            pathMan.waypoints = new Transform[0];
            //reset position and parent container gameobject to this manager gameobject
            path.transform.position = script.gameObject.transform.position;
            path.transform.parent = script.gameObject.transform;
            //we passed all prior checks, toggle waypoint placing on
            placing = true;
            //fokus sceneview for placing new waypoints
            SceneView sceneView = (SceneView)SceneView.sceneViews[0];
            sceneView.Focus();
        } //finish path button

        //create new bezier path button
        if (!placing && GUILayout.Button("Start Bezier Path", GUILayout.Height(40)))
        {
            //same as above
            if (pathName == "")
            {
                Debug.LogWarning("no path name defined");
                return;
            }

            if (script.transform.FindChild(pathName) != null)
            {
                Debug.LogWarning("path name already given");
                return;
            }

            path = new GameObject(pathName);
            //attach BezierPathManager component to this new waypoint container
            bezierPathMan = path.AddComponent<BezierPathManager>();
            bezierPathMan.showGizmos = true;
            //create waypoint list instance of BezierPathManager
            bezierPathMan.points = new List<BezierPoint>();
            //reset position and parent container gameobject to this manager gameobject
            path.transform.position = script.gameObject.transform.position;
            path.transform.parent = script.gameObject.transform;
            //we passed all prior checks, toggle waypoint placing on
            placing = true;
            //fokus sceneview for placing new waypoints
            SceneView sceneView = (SceneView)SceneView.sceneViews[0];
            sceneView.Focus();
        }

        //finish path button
        if (placing && GUILayout.Button("Finish Editing", GUILayout.Height(40)))
        {
            //return if no path was started or not enough waypoints, so wpList/bwpList is empty
            if (pathMan && wpList.Count < 2 || bezierPathMan && bwpList.Count < 2)
            {
                Debug.LogWarning("not enough waypoints placed");

                //if we have created a path already, destroy it again
                if (path)
                    DestroyImmediate(path);
            }
            else if (pathMan)
            {
                //switch name of last created waypoint to waypointEnd,
                //so we will recognize this path ended (this gets an other editor gizmo)
                wpList[wpList.Count - 1].name = "WaypointEnd";
                //do the same with first waypoint
                wpList[0].name = "WaypointStart";
            }
            else if (bezierPathMan)
            {
                //do the same with the bezier path points in case they were used
                bwpList[bwpList.Count - 1].wp.name = "WaypointEnd";
                bwpList[0].wp.name = "WaypointStart";
            }

            //toggle placing off
            placing = false;

            //clear list with temporary waypoint references,
            //we only needed this list for getting first and last waypoint easily
            wpList.Clear();
            bwpList.Clear();
            //reset path name input field
            pathName = "";
            //make the new path the active selection
            Selection.activeGameObject = path;
        }

        EditorGUILayout.Space();

        GUILayout.Label("Hint:\nPress 'Start Path' to begin a new path," +
                        "\npress 'p' on your keyboard to place\nwaypoints onto objects with colliders." +
                        "\nPress 'Finish Editing' to end your path.");
    }


    //create waypoint
    void PlaceWaypoint(Vector3 placePos)
    {
        //instantiate waypoint gameobject
        GameObject wayp = new GameObject("Waypoint");

        //with every new waypoint, our waypoints array should increase by 1
        //but arrays gets erased on resize...
        //here we use a classical rule of three:
        //first we create a new array, "wpCache", this will be our waypoint cache array,
        //then we copy our PathManager waypoints array into that newly created array,
        //now we can resize the waypoints array, because we cached its values.
        //with the array resized, we recopy old information from wpCache into it.
        //finally, the last (new) entry should be a reference to the newly created waypoint gO
        //result: a resized array with old information and one new entry.
        Transform[] wpCache = new Transform[pathMan.waypoints.Length];
        System.Array.Copy(pathMan.waypoints, wpCache, pathMan.waypoints.Length);

        pathMan.waypoints = new Transform[pathMan.waypoints.Length + 1];
        System.Array.Copy(wpCache, pathMan.waypoints, wpCache.Length);
        pathMan.waypoints[pathMan.waypoints.Length - 1] = wayp.transform;

        //this is executed on placing of the first waypoint,
        //we position our path container transform to first waypoint position,
        //so the transform (and grab/rotate/scale handles) aren't out of sight
        if (wpList.Count == 0)
            pathMan.transform.position = placePos;

        //position current waypoint at clicked position in scene view
        wayp.transform.position = placePos;
        //look up and parent it to the defined path 
        wayp.transform.parent = pathMan.transform;
        //add waypoint to temporary list
        wpList.Add(wayp);
        //rename waypoint to match the list count
        wayp.name = "Waypoint" + (wpList.Count - 1);
    }


    //create bezier waypoint
    void PlaceBezierPoint(Vector3 placePos)
    {
        //create new bezier point property class
        BezierPoint newPoint = new BezierPoint();

        //instantiate waypoint gameobject
        GameObject wayp = new GameObject("Waypoint");
        //assign waypoint to the class
        newPoint.wp = wayp.transform;

        //same as in PlaceWaypoint(),
        //but we have to restrict x, z values due to minor issues with bezier points
        if (bwpList.Count == 0)
            bezierPathMan.transform.position = new Vector3(0, placePos.y, 0);

        //overwrite bezier points for the first point,
        //because some weird stuff is going on when repositioning the path
        //once finished, the exact bezier positions will get recalculated anyways
        if (bwpList.Count == 1)
        {
            bwpList[0].bp = new[] { bwpList[0].wp.transform.position - new Vector3(2, 0, 0),
                                    bwpList[0].wp.transform.position + new Vector3(0, 0, 2) };
        }

        //position current waypoint at clicked position in scene view
        wayp.transform.position = placePos;

        //look up and parent it to the defined path 
        wayp.transform.parent = bezierPathMan.transform;
        //zero out its y-value to correctly work in 2D space
        Vector3 localPos = wayp.transform.localPosition;
        localPos.y = 0f;
        wayp.transform.localPosition = localPos;

        //create new array with bezier point handle positions for this waypoint
        newPoint.bp = new[] { placePos - new Vector3(2, 0, 0),
                              placePos + new Vector3(0, 0, 2) };

        //add waypoint to the list of waypoints
        bezierPathMan.points.Add(newPoint);
        //add waypoint to temporary list
        bwpList.Add(newPoint);
        //rename waypoint to match the list count
        wayp.name = "Waypoint" + (bwpList.Count - 1);
    }
}
