using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement playerMovement;
    bool isHit;
    [SerializeField] GameObject Health_1;
    [SerializeField] GameObject Health_2;
    [SerializeField] GameObject Health_3;
    [SerializeField] LevelLoader levelLoader;
    int hp;
    void Start()
    {
        HealthSetup();
    }

    public void PlayerGotHit()
    {
        if (isHit)
            return;
        hp--;
        LoseHeartUI();
        playerMovement.PlayerHit();
    }

    void LoseHeartUI()
    {
        if(hp == 2)
            Health_3.SetActive(false);
        if(hp == 1)
            Health_2.SetActive(false);
        if (hp == 0)
        {
            Health_1.SetActive(false);
            //levelLoader.GameOver();
        }
    }

    public void PlayerConcious()
    {
        playerMovement.SetCanMove(true);
    }

    public void PlayerDies()
    {
        if (hp == 0)
            levelLoader.GameOver();
    }

    public void IsHitTrue()
    {
        isHit = false;
        playerMovement.SetIsHit(isHit);
    }

    void HealthSetup()
    {
        hp = 3;
        playerMovement = GetComponent<PlayerMovement>();
    }

}
