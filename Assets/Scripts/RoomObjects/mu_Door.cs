﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Conditions for opening a door.
/// Note that "None" doesn't mean it doesn't open - it means it opens on contact. "Normal" door behavior.
/// </summary>
public enum DoorCondition
{
    None,
    AreaKey,
    KeyItem,
    Special
}

/// <summary>
/// Door.
/// RoomEvent integration for conditions outside of those that are part of the Door class.
/// </summary>
public class mu_Door : MonoBehaviour
{
    public RoomController room;
    private PlayerController player;
    public mu_Door mirror;
    public Sprite[] frames;
    new public BoxCollider collider;
    new public SpriteRenderer renderer;
    public AudioClip clip;
    public AudioSource source;
    public DoorCondition condition;
    public DoorCondition postUnlockCondition;
    public mu_RoomEvent specialCondition;
    public Direction direction;
    public bool open = false;
    public bool transitionInProgress = false;
    public Bounds boundsToOpen;
    public mufm_Generic Flag;
    private static int DoorSensitivityZone = 2;
    private int counter = 0;
    private static int DoorDelayFrames = 8;

	// Use this for initialization
	void Start ()
    {
	    if (mirror.mirror != this)
        {
            throw new System.Exception("Doors must be paired!");
        }
        GetBoundsToOpen();
        player = room.world.player;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((condition == DoorCondition.AreaKey || condition == DoorCondition.KeyItem) && Flag.CheckFlag() == true)
        {
            Unlock();
        }
        if (room.world.activeRoom == room && transitionInProgress == false)
        {
            if (counter < 1)
            {
                counter = 0;
            }
            if (counter == 0 && player.Locked == false && open == true)
            {
                Close();
            }
            if (open == true)
            {
                switch (direction)
                {
                    case Direction.Down:
                        if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                        && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true)
                        {
                            StartCoroutine(DrawPlayerThrough());
                        }
                        else if (condition == DoorCondition.None)
                        {
                            counter--;
                        }
                        break;
                    case Direction.Up:
                        if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                        && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true)
                        {
                            StartCoroutine(DrawPlayerThrough());
                        }
                        else if (condition == DoorCondition.None)
                        {
                            counter--;
                        }
                        break;
                    case Direction.Left:
                        if (boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                        && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true)
                        {
                            StartCoroutine(DrawPlayerThrough());
                        }
                        else if (condition == DoorCondition.None)
                        {
                            counter--;
                        }
                        break;
                    case Direction.Right:
                        if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                        && boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true)
                        {
                            StartCoroutine(DrawPlayerThrough());
                        }
                        else if (condition == DoorCondition.None)
                        {
                            counter--;
                        }
                        break;
                }
            }
            else if (open == false)
            {
                if (counter >= DoorDelayFrames)
                {
                    StartCoroutine(Open());
                }
                else
                {
                    switch (condition)
                    {
                        case DoorCondition.None:
                            switch (direction)
                            {
                                case Direction.Down:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_U)
                                    {
                                        counter++;
                                    }
                                    else
                                    {
                                        counter--;
                                    }
                                    break;
                                case Direction.Up:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_D)
                                    {
                                        counter++;
                                    }
                                    else
                                    {
                                        counter--;
                                    }
                                    break;
                                case Direction.Left:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_R)
                                    {
                                        counter++;
                                    }
                                    else
                                    {
                                        counter--;
                                    }
                                    break;
                                case Direction.Right:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_L)
                                    {
                                        counter++;
                                    }
                                    else
                                    {
                                        counter--;
                                    }
                                    break;
                            }
                            break;
                        case DoorCondition.AreaKey:
                            switch (direction)
                            {
                                case Direction.Down:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_U)
                                    {
                                        if (room.world.GameStateManager.areaKeys[(int)room.world.Area] > 0)
                                        {
                                            room.world.GameStateManager.areaKeys[(int)room.world.Area]--;
                                            Unlock();
                                            source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_KeySFX));
                                        }
                                    }
                                    break;
                                case Direction.Up:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_D)
                                    {
                                        if (room.world.GameStateManager.areaKeys[(int)room.world.Area] > 0)
                                        {
                                            room.world.GameStateManager.areaKeys[(int)room.world.Area]--;
                                            Unlock();
                                            source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_KeySFX));
                                        }
                                    }
                                    break;
                                case Direction.Left:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.max.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_R)
                                    {
                                        if (room.world.GameStateManager.areaKeys[(int)room.world.Area] > 0)
                                        {
                                            room.world.GameStateManager.areaKeys[(int)room.world.Area]--;
                                            Unlock();
                                            source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_KeySFX));
                                        }
                                    }
                                    break;
                                case Direction.Right:
                                    if (boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.min.y, player.collider.bounds.center.z)) == true
                                    && boundsToOpen.Contains(new Vector3(player.collider.bounds.min.x, player.collider.bounds.max.y, player.collider.bounds.center.z)) == true
                                    && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerWalk_L)
                                    {
                                        if (room.world.GameStateManager.areaKeys[(int)room.world.Area] > 0)
                                        {
                                            room.world.GameStateManager.areaKeys[(int)room.world.Area]--;
                                            Unlock();
                                            source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_KeySFX));
                                        }
                                    }
                                    break;
                            }
                            break;
                        case DoorCondition.Special:
                            if (specialCondition.EventActive == true && open == false && transitionInProgress == false)
                            {
                                if (counter < int.MaxValue)
                                {
                                    counter++;
                                }
                            }
                            break;
                    }
                }
            }
        }
	}

    /// <summary>
    /// Opens a door.
    /// </summary>
    IEnumerator Open ()
    {
        transitionInProgress = true;
        if (mirror.transitionInProgress == false)
        {
            StartCoroutine(mirror.Open());
        }
        int i = 0;
        while (i < 30)
        {
            i++;
            if (i == 15)
            {
                renderer.sprite = frames[1];
            }
            yield return null;
        }
        renderer.sprite = frames[2];
        source.PlayOneShot(clip);
        open = true;
        GetBoundsToOpen();
        collider.enabled = false;
        transitionInProgress = false;

    }

    /// <summary>
    /// Sucks the player through to the other side of an open door.
    /// </summary>
    IEnumerator DrawPlayerThrough ()
    {
        player.mover.virtualPosition = player.transform.position;
        player.mover.heading = Vector3.zero;
        transitionInProgress = true;
        mirror.transitionInProgress = true;
        Vector3 PosMod = Vector3.zero;
        switch (direction)
        {
            case Direction.Down:
                player.animator.Play(PlayerAnimatorHashes.CutsceneWalk_U);
                PosMod = Vector3.up;
                break;
            case Direction.Up:
                player.animator.Play(PlayerAnimatorHashes.CutsceneWalk_D);
                PosMod = Vector3.down;
                break;
            case Direction.Left:
                player.animator.Play(PlayerAnimatorHashes.CutsceneWalk_R);
                PosMod = Vector3.right;
                break;
            case Direction.Right:
                player.animator.Play(PlayerAnimatorHashes.CutsceneWalk_L);
                PosMod = Vector3.left;
                break;
        }
        player.Locked = true;
        for (int i = 0; i < 44; i++)
        {
            player.transform.position += PosMod;
            yield return null;
        }
        if (room.world.cameraController.PlayerLockedToScroll == false)
        {
            player.Locked = false;
        }
        switch (direction)
        {
            case Direction.Down:
                player.animator.Play(PlayerAnimatorHashes.PlayerStand_U);
                break;
            case Direction.Up:
                player.animator.Play(PlayerAnimatorHashes.PlayerStand_D);
                break;
            case Direction.Left:
                player.animator.Play(PlayerAnimatorHashes.PlayerStand_R);
                break;
            case Direction.Right:
                player.animator.Play(PlayerAnimatorHashes.PlayerStand_L);
                break;
        }
        transitionInProgress = false;
        mirror.transitionInProgress = false;
    }

    /// <summary>
    /// Closes a door.
    /// </summary>
    void Close()
    {
        renderer.sprite = frames[0];
        mirror.renderer.sprite = mirror.frames[0];
        collider.enabled = true;
        mirror.collider.enabled = true;
        transitionInProgress = false;
        mirror.transitionInProgress = false;
        open = false;
        GetBoundsToOpen();
        mirror.open = false;
        mirror.GetBoundsToOpen();
    }

    /// <summary>
    /// Gets the bounds the door uses to open or draw player through.
    /// </summary>
    void GetBoundsToOpen()
    {
        if (open == true && condition == DoorCondition.Special) // special conditions make doors stay open, so we have to draw the bounds in a little to make that feel right
        {
            if (direction == Direction.Down || direction == Direction.Up)
            {
                boundsToOpen = new Bounds(collider.bounds.center, new Vector3(collider.bounds.size.x, collider.bounds.size.y - DoorSensitivityZone, collider.bounds.size.z));
            }
            else
            {
                boundsToOpen = new Bounds(collider.bounds.center, new Vector3(collider.bounds.size.x - DoorSensitivityZone, collider.bounds.size.y, collider.bounds.size.z));
            }
        }
        else
        {
            if (direction == Direction.Down || direction == Direction.Up)
            {
                boundsToOpen = new Bounds(collider.bounds.center, new Vector3(collider.bounds.size.x, collider.bounds.size.y + DoorSensitivityZone, collider.bounds.size.z));
            }
            else
            {
                boundsToOpen = new Bounds(collider.bounds.center, new Vector3(collider.bounds.size.x + DoorSensitivityZone, collider.bounds.size.y, collider.bounds.size.z));
            }
        }
    }

    /// <summary>
    /// Unlocks a door.
    /// </summary>
    void Unlock()
    {
        renderer.sprite = frames[0];
        mirror.renderer.sprite = mirror.frames[0];
        condition = postUnlockCondition;
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(boundsToOpen.min.x, boundsToOpen.min.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.max.x, boundsToOpen.min.y, boundsToOpen.min.z - 100));
        Gizmos.DrawLine(new Vector3(boundsToOpen.max.x, boundsToOpen.min.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.max.x, boundsToOpen.max.y, boundsToOpen.min.z - 100));
        Gizmos.DrawLine(new Vector3(boundsToOpen.min.x, boundsToOpen.min.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.min.x, boundsToOpen.max.y, boundsToOpen.min.z - 100));
        Gizmos.DrawLine(new Vector3(boundsToOpen.min.x, boundsToOpen.max.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.max.x, boundsToOpen.max.y, boundsToOpen.min.z - 100));

    }
#endif
}
