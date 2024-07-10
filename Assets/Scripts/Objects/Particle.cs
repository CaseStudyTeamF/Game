using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField]
    float rotation = 0;

    [SerializeField]
    int duration = 60;

    [SerializeField]
    bool random = false;

    [SerializeField]
    internal float angle = 0;

    [SerializeField]
    internal float speed = 0;

    private void Start()
    {
        if(random)
            angle = Random.Range(0, 360);
    }

    private void FixedUpdate()
    {
        Vector3 rotate= new Vector3(0, 0, rotation);
        transform.Rotate(rotate);


        Vector3 newPos = transform.position;
        newPos.x += Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
        newPos.y += Mathf.Sin(angle * Mathf.Deg2Rad) * speed;
        transform.position = newPos;


        duration--;
        if (duration < 0)
        {
            Vector3 scale = transform.localScale * 0.9f;
            transform.localScale = scale;

            if (scale.x < 0.1f)
                Destroy(gameObject);
        }

        
    }

}
