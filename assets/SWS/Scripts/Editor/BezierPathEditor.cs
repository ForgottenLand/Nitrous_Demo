/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//custom BezierPathManager inspector
//inspect BezierPathManager.cs and extend from Editor class
[CustomEditor(typeof(BezierPathManager))]
public class BezierPathEditor : Editor
{

    //define reference we want to use/control (BezierPathManager)
    private BezierPathManager script;
    //store old path manager gameobject position to relatively reposition waypoints
    //(otherwise the bezier points wouldn't move along with the path position)
    private Vector3 oldGizmoPos;

    //called whenever this inspector window is loaded 
    public void OnEnable()
    {
        //we create a reference to our script object by passing in the target
        script = (BezierPathManager)target;
        //initialize path position
        oldGizmoPos = script.transform.position;
    }


    //adds a waypoint when clicking on the "+" button in the inspector
    private void AddWaypointAtIndex(int index)
    {
        //register all scene objects so we can undo to this current state
        //before adding this waypoint easily
        Undo.RegisterSceneUndo("WPAdd");

        //create a new waypoint property class
        BezierPoint point = new BezierPoint();

        //create new waypoint gameobject
        GameObject wp = new GameObject("Waypoint");
        //set its position to the last one
        wp.transform.position = script.points[index].wp.position;
        //assign it to the class
        point.wp = wp.transform;
        //assign new bezier point positions
        point.bp = new[] { wp.transform.position - new Vector3(2f, 0f, 0f),
                           wp.transform.position + new Vector3(0, 0, 2f) };

        //parent it to the path gameobject
        wp.transform.parent = script.transform;
        //zero out its y-value to correctly work in 2D space
        Vector3 localPos = wp.transform.localPosition;
        localPos.y = 0f;
        wp.transform.localPosition = localPos;

        //finally, insert this new waypoint after the one clicked in the list
        script.points.Insert(index + 1, point);
    }


    //removes a waypoint when clicking on the "-" button in the inspector
    private void RemoveWaypointAtIndex(int index)
    {
        //register all scene objects so we can undo to this current state
        //before removing a waypoint easily
        Undo.RegisterSceneUndo("WPDeleted");

        //destroy the gameobject corresponding waypoint to "index",
        DestroyImmediate(script.points[index].wp.gameObject);

        //remove the point in the list
        script.points.RemoveAt(index);
    }


