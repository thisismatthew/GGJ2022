using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float CharacterSpeed;
    private float horizontal, vertical;
    private Vector2 inputDir;
    private Rigidbody2D rb;
    public bool Moving;
    //public Color c_moving, c_stopped;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Debug.Log(inputDir);
        inputDir = new Vector2(horizontal, vertical).normalized;
        if (inputDir != Vector2.zero)
        {
            Moving = true;
            rb.velocity = inputDir * CharacterSpeed;
            //rb.MovePosition((Vector2)transform.position + (inputDir * CharacterSpeed));
        }
        else
        {
            Moving = false;
            //GetComponent<SpriteRenderer>().color = c_stopped;
            rb.velocity = Vector2.zero;
        }
    }
}
