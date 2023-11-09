using UnityEngine;

public static class Helpers
{
    private static PlayerAnimation playerAnimation;
    private static AudioManager audioManager;

    public static PlayerAnimation PlayerAnimation
    {
        get
        {
            if (playerAnimation == null) playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
            return playerAnimation;
        }
    }
    
    public static AudioManager AudioManager
    {
        get
        {
            if(audioManager == null) audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();
            return audioManager;
        }
    }

}
