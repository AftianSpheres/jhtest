using UnityEngine;
using System.Collections;

/// <summary>
/// MonoBehaviour for a GameObject acting as a tilemap overlay that "unrolls" in one direction when its "stem" is interacted with.
/// This is basically just for ladders, but it's possible I might want to reuse this behavior for something else, I guess? So it's marginally more generic than "ladders."
/// But, basically: rope ladders. This is rope ladders.
/// </summary>
public class mu_UnrollingTiles : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;
    public Bounds activationZone;
    public Collider stemCollider;
    public Collider[] rollOutTileColliders;
    public mufm_Generic flag;
    public RoomController room;
    public Sprite stemOpenSprite;
    public Sprite capOpenSprite;
    public Sprite[] rollOutSprites;
    public SpriteRenderer stemRenderer;
    public SpriteRenderer capRenderer;
    public SpriteRenderer[] rollOutTileRenderers;
    public Direction direction;
    public bool deactivateCollisionWhenUnrolled;
    public bool activated;
    public int numberOfTilesAfterStem;
    public int singleTileUnrollTime;


    /// <summary>
    /// MonoBehaviour.Start()
    /// </summary>
    void Start ()
    {
	    if (flag.CheckFlag() == true)
        {
            stemRenderer.sprite = stemOpenSprite;
            stemCollider.enabled = !deactivateCollisionWhenUnrolled;
            capRenderer.sprite = capOpenSprite;
            for (int i = 0; i < numberOfTilesAfterStem; i++)
            {
                rollOutTileRenderers[i].sprite = rollOutSprites[rollOutSprites.Length - 1];
                rollOutTileColliders[i].enabled = !deactivateCollisionWhenUnrolled;
            }
            activated = true;
        }
        else
        {
            stemCollider.enabled = deactivateCollisionWhenUnrolled;
            for (int i = 0; i < numberOfTilesAfterStem; i++)
            {
                rollOutTileColliders[i].enabled = deactivateCollisionWhenUnrolled;
            }
        }
	}
	
    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
	void Update ()
    {
	    if (activated == false && room.isActiveRoom == true)
        {
            if (activationZone.Intersects(room.world.player.collider.bounds) == true)
            {
                if (HardwareInterfaceManager.Instance.Confirm.BtnDown == true && room.world.player.facingDir == direction)
                {
                    StartCoroutine(StartUnrolling());
                }
            }
        }
	}

    /// <summary>
    /// Coroutine: sets the ladder-or-whatever to unrolling.
    /// </summary>
    IEnumerator StartUnrolling ()
    {
        activated = true;
        flag.ActivateFlag();
        stemRenderer.sprite = stemOpenSprite;
        stemCollider.enabled = !deactivateCollisionWhenUnrolled;
        source.PlayOneShot(clip);
        int tilesUnrolled = 0;
        while (tilesUnrolled < numberOfTilesAfterStem)
        {
            int t = 0;
            int t2 = 0;
            int spriteIndex = 0;
            while (t < singleTileUnrollTime)
            {
                t++;
                t2++;
                if (t2 > ((float)singleTileUnrollTime / rollOutSprites.Length))
                {

                    rollOutTileRenderers[tilesUnrolled].sprite = rollOutSprites[spriteIndex];
                    source.PlayOneShot(clip);
                    spriteIndex++;
                    t2 = 0;
                }
                yield return null;
            }
            rollOutTileRenderers[tilesUnrolled].sprite = rollOutSprites[spriteIndex];
            source.PlayOneShot(clip);
            rollOutTileColliders[tilesUnrolled].enabled = !deactivateCollisionWhenUnrolled;
            tilesUnrolled++;
            if (tilesUnrolled == numberOfTilesAfterStem)
            {
                capRenderer.sprite = capOpenSprite; // SAVING ONE FRAME OF ANIMATION, less because it matters and more because I'll start convulsing if I don't
            }
            yield return null;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(activationZone.min.x, activationZone.min.y, activationZone.min.z - 100), new Vector3(activationZone.max.x, activationZone.min.y, activationZone.min.z - 100));
        Gizmos.DrawLine(new Vector3(activationZone.max.x, activationZone.min.y, activationZone.min.z - 100), new Vector3(activationZone.max.x, activationZone.max.y, activationZone.min.z - 100));
        Gizmos.DrawLine(new Vector3(activationZone.min.x, activationZone.min.y, activationZone.min.z - 100), new Vector3(activationZone.min.x, activationZone.max.y, activationZone.min.z - 100));
        Gizmos.DrawLine(new Vector3(activationZone.min.x, activationZone.max.y, activationZone.min.z - 100), new Vector3(activationZone.max.x, activationZone.max.y, activationZone.min.z - 100));
    }
# endif
}
