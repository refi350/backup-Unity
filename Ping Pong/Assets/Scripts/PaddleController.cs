using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 1f;
    public int playerIndex = 1;
    public float limit = 0.1f;
    public Transform pos;
    float verticalMovement;
    public BoxCollider2D spriteSizeY;
    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log(spriteHeight);
    }

    // Update is called once per frame
    void Update()
    {
        verticalMovement = Input.GetAxis("Vertical" + playerIndex);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, verticalMovement * speed);

        float spriteHeight = spriteSizeY.bounds.size.y;
        float screenLimit = Camera.main.orthographicSize * Camera.main.aspect - spriteHeight / 2 - limit;
        if(pos.transform.position.y > screenLimit)
        {
            transform.position = new Vector2(transform.position.x, screenLimit);
        }
        else if(pos.transform.position.y < -screenLimit)
        {
            transform.position = new Vector2(transform.position.x, -screenLimit);
        }
    }
}

