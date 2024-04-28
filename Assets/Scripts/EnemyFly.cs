using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyFly : MonoBehaviour
{
    [SerializeField] private float moveLength = 1;
    [SerializeField] private float enemyMoveSpeed = 10;
    [SerializeField, Header("s")] private float interval = 10000.0f; 
    [SerializeField] private bool isTrigger = false;
    [SerializeField] private float fallStartPos = -2.0f; 
    [SerializeField] private GameObject fallObject; 


    private Rigidbody2D rigidBody2d;
    private Collider2D collider2d;
    private float startX = 0;
    private float timeSinceLastSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        this.collider2d = this.GetComponent<Collider2D>();
        this.startX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // 生成間隔経過時間を計測
        this.timeSinceLastSpawn += Time.deltaTime;

        // 生成間隔になったらオブジェクトを生成
        if (this.timeSinceLastSpawn > this.interval)
        {
            // オブジェクトを生成
            GameObject newObject = Instantiate(
                this.fallObject,
                new Vector3(
                    this.transform.position.x,
                    this.transform.position.y + this.fallStartPos,
                    this.transform.position.z
                    ),
                Quaternion.identity
            );

            // 生成間隔タイマーをリセット
            timeSinceLastSpawn = 0.0f;
        }
    }

    private void FixedUpdate(){
        if (this.rigidBody2d == null){
            return;
        }



        this.transform.position = new Vector3(this.startX + (float)Math.Sin(Time.time * this.enemyMoveSpeed) * this.moveLength, this.transform.position.y, this.transform.position.z);
    }
}

