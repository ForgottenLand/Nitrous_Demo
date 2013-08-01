/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//advantages you get from using Serialized Objects
//opportunity to:
//-use custom inspector windows
//-modify scripts of prefabs or revert specific values by right clicking
//-undo and redo
//-control common data types (only pass in a value, automatically use the right data type) 

//custom bezierMove inspector
//inspect bezierMove.cs and extend from Editor class
[CustomEditor(typeof(bezierMove))]
public class bezierMoveEditor : Editor
{
    //define Serialized Objects we want to use/control from bezierMove.cs
    //this will be our serialized reference to the inspected script
    private SerializedObject m_Object;

    //serialized message list property of bezierMove
    SerializedProperty m_List;
    //local MessageOptions list
    private List<MessageOptions> mList;
    //inspector scrollbar x/y position, modified by mouse input
    private Vector2 scrollPosMessage;
    //whether message settings menu should be visible
    private bool showMessageSetup = false;


    //called whenever this inspector window is loaded 
    public void OnEnable()
    {
        //we create a reference to our script object by passing in the target
        m_Object = new SerializedObject(target);
        //get reference to message list
        m_List = m_Object.FindProperty("_messages");
    }


    //returns BezierPathManager component of variable "pathContainer" for later use
    //if no Path Container is set, this will return null, so we need to check that below
    private BezierPathManager GetPathTransform()
    {
        //get pathContainer from serialized property and return its BezierPathManager component
        return m_Object.FindProperty("pathContainer").objectReferenceValue as BezierPathManager;
    }


    //return message list from bezierMove.cs
    //we create a new list and fill it with all given values for serialization
    private List<MessageOptions> GetMessageList()
    {
        //create new list for returning
        List<MessageOptions> msgOpt = new List<MessageOptions>();

        //loop through whole list
        for (int i = 0; i < m_List.arraySize; i++)
        {
            //get serialized MessageOption slot
            SerializedProperty slot = m_List.GetArrayElementAtIndex(i);
            //create new MessageOption to store values
            MessageOptions opt = new MessageOptions();
            //store values of serialized MessageOption properties
            SerializedProperty msgList = slot.FindPropertyRelative("message");
            SerializedProperty typeList = slot.FindPropertyRelative("type");
            SerializedProperty objList = slot.FindPropertyRelative("obj");
            SerializedProperty textList = slot.FindPropertyRelative("text");
            SerializedProperty numList = slot.FindPropertyRelative("num");
            SerializedProperty vect2List = slot.FindPropertyRelative("vect2");
            SerializedProperty vect3List = slot.FindPropertyRelative("vect3");
            SerializedProperty callAtPos = slot.FindPropertyRelative("pos");

            //loop through values of this MessageOption
            for (int j = 0; j < msgList.arraySize; j++)
            {
                //fill created MessageOption with serialized values
                opt.message.Add(msgList.GetArrayElementAtIndex(j).stringValue);

                //abort if opened at runtime without specification of message settings before
                if (typeList.arraySize != msgList.arraySize)
                {
                    Debug.LogWarning("Resized Message List at runtime! Please open Message Settings in the editor first.");
                    return null;
                }

                opt.type.Add((MessageOptions.ValueType)typeList.GetArrayElementAtIndex(j).enumValueIndex);
                opt.obj.Add(objList.GetArrayElementAtIndex(j).objectReferenceValue);
                opt.text.Add(textList.GetArrayElementAtIndex(j).stringValue);
                opt.num.Add(numList.GetArrayElementAtIndex(j).floatValue);
                opt.vect2.Add(vect2List.GetArrayElementAtIndex(j).vector2Value);
                opt.vect3.Add(vect3List.GetArrayElementAtIndex(j).vector3Value);
                opt.pos = callAtPos.floatValue;
            }
            //add filled MessageOption to the list
            msgOpt.Add(opt);
        }
        //return final list of messages
        return msgOpt;
    }


    //add new MessageOption at the end of waypoint's message list 
    private void AddMessageOption(int index)
    {
        //get MessageOption list at index and extend every value by one
        SerializedProperty slot = m_List.GetArrayElementAtIndex(index);
        slot.FindPropertyRelative("message").arraySize++;
        slot.FindPropertyRelative("type").arraySize++;
        slot.FindPropertyRelative("obj").arraySize++;
        slot.FindPropertyRelative("text").arraySize++;
        slot.FindPropertyRelative("num").arraySize++;
        slot.FindPropertyRelative("vect2").arraySize++;
        slot.FindPropertyRelative("vect3").arraySize++;
    }


