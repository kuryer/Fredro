using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;
    public void ChangeScene(int index)
    {
        levelLoader.LoadIndexScene(index);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

}
