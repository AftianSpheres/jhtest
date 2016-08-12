using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

[Serializable]
[CustomEditor(typeof(RoomController))]
public class RoomControllerEditor : Editor
{
    RoomController _room;
    bool _collision = false;
    bool _entryPoints = false;
    bool _optional = false;
    bool _pitfalls = false;
    bool _priorities = false;
    bool _full = false;
    bool _fullObj = false;
    bool _shootthru = false;
    bool _shootthruObj = false;

    void OnEnable()
    {
        bool dirty = false;
        GameObject childBuffer;
        _room = (RoomController)target;
        _room.world = FindObjectOfType<WorldController>();
        RoomCollider collider = _room.gameObject.GetComponent<RoomCollider>();
        if (collider == null)
        {
            collider = _room.gameObject.AddComponent<RoomCollider>();
        }
        if (_room.collision != collider)
        {
            _room.collision = collider;
            dirty = true;
        }

        collider.hideFlags = HideFlags.HideInInspector;
        RoomPriorityMap priorityMap = _room.gameObject.GetComponent<RoomPriorityMap>();
        if (priorityMap == null)
        {
            priorityMap = _room.gameObject.AddComponent<RoomPriorityMap>();
        }
        if (_room.priorityMap != priorityMap)
        {
            _room.priorityMap = priorityMap;
            dirty = true;
        }
        priorityMap.hideFlags = HideFlags.HideInInspector;
        UnityTileMap.TileMapBehaviour tileMap = _room.gameObject.GetComponentInChildren<UnityTileMap.TileMapBehaviour>();
        if (tileMap == null)
        {
            childBuffer = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EmptyTilemap.asset"));
            childBuffer.transform.SetParent(_room.transform);
            childBuffer.transform.localPosition = new Vector3(0, 0, 200);
            tileMap = childBuffer.GetComponent<UnityTileMap.TileMapBehaviour>();
        }
        if (_room.tilemap != tileMap)
        {
            _room.tilemap = tileMap;
            dirty = true;
        }
        if (_room.transform.Find("Enemies") == null)
        {
            childBuffer = new GameObject();
            childBuffer.name = "Enemies";
            childBuffer.transform.SetParent(_room.transform);
            dirty = true;
        }
        if (_room.transform.Find("RoomObjects") == null)
        {
            childBuffer = new GameObject();
            childBuffer.name = "RoomObjects";
            childBuffer.transform.SetParent(_room.transform);
            dirty = true;
        }
        if (dirty == true)
        {
            EditorUtility.SetDirty(_room);
            EditorSceneManager.MarkSceneDirty(_room.gameObject.scene);
            dirty = false;
        }
    }

    public override void OnInspectorGUI()
    {
        // Main block

        mu_Checkpoint c = _room.gameObject.GetComponentInChildren<mu_Checkpoint>();
        if (c != null && c != _room.RoomCheckpoint)
        {
            _room.RoomCheckpoint = c;
            EditorUtility.SetDirty(_room);
        }
        SerializedObject s = new SerializedObject(_room);
        EditorGUILayout.LabelField("Room coordinates");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("X", GUILayout.MaxWidth(32));
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(32));
        EditorGUILayout.Separator();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        SerializedProperty prop;
        EditorGUILayout.Separator();
        prop = s.FindProperty("xPosition");
        EditorGUILayout.PropertyField(prop, new GUIContent(), GUILayout.MaxWidth(32));
        EditorGUILayout.Separator();
        prop = s.FindProperty("yPosition");
        EditorGUILayout.PropertyField(prop, new GUIContent(), GUILayout.MaxWidth(32));
        EditorGUILayout.Separator();
        EditorGUILayout.EndHorizontal();
        prop = s.FindProperty("Subregion");
        EditorGUILayout.PropertyField(prop, new GUIContent("Subregion", "Room's subregion type value"));
        prop = s.FindProperty("bgm");
        EditorGUILayout.PropertyField(prop, new GUIContent("BGM", "BGM to play within room"));

