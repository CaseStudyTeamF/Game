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
        if(collision.transform.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            float totalImpulse = 0;
            foreach (ContactPoint2D contact in contacts)
            {
                totalImpulse += contact.normalImpulse;
            }

            if(!PlayerMove.TakeDamage())
            {
                PlayerParticle.hitEffect(transform.position);
                SoundPlayer.playSound(SE.Hit);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate(){
        if (this.rigidBody2d == null){
            return;
        }

        this.landingCheck();
        this.collideHorizontal();

        float speed = this.enemyMoveSpeed - Math.Abs(this.rigidBody2d.velocity.x);
        float control = this.isTouchGround ? 1.0f : 0.001f;
        Vector2 force = new Vector2(this.direction.x * speed * control, 0);
        this.rigidBody2d.AddForce(force);

    }


    void landingCheck()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 10f);
        this.isTouchGround = raycastHit2D.distance < transform.localScale.y * 2.0f + 0.1f;
    }

    void collideHorizontal()
    {
        Vector3 rayOrigin = this.transform.position + new Vector3(0, -this.transform.localScale.y / 2 + 0.1f);

        // 落下予測
        // 落下しそうな場合は折り返す
        const float rayLength =  2.0f;
        RaycastHit2D downRaycastHit = Physics2D.Raycast(rayOrigin, new Vector2(this.direction.x, Vector2.down.y), rayLength);
        //Debug.DrawRay(rayOrigin, new Vector2(this.direction.x, Vector2.down.y) * rayLength, Color.red, 0.1f, false);
        if (downRaycastHit.collider == null) {
            this.direction *= Vector2.left;
            this.rigidBody2d.velocity = Vector3.zero;   // いったん停止しないと速度次第で落下するので速度を0に(直接代入はよくないが)
            return;
        }
        
        // 左から右に方向転換
        RaycastHit2D raycastHit2DL = Physics2D.Raycast(rayOrigin, Vector2.left, 100f);
        if (raycastHit2DL.collider != null && raycastHit2DL.distance < this.transform.localScale.x * 2.0f + 0.1f) {
            this.direction = Vector2.right;
        }

        // 右から左にに方向転換
        RaycastHit2D raycastHit2DR = Physics2D.Raycast(rayOrigin, Vector2.right, 100f);
        if (raycastHit2DR.collider != null && raycastHit2DR.distance < this.transform.localScale.x * 2.0f + 0.1f) {
            this.direction = Vector2.left;
        }
        
        //Debug.Log("LEFT " + raycastHit2DL.distance);
        //Debug.Log("RIGHT " + raycastHit2DR.distance);
        //Debug.Log(direction);
    }

}