    //remove MessageOption at the end of waypoint's message list 
    private void RemoveMessageOption(int index)
    {
        //get MessageOption list of index and decrease every value by one
        SerializedProperty slot = m_List.GetArrayElementAtIndex(index);
        slot.FindPropertyRelative("message").arraySize--;
        slot.FindPropertyRelative("type").arraySize--;
        slot.FindPropertyRelative("obj").arraySize--;
        slot.FindPropertyRelative("text").arraySize--;
        slot.FindPropertyRelative("num").arraySize--;
        slot.FindPropertyRelative("vect2").arraySize--;
        slot.FindPropertyRelative("vect3").arraySize--;
    }


    //methods to set a MessageOption value of a given waypoint
    //these are overwritten for every available data type value
    //parameters are list index, property, data type slot, type value
    private void SetMessageOption(int index, string field, int slot, string value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative(field).GetArrayElementAtIndex(slot).stringValue = value; }

    private void SetMessageOption(int index, string field, int slot, MessageOptions.ValueType value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative(field).GetArrayElementAtIndex(slot).enumValueIndex = (int)value; }

    private void SetMessageOption(int index, string field, int slot, Object value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative(field).GetArrayElementAtIndex(slot).objectReferenceValue = value; }

    private void SetMessageOption(int index, string field, int slot, float value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative(field).GetArrayElementAtIndex(slot).floatValue = value; }

    private void SetMessageOption(int index, string field, int slot, Vector2 value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative(field).GetArrayElementAtIndex(slot).vector2Value = value; }

    private void SetMessageOption(int index, string field, int slot, Vector3 value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative(field).GetArrayElementAtIndex(slot).vector3Value = value; }

    //method to set the message path progress position - where to call the message
    private void SetMessageKeyPos(int index, float value)
    { m_List.GetArrayElementAtIndex(index).FindPropertyRelative("pos").floatValue = value; }


    //called whenever the inspector gui gets rendered
    public override void OnInspectorGUI()
    {
        //this pulls the relative variables from unity runtime and stores them in the object
        //always call this first
        m_Object.Update();

        //show default bezierMove.cs public variables in inspector
        DrawDefaultInspector();

        //get Path Manager component by calling method GetWaypointArray()
        var path = GetPathTransform();

        EditorGUILayout.Space();
        //make the default styles used by EditorGUI look like controls
        EditorGUIUtility.LookLikeControls();

        //draw bold delay settings label
        GUILayout.Label("Settings:", EditorStyles.boldLabel);

        //check whether a Bezier Path Manager component is set, if not display a label
        if (path == null)
        {
            GUILayout.Label("No path set.");
        }
        else
        {
            //draw message options
            MessageSettings();
        }

        //we push our modified variables back to our serialized object
        //always call this after all fields and buttons
        m_Object.ApplyModifiedProperties();
    }


