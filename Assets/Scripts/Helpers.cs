using UnityEngine;

public static class Helpers
{
    private static PlayerAnimation playerAnimation;

    public static PlayerAnimation PlayerAnimation
    {
        get
        {
            if (playerAnimation == null) playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
            return playerAnimation;
        }
    }
}
