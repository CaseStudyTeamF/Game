using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] int Life = 3;

    float targetSpeed;

    int coolDown = 0;
    float HitStop = 0;
    bool jumpInput = false;

    // �������菈���p
    [SerializeField] float MinPower = 100;
    [SerializeField] float MaxPower = 200;

    Vector3 clickStartPos;
    Vector2 power;


    Rigidbody2D rigidBody2d;

    [SerializeField] GameObject mainCamera;
    CameraMove camMove;
    [SerializeField] GameObject powerArrow;
    PowerArrowBehaviour arrow;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        camMove = mainCamera.GetComponent<CameraMove>();
        arrow = powerArrow.GetComponent<PowerArrowBehaviour>();
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
        UpdateStatus();
        HorizontalMove();
        Jump();

        coolDown = Mathf.Max(0, coolDown - 1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // ���k�i����j�̏���
        if (collision.CompareTag("PressMachine"))
        {
            // �N���b�N���̏���
            if (Input.GetMouseButton(0))
            {
                // �N���b�N���̏���
                if(clickStartPos == Vector3.zero)
                {
                    clickStartPos = Input.mousePosition;
                }
                else
                {
                    // ���������̏���
                    power = clickStartPos - Input.mousePosition;

                    // �͂̏��
                    if(power.magnitude > MaxPower)
                    {
                        float powerCorrect = MaxPower / power.magnitude;
                        power.x *= powerCorrect; 
                        power.y *= powerCorrect;
                    }

                    // �������ɂ͔�΂Ȃ��悤�ɂ���
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

                    arrow.drawUpdate(power);
                    
                    // �͂��シ����Ȃ���������i��΂��Ȃ��̂Łj 
                    if(power.magnitude < MinPower)
                    {
                        powerArrow.SetActive(false);
                    } else
                    {
                        powerArrow.SetActive(true);
                    }
                }
                
            }


            // �����[�X����
            if (!Input.GetMouseButton(0) && clickStartPos != Vector3.zero)
            {
                if (power.magnitude >= MinPower && coolDown <= 0)
                {
                    Life--;
                    rigidBody2d.AddForce(power * 0.1f * rigidBody2d.mass, ForceMode2D.Impulse);
                    coolDown = 60;
                }
                clickStartPos = Vector3.zero;
                powerArrow.SetActive(false);

            }
        }
    }


    // �Փˎ��̉��o
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

        if (totalImpulse > 40)
        {
            Debug.Log("Stop");
            //HitStop = 0.02f;
            //Time.timeScale = 0;
        }
    }


    void UpdateStatus()
    {
        switch(Life)
        {
            // ����4�Ŕ�Ԏ��͖���
            case 4:
                rigidBody2d.mass = 5;
                rigidBody2d.drag = 0;
                break;
            case 3:
                rigidBody2d.mass = 3;
                rigidBody2d.drag = 0;
                break;
            case 2:
                rigidBody2d.mass = 2;
                rigidBody2d.drag = 1;
                break;
            case 1:
                rigidBody2d.mass = 1;
                rigidBody2d.drag = 2;
                break;

        }
        transform.localScale = new Vector3(0.1f * Life + 0.2f, 0.1f * Life + 0.2f);
    }

    // ���E�ړ��̏���
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

    // �W�����v�̏���
    void Jump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if (jumpInput == false)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 10f);
                //Debug.Log(raycastHit2D.distance);
                if (raycastHit2D.distance < transform.localScale.y * 2.0f + 0.1f)
                {
                    rigidBody2d.AddForce(new Vector3(0, 16), ForceMode2D.Impulse);
                }
            }
            jumpInput = true;
        } else
        {
            jumpInput = false;
        }
    }




}
