using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        if (rb == null)
            return;

        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        Vector2 force;
        force.x = 8.0f * Mathf.Cos(angle);
        force.y = 8.0f * Mathf.Sin(angle);

        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
