using UnityEngine;
using System.Collections;

enum CancerTowerState
{
    Neutral,
    FiringBits,
    RecoveringBits
}

public class EnemyCancerTower : EnemyModule
{
    public SpriteRenderer[] bits;
    public BoxCollider2D[] bitColliders;
    public Sprite[] bitFrames;
    public AudioClip scream;
    public AudioClip toss;
    CancerTowerState state;
    int TimeToNextFiring;
    int frameCtr;

    int[] bitFrameIndices;
    Vector3[] bitDefaultPositions;
    Vector3[] bitOrigins;
    Vector3[] bitDestinations;
    Vector3[] bitVirtualPositions;
    float m = 0;

    // Use this for initialization
    void Start ()
    {
        frameCtr = 0;
        bitDefaultPositions = new Vector3[bits.Length];
        bitFrameIndices = new int[bits.Length];
        bitOrigins = new Vector3[bits.Length];
        bitVirtualPositions = new Vector3[bitDefaultPositions.Length];
        bitDestinations = new Vector3[bitDefaultPositions.Length];
        SetFiringDelay();
        for (int i = 0; i < bitVirtualPositions.Length; i++)
        {
            bitFrameIndices[i] = i;
            bitDefaultPositions[i] = bitOrigins[i] = bits[i].transform.position;
            bitDestinations[i] = bitDefaultPositions[i];
            bitVirtualPositions[i] = bitDefaultPositions[i];
        }
        state = CancerTowerState.Neutral;
}
	
	// Update is called once per frame
	void Update ()
    {
        if (common.room.isActiveRoom && !common.isDead)
        {
            for (int i = 0; i < bitVirtualPositions.Length; i++)
            {
                bits[i].transform.position = new Vector3(Mathf.Round(bitVirtualPositions[i].x), Mathf.Round(bitVirtualPositions[i].y), bits[i].transform.position.z);
            }
            switch (state)
            {
                case CancerTowerState.Neutral:
                    if (TimeToNextFiring == 45) common.source.PlayOneShot(scream);
                    if (TimeToNextFiring < 1)
                    {
                        for (int i = 0; i < bitDestinations.Length; i++)
                        {
                            bitDestinations[i] = common.room.world.player.collider.bounds.center + new Vector3(-bitColliders[i].offset.x, bitColliders[i].offset.y, 0);
                            bitOrigins[i] = bits[i].transform.position;
                        }
                        m = 0;
                        state = CancerTowerState.FiringBits;
                        common.Vulnerable = true;
                    }
                    else if (frameCtr % 20 == 0)
                    {
                        m = 0;
                        for (int i = 0; i < bitDefaultPositions.Length; i++)
                        {
                            bitOrigins[i] = bits[i].transform.position;
                            bitDestinations[i] = bitDefaultPositions[i];
                        }
                    }
                    else if (frameCtr % 10 == 0)
                    {
                        m = 0;
                        for (int i = 0; i < bitDefaultPositions.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                bitDestinations[i] = bitDefaultPositions[i] + (Vector3.right * Random.Range(1, 7)) + (Random.Range(-2f, 2f) * Vector3.up);
                            }
                            else
                            {
                                bitDestinations[i] = bitDefaultPositions[i] + (Vector3.left * Random.Range(1, 7)) + (Random.Range(-2f, 2f) * Vector3.up);
                            }
                            bitOrigins[i] = bits[i].transform.position;
                        }
                    }
                    else
                    {
                        m += .1f;
                    }
                    break;
                case CancerTowerState.FiringBits:
                    float cm = 0;
                    for (int i = 0; i < bitDefaultPositions.Length; i++)
                    {
                        cm += Vector2.Distance(new Vector2(bitDefaultPositions[i].x, bitDefaultPositions[i].y), new Vector2(bitDestinations[i].x, bitDestinations[i].y));
                    }
                    cm /= (bitDefaultPositions.Length + 1);
                    cm = (0.05f + ((0.05f / 160f) * cm * 0.5f)) / 5f;
                    if (cm < 0.05f) cm = 0.05f;
                    else if (cm > 1/30f) cm = 1/30f;
                    m += cm;
                    if (m > 1f)
                    {
                        for (int i = 0; i < bitDefaultPositions.Length; i++)
                        {
                            bitDestinations[i] = bitDefaultPositions[i];
                            bitOrigins[i] = bits[i].transform.position;
                            m = 0;
                        }
                        state = CancerTowerState.RecoveringBits;
                    }
                    break;
                case CancerTowerState.RecoveringBits:
                    m += 1f / 40f;
                    if (m > 1f)
                    {
                        SetFiringDelay();
                        state = CancerTowerState.Neutral;
                        common.Vulnerable = false;
                    }
                    break;
            }
            for (int i = 0; i < bits.Length; i++)
            {
                bitVirtualPositions[i] = Vector3.Lerp(bitOrigins[i], bitDestinations[i], m);
                if (frameCtr % 6 == 0)
                {
                    bitFrameIndices[i]++;
                    if (bitFrameIndices[i] >= bitFrameIndices.Length) bitFrameIndices[i] = 0;
                    bits[i].sprite = bitFrames[bitFrameIndices[i]];
                }
                Bounds bc = new Bounds(new Vector3(bitColliders[i].bounds.center.x, bitColliders[i].bounds.center.y, 0), new Vector3(bitColliders[i].bounds.size.x, bitColliders[i].bounds.size.y, float.MaxValue));
                if (bc.Intersects(common.room.world.player.collider.bounds))
                {
                    common.room.world.player.Hit(common.ShotDmg, bitColliders[i].bounds.center, common.Weight);
                }
            }
            frameCtr++;
            TimeToNextFiring--;
        }
    }

    void SetFiringDelay()
    {
        TimeToNextFiring = Random.Range(150, 420);
    }

    new public void Respawn()
    {
        SetFiringDelay();
        frameCtr = 0;
        m = 0;
        for (int i = 0; i < bitVirtualPositions.Length; i++)
        {
            bits[i].gameObject.SetActive(true);
            bitFrameIndices[i] = i;
            bitDestinations[i] = bitDefaultPositions[i];
            bitVirtualPositions[i] = bitDefaultPositions[i];
        }
        state = CancerTowerState.Neutral;
    }
}
