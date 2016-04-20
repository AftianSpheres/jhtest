using UnityEngine;
using System;
using System.Collections;

public enum rcGameObjectSearchMode
{
    layer0,
    layer0_full,
    layer0_shootthru,
    layer1,
    layer1_full,
    layer1_shootthru,
    all,
    all_full,
    all_shootthru
}

public class RoomCollider : MonoBehaviour
{
    [SerializeField]
    private Bounds[] _layer0_full;
    public Bounds[] layer0_full
    {
        get
        {
            Bounds[] ret = new Bounds[_layer0_full.Length + layer0_full_obj.Length];
            int v = 0;
            _layer0_full.CopyTo(ret, v);
            v += _layer0_full.Length;
            for (int i = 0; i < layer0_full_obj.Length; i++)
            {
                ret[v + i] = layer0_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    [SerializeField]
    private Bounds[] _layer0_shootthru;
    public Bounds[] layer0_shootthru
    {
        get
        {
            Bounds[] ret = new Bounds[_layer0_shootthru.Length + layer0_shootthru_obj.Length];
            int v = 0;
            _layer0_shootthru.CopyTo(ret, v);
            v += _layer0_shootthru.Length;
            for (int i = 0; i < layer0_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer0_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    [SerializeField]
    private Bounds[] _layer1_full;
    public Bounds[] layer1_full
    {
        get
        {
            Bounds[] ret = new Bounds[_layer1_full.Length + layer1_full_obj.Length];
            int v = 0;
            _layer1_full.CopyTo(ret, v);
            v += _layer1_full.Length;
            for (int i = 0; i < layer1_full_obj.Length; i++)
            {
                ret[v + i] = layer1_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    [SerializeField]
    private Bounds[] _layer1_shootthru;
    public Bounds[] layer1_shootthru
    {
        get
        {
            Bounds[] ret = new Bounds[_layer1_shootthru.Length + layer1_shootthru_obj.Length];
            int v = 0;
            _layer1_shootthru.CopyTo(ret, v);
            v += _layer1_shootthru.Length;
            for (int i = 0; i < layer1_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer1_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    public Bounds[] allCollision
    {
        get
        {
            Bounds[] ret = new Bounds[_layer0_full.Length + _layer0_shootthru.Length + _layer1_full.Length + _layer1_shootthru.Length + layer0_full_obj.Length + layer0_shootthru_obj.Length + layer1_full_obj.Length + layer1_shootthru_obj.Length];
            int v = 0;
            _layer0_full.CopyTo(ret, v);
            v += _layer0_full.Length;
            _layer0_shootthru.CopyTo(ret, v);
            v += _layer0_shootthru.Length;
            _layer1_full.CopyTo(ret, v);
            v += _layer1_full.Length;
            _layer1_shootthru.CopyTo(ret, v);
            v += _layer1_shootthru.Length;
            for (int i = 0; i < layer0_full_obj.Length; i++)
            {
                ret[v + i] = layer0_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer0_full_obj.Length;
            for (int i = 0; i < layer0_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer0_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer0_shootthru_obj.Length;
            for (int i = 0; i < layer1_full_obj.Length; i++)
            {
                ret[v + i] = layer1_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer1_full_obj.Length;
            for (int i = 0; i < layer1_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer1_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    public Bounds[] allFull
    {
        get
        {
            Bounds[] ret = new Bounds[_layer0_full.Length + _layer1_full.Length + layer0_full_obj.Length + layer1_full_obj.Length];
            int v = 0;
            _layer0_full.CopyTo(ret, v);
            v += _layer0_full.Length;
            _layer1_full.CopyTo(ret, v);
            v += _layer1_full.Length;
            for (int i = 0; i < layer0_full_obj.Length; i++)
            {
                ret[v + i] = layer0_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer0_full_obj.Length;
            for (int i = 0; i < layer1_full_obj.Length; i++)
            {
                ret[v + i] = layer1_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    public Bounds[] allShootthru
    {
        get
        {
            Bounds[] ret = new Bounds[_layer0_shootthru.Length + _layer1_shootthru.Length + layer0_shootthru_obj.Length + layer1_shootthru_obj.Length];
            int v = 0;
            _layer0_shootthru.CopyTo(ret, v);
            v += _layer0_shootthru.Length;
            _layer1_shootthru.CopyTo(ret, v);
            v += _layer1_shootthru.Length;
            for (int i = 0; i < layer0_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer0_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer0_shootthru_obj.Length;
            for (int i = 0; i < layer1_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer1_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    public Bounds[] layer0
    {
        get
        {
            Bounds[] ret = new Bounds[_layer0_full.Length + _layer0_shootthru.Length + layer0_full_obj.Length + layer0_shootthru_obj.Length];
            int v = 0;
            _layer0_full.CopyTo(ret, v);
            v += _layer0_full.Length;
            _layer0_shootthru.CopyTo(ret, v);
            v += _layer0_shootthru.Length;
            for (int i = 0; i < layer0_full_obj.Length; i++)
            {
                ret[v + i] = layer0_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer0_full_obj.Length;
            for (int i = 0; i < layer0_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer0_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }
    public Bounds[] layer1
    {
        get
        {
            Bounds[] ret = new Bounds[_layer1_full.Length + _layer1_shootthru.Length + layer1_full_obj.Length + layer1_shootthru_obj.Length];
            int v = 0;
            _layer1_full.CopyTo(ret, v);
            v += _layer1_full.Length;
            _layer1_shootthru.CopyTo(ret, v);
            v += _layer1_shootthru.Length;
            for (int i = 0; i < layer1_full_obj.Length; i++)
            {
                ret[v + i] = layer1_full_obj[i].GetComponent<BoxCollider>().bounds;
            }
            v += layer1_full_obj.Length;
            for (int i = 0; i < layer1_shootthru_obj.Length; i++)
            {
                ret[v + i] = layer1_shootthru_obj[i].GetComponent<BoxCollider>().bounds;
            }
            return ret;
        }
    }

    [SerializeField]
    private GameObject[] layer0_full_obj;
    [SerializeField]
    private GameObject[] layer0_shootthru_obj;
    [SerializeField]
    private GameObject[] layer1_full_obj;
    [SerializeField]
    private GameObject[] layer1_shootthru_obj;

    public GameObject GetAssocGameObject (int index, rcGameObjectSearchMode mode)
    {
        GameObject ret;
        GameObject[] tmp;
        switch (mode)
        {
            case rcGameObjectSearchMode.layer0:
                tmp = new GameObject[layer0_full_obj.Length + layer0_shootthru_obj.Length];
                layer0_full_obj.CopyTo(tmp, 0);
                layer0_shootthru_obj.CopyTo(tmp, layer0_full_obj.Length);
                index -= (_layer0_full.Length + _layer0_shootthru.Length);
                break;
            case rcGameObjectSearchMode.layer0_full:
                tmp = new GameObject[layer0_full_obj.Length];
                layer0_full_obj.CopyTo(tmp, 0);
                index -= (_layer0_full.Length);
                break;
            case rcGameObjectSearchMode.layer0_shootthru:
                tmp = new GameObject[layer0_shootthru_obj.Length];
                layer0_shootthru_obj.CopyTo(tmp, 0);
                index -= (_layer0_shootthru.Length);
                break;
            case rcGameObjectSearchMode.layer1:
                tmp = new GameObject[layer1_full_obj.Length + layer1_shootthru_obj.Length];
                layer1_full_obj.CopyTo(tmp, 0);
                layer1_shootthru_obj.CopyTo(tmp, layer1_full_obj.Length);
                index -= (_layer1_full.Length + _layer1_shootthru.Length);
                break;
            case rcGameObjectSearchMode.layer1_full:
                tmp = new GameObject[layer1_full_obj.Length];
                layer1_full_obj.CopyTo(tmp, 0);
                index -= (_layer1_full.Length);
                break;
            case rcGameObjectSearchMode.layer1_shootthru:
                tmp = new GameObject[layer1_shootthru_obj.Length];
                layer1_shootthru_obj.CopyTo(tmp, 0);
                index -= (_layer1_shootthru.Length);
                break;
            case rcGameObjectSearchMode.all:
                tmp = new GameObject[layer0_full_obj.Length + layer0_shootthru_obj.Length + layer1_full_obj.Length + layer1_shootthru_obj.Length];
                layer0_full_obj.CopyTo(tmp, 0);
                layer0_shootthru_obj.CopyTo(tmp, layer0_full_obj.Length);
                layer1_full_obj.CopyTo(tmp, layer0_full_obj.Length + layer0_shootthru_obj.Length);
                layer1_shootthru_obj.CopyTo(tmp, layer0_full_obj.Length + layer0_shootthru_obj.Length + layer1_full_obj.Length);
                index -= (_layer0_full.Length + _layer0_shootthru.Length + _layer1_full.Length + _layer1_shootthru.Length);
                break;
            case rcGameObjectSearchMode.all_full:
                tmp = new GameObject[layer0_full_obj.Length + layer1_full_obj.Length];
                layer0_full_obj.CopyTo(tmp, 0);
                layer1_full_obj.CopyTo(tmp, layer0_full_obj.Length);
                index -= (_layer0_full.Length + _layer1_full.Length);
                break;
            case rcGameObjectSearchMode.all_shootthru:
                tmp = new GameObject[layer0_shootthru_obj.Length + layer1_shootthru_obj.Length];
                layer0_shootthru_obj.CopyTo(tmp, 0);
                layer1_shootthru_obj.CopyTo(tmp, layer0_shootthru_obj.Length);
                index -= (_layer0_shootthru.Length + _layer1_shootthru.Length);
                break;
            default:
                throw new Exception("Called RoomCollider.GetAssocGameObject with invalid argument: " + mode.ToString()); 
        }
        if (index > 0 && index < tmp.Length)
        {
            Debug.Log(index);
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
        for (int i = 0; i < _layer0_full.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(_layer0_full[i].min.x, _layer0_full[i].min.y, 0), new Vector3(_layer0_full[i].max.x, _layer0_full[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(_layer0_full[i].max.x, _layer0_full[i].min.y, 0), new Vector3(_layer0_full[i].max.x, _layer0_full[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_layer0_full[i].min.x, _layer0_full[i].min.y, 0), new Vector3(_layer0_full[i].min.x, _layer0_full[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_layer0_full[i].min.x, _layer0_full[i].max.y, 0), new Vector3(_layer0_full[i].max.x, _layer0_full[i].max.y, 0));
        }

        for (int i = 0; i < _layer0_shootthru.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(_layer0_shootthru[i].min.x, _layer0_shootthru[i].min.y, 0), new Vector3(_layer0_shootthru[i].max.x, _layer0_shootthru[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(_layer0_shootthru[i].max.x, _layer0_shootthru[i].min.y, 0), new Vector3(_layer0_shootthru[i].max.x, _layer0_shootthru[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_layer0_shootthru[i].min.x, _layer0_shootthru[i].min.y, 0), new Vector3(_layer0_shootthru[i].min.x, _layer0_shootthru[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(_layer0_shootthru[i].min.x, _layer0_shootthru[i].max.y, 0), new Vector3(_layer0_shootthru[i].max.x, _layer0_shootthru[i].max.y, 0));
        }

        for (int i = 0; i < _layer1_full.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(_layer1_full[i].min.x, _layer1_full[i].min.y, _layer1_full[i].min.z - 100), new Vector3(_layer1_full[i].max.x, _layer1_full[i].min.y, _layer1_full[i].min.z - 100));
            Gizmos.DrawLine(new Vector3(_layer1_full[i].max.x, _layer1_full[i].min.y, _layer1_full[i].min.z - 100), new Vector3(_layer1_full[i].max.x, _layer1_full[i].max.y, _layer1_full[i].min.z - 100));
            Gizmos.DrawLine(new Vector3(_layer1_full[i].min.x, _layer1_full[i].min.y, _layer1_full[i].min.z - 100), new Vector3(_layer1_full[i].min.x, _layer1_full[i].max.y, _layer1_full[i].min.z - 100));
            Gizmos.DrawLine(new Vector3(_layer1_full[i].min.x, _layer1_full[i].max.y, _layer1_full[i].min.z - 100), new Vector3(_layer1_full[i].max.x, _layer1_full[i].max.y, _layer1_full[i].min.z - 100));
        }

        for (int i = 0; i < _layer1_shootthru.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(_layer1_shootthru[i].min.x, _layer1_shootthru[i].min.y, _layer1_shootthru[i].min.z - 100), new Vector3(_layer1_shootthru[i].max.x, _layer1_shootthru[i].min.y, _layer1_shootthru[i].min.z - 100));
            Gizmos.DrawLine(new Vector3(_layer1_shootthru[i].max.x, _layer1_shootthru[i].min.y, _layer1_shootthru[i].min.z - 100), new Vector3(_layer1_shootthru[i].max.x, _layer1_shootthru[i].max.y, _layer1_shootthru[i].min.z - 100));
            Gizmos.DrawLine(new Vector3(_layer1_shootthru[i].min.x, _layer1_shootthru[i].min.y, _layer1_shootthru[i].min.z - 100), new Vector3(_layer1_shootthru[i].min.x, _layer1_shootthru[i].max.y, _layer1_shootthru[i].min.z - 100));
            Gizmos.DrawLine(new Vector3(_layer1_shootthru[i].min.x, _layer1_shootthru[i].max.y, _layer1_shootthru[i].min.z - 100), new Vector3(_layer1_shootthru[i].max.x, _layer1_shootthru[i].max.y, _layer1_shootthru[i].min.z - 100));
        }
    }
#endif
}