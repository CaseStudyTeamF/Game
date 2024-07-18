using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

internal enum Shot
{
    
}

public class PlayerParticle : MonoBehaviour
{
    bool highSpeed = false;
    int count = 0;

    bool clicked = false;
    Vector3 holdPos = Vector3.zero;

    [SerializeField]
    GameObject normal, space, ice, superball, hold, hit;

    internal static bool holdEffect = false;

    static GameObject hitObj, clearObj, icy, bouncy, neutral, emitter;

    //int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        hitObj = hit;
        clearObj = space;
        icy = ice;
        bouncy = superball;
        neutral = normal;
        emitter = normal;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (PlayerMove.HighSpeed)
        {
            if (count % 5 == 0)
            {
                GameObject particle = normal;
                switch (PlayerMove.shootType)
                {
                    case ShootType.Anti_Gravity:
                        particle = space;
                        break;
                    case ShootType.Slip:
                        particle = ice;
                        break;
                    case ShootType.SuperBall:
                        particle = superball;
                        break;
                }

                Vector3 pos = new Vector3(Random.Range(-0.5f, 0.5f),
                                            Random.Range(-0.5f, 0.5f), 0);
                Instantiate(particle, transform.position + pos, transform.rotation);
            }

            count++;
        }


        if (Input.GetMouseButton(0) && holdEffect)
        {
            if (!clicked)
            {
                holdPos = Input.mousePosition;
                holdPos = Camera.main.ScreenToWorldPoint(holdPos);
                holdPos.z = 0;
            }
            GameObject particle = hold;
            Instantiate(particle, holdPos, transform.rotation);

            clicked = true;
        }
        else
        {
            clicked = false;
        }

        //if (Input.GetMouseButton(0) && frame % 5 == 0)
        //{
        //    Vector3 clickPos = Input.mousePosition;
        //    clickPos = Camera.main.ScreenToWorldPoint(clickPos);
        //    clickPos.z = 0;
        //    GameObject particle = ice;
        //    Instantiate(particle, clickPos, transform.rotation);
        //}

        //frame++;

    }

    internal static void hitEffect(Vector3 pos, bool constantAngle = false, bool constantSpeed = false)
    {
        float firstangle = Random.Range(0, 360);
        float a = 0;
        while (a < 360)
        {
            GameObject effect = Instantiate(hitObj, pos, Quaternion.identity);
            Particle p = effect.GetComponent<Particle>();
            p.angle = a;

            if (!constantAngle)
                p.angle += firstangle;

            if (!constantSpeed)
                p.speed = Random.Range(0.2f, 0.3f);

            a += 30;
        }
    }
    internal static void clearEffect(Vector3 pos)
    {
        float a = 0;
        while (a < 360)
        {
            GameObject effect = Instantiate(clearObj, pos, Quaternion.identity);
            Particle p = effect.GetComponent<Particle>();
            p.angle = a;
            p.speed = 0.1f;

            a += 30;
        }
    }
    internal static void circleEffect(Vector3 pos, ShootType type)
    {
        float a = 0;
        while (a < 360)
        {
            switch (type)
            {
                case ShootType.Anti_Gravity:
                    emitter = clearObj;
                    break;
                case ShootType.Slip:
                    emitter = icy;
                    break;
                case ShootType.SuperBall:
                    emitter = bouncy;
                    break;
                default:
                    emitter = neutral;
                    break;
            }
            GameObject effect = Instantiate(emitter, pos, Quaternion.identity);

            Particle p = effect.GetComponent<Particle>();
            p.angle = a;
            p.speed = 0.2f;

            a += 30;
        }
    }
}
