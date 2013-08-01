/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class BezierPathManager : MonoBehaviour
{
    //final list of path point positions
    public Vector3[] waypoints;
    //distance between points on the curved path
    public float spacing = 1f;
    //smoothing factor
    public int interpolations = 0;

    public Color color1 = new Color(1, 0, 1, 0.5f); //cube color
    public Color color2 = new Color(1, 235 / 255f, 4 / 255f, 0.5f); //sphere color
    public Color color3 = new Color(1, 235 / 255f, 4 / 255f, 0.5f); //curved lines color

    //waypoint gizmo radius
    private float radius = .4f;
    //waypointStart/-End box gizmo size
    private Vector3 size = new Vector3(.7f, .7f, .7f);
    //if set, prefab to replace the waypoint gameobjects 
    public GameObject waypointPrefab;

    //class to store waypoint gameobjects, positions and bezier points
    //(located at the bottom of this script)
    public List<BezierPoint> points;
	//resulting path points, this gets converted to the final waypoint array
	private List<Vector3> pointList = new List<Vector3>();
    public bool showGizmos = false;


    void OnDrawGizmos()
    {
        //differ between children waypoint types:
        //waypointStart or waypointEnd, draw small cube gizmo using color1
        //standard waypoint, draw small sphere using color2
        foreach (Transform child in transform)
        {
            if (child.name == "WaypointStart" || child.name == "WaypointEnd")
            {
                //assign chosen color1 to current gizmo color
                Gizmos.color = color1;
                //draw wire cube at waypoint position
                Gizmos.DrawWireCube(child.position, size);
            }
            else
            {
                //assign chosen color2 to current gizmo color
                Gizmos.color = color2;
                //draw wire sphere at waypoint position
                Gizmos.DrawWireSphere(child.position, radius);
            }
        }

        //draw curved lines
        DrawCurved();
    }

	
	void OnDrawGizmosSelected()
	{
        if (!showGizmos) return;
        
        //delete pointList because in the editor,
        //all points are calculated new in every OnDrawGizmos call
        pointList.Clear();
        CalculatePath();

        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++)
        {
            //draw a sphere and a line to the sphere for the first bezier point
            Gizmos.DrawSphere(points[i].bp[0], 0.5f / 2.5f);
            Gizmos.DrawLine(points[i].wp.position, points[i].bp[0]);

            //first and last point only have one bezier point,
            //all other ones have an additional bezier point
            if (i > 0 && i < points.Count - 1)
            {
                Gizmos.DrawSphere(points[i].bp[1], 0.5f / 2.5f);
                Gizmos.DrawLine(points[i].wp.position, points[i].bp[1]);
            }
        }
        
        // only draw pointlist if gameobject with PathCreator is selected
        // if path is editable yet, draw the resulting current points from pointList			
        Gizmos.color = color3;
        foreach (Vector3 point in waypoints)
        {
            Gizmos.DrawSphere(point, 0.08f);
        }
	}	


    void CalculatePath()
    {
        for (int i = 0; i < points.Count; i++)
		{	
            if (i < points.Count - 1)
            {
                //determine if this pathPoint points to pathPoints[i + 1] left or right direction.
                //then choose an "end" bezier point from there
                float endSide = Vector3.Cross(GetDirection(points[i + 1].wp), points[i + 1].wp.position - points[i].wp.position).y;
                //determine if pathPoints[i + 1] points to this pathPoints left or right direction.
                float startSide = Vector3.Cross(GetDirection(points[i].wp), points[i + 1].wp.position - points[i].wp.position).y;

                //construct path
                pointList.AddRange(GetWayPoints(
                    points[i].wp.position, //start point
                    startSide > 0 ? points[i].bp[1] : points[i].bp[0], //bezier point for start point
                    endSide > 0 ? points[i + 1].bp[0] : points[i + 1].bp[1], //bezier point for end point
                    points[i + 1].wp.position, //end point
                    spacing < 0.5f ? 0.5f : spacing)); //distance between points 
            }
		}

        //if the path actually contains 2 or more than 2 waypoints it forms a path
        if (points.Count >= 2)
        {
            //add another point at the last waypoint position if not done already
            //(sometimes the spacing between points doesn't allow a point exactly
            //at the last waypoint, so we manually insert one here)
            if (pointList[pointList.Count - 1] != points[points.Count - 1].wp.position)
                pointList.Add(points[points.Count - 1].wp.position);
        
            //smooth out path with the given interpolation factor
            if (interpolations > 0)
                pointList = SmoothCurve(pointList, interpolations);
        }

        //convert path list to an array
        waypoints = pointList.ToArray();
    }
	

	// function to calculate the line, consisting of points
	List<Vector3> GetWayPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float pointDistance)
	{
		//p0 = start path point, p3 = end path point
		//p1 = bezier point for start point, p2 = bezier point for end point

        // get the x, y and z values of the points for easy calculating
        float x0 = p0.x;
        float x1 = p1.x;
        float x2 = p2.x;
        float x3 = p3.x;
        float y0 = p0.z;
        float y1 = p1.z;
        float y2 = p2.z;
        float y3 = p3.z;
        float z0 = p0.y;
        float z1 = p1.y;
        float z2 = p2.y;
        float z3 = p3.y; 							
		
		//temporary point list that gets returned in this function
		List<Vector3> pList = new List<Vector3>();
		
		//determine the point from which to go on, first path point or last created point
		Vector3 lastPoint = pointList.Count == 0 ? p0 : pointList[pointList.Count - 1];			
		if (pointList.Count == 0) pList.Add(p0); // add first path point to the list				
			
		//bezier formula by Pierre Bezier
		for (int n = 0; n <= 10; n++)
		{
			float i = (float) n / 10; // cannot increment i as a float
			float rest = 1 - i;

            float x = x0 * (rest * rest * rest) + x1 * i * 3 * (rest * rest) + x2 * 3 * (i * i) * rest + x3 * (i * i * i);
            float y = y0 * (rest * rest * rest) + y1 * i * 3 * (rest * rest) + y2 * 3 * (i * i) * rest + y3 * (i * i * i);
            float z = z0 * (rest * rest * rest) + z1 * i * 3 * (rest * rest) + z2 * 3 * (i * i) * rest + z3 * (i * i * i);
            
            Vector3 nextBezierPoint = new Vector3(x, z, y); 
			float distToNextBezierPoint = Vector3.Distance(lastPoint, nextBezierPoint);			
			
			//draw points in direction of the next bezier point, with given distance			
			if (distToNextBezierPoint >= pointDistance)
			{
				//only draw something if next bezier point is NOT closer than minimum pointDistance
				Vector3 newPoint = new Vector3(0, 0, 0);
				
				//if next bezier point is far away, draw more points in its direction
				for (int j = 1; j <= Mathf.Floor(distToNextBezierPoint / pointDistance); j++)				
				{					
					newPoint = Vector3.MoveTowards(lastPoint, nextBezierPoint, j * pointDistance); 
					pList.Add(newPoint);			
				}
				
				//next checkpoint for determine the distance in the next loop
				if (newPoint != Vector3.zero)
					lastPoint = newPoint;
			}					
		}
		
		return pList;
	}


    //get the direction of the position handle as a vector3
    public Vector3 GetDirection(Transform waypoint)
    {
        //circle function	
        float cos = Mathf.Cos(waypoint.rotation.eulerAngles.y * Mathf.Deg2Rad);
        float sin = Mathf.Sin(waypoint.rotation.eulerAngles.y * Mathf.Deg2Rad);


        return new Vector3(sin, 0, cos);
    }


    //calculate the Vector3 position on the path due to position
	public Vector3 GetPositionOnPath(float position)
    {
        position = waypoints.Length * position;

        if (position + 1 > waypoints.Length) 
            return waypoints[waypoints.Length-1];

        return Vector3.Lerp( // lerping the object along the path
                waypoints[Mathf.FloorToInt(position)], // start Point
                waypoints[Mathf.FloorToInt(position) + 1], // end Point
                Time.time); // lerp value for between	
	}


    //credits to "Codetastic", taken from here:
    //http://answers.unity3d.com/questions/392606/line-drawing-how-can-i-interpolate-between-points.html
    //-----------------------------------------
    public List<Vector3> SmoothCurve(List<Vector3> pathToCurve, int interpolations)
    {
        List<Vector3> tempPoints;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (interpolations < 1)
            interpolations = 1;

        pointsLength = pathToCurve.Count;

        curvedLength = (pointsLength * Mathf.RoundToInt(interpolations)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            tempPoints = new List<Vector3>(pathToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    tempPoints[i] = (1 - t) * tempPoints[i] + t * tempPoints[i + 1];
                }
            }

            curvedPoints.Add(tempPoints[0]);
        }

        return curvedPoints;
    }


    //helper array for curved paths, includes control points for waypoint array
    Vector3[] gizmoPoints;

    //taken and modified from
    //http://code.google.com/p/hotween/source/browse/trunk/Holoville/HOTween/Core/Path.cs
    //draws the full path
    void DrawCurved()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        gizmoPoints = new Vector3[waypoints.Length + 2];

        for (int i = 0; i < waypoints.Length; i++)
        {
            gizmoPoints[i + 1] = waypoints[i];
        }

        gizmoPoints[0] = gizmoPoints[1];
        gizmoPoints[gizmoPoints.Length - 1] = gizmoPoints[gizmoPoints.Length - 2];

        Gizmos.color = color3;
        Vector3[] drawPs;
        Vector3 currPt;

        // Store draw points.
        int subdivisions = gizmoPoints.Length * 10;
        drawPs = new Vector3[subdivisions + 1];
        for (int i = 0; i <= subdivisions; ++i)
        {
            float pm = i / (float)subdivisions;
            currPt = GetPoint(pm);
            drawPs[i] = currPt;
        }

        // Draw path.
        Vector3 prevPt = drawPs[0];
        for (int i = 1; i < drawPs.Length; ++i)
        {
            currPt = drawPs[i];
            Gizmos.DrawLine(currPt, prevPt);
            prevPt = currPt;
        }
    }


    //taken from
    //http://code.google.com/p/hotween/source/browse/trunk/Holoville/HOTween/Core/Path.cs
    // Gets the point on the curve at the given percentage (0 to 1).
    // t: The percentage (0 to 1) at which to get the point.
    private Vector3 GetPoint(float t)
    {
        int numSections = gizmoPoints.Length - 3;
        int tSec = (int)Math.Floor(t * numSections);
        int currPt = numSections - 1;

        if (currPt > tSec)
            currPt = tSec;
        else if (currPt < 0)
            currPt = 0;

        float u = t * numSections - currPt;

        Vector3 a = gizmoPoints[currPt];
        Vector3 b = gizmoPoints[currPt + 1];
        Vector3 c = gizmoPoints[currPt + 2];
        Vector3 d = gizmoPoints[currPt + 3];

        return .5f * (
                       (-a + 3f * b - 3f * c + d) * (u * u * u)
                       + (2f * a - 5f * b + 4f * c - d) * (u * u)
                       + (-a + c) * u
                       + 2f * b
                   );
    }
}


//class for waypoint properties
[System.Serializable]
public class BezierPoint
{
    //waypoint transform
    public Transform wp = null;
    //bezier points stored as Vector3 array
    //[0] = first point, [1] = second point
    public Vector3[] bp = new Vector3[2];
    //calculated angle for the bezier points
    public float bpOffsetAngle = 0f;
}