    public override void OnInspectorGUI()
    {
        //don't draw inspector fields if the path contains less than 2 points
        //(a path with less than 2 points really isn't a path)
        if (script.points.Count < 2)
            return;

        //create new color fields for editing waypoint gizmo colors 
        script.color1 = EditorGUILayout.ColorField("Color1", script.color1);
        script.color2 = EditorGUILayout.ColorField("Color2", script.color2);
        script.color3 = EditorGUILayout.ColorField("Color3", script.color3);

        //let iTween calculate path length of all waypoints
        float pathLength = iTween.PathLength(script.waypoints);
        //path length label, show calculated path length
        GUILayout.Label("Straight Path Length: " + pathLength);

        //checkbox Field to enable editable path properties
        EditorGUILayout.BeginHorizontal();
        script.showGizmos = EditorGUILayout.Toggle("Show Gizmos", script.showGizmos);
        EditorGUILayout.EndHorizontal();

        //check if the path gameobject position has changed since the last call
        if (!script.showGizmos && script.transform.position != oldGizmoPos)
        {
            //if not in edit mode, ignore user input
            script.transform.position = oldGizmoPos;
        }
        else
        {
            //calculate the difference between the two positions
            Vector3 difPos = script.transform.position - oldGizmoPos;
            oldGizmoPos = script.transform.position;

            if (difPos != Vector3.zero)
            {
                //enable edit mode when moving the path object
                script.showGizmos = true;
                //add the difference to the bezier points per waypoint,
                //so they are moving with the path
                for (int i = 0; i < script.points.Count; i++)
                {
                    script.points[i].bp[0] += difPos;
                    script.points[i].bp[1] += difPos;
                }
            }
        }

        //hide following settings
        if (!script.showGizmos) return;

        //Float Field to modify the distance between generated path points
        EditorGUILayout.BeginHorizontal();
        script.spacing = EditorGUILayout.FloatField("Spacing", script.spacing);
        EditorGUILayout.EndHorizontal();

        //Int Slider to modify the smoothing factor of the final path
        EditorGUILayout.BeginHorizontal();
        script.interpolations = EditorGUILayout.IntSlider("Interpolations", script.interpolations, 0, 4);
        EditorGUILayout.EndHorizontal();

        //waypoint index header
        GUILayout.Label("Waypoints: ", EditorStyles.boldLabel);

        //loop through the waypoint array
        for (int i = 0; i < script.points.Count; i++)
        {
            GUILayout.BeginHorizontal();
            //indicate each array slot with index number in front of it
            GUILayout.Label((i + 1) + ".", GUILayout.Width(20));
            //create an object field for every waypoint
            var result = EditorGUILayout.ObjectField(script.points[i].wp, typeof(Transform), true) as Transform;

            //if the object field has changed, set waypoint to new input
            if (GUI.changed)
                script.points[i].wp = result;

            //display an "Add Waypoint" button for every array row except the last one
            //on click we call AddWaypointAtIndex() to insert a new waypoint slot AFTER the selected slot
            if (i < script.points.Count - 1 && GUILayout.Button("+", GUILayout.Width(30f)))
            {
                AddWaypointAtIndex(i);
                break;
            }

            //display an "Remove Waypoint" button for every array row except the first and last one
            //on click we call RemoveWaypointAtIndex() to delete the selected waypoint slot
            if (i > 0 && i < script.points.Count - 1 && GUILayout.Button("-", GUILayout.Width(30f)))
            {
                RemoveWaypointAtIndex(i);
                break;
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        //invert direction of whole path
        if (GUILayout.Button("Invert Direction"))
        {
            //register all scene objects so we can undo to this current state
            //before inverting all waypoints easily
            Undo.RegisterSceneUndo("WPInvert");

            //Array.Copy() just gives us references and would change both arrays,
            //so we classically create a new array with length of the current one,
            //loop through them and copy all position data to the newly created array 
            BezierPoint[] waypointCache = new BezierPoint[script.points.Count];
            //cache "old value"
            for (int i = 0; i < waypointCache.Length; i++)
            {
                waypointCache[i] = script.points[i];
            }
            //reverse order based on the old list
            for (int i = 0; i < waypointCache.Length; i++)
            {
                script.points[waypointCache.Length - 1 - i] = waypointCache[i];
            }
            //rename the first and last waypoint to match the new order
            script.points[0].wp.name = "WaypointStart";
            script.points[script.points.Count - 1].wp.name = "WaypointEnd";
        }

        EditorGUILayout.Space();

        //draw object field for waypoint prefab
        script.waypointPrefab = (GameObject)EditorGUILayout.ObjectField("Waypoint Prefab", script.waypointPrefab, typeof(GameObject), true);

        //replace all waypoints with the prefab
        if (GUILayout.Button("Replace Waypoints"))
        {
            //abort if no waypoint is set
            if (script.waypointPrefab == null)
            {
                Debug.LogWarning("No Waypoint Prefab set. Aborting.");
                return;
            }

            ReplaceWaypoints();
        }

        if (GUI.changed)
            SceneView.RepaintAll();
    }


    private void ReplaceWaypoints()
    {
        //loop through waypoint list
        for (int i = 0; i < script.points.Count; i++)
        {
            //get current waypoint at index position
            Transform curWP = script.points[i].wp;
            //instantiate new waypoint at old position
            Transform newCur = ((GameObject)Instantiate(script.waypointPrefab, curWP.position, Quaternion.identity)).transform;
            //parent new waypoint to this path
            newCur.parent = script.transform;
            //replace old waypoint at index
            script.points[i].wp = newCur;

            //rename new waypoint to corresponding convention
            if (i == 0)
                newCur.name = "WaypointStart";
            else if (i == script.points.Count - 1)
                newCur.name = "WaypointEnd";
            else
                newCur.name = "Waypoint";

            //destroy old waypoint object
            DestroyImmediate(curWP.gameObject);
        }
    }


    //if this path is selected, display small info boxes above all waypoint positions
    //also display handles for the waypoints and their bezier points
    void OnSceneGUI()
    {
        //do not execute further code if we have no waypoints defined
        //(just to make sure, practically this can not occur)
        if (script.points.Count == 0) return;

        //if left mouse button was clicked, cache undo
        if (Event.current.type == EventType.mouseDown)
            Undo.RegisterSceneUndo("HandlerChange");

        //begin GUI block
        Handles.BeginGUI();
        //loop through waypoint list
        for (int i = 0; i < script.points.Count; i++)
        {
            if (!script.points[i].wp) return;
            //translate waypoint vector3 position in world space into a position on the screen
            var guiPoint = HandleUtility.WorldToGUIPoint(script.points[i].wp.position);
            //create rectangle with that positions and do some offset
            var rect = new Rect(guiPoint.x - 50.0f, guiPoint.y - 40, 100, 20);
            //draw box at position with current waypoint name
            GUI.Box(rect, "Waypoint: " + (i + 1));
        }
        Handles.EndGUI(); //end GUI block

        if (!script.showGizmos) return;

        //draw the handles for the path points
        for (int i = 0; i < script.points.Count; i++)
        {
            //get related property class
            BezierPoint point = script.points[i];
            //pathpoint position handle					
            Vector3 handle = Handles.PositionHandle(point.wp.position, point.wp.rotation);

            //assign position handle
            SetPosition(i, handle,
                //last point in the path. if 0 it will be the next point										
            i > 0 ? script.points[i - 1].wp.position : script.points[i + 1].wp.position,
                //next point in the path. if it's the final one, it will turn on the last one 
            i < script.points.Count - 1 ? script.points[i + 1].wp.position :
                script.points[i - 1].wp.position);

            //draw bezier points
            //first and last point only have one bezier point
            if (i == 0 || i == script.points.Count - 1)
            {
                PositionBezierPoints(i,
                Handles.FreeMoveHandle(
                    point.bp[0],
                    Quaternion.identity, 0.5f, Vector3.zero,
                    Handles.SphereCap),
                    point.bp[1]);
            }
            else
            {
                //draw two bezier points for all other ones
                PositionBezierPoints(i,
                Handles.FreeMoveHandle(
                    point.bp[0],
                    Quaternion.identity, 0.5f, Vector3.zero,
                    Handles.SphereCap),
                Handles.FreeMoveHandle(
                    point.bp[1],
                    Quaternion.identity, 0.5f, Vector3.zero,
                    Handles.SphereCap));
            }
        }
    }


    public void SetPosition(int index, Vector3 pos, Vector3 lastPathPoint, Vector3 nextPathPoint)
    {
        //get related property class
        BezierPoint point = script.points[index];
        //calculate difference in positions
        Vector3 difference = pos - point.wp.position;
        point.wp.position = pos;

        //move pathpoints and bezier points in relation					
        point.bp[0] += difference;
        point.bp[1] += difference;

        //if it's the first or the very last path point, rotate it by 90 degrees. 
        //so the bezier points will point in the direction of the next path point
        float additionalRotation = lastPathPoint == nextPathPoint ? 90f : 0;

        //rotation in direction of the last point
        Vector3 directionToLast = lastPathPoint - point.wp.position;
        Quaternion rotToLast = directionToLast == Vector3.zero ? Quaternion.identity :
                                Quaternion.AngleAxis(Quaternion.LookRotation(directionToLast).eulerAngles.y + additionalRotation, Vector3.up);

        //rotation in direction to the next point
        Vector3 directionToNext = nextPathPoint - point.wp.position;
        Quaternion rotToNext = directionToNext == Vector3.zero ? Quaternion.identity :
                                Quaternion.AngleAxis(Quaternion.LookRotation(directionToNext).eulerAngles.y + additionalRotation, Vector3.up);

        //rotate every path point middle between last and next path point	
        point.wp.rotation = Quaternion.Lerp(rotToLast, rotToNext, 0.5f);
        float angle = point.wp.rotation.eulerAngles.y;

        //rotation function 
        for (int i = 0; i <= 1; i++)
        {
            float bezierAngle = (point.bpOffsetAngle + angle) % 360;

            point.bp[i] = CalculatePoints(index, bezierAngle,
                                          point.bp[i], i == 0 ? -1 : 1);
        }
    }


    public void PositionBezierPoints(int index, Vector3 pos1, Vector3 pos2)
    {
        //Explanation//
        // 
        //bezierPointOffsetAngle is the offset from the mathematic radius function:
        //x = point + cos(a) * radius, y = point + sin(a) * radius
        //this would give the end point of the radius. As the user drags the point around,
        //we have to calculate the offset to this point.
        //the symmetrical point on the other side will be mirrored to the dragged point.
        //
        //bezierPointOffsetAngle is here limited from -90 to 90 degrees.
        //because LookRotation always returns an absolute result (no negative values).
        //with Vector3.Cross we can check if the point is on the right or the left hand side.
        //thus the offsetAngle can be modified to be greater than 90 degrees. the result of that is,
        //that the bezier point can be moved by a greater angle than -90 <> 90 degrees.

        //get related property class
        BezierPoint point = script.points[index];

        //angle offset from bezier points angle to current rotation
        if (pos1 != point.bp[0])
        {
            //if bezierPoint 0 was moved	
            point.bpOffsetAngle = Quaternion.Angle(Quaternion.LookRotation(point.wp.position - pos1), point.wp.rotation) - 90f;

            //if cross product is positive, point is on the other side of the vector
            if (Vector3.Cross(script.GetDirection(point.wp), (pos1 - point.wp.position)).y > 0)
            {
                //subtract angle from either 180 or -180. depends on the rotation direction
                point.bpOffsetAngle = (point.bpOffsetAngle > 0 ? 180 : -180) - point.bpOffsetAngle;
            }
        }
        else if (pos2 != point.bp[1])
        {
            //if bezierPoint 1 was moved	
            point.bpOffsetAngle = -1 * Quaternion.Angle(Quaternion.LookRotation(point.wp.position - pos2), point.wp.rotation) + 90f;

            //same function as above
            if (Vector3.Cross(script.GetDirection(point.wp), (pos2 - point.wp.position)).y < 0)
            {
                point.bpOffsetAngle = (point.bpOffsetAngle > 0 ? 180 : -180) - point.bpOffsetAngle;
            }
        }

        //assign moved handle positions as bezier point positions
        point.bp[0] = pos1;
        point.bp[1] = pos2;
    }


    //helper method for calculating the waypoint rotation in SetPosition()
    private Vector3 CalculatePoints(int index, float angle, Vector3 oldPoint, int negation)
    {
        //get related waypoint transform
        Transform wp = script.points[index].wp;

        //circle function			
        float cos = Mathf.Cos(-1 * angle * Mathf.Deg2Rad);
        float sin = Mathf.Sin(-1 * angle * Mathf.Deg2Rad);
        float distance = Vector3.Distance(wp.position, oldPoint);

        float xRot = wp.position.x + cos * distance * negation;
        float zRot = wp.position.z + sin * distance * negation;

        return new Vector3(xRot, wp.position.y, zRot);
    }
}