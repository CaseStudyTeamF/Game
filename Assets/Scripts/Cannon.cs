using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField]GameObject targetObject;

    Rigidbody2D rigidBody2d;

    GameObject player;
    Vector3 direction;

    bool isMove = false;

    float  elapsedTime = 0.0f;

    // Start is called before the first frame update
    void Start() {
        
        Debug.Log(time);
        this.direction = this.targetObject.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void FixedUpdate() {
        if(!this.isMove){
            return;
        }
        
        this.elapsedTime += Time.deltaTime;

        if(this.elapsedTime > this.time){
            this.elapsedTime = 0;
            this.isMove = false;
            return;
        }

        Debug.Log(time);

        // 自分の位置と送り先の差分
        float now = this.elapsedTime / this.time;
        float angle = now * 180 + 180;
        float per = (Mathf.Cos(angle * Mathf.Deg2Rad) + 1) / 2;

        this.player.transform.position = this.transform.position + this.direction * per;
        rigidBody2d.velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            this.player = collision.gameObject;
            this.rigidBody2d = this.player.GetComponent<Rigidbody2D>();
            this.isMove = true;
        }
    }

}
