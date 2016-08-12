using UnityEngine;
using System.Collections;

enum FallingRockState
{
    None,
    Undeployed,
    Falling,
    Breaking
};

public class mu_FallingRock : MonoBehaviour
{
    public RoomController room;
    public AudioClip fallSFX;
    public AudioClip impactSFX;
    public AudioSource source;
    public BoxCollider rockCollider;
    public Sprite rockSprite;
    public Sprite breakSprite;
    public Sprite rubbleSprite;
    public SpriteRenderer rockRenderer;
    public SpriteRenderer rubbleRenderer0;
    public SpriteRenderer rubbleRenderer1;
    public SpriteRenderer rubbleRenderer2;
    public SpriteRenderer rubbleRenderer3;
    public SpriteRenderer shadowRenderer;
    public int damage;
    public int weight;
    public int fallLength;
    public int minFallTimeDelay;
    public int maxFallTimeDelay;
    public Bounds permittedArea;
    int timer;
    FallingRockState state;

	// Use this for initialization
	void Awake ()
    {
        state = FallingRockState.Undeployed;
        rockCollider.enabled = rockRenderer.enabled = rubbleRenderer0.enabled = rubbleRenderer1.enabled = rubbleRenderer2.enabled = rubbleRenderer3.enabled = shadowRenderer.enabled = false;
        rockRenderer.sprite = rockSprite;
        rubbleRenderer0.sprite = rubbleRenderer1.sprite = rubbleRenderer2.sprite = rubbleRenderer3.sprite = rubbleSprite;
        timer = Random.Range(minFallTimeDelay, maxFallTimeDelay + 1);
        transform.position = new Vector3(Random.Range((int)permittedArea.min.x, (int)permittedArea.max.x + 1 - rockSprite.bounds.size.x), Random.Range((int)permittedArea.min.y + rockSprite.bounds.size.y, (int)permittedArea.max.y + 1), transform.position.z);
    }

    // Update is called once per frame
    void Update ()
    {
        if (room.isActiveRoom == true)
        {
            switch (state)
            {
                case FallingRockState.Undeployed:
                    timer--;
                    if (timer < 15)
                    {
                        shadowRenderer.enabled = true;
                    }
                    if (timer < 1)
                    {
                        rockRenderer.enabled = true;
                        rockRenderer.sprite = rockSprite;
                        rockRenderer.transform.position = rockRenderer.transform.position + (fallLength * Vector3.up);
                        state = FallingRockState.Falling;
                        timer = fallLength;
                        source.PlayOneShot(fallSFX);
                    }
                    break;
                case FallingRockState.Falling:
                    timer--;
                    rockRenderer.transform.position += Vector3.down;
                    if (timer < 1)
                    {
                        source.Stop();
                        source.PlayOneShot(impactSFX);
                        state = FallingRockState.Breaking;
                        rockRenderer.sprite = breakSprite;
                        rockCollider.enabled = rubbleRenderer0.enabled = rubbleRenderer1.enabled = rubbleRenderer2.enabled = rubbleRenderer3.enabled = true;
                        rubbleRenderer0.transform.position = rockRenderer.transform.position + (Vector3.left * (4 + rubbleSprite.bounds.size.x));
                        rubbleRenderer1.transform.position = rockRenderer.transform.position + (Vector3.right * (4 + breakSprite.bounds.size.x));
                        rubbleRenderer2.transform.position = rockRenderer.transform.position + (Vector3.left * (4 + rubbleSprite.bounds.size.x)) + (Vector3.up * 6);
                        rubbleRenderer3.transform.position = rockRenderer.transform.position + (Vector3.right * (4 + breakSprite.bounds.size.x)) + (Vector3.up * 6);
                        timer = 60;
                    }
                    break;
                case FallingRockState.Breaking:
                    timer--;
                    rubbleRenderer0.transform.position += Vector3.down + Vector3.left;
                    rubbleRenderer1.transform.position += Vector3.down + Vector3.right;
                    rubbleRenderer2.transform.position += 2 * Vector3.left;
                    rubbleRenderer3.transform.position += 2 * Vector3.right;
                    if (timer % 4 == 0)
                    {
                        rubbleRenderer0.enabled = rubbleRenderer1.enabled = rubbleRenderer2.enabled = rubbleRenderer3.enabled = !rubbleRenderer0.enabled;
                    }
                    if (timer < 44)
                    {
                        rockCollider.enabled = false;
                    }
                    else
                    {
                        if (rockCollider.bounds.Intersects(room.world.player.collider.bounds))
                        {
                            room.world.player.Hit(damage, rockCollider.bounds.center, weight);
                        }
                    }
                    if (timer < 1)
                    {
                        rockRenderer.enabled = rubbleRenderer0.enabled = rubbleRenderer1.enabled = rubbleRenderer2.enabled = rubbleRenderer3.enabled = shadowRenderer.enabled = false;
                        state = FallingRockState.Undeployed;
                        timer = Random.Range(minFallTimeDelay, maxFallTimeDelay + 1);
                        transform.position = new Vector3(Random.Range((int)permittedArea.min.x, (int)permittedArea.max.x + 1 - rockSprite.bounds.size.x), Random.Range((int)permittedArea.min.y + rockSprite.bounds.size.y, (int)permittedArea.max.y + 1), transform.position.z);
                    }
                    break;
            }
        }
	}
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(permittedArea.min.x, permittedArea.min.y, permittedArea.min.z - 100), new Vector3(permittedArea.max.x, permittedArea.min.y, permittedArea.min.z - 100));
        Gizmos.DrawLine(new Vector3(permittedArea.max.x, permittedArea.min.y, permittedArea.min.z - 100), new Vector3(permittedArea.max.x, permittedArea.max.y, permittedArea.min.z - 100));
        Gizmos.DrawLine(new Vector3(permittedArea.min.x, permittedArea.min.y, permittedArea.min.z - 100), new Vector3(permittedArea.min.x, permittedArea.max.y, permittedArea.min.z - 100));
        Gizmos.DrawLine(new Vector3(permittedArea.min.x, permittedArea.max.y, permittedArea.min.z - 100), new Vector3(permittedArea.max.x, permittedArea.max.y, permittedArea.min.z - 100));
    }
#endif
}
