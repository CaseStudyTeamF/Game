using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float enemyMoveSpeed = 10;
    [SerializeField, Header("最初に歩く方向")] private bool isLeftMove = true; 

    private Rigidbody2D rigidBody2d;
    private Vector2 direction = Vector2.left;
    private bool isTouchGround = true; // 地面に接触しているか

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        this.direction = this.isLeftMove ? Vector2.left : Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {  
        if (collision.gameObject.CompareTag("Wall")){
            this.direction *= Vector2.left; // 壁に接触した場合は方向を反転させる
        }
        if(collision.gameObject.CompareTag("Ground")){
            this.isTouchGround = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")){
            this.isTouchGround = false;
        }
    }


    private void FixedUpdate(){
        if (this.rigidBody2d == null){
            return;
        }

        float speed = this.enemyMoveSpeed - Math.Abs(this.rigidBody2d.velocity.x);
        float control = this.isTouchGround ? 1.0f : 0.001f;
        float fallSpeed = -Math.Abs(this.rigidBody2d.velocity.y);    // 絶対落とすマン
        Vector2 force = new Vector2(this.direction.x * speed * control, fallSpeed);
        this.rigidBody2d.AddForce(force);
    }

}
