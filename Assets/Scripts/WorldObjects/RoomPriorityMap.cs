using UnityEngine;
using System.Collections;

public class RoomPriorityMap : MonoBehaviour
{
    public Bounds[] zones;
    public int[] priorities;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < zones.Length; i++)
        {
            Gizmos.color = Color.Lerp(Color.magenta, Color.cyan, (float)priorities[i] / 8);
            Gizmos.DrawLine(new Vector3(zones[i].min.x, zones[i].min.y, 0), new Vector3(zones[i].max.x, zones[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(zones[i].max.x, zones[i].min.y, 0), new Vector3(zones[i].max.x, zones[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(zones[i].min.x, zones[i].min.y, 0), new Vector3(zones[i].min.x, zones[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(zones[i].min.x, zones[i].max.y, 0), new Vector3(zones[i].max.x, zones[i].max.y, 0));
        }
    }
#endif
}
