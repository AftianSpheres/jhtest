using UnityEngine;

public class SpriteMover : MonoBehaviour
{
    new public Collider2D collider;
    public Vector3 heading;
    public Vector3 virtualPosition;
    Bounds prospectivePos;

    void Awake ()
    {
        virtualPosition = transform.position;
    }

	// Update is called once per frame
	void Update ()
    {
        if (heading != Vector3.zero)
        {
            virtualPosition += heading;
            heading = Vector3.zero;
            transform.position = new Vector3((int)virtualPosition.x, (int)virtualPosition.y, transform.position.z);
        }
	}
}
