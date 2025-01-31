using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private variable
    private float dir;
    private bool facingright = true;
    private bool isGrounded = true;
    //private bool isAttack = false;

    //public variable
    public float speed = 2f;
    public Rigidbody2D Rb_source;
    public float jump_height = 5f;
    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // variabel dir untuk menentukan kanan dan kiri
        dir = Input.GetAxis("Horizontal");
        if (dir < 0 && facingright)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingright = false;
        }
        else if (dir > 0 && facingright == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingright = true;
        }

        //jump hendling
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jump();
        }

        //animation hendling
        if (dir != 0f)
        {
            anim.SetFloat("speed", 1f);
        }
        else if (dir == 0f)
        {
            anim.SetFloat("speed", 0f);
        }

        //attack animation hendling
        if (Input.GetMouseButtonDown(0)){
            anim.SetTrigger("attack");
        }
    }

    private void FixedUpdate()
    {
        // move player
        transform.position += speed * Time.fixedDeltaTime * new Vector3(dir, 0f, 0f);
    }

    void jump()
    {
        Rb_source.linearVelocity = new Vector2(Rb_source.linearVelocity.x, 0f); // Reset kecepatan vertikal agar lompatan konsisten
        Rb_source.AddForce(Vector2.up * jump_height, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("jump",false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            anim.SetBool("jump",true);
        }

    }
}