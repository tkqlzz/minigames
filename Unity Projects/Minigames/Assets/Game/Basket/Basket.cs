using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour
{
    public GameObject score;

    public GameObject star;
    public GameObject roundObj;
    public AudioClip basket;

    public GameObject hoop;
    
    public static int goalCount;
    public static int round;

    // Use this for initialization
    void Start()
    {
        goalCount = 0;
        round = 1;
        StartCoroutine(MoveHoop());
    }

    private void Update()
    {
        score.GetComponent<GUIText>().text = ScoreManagerBasket.score.ToString();
        star.GetComponent<GUIText>().text = ScoreManagerBasket.starPoint.ToString();
        roundObj.GetComponent<GUIText>().text = round.ToString();
    }

    IEnumerator MoveHoop()
    {
        Vector3 vec = hoop.transform.position;
        while (true)
        {
            while (round == 4 || round == 5)
            {
                while (hoop.transform.position.x < -2f)
                {
                    vec.x += 0.1f;
                    hoop.transform.position = vec;
                    yield return new WaitForSeconds(0.01f);
                }
                while (hoop.transform.position.x > -22f)
                {
                    vec.x -= 0.1f;
                    hoop.transform.position = vec;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

        void OnCollisionEnter()
    {
        GetComponent<AudioSource>().Play();
    }

    void OnTriggerEnter()
    {
        if (GameManagerBasket.isUseScoreBooster)
            ScoreManagerBasket.score += 15;
        else
            ScoreManagerBasket.score += 10;
        ScoreManagerBasket.starPoint++;
        goalCount++;
        AudioSource.PlayClipAtPoint(basket, transform.position);
    }
}