using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ShootType
{
    Normal,
    Anti_Gravity,
    SuperBall,
    Slip,
    Size
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Sprite Powerful;
    [SerializeField] Sprite Normal;
    [SerializeField] Sprite Baby;
    [SerializeField] Vector3 initPos =  new Vector3(-14, -7, 0);
    [SerializeField] float deathByFallPos =  -30;

    public static ShootType shootType = ShootType.Normal;
    public static bool[] shooterPermission = new bool[(int)ShootType.Size];

    public static int Life = 3;

    float targetSpeed;

    int coolDown = 0;
    float HitStop = 0;
    bool jumpInput = false;
    float jumpPower = 0;
    
    static bool HighSpeed = false;
    static int invisTime = 0;

    bool rightClick = false;

    // 引っ張り処理用
    [SerializeField] float MinPower = 100;
    [SerializeField] float MaxPower = 200;

    Vector3 clickStartPos;
    Vector2 power;


    Rigidbody2D rigidBody2d;
    SpriteRenderer sr;

    [SerializeField] GameObject mainCamera;
    CameraMove camMove;
    [SerializeField] GameObject powerArrow;
    PowerArrowBehaviour arrow;

    [Header("当たり判定")]
    [SerializeField] CapsuleCollider2D X_Collider;
    [SerializeField] CapsuleCollider2D Y_Collider;

    [Header("マテリアル")]
    [SerializeField] PhysicsMaterial2D friction;
    [SerializeField] PhysicsMaterial2D bound;
    [SerializeField] PhysicsMaterial2D slip;


    // Start is called before the first frame update
    void Start()
    {
        Life = 3;
        rigidBody2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        camMove = mainCamera.GetComponent<CameraMove>();
        arrow = powerArrow.GetComponent<PowerArrowBehaviour>();

        // パチンコの制限
        shooterPermission[(int)ShootType.Normal] = true;
        shooterPermission[(int)ShootType.Anti_Gravity] = false;
        shooterPermission[(int)ShootType.SuperBall] = false;
        shooterPermission[(int)ShootType.Slip] = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetSpeed = 0;
        if (Input.GetKey(KeyCode.A))
        {
            targetSpeed = -6f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetSpeed = 6f;
        }
        
        if(HitStop > 0)
        {
            HitStop -= Time.unscaledDeltaTime;
        } else
        {
            HitStop = 0;
            Time.timeScale = 1;
        }

    }

    private void FixedUpdate()
    {
        if (this.transform.position.y < this.deathByFallPos){
            this.transform.position = this.initPos;
            this.rigidBody2d.velocity = Vector3.zero;
            Life--;
        }        

        if(Life <= 0)
        {
            Life = 3;
            transform.position = this.initPos;
            rigidBody2d.velocity = Vector3.zero;
        }

        //Debug.Log(HighSpeed);
        Invincible();
        if (HighSpeed)
        {
            // 色
            Color color = sr.color;
            switch(shootType)
            {
                case ShootType.Normal:
                    color = new Color32(255, 255, 0, 255);
                    break;
                case ShootType.Anti_Gravity:
                    color = new Color32(64, 64, 64, 255);
                    break;
                case ShootType.SuperBall:
                    color = new Color32(255, 0, 165, 255);
                    break;
                case ShootType.Slip:
                    color = new Color32(0, 255, 255, 255);
                    break;
            }
            //color.a = Mathf.Max(50 - rigidBody2d.velocity.magnitude, 0) / 50;
            sr.color = color;


            if (rigidBody2d.velocity.magnitude < 10)
            {
                HighSpeed = false;

                // 反重力解除
                rigidBody2d.gravityScale = 1;
                // 弾性解除
                X_Collider.sharedMaterial = slip;
                Y_Collider.sharedMaterial = friction;
                // 滑り解除
                Y_Collider.sharedMaterial = friction;
            }
        }


        UpdateStatus();
        HorizontalMove();
        Jump();

        coolDown = Mathf.Max(0, coolDown - 1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 圧縮（解放）の処理
        if (collision.CompareTag("PressMachine"))
        {
            // クリック中の処理
            if (Input.GetMouseButton(0))
            {
                // クリック時の処理
                if(clickStartPos == Vector3.zero)
                {
                    clickStartPos = Input.mousePosition;
                }
                else
                {
                    // 長押し中の処理
                    power = clickStartPos - Input.mousePosition;

                    // 力の上限
                    if(power.magnitude > MaxPower)
                    {
                        float powerCorrect = MaxPower / power.magnitude;
                        power.x *= powerCorrect; 
                        power.y *= powerCorrect;
                    }

                    // 下方向には飛ばないようにする
                    float angle = Mathf.Atan2(power.y, power.x) * Mathf.Rad2Deg;
                    if (angle < 0 && angle > -90)
                    {
                        power.x = power.magnitude;
                        power.y = 0;
                    }
                    if(angle <= -90)
                    {
                        power.x = -power.magnitude;
                        power.y = 0;
                    }

                    Vector2 arrowSize = power / transform.localScale / 2.0f;
                    arrow.drawUpdate(arrowSize);
                    
                    // 力が弱すぎるなら矢印を消す（飛ばせないので）
                    if(power.magnitude < MinPower)
                    {
                        powerArrow.SetActive(false);
                    } else
                    {
                        powerArrow.SetActive(true);
                    }
                }
                
            }

            if(Input.GetMouseButton(1))
            {
                if(!rightClick)
                {
                    switch (shootType)
                    {
                        case ShootType.Normal:
                            shootType = ShootType.Anti_Gravity;
                            break;
                        case ShootType.Anti_Gravity:
                            shootType = ShootType.SuperBall;
                            break;
                        case ShootType.SuperBall:
                            shootType = ShootType.Slip;
                            break;
                        case ShootType.Slip:
                            shootType = ShootType.Normal;
                            break;
                    }
                }
                rightClick = true;
            } else
            {
                rightClick = false;
            }


            // リリース処理
            if (!Input.GetMouseButton(0) && clickStartPos != Vector3.zero)
            {
                if (power.magnitude >= MinPower && coolDown <= 0)
                {
                    Life--;
                    rigidBody2d.AddForce(power * 0.15f * rigidBody2d.mass, ForceMode2D.Impulse);
                    coolDown = 60;
                    HighSpeed = true;
                    SoundPlayer.playSound(SE.Shot);

                    switch(shootType)
                    {
                        case ShootType.Normal:
                            break;
                        case ShootType.Anti_Gravity:
                            rigidBody2d.gravityScale = -1;
                            break;
                        case ShootType.SuperBall:
                            X_Collider.sharedMaterial = bound;
                            Y_Collider.sharedMaterial = bound;
                            break;
                        case ShootType.Slip:
                            Y_Collider.sharedMaterial = slip;
                            break;
                    }
                }
                clickStartPos = Vector3.zero;
                powerArrow.SetActive(false);

            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PressMachine"))
        {
            powerArrow.SetActive(false);
            clickStartPos = Vector3.zero;
        }
    }

    // 衝突時の演出
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        float totalImpulse = 0;
        foreach(ContactPoint2D contact in contacts)
        {
            totalImpulse += contact.normalImpulse;
        }
        //Debug.Log(totalImpulse);

        if(collision.gameObject.CompareTag("Door"))
        {
            if (HighSpeed)
            {
                SceneManager.LoadScene("Result");
            }
        }
    }


    void UpdateStatus()
    {
        switch(Life)
        {
            case 3:
                rigidBody2d.mass = 2;
                sr.sprite = Powerful;
                jumpPower = 7;
                break;
            case 2:
                rigidBody2d.mass = 1;
                sr.sprite = Normal;
                jumpPower = 10;
                break;
            case 1:
                rigidBody2d.mass = 0.5f;
                sr.sprite = Baby;
                jumpPower = 12;
                break;

        }
        transform.localScale = new Vector3(Life * 0.125f, Life * 0.125f);
    }

    // 左右移動の処理
    void HorizontalMove()
    {
        float velocity = 0;

        if (targetSpeed > 0)
            velocity = Mathf.Clamp(targetSpeed - rigidBody2d.velocity.x, 0, targetSpeed);

        if (targetSpeed < 0)
            velocity = Mathf.Clamp(targetSpeed - rigidBody2d.velocity.x, targetSpeed, 0);


        Vector2 moveForce = new Vector2(velocity * 8.0f, 0);

        rigidBody2d.AddForce(moveForce * rigidBody2d.mass);
    }

    // ジャンプの処理
    void Jump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if (jumpInput == false)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 10f);

                // レイが当たらない（地面が遠すぎる）時の処理
                if (raycastHit2D == false)
                    return;

                if (raycastHit2D.distance < transform.localScale.y * 4.0f + 0.5f)
                {
                    rigidBody2d.AddForce(new Vector3(0, jumpPower) * rigidBody2d.mass, ForceMode2D.Impulse);
                }
            }
            jumpInput = true;
        } else
        {
            jumpInput = false;
        }
    }


    // 被弾時の無敵処理
    void Invincible()
    {
        if(invisTime > 0)
        {
            Color color = Color.white;
            color.a = 0.5f;
            sr.color = color;
            invisTime--;
        } else
        {
            sr.color = Color.white;
        }
    }


    // 被弾時の処理
    public static bool TakeDamage()
    {
        if (HighSpeed || Life >= 3)
            return false;

        if(invisTime <= 0)
        {
            Life--;
            invisTime = 60;
        }
        return true;
    }

}
