using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoadManager : Manager<LevelLoadManager>
{
    public static int[] preWorldChangeLevelSceneIDs =
    {
        2, // testmap
        4, // village
        6, // forest
        8, // valley
        10, // marina
        12, // circle
        3, // tutorial/intro
        13, // clockwork tower ascent
        14, // endgame
    };
    public static int[] postWorldChangeLevelSceneIDs =
    {
        2, // testmap
        5, // village
        7, // forest
        9, // valley
        11, // marina
        12, // circle
        3, // tutorial/intro
        13, // clockwork tower ascent
        14, // endgame
    };

    public void EnterLevel (AreaType level, int[] roomCoords, int EntryPointIndex, Direction playerInitialFacingDir)
    {
        int index;
        if ((GameStateManager.Instance.eventFlags_Global & EventFlags_Global.MidgameWorldChanges) == EventFlags_Global.MidgameWorldChanges)
        {
            index = postWorldChangeLevelSceneIDs[(int)level];
        }
        else
        {
            index = preWorldChangeLevelSceneIDs[(int)level];
        }
        StartCoroutine(_in_EnterLevel(index, roomCoords, EntryPointIndex, playerInitialFacingDir));
    }

    IEnumerator _in_EnterLevel(int index, int[] roomCoords, int EntryPointIndex, Direction playerInitialFacingDir)
    {
        switch (playerInitialFacingDir)
        {
            case Direction.Down:
                GameStateManager.Instance.levelLoadPlayerAnimHash = PlayerAnimatorHashes.PlayerStand_D;
                break;
            case Direction.Up:
                GameStateManager.Instance.levelLoadPlayerAnimHash = PlayerAnimatorHashes.PlayerStand_U;
                break;
            case Direction.Left:
                GameStateManager.Instance.levelLoadPlayerAnimHash = PlayerAnimatorHashes.PlayerStand_L;
                break;
            case Direction.Right:
                GameStateManager.Instance.levelLoadPlayerAnimHash = PlayerAnimatorHashes.PlayerStand_L;
                break;
        }
        for (int i = 0; i < 8; i++)
        {
            yield return null; // this gives the scene calling EnterLevel time to do neat screen transitions, etc. even if we load instantly
        }
        AsyncOperation loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        while (loading.isDone == false)
        {
            yield return null;
        }
        WorldController world;
        try
        {
            world = GameObject.Find("World").GetComponent<WorldController>();
        }
        catch (System.NullReferenceException)
        {
            throw new System.Exception("LevelLoadManager loaded a scene that isn't configured as a valid level. Scene name: " + SceneManager.GetActiveScene().name + ", scene index: " + SceneManager.GetActiveScene().buildIndex);
        }
        RoomController DestinationRoom = world.rooms[roomCoords[0], roomCoords[1]];
        world.player.DontWarp = true;
        world.player.transform.position = new Vector3(DestinationRoom.bounds.min.x + DestinationRoom.EntryPoints[EntryPointIndex].x,
            DestinationRoom.bounds.min.y + (HammerConstants.SizeOfOneTile * 1.5f) + DestinationRoom.EntryPoints[EntryPointIndex].y, world.player.transform.position.z);
        world.player.mover.virtualPosition = world.player.transform.position;
        world.player.mover.heading = Vector3.zero;
        Debug.Log("Loaded scene: " + SceneManager.GetActiveScene().name);
        StartCoroutine(world.cameraController.InstantChangeScreen(DestinationRoom, 30));

    }
}