        Rect r = EditorGUILayout.RectField(new GUIContent("Room bounds", "Bounds of room in 2D space"), new Rect(_room.bounds.min.x, _room.bounds.min.y, _room.bounds.size.x, _room.bounds.size.y));
        prop = s.FindProperty("bounds");
        prop.boundsValue = new Bounds(new Vector3(r.center.x, r.center.y, 0), new Vector3(r.size.x, r.size.y, 5000));

        // Optional parameters

        _optional = EditorGUILayout.Foldout(_optional, "Optional parameters");
        if (_optional)
        {
            prop = s.FindProperty("BigRoomCellSize");
            EditorGUILayout.PropertyField(prop, new GUIContent("Cell size", "Size of room in 10x8 world cells. Only needed for big rooms."));
            prop = s.FindProperty("Boss");
            EditorGUILayout.PropertyField(prop, new GUIContent("Boss", "Room's associated boss container, if any"));
            prop = s.FindProperty("fx");
            EditorGUILayout.PropertyField(prop, new GUIContent("FX Layer", "Room's associated FXLayer, if any"));
            prop = s.FindProperty("replacementValue");
            EditorGUILayout.PropertyField(prop, new GUIContent("Replacement value", "Room's priority, when compared to potential ReplacementRooms - higher priority overwrites lower"));

            _entryPoints = EditorGUILayout.Foldout(_entryPoints, new GUIContent("Entry points", "Valid warp entry points within room"));
            if (_entryPoints)
            {
                prop = s.FindProperty("EntryPoints");
                for (int i = 0; i < prop.arraySize; i++)
                {
                    EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("Entry point " + i.ToString(), "Local coordinates of entry point " + i.ToString()));
                }
            }