    void MessageSettings()
    {
        //path is set and boolean for displaying message settings is true
        //(button below was clicked)
        if (showMessageSetup)
        {
            //draw button for hiding message settings
            if (GUILayout.Button("Hide Message Settings"))
                showMessageSetup = false;

            EditorGUILayout.BeginHorizontal();

            //delete all message values and disable settings
            if (GUILayout.Button("Deactivate all Messages"))
            {
                //display custom dialog and wait for user input to delete all message values
                if (EditorUtility.DisplayDialog("Are you sure?",
                "This will delete all message slots to reduce memory usage.",
                "Continue",
                "Cancel"))
                {
                    //user clicked "Yes", reset array size to zero and hide settings
                    m_List.arraySize = 0;
                    showMessageSetup = false;
                    return;
                }
            }

            EditorGUILayout.EndHorizontal();

            //button to insert a new message after the last slot
            if (GUILayout.Button("+ Add new Message +"))
            {
                //increase the message count by one
                m_List.arraySize++;
                //get the message list with the new slot
                mList = GetMessageList();
                //insert a new callable message,
                //only if the message has no slot already
                if (mList[mList.Count - 1].message.Count == 0)
                {
                    AddMessageOption(mList.Count - 1);
                }
                //when adding a new message, the values from the latest message
                //gets copied to the new one. We want a fresh message instead,
                //so we remove all other slots until one slot remains
                for (int i = mList[mList.Count - 1].message.Count - 1; i > 0; i--)
                {
                    RemoveMessageOption(mList.Count - 1);
                }
            }

            //begin a scrolling view inside GUI, pass in Vector2 scroll position 
            scrollPosMessage = EditorGUILayout.BeginScrollView(scrollPosMessage, GUILayout.Height(245));
            //get modifiable list of MessageProperties
            mList = GetMessageList();

            //loop through list
            for (int i = 0; i < mList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                //draw label with waypoint index, display total count of messages for this waypoint
                //increased by one (so it does not start at zero)
                EditorGUILayout.HelpBox("Message " + (i + 1) + " - Message Count: " + mList[i].message.Count, MessageType.None);

                //add new message to this waypoint
                if (GUILayout.Button("+"))
                    AddMessageOption(i);

                //remove last message from this waypoint
                if (GUILayout.Button("-"))
                {
                    RemoveMessageOption(i);
                    //delete the complete message if no slots are left
                    if (mList[i].message.Count == 1)
                        m_List.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();

                //display text box with path progress input field (call position),
                //clamped between 0 (start) and 1 (end)
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Call at path position in %:", MessageType.None);
                float callAtPosField = EditorGUILayout.FloatField(mList[i].pos, GUILayout.Width(60));
                callAtPosField = Mathf.Clamp01(callAtPosField);
                EditorGUILayout.EndHorizontal();

                //loop through messages
                for (int j = 0; j < mList[i].message.Count; j++)
                {
                    //display text box and message name input field
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Method:", MessageType.None);
                    string messageField = EditorGUILayout.TextField(mList[i].message[j]);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();

                    //display enum for selectable data types
                    //declare variables for each data type for storing their input value
                    MessageOptions.ValueType typeField = (MessageOptions.ValueType)EditorGUILayout.EnumPopup(mList[i].type[j]);
                    Object objField; string textField; float numField; Vector2 vect2Field; Vector3 vect3Field;

                    //draw corresponding data type field for selected enum type
                    //store input in the values above. if the field has changed,
                    //set the corresponding type value for the current MessageOption 
                    switch (typeField)
                    {
                        case MessageOptions.ValueType.None:
                            break;
                        case MessageOptions.ValueType.Object:
                            objField = EditorGUILayout.ObjectField(mList[i].obj[j], typeof(Object), true);
                            if (GUI.changed) SetMessageOption(i, "obj", j, objField);
                            break;
                        case MessageOptions.ValueType.Text:
                            textField = EditorGUILayout.TextField(mList[i].text[j]);
                            if (GUI.changed) SetMessageOption(i, "text", j, textField);
                            break;
                        case MessageOptions.ValueType.Numeric:
                            numField = EditorGUILayout.FloatField(mList[i].num[j]);
                            if (GUI.changed) SetMessageOption(i, "num", j, numField);
                            break;
                        case MessageOptions.ValueType.Vector2:
                            vect2Field = EditorGUILayout.Vector2Field("", mList[i].vect2[j]);
                            if (GUI.changed) SetMessageOption(i, "vect2", j, vect2Field);
                            break;
                        case MessageOptions.ValueType.Vector3:
                            vect3Field = EditorGUILayout.Vector3Field("", mList[i].vect3[j]);
                            if (GUI.changed) SetMessageOption(i, "vect3", j, vect3Field);
                            break;
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator();

                    //regardless of the data type,
                    //set the call position, message name and enum type of this MessageOption
                    if (GUI.changed)
                    {
                        SetMessageKeyPos(i, callAtPosField);
                        SetMessageOption(i, "message", j, messageField);
                        SetMessageOption(i, "type", j, typeField);
                    }
                }
            }
            //ends the scrollview defined above
            EditorGUILayout.EndScrollView();
        }
        else
        {
            //draw button to toggle showDelaySetup
            if (GUILayout.Button("Show Message Settings"))
            {
                showMessageSetup = true;
            }
        }
    }


    //if this path is selected, display small info boxes
    //-above all waypoint positions
    //-at the approximate message position on the path
    void OnSceneGUI()
    {
        //get Bezier Path Manager component
        var path = GetPathTransform();

        //do not execute further code if we have no path defined
        //or delay settings are not visible
        if (path == null) return;

        mList = GetMessageList();

        //begin GUI block
        Handles.BeginGUI();
        //loop through waypoint array
        for (int i = 0; i < path.points.Count; i++)
        {
            //translate waypoint vector3 position in world space into a position on the screen
            var guiPoint = HandleUtility.WorldToGUIPoint(path.points[i].wp.transform.position);
            //create rectangle with that positions and do some offset
            var rect = new Rect(guiPoint.x - 50.0f, guiPoint.y - 40, 100, 20);
            //draw box at rect position with current waypoint name
            GUI.Box(rect, "Waypoint: " + (i + 1));
        }
        Handles.EndGUI(); //end GUI block

        //don't draw approximate message positions at runtime,
        //since the exact positions get displayed via bezierMove.cs
        if (EditorApplication.isPlaying)
            return;

        //begin GUI block
        Handles.BeginGUI();
        //loop through message list
        for (int i = 0; i < mList.Count; i++)
        {
            //translate message vector3 position in world space into a position on the screen
            var guiPoint = HandleUtility.WorldToGUIPoint(path.GetPositionOnPath(mList[i].pos));
            //create rectangle with that positions and do some offset
            var rect = new Rect(guiPoint.x - 40, guiPoint.y + 18, 80, 36);
            //draw box at rect position with current message index
            GUI.Box(rect, "Message: " + (i + 1) + "\n(approx.)");
        }
        Handles.EndGUI(); //end GUI block
    }

}