using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Managers")]
    [SerializeField] List<LevelManager> levels;
    [SerializeField] LevelManager previousLevelManager;
    [SerializeField] LevelManager activeLevelManager;
    [SerializeField] int activeLevelManagerIndex = 0;

    [Header("Progress Levels")]
    [SerializeField] int actualLevel = 0;

    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [Header("Ladder")]
    public int lastLadderPosition;
    private void Awake()
    {
        lastLadderPosition = levels[0].GenerateLevel(-1);
        lastLadderPosition = levels[1].GenerateLevel(lastLadderPosition);
        lastLadderPosition = levels[2].GenerateLevel(lastLadderPosition);
        lastLadderPosition = levels[3].GenerateLevel(lastLadderPosition);
    }

    public void GoToNextLevel()
    {
        GetNextLevel();
        ChangeCamera();
        MoveAndGenerateLevels();
        InitializeAndClosePrevious();
    }

    void GetNextLevel()
    {
        if (activeLevelManagerIndex == levels.Count - 1)
            activeLevelManagerIndex = 0;
        else
            activeLevelManagerIndex++;

        previousLevelManager = activeLevelManager;
        activeLevelManager = levels[activeLevelManagerIndex];
        actualLevel++;
    }

    void ChangeCamera()
    {
        virtualCamera.Follow = activeLevelManager.cameraFollow.transform;
    }

    void MoveAndGenerateLevels()
    {
        if (actualLevel < 2)
            return;
        LevelManager toMove = levels[GetLevelManagerIndex()];
        toMove.MoveUp();
        int newLadderPosition = toMove.GenerateLevel(lastLadderPosition);
        lastLadderPosition = newLadderPosition;
    }

    int GetLevelManagerIndex()
    {
        int index = activeLevelManagerIndex - 2;
        if (index == -2)
            return 2;
        if (index == -1)
            return 3;

        Debug.Log(index);
        return index;
    }

    void InitializeAndClosePrevious()
    {
        previousLevelManager.DeactivateLevel();
        activeLevelManager.ActivateLevel();
    }
}
