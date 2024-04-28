using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    private Rigidbody2D rigidBody2d;
    private Collider2D collider2d;

    void Start()
    {
        this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        this.collider2d = this.GetComponent<Collider2D>();
    }

    void Update()
    {
        if(this.transform.position.y < -100){
            Destroy(this.gameObject);            
        }
    }

    // 衝突判定   
    private void OnCollisionEnter2D(Collision2D collision)
    {  
        Destroy(this.gameObject);
    }

    private void OnTrigerEnter2D(Collision2D collision)
    {  
        Destroy(this.gameObject);
    }

}
