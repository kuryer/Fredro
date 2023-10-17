using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    int score;
    [SerializeField] TextMeshProUGUI textMeshPro;


    private void Update()
    {
        textMeshPro.text = "Score " + score.ToString();
    }

    private void OnEnable()
    {
        StartCoroutine(AddScore());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator AddScore()
    {
        while (true)
        {
            score += 5;
            yield return new WaitForSeconds(3f);
        }
    }
    public void AddPointsForFixedTile()
    {
        score += 100;
    }
}
