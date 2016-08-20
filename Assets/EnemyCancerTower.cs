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
    CancerTowerState state;
    int TimeToNextFiring;
    int frameCtr;

    Vector3[] bitDefaultPositions;
    Vector3[] bitOrigins;
    Vector3[] bitDestinations;
    Vector3[] bitVirtualPositions;
    bool[] bitsReachedTerminus;
    float m = 0;

    // Use this for initialization
    void Start ()
    {
        frameCtr = 0;
        bitsReachedTerminus = new bool[] {false, false, false, false};
        bitDefaultPositions = new Vector3[bits.Length];
        bitOrigins = new Vector3[bits.Length];
        bitVirtualPositions = new Vector3[bitDefaultPositions.Length];
        bitDestinations = new Vector3[bitDefaultPositions.Length];
        SetFiringDelay();
        for (int i = 0; i < bitVirtualPositions.Length; i++)
        {
            bitDefaultPositions[i] = bitOrigins[i] = bits[i].transform.position;
            bitDestinations[i] = bitDefaultPositions[i];
            bitVirtualPositions[i] = bitDefaultPositions[i];
        }
        state = CancerTowerState.Neutral;
}
	
	// Update is called once per frame
	void Update ()
    {
        if (common.room.isActiveRoom)
        {
            for (int i = 0; i < bitVirtualPositions.Length; i++)
            {
                bits[i].transform.position = new Vector3(Mathf.Round(bitVirtualPositions[i].x), Mathf.Round(bitVirtualPositions[i].y), bits[i].transform.position.z);
            }
            switch (state)
            {
                case CancerTowerState.Neutral:
                    if (TimeToNextFiring < 1)
                    {
                        for (int i = 0; i < bitDestinations.Length; i++)
                        {
                            bitDestinations[i] = common.room.world.player.collider.bounds.center + new Vector3(-bitColliders[i].offset.x, bitColliders[i].offset.y, 0);
                            bitOrigins[i] = bits[i].transform.position;
                        }
                        m = 0;
                        state = CancerTowerState.FiringBits;
                    }
                    else if (frameCtr > 20)
                    {
                        m = 0;
                        for (int i = 0; i < bitDefaultPositions.Length; i++)
                        {
                            bitOrigins[i] = bits[i].transform.position;
                            bitDestinations[i] = bitDefaultPositions[i];
                        }
                        frameCtr = 0;
                    }
                    else if (frameCtr == 10)
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
                    }
                    break;
            }
            for (int i = 0; i < bits.Length; i++)
            {
                bitVirtualPositions[i] = Vector3.Lerp(bitOrigins[i], bitDestinations[i], m);
            }
            frameCtr++;
            TimeToNextFiring--;
        }
    }

    void SetFiringDelay()
    {
        TimeToNextFiring = Random.Range(150, 420);
    }
}