            _pitfalls = EditorGUILayout.Foldout(_pitfalls, new GUIContent("Pitfalls", "Pitfall zones within room"));
            if (_pitfalls)
            {
                prop = s.FindProperty("pitfallZones");
                prop.arraySize = EditorGUILayout.IntField(new GUIContent("Count", "Number of pitfall zones in room"), prop.arraySize);
                for (int i = 0; i < prop.arraySize; i++)
                {
                    SerializedProperty p = prop.GetArrayElementAtIndex(i);
                    r = EditorGUILayout.RectField(new GUIContent("Pitfall " + i.ToString() + " bounds", "Worldspace bounds of pitfall " + i.ToString()), 
                        new Rect(p.boundsValue.min.x, p.boundsValue.min.y, p.boundsValue.size.x, p.boundsValue.size.y));
                    p.boundsValue = new Bounds(new Vector3(r.center.x, r.center.y, 0), new Vector3(r.size.x, r.size.y, 5000));
                }
            }  
        }

        // Collision map

        SerializedObject ss = new SerializedObject(_room.collision);
        _collision = EditorGUILayout.Foldout(_collision, new GUIContent("Collision", "Room's collision map"));

        if (_collision)
        {
            prop = ss.FindProperty("_fullCollide");
            _full = EditorGUILayout.Foldout(_full, new GUIContent("Full collision", "Full-collision zones in room"));
            if (_full) DisplayColliderZones(prop, ref r);
            prop = ss.FindProperty("_shootthru");
            _shootthru = EditorGUILayout.Foldout(_shootthru, new GUIContent("Shoot-through collision", "Shoot-through collision zones in room"));
            if (_shootthru) DisplayColliderZones(prop, ref r);
            prop = ss.FindProperty("_fullCollide_obj");
            _fullObj = EditorGUILayout.Foldout(_fullObj, new GUIContent("Full collision objects", "External colliders to incorporate as full collision"));
            if (_fullObj) DisplayColliderObjects(prop);
            prop = ss.FindProperty("_shootThru_obj");
            _shootthruObj = EditorGUILayout.Foldout(_shootthruObj, new GUIContent("Shoot-through collision objects", "External colliders to incorporate as shoot-through collision"));
            if (_shootthruObj) DisplayColliderObjects(prop);
        }
        ss.ApplyModifiedProperties();

        // Priority map

        ss = new SerializedObject(_room.priorityMap);
        _priorities = EditorGUILayout.Foldout(_priorities, new GUIContent("Priorities", "Room's priority map"));
        if (_priorities)
        {
            SerializedProperty zones = ss.FindProperty("zones");
            SerializedProperty priorities = ss.FindProperty("priorities");
            if (zones.arraySize > priorities.arraySize)
            {
                priorities.arraySize = zones.arraySize;
            }
            else if (priorities.arraySize > zones.arraySize)
            {
                zones.arraySize = priorities.arraySize;
            }
            zones.arraySize = priorities.arraySize = EditorGUILayout.IntField(new GUIContent("Count", "Number of priority zones in room"), zones.arraySize);
            for (int i = 0; i < zones.arraySize; i++)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Zone " + i.ToString());
                EditorGUILayout.PropertyField(priorities.GetArrayElementAtIndex(i), new GUIContent("Priority", "Priority value of zone"));
                SerializedProperty p = zones.GetArrayElementAtIndex(i);
                r = EditorGUILayout.RectField(new GUIContent("Bounds", "Worldspace bounds of zone"), new Rect(p.boundsValue.min.x, p.boundsValue.min.y, p.boundsValue.size.x, p.boundsValue.size.y));
                p.boundsValue = new Bounds(new Vector3(r.center.x, r.center.y, 0), new Vector3(r.size.x, r.size.y, 5000));
            }
            EditorGUILayout.Separator();
        }
        ss.ApplyModifiedProperties();

        // Auto-updates

        prop = s.FindProperty("Enemies");
        RegisteredSprite[] ecs = _room.transform.Find("Enemies").GetComponentsInChildren<RegisteredSprite>();
        prop.arraySize = ecs.Length;
        prop.ClearArray();
        for (int i = 0; i < ecs.Length; i++)
        {
            prop.InsertArrayElementAtIndex(i);
            prop.GetArrayElementAtIndex(i).objectReferenceValue = ecs[i];
        }
        prop = s.FindProperty("NonEnemyOccupants");
        ecs = _room.transform.Find("RoomObjects").GetComponentsInChildren<RegisteredSprite>();
        prop.arraySize = ecs.Length;
        prop.ClearArray();
        for (int i = 0; i < ecs.Length; i++)
        {
            prop.InsertArrayElementAtIndex(i);
            prop.GetArrayElementAtIndex(i).objectReferenceValue = ecs[i];
        }
        s.ApplyModifiedProperties();
    }

    void DisplayColliderObjects(SerializedProperty prop)
    {
        prop.arraySize = EditorGUILayout.IntField(new GUIContent("Count", "Number of objects in room"), prop.arraySize);
        for (int i = 0; i < prop.arraySize; i++)
        {
            SerializedProperty p = prop.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(p, new GUIContent("Object " + i.ToString(), "Object associated with collider" + i.ToString()));
        }
    }

    void DisplayColliderZones(SerializedProperty prop, ref Rect r)
    {
        prop.arraySize = EditorGUILayout.IntField(new GUIContent("Count", "Number of zones in room"), prop.arraySize);
        for (int i = 0; i < prop.arraySize; i++)
        {
            SerializedProperty p = prop.GetArrayElementAtIndex(i);
            r = EditorGUILayout.RectField(new GUIContent("Zone " + i.ToString() + " bounds", "Worldspace bounds of zone " + i.ToString()),
                new Rect(p.boundsValue.min.x, p.boundsValue.min.y, p.boundsValue.size.x, p.boundsValue.size.y));
            p.boundsValue = new Bounds(new Vector3(r.center.x, r.center.y, 0), new Vector3(r.size.x, r.size.y, 5000));
        }
    }
}
