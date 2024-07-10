using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    bool recovered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recovered)
            return;

        if (collision.CompareTag("Player"))
        {
            if (PlayerMove.Life < 3)
            {
                PlayerMove.Life++;
                recovered = true;
                gameObject.SetActive(false);
            }
        }
    }
}
