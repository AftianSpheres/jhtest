using UnityEngine;
using System.Collections;

public class UIElementLayeringHelper : MonoBehaviour
{
    void Start()
    {
        MeshRenderer m = GetComponent<MeshRenderer>();
        m.sortingLayerID = HammerConstants.l_UI;
    }
}
