using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
[CustomEditor(typeof(MapCreator))]
public class MapCreatorInspector : Editor
{
    public MapCreator mapCreator
    {
        get
        {
            /// target == MapCreator;
            return (MapCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        if (GUILayout.Button("Save"))
        {
            mapCreator.Save();
        }
        if (GUILayout.Button("Load"))
        {
            mapCreator.Load();
        }
        EditorGUILayout.Space(20);
        GUILayout.Label("Create Area");
        mapCreator.CreateArea.x = (int)EditorGUILayout.Slider("Area X", mapCreator.CreateArea.x, 0, mapCreator.width);
        mapCreator.CreateArea.y = (int)EditorGUILayout.Slider("Area Y", mapCreator.CreateArea.y, 0, mapCreator.depth);
        mapCreator.CreateArea.width = (int)EditorGUILayout.Slider("Area Width", mapCreator.CreateArea.width, 1, mapCreator.width - mapCreator.CreateArea.x + 1);
        mapCreator.CreateArea.height = (int)EditorGUILayout.Slider("Area Height", mapCreator.CreateArea.height, 1, mapCreator.depth - mapCreator.CreateArea.y + 1);
        EditorGUILayout.Space(20);


        Event e = Event.current;
        if (e.isKey)
        {
            if (Event.current.keyCode == KeyCode.UpArrow && Event.current.type == EventType.KeyDown)
            {
                mapCreator.pos.y++;
                if (mapCreator.pos.y >= mapCreator.depth)
                {
                    mapCreator.pos.y = mapCreator.depth;
                }
                mapCreator.UpdateTileCursor();
            }
            if (Event.current.keyCode == KeyCode.LeftArrow && Event.current.type == EventType.KeyDown)
            {
                mapCreator.pos.x--;
                if (mapCreator.pos.x <= 0)
                {
                    mapCreator.pos.x = 0;
                }
                mapCreator.UpdateTileCursor();
            }
            if (Event.current.keyCode == KeyCode.RightArrow && Event.current.type == EventType.KeyDown)
            {
                mapCreator.pos.x++;
                if (mapCreator.pos.x >= mapCreator.width)
                {
                    mapCreator.pos.x = mapCreator.width;
                }
                mapCreator.UpdateTileCursor();
            }
            if (Event.current.keyCode == KeyCode.DownArrow && Event.current.type == EventType.KeyDown)
            {
                mapCreator.pos.y--;
                if (mapCreator.pos.y <= 0)
                {
                    mapCreator.pos.y = 0;
                }
                mapCreator.UpdateTileCursor();
            }
            if (Event.current.keyCode == KeyCode.Z && Event.current.type == EventType.KeyDown)
            {
                mapCreator.BuildUp();
                mapCreator.UpdateTileCursor();
            }
            if (Event.current.keyCode == KeyCode.X && Event.current.type == EventType.KeyDown)
            {
                mapCreator.BreakDown();
                mapCreator.UpdateTileCursor();
            }
        }




        GUILayout.Label("TileCursor Move Button");
        if (GUILayout.Button("ก่"))
        {
            mapCreator.pos.y++;
            if (mapCreator.pos.y >= mapCreator.depth)
            {
                mapCreator.pos.y = mapCreator.depth;
            }
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("ก็"))
        {
            mapCreator.pos.x--;
            if (mapCreator.pos.x <= 0)
            {
                mapCreator.pos.x = 0;
            }
        }
        if (GUILayout.Button("กๆ"))
        {
            mapCreator.pos.x++;
            if (mapCreator.pos.x >= mapCreator.width)
            {
                mapCreator.pos.x = mapCreator.width;
            }
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("ก้"))
        {
            mapCreator.pos.y--;
            if (mapCreator.pos.y <= 0)
            {
                mapCreator.pos.y = 0;
            }
        }
        EditorGUILayout.Space(20);

        GUILayout.Label("Single Tile Menu");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("BuildUp"))
        {

            mapCreator.BuildUp();
        }
        if (GUILayout.Button("BreakDown"))
        {
            mapCreator.BreakDown();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        GUILayout.Label("Area Tile Menu");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("BuildUpArea"))
        {

            mapCreator.BuildUpArea();
        }
        if (GUILayout.Button("BreakDownArea"))
        {
            mapCreator.BreakDownArea();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);


        GUILayout.Label("Random Area Tile Menu");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("BuildUpRandomArea"))
        {

            mapCreator.BuildUpRandomArea();
        }
        if (GUILayout.Button("BreakDownRandomArea"))
        {
            mapCreator.BreakDownRandomArea();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);



        GUILayout.Label("Clear All Tile");
        if (GUILayout.Button("Clear"))
        {
            mapCreator.Clear();
        }




        if (GUI.changed)
        {
            mapCreator.UpdateTileCursor();
        }
    }
}
