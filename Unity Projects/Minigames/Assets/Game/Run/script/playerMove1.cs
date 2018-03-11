using UnityEngine;
using System.Collections;

public class playerMove1 : MonoBehaviour
{
    public float speed = 20;

    void Start ()
    {

    }
    void Update ()
    {
        float inputx = Input.GetAxisRaw("Horizontal");
        float inputy = Input.GetAxisRaw("Vertical");
        transform.Translate(speed * inputx * Time.deltaTime, speed * inputy * Time.deltaTime, 0);
    }
}