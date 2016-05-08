using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections;

public enum rcGameObjectSearchMode
{
    all,
    fullCollide,
    shootThru
}

public class RoomCollider : MonoBehaviour
{
    [SerializeField]
    private Bounds[] _fullCollide;
    public Bounds[] fullCollide
    {
        get
        {
            Bounds[] ret = new Bounds[_fullCollide.Length + _fullCollide_obj.Length];
            int v = 0;
            _fullCollide.CopyTo(ret, v);
            v += _fullCollide.Length;
            for (int i = 0; i < _fullCollide_obj.Length; i++)
            {
                ret[v + i] = _fullCollide_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    [SerializeField]
    private Bounds[] _shootthru;
    public Bounds[] shootThru
    {
        get
        {
            Bounds[] ret = new Bounds[_shootthru.Length + _shootThru_obj.Length];
            int v = 0;
            _shootthru.CopyTo(ret, v);
            v += _shootthru.Length;
            for (int i = 0; i < _shootThru_obj.Length; i++)
            {
                ret[v + i] = _shootThru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
 
    public Bounds[] allCollision
    {
        get
        {
            Bounds[] ret = new Bounds[_fullCollide.Length + _shootthru.Length + _fullCollide_obj.Length + _shootThru_obj.Length];
            int v = 0;
            _fullCollide.CopyTo(ret, v);
            v += _fullCollide.Length;
            _shootthru.CopyTo(ret, v);
            v += _shootthru.Length;
            for (int i = 0; i < _fullCollide_obj.Length; i++)
            {
                ret[v + i] = _fullCollide_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += _fullCollide_obj.Length;
            for (int i = 0; i < _shootThru_obj.Length; i++)
            {
                ret[v + i] = _shootThru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }

    public Bounds[] allNonObjCollision
    {
        get
        {
            Bounds[] ret = new Bounds[_fullCollide.Length + _shootthru.Length];
            int v = 0;
            _fullCollide.CopyTo(ret, v);
            v += _fullCollide.Length;
            _shootthru.CopyTo(ret, v);
            return ret;
        }
    }

    [SerializeField]
    private GameObject[] _fullCollide_obj;
    [SerializeField]
    private GameObject[] _shootThru_obj;

    public GameObject GetAssocGameObject (int index, rcGameObjectSearchMode mode)
    {
        GameObject ret;
        GameObject[] tmp;
        switch (mode)
        {
            case rcGameObjectSearchMode.all:
                tmp = new GameObject[_fullCollide_obj.Length + _shootThru_obj.Length];
                _fullCollide_obj.CopyTo(tmp, 0);
                _shootThru_obj.CopyTo(tmp, _fullCollide_obj.Length);
                index -= (_fullCollide.Length + _shootthru.Length);
                break;
            case rcGameObjectSearchMode.fullCollide:
                tmp = new GameObject[_fullCollide_obj.Length];
                _fullCollide_obj.CopyTo(tmp, 0);
                index -= (_fullCollide.Length);
                break;
            case rcGameObjectSearchMode.shootThru:
                tmp = new GameObject[_shootThru_obj.Length];
                _shootThru_obj.CopyTo(tmp, 0);
                index -= (_shootthru.Length);
                break;
            default:
                throw new Exception("Called RoomCollider.GetAssocGameObject with invalid argument: " + mode.ToString()); 
        }
        if (index > 0 && index < tmp.Length)
        {
            ret = tmp[index];
        }
        else
        {
            ret = default(GameObject);
        }
        return ret;
    }


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _fullCollide.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(_fullCollide[i].min.x, _fullCollide[i].min.y, 0), new Vector3(_fullCollide[i].max.x, _fullCollide[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(_fullCollide[i].max.x, _fullCollide[i].min.y, 0), new Vector3(_fullCollide[i].max.x, _fullCollide[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_fullCollide[i].min.x, _fullCollide[i].min.y, 0), new Vector3(_fullCollide[i].min.x, _fullCollide[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_fullCollide[i].min.x, _fullCollide[i].max.y, 0), new Vector3(_fullCollide[i].max.x, _fullCollide[i].max.y, 0));
        }

        for (int i = 0; i < _shootthru.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(_shootthru[i].min.x, _shootthru[i].min.y, 0), new Vector3(_shootthru[i].max.x, _shootthru[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(_shootthru[i].max.x, _shootthru[i].min.y, 0), new Vector3(_shootthru[i].max.x, _shootthru[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_shootthru[i].min.x, _shootthru[i].min.y, 0), new Vector3(_shootthru[i].min.x, _shootthru[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_shootthru[i].min.x, _shootthru[i].max.y, 0), new Vector3(_shootthru[i].max.x, _shootthru[i].max.y, 0));
        }
    }
#endif
}