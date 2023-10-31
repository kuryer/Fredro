using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    AudioManager manager;
    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();
    }
    public void AudioStep()
    {
        manager.Play("Player_Step");
    }
    public void AudioGotHit()
    {
        manager.Play("Player_GotHitFixed");
    }
    public void AudioRepair()
    {
        manager.Play("Player_Repair");
    }
    public void AudioJump()
    {
        manager.Play("Player_Jump");
    }
}
