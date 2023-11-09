using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioStopper : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(audioManager.FadeOut("Enemy_Walk", 3f));
        }
    }
}
