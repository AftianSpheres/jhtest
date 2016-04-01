using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class RoomEditor : MonoBehaviour
{
    public RoomController room;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //EditorGUI.DrawRect(rect, color);
	}

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(room.bounds.min.x, room.bounds.min.y, room.bounds.center.z), new Vector3(room.bounds.max.x, room.bounds.min.y, room.bounds.center.z));
        Gizmos.DrawLine(new Vector3(room.bounds.max.x, room.bounds.min.y, room.bounds.center.z), new Vector3(room.bounds.max.x, room.bounds.max.y, room.bounds.center.z));
        Gizmos.DrawLine(new Vector3(room.bounds.min.x, room.bounds.min.y, room.bounds.center.z), new Vector3(room.bounds.min.x, room.bounds.max.y, room.bounds.center.z));
        Gizmos.DrawLine(new Vector3(room.bounds.min.x, room.bounds.max.y, room.bounds.center.z), new Vector3(room.bounds.max.x, room.bounds.max.y, room.bounds.center.z));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(room.bounds.min.x, room.bounds.min.y, room.bounds.min.z), new Vector3(room.bounds.max.x, room.bounds.min.y, room.bounds.min.z));
        Gizmos.DrawLine(new Vector3(room.bounds.max.x, room.bounds.min.y, room.bounds.min.z), new Vector3(room.bounds.max.x, room.bounds.max.y, room.bounds.min.z));
        Gizmos.DrawLine(new Vector3(room.bounds.min.x, room.bounds.min.y, room.bounds.min.z), new Vector3(room.bounds.min.x, room.bounds.max.y, room.bounds.min.z));
        Gizmos.DrawLine(new Vector3(room.bounds.min.x, room.bounds.max.y, room.bounds.min.z), new Vector3(room.bounds.max.x, room.bounds.max.y, room.bounds.min.z));
    }
}
