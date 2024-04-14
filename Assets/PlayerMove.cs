using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigidBody2d;
    float targetSpeed;

    int coolDown = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        targetSpeed = 0;
        if (Input.GetKey(KeyCode.A))
        {
            targetSpeed = -5;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetSpeed = 5;
        }

        if (Input.GetKey(KeyCode.RightArrow))
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
            transform.rotation = Quaternion.Euler(0, 0, 90);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.rotation = Quaternion.Euler(0, 0, 180);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.rotation = Quaternion.Euler(0, 0, 270);

    }

    private void FixedUpdate()
    {
        MoveVer2();
        coolDown = Mathf.Max(0, coolDown - 1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 圧縮（解放）の処理
        if (collision.CompareTag("PressMachine"))
        {
            if(Input.GetKey(KeyCode.Space) && coolDown <= 0) 
            {
                Vector2 force;
                force.x = 60 * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad);
                force.y = 60 * Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad);

                rigidBody2d.AddForce(force, ForceMode2D.Impulse);
                coolDown = 60;
            }
        }
    }

    void Move()
    {

        float rangedVelocity = Mathf.Max(Mathf.Min(rigidBody2d.velocity.x, 5), -5);

        // 飛び出してすぐの間は減速しない
        if (coolDown > 0)
            rangedVelocity = 0;

        float moveSpeed = targetSpeed - rangedVelocity;

        Vector2 moveForce = new Vector2(moveSpeed * 8.0f, 0);

        rigidBody2d.AddForce(moveForce);

        Debug.Log(moveForce);
    }


    // 左右移動の処理
    void MoveVer2()
    {
        float velocity = 0;

        if (targetSpeed > 0)
            velocity = Mathf.Clamp(targetSpeed - rigidBody2d.velocity.x, 0, targetSpeed);

        if (targetSpeed < 0)
            velocity = Mathf.Clamp(targetSpeed - rigidBody2d.velocity.x, targetSpeed, 0);


        Vector2 moveForce = new Vector2(velocity * 8.0f, 0);

        rigidBody2d.AddForce(moveForce);

    }






}
