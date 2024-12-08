using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   private float speed = 10f;

    private bool isJumping;
    private Rigidbody2D body;
    private bool onground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        private void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        isJumping = false;
    }


    private void FixedUpdate() {
        
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocityY);
        if (!isJumping && Input.GetKey(KeyCode.UpArrow)) {
        body.linearVelocity = new Vector2(body.linearVelocityX, speed);

        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Platform") {
            isJumping = false;
        }

        if (col.gameObject.tag == "Spike") {
           dead();
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Spike") {

            dead();
        }
        if (col.gameObject.tag == "Door") {
            Console.WriteLine("You cleared level");
        }

    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Platform") {
            isJumping = true;
        }
    }

    void dead() {
        // need to implement if player dies
    }

    

    
}
