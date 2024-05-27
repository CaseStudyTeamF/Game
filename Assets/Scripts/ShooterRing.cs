using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRing : MonoBehaviour
{
    [SerializeField] bool unlimit;
    [SerializeField] ShootType forceType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(unlimit)
            {
                PlayerMove.allowChangeType = true;
            }
            else
            {
                // パチンコタイプの変更
                PlayerMove.shootType = forceType;
                PlayerMove.allowChangeType = false;
            }
        }
    }
}
