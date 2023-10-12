using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement playerMovement;
    bool isHit;
    void Start()
    {
        HealthSetup();
    }

    public void PlayerGotHit()
    {
        if (isHit)
            return;
        playerMovement.PlayerGotHit();
    }

    public void PlayerConcious()
    {
        playerMovement.SetCanMove(true);
    }

    public void IsHitTrue()
    {
        isHit = true;
        playerMovement.SetIsHit(isHit);
    }

    void HealthSetup()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

}
