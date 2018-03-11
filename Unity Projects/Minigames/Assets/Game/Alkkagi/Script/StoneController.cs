using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class StoneController : MonoBehaviour {
    private Rigidbody rigid;
    private PhotonView photonView;

    public ParticleSystem explosion;
    public SpriteRenderer arrow;

    private Vector2 posStart;
    private Vector2 posEnd;
    private Vector2 vec2;
    private int max_power = 250;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        photonView = PhotonView.Get(this);
        arrow.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (rigid.position.z > 0) {
            if (gameObject.name == "Black-Stone")
                ScoreManagerAlkkagi.whiteScore++;
            else if (gameObject.name == "White-Stone")
                ScoreManagerAlkkagi.blackScore++;

            Destroy(gameObject);        
        }
        
        if (rigid.velocity.x == 0 && rigid.velocity.y == 0)
        {
            TurnManagerAlkkagi.isTurn = true;
        }
        

        /*
        if (rigid.rotation.z > 0) {
            Debug.Log(vec.z);
            rotation();
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Black-Stone" || collision.gameObject.name == "White-Stone")
            Instantiate(explosion, transform.position, Quaternion.identity);
    }



    void Rotation()
    {
        rigid.MoveRotation(Quaternion.Euler(0, 0, 0));
    }

    void OnMouseUp()
    {
        if (TurnManagerAlkkagi.isTurn && GameManagerAlkkagi.playerColor == TurnManagerAlkkagi.color)
        {
            Vector2 mouseDragPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objectPositon = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 vec = (objectPositon - mouseDragPosition);

            if (arrow.enabled)
                arrow.enabled = false;

            if (vec.x > max_power)
                vec.x = max_power;
            else if (vec.x < -max_power)
                vec.x = -max_power;
            if (vec.y > max_power)
                vec.y = max_power;
            else if (vec.y < -max_power)
                vec.y = -max_power;

            if (gameObject.name == "Black-Stone" && GameManagerAlkkagi.playerColor == 0)
            {
                //TurnManagerAlkkagi.isTurn = false;
                photonView.RPC("Shot", PhotonTargets.All, vec);
            }

            if (gameObject.name == "White-Stone" && GameManagerAlkkagi.playerColor == 1)
            {
                //TurnManagerAlkkagi.isTurn = false;
                photonView.RPC("Shot", PhotonTargets.All, vec);
            }
            
        }
    }
    static public Quaternion GetRotFromVectors(Vector2 posStart, Vector2 posEnd)
    {
        return Quaternion.Euler(0, 0, -Mathf.Atan2(posEnd.x - posStart.x, posEnd.y - posStart.y) * Mathf.Rad2Deg);
    }

    void OnMouseDown()
    {
        if (TurnManagerAlkkagi.isTurn && (GameManagerAlkkagi.playerColor == TurnManagerAlkkagi.color))
        {
            if(gameObject.name == "Black-Stone" && GameManagerAlkkagi.playerColor == 0)
            {
                EnableArrow();
            }
            if (gameObject.name == "White-Stone" && GameManagerAlkkagi.playerColor == 1)
            {
                EnableArrow();
            }
        }
    }
    void EnableArrow()
    {
        posStart = Camera.main.WorldToScreenPoint(transform.position);
        if (!arrow.enabled)
        {
            arrow.transform.localScale = new Vector2(2, 1);
            arrow.enabled = true;
        }
    }

    void OnMouseDrag()
    {
        if (TurnManagerAlkkagi.isTurn && GameManagerAlkkagi.playerColor == TurnManagerAlkkagi.color)
        {
            posEnd = Input.mousePosition;

            vec2 = (posStart - posEnd);
           
            if (vec2.x > max_power)
                vec2.x = max_power;
            else if (vec2.x < -max_power)
                vec2.x = -max_power;
            if (vec2.y > max_power)
                vec2.y = max_power;
            else if (vec2.y < -max_power)
                vec2.y = -max_power;


            vec2.y = Mathf.Max(Mathf.Abs(vec2.x), Mathf.Abs(vec2.y));
            vec2.y /= 10;
            vec2.x = 4;

            arrow.transform.localScale = vec2;

            arrow.transform.rotation = GetRotFromVectors(posEnd, posStart);
        } else
        {
            arrow.enabled = false;
        }
    }

    [PunRPC]
    void Shot(Vector3 vec)
    {
        rigid.AddForce((vec), ForceMode.Impulse);
        TurnManagerAlkkagi.color = (TurnManagerAlkkagi.color + 1) % 2;
        TimeManagerAlkkagi.time = 15;
    }

}
