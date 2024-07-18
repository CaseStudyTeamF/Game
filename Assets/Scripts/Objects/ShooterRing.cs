using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRing : MonoBehaviour
{
    //[SerializeField] bool unlimit;
    [SerializeField] ShootType forceType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(PlayerMove.shootType != forceType)
            {
                // パチンコタイプの変更
                PlayerMove.shootType = forceType;
                PlayerMove.allowChangeType = false;

                Vector3 playerPos = collision.transform.position;

                switch (forceType)
                {
                    case ShootType.Anti_Gravity:
                        PlayerParticle.circleEffect(playerPos, ShootType.Anti_Gravity);
                        break;
                    case ShootType.SuperBall:
                        PlayerParticle.circleEffect(playerPos, ShootType.SuperBall);
                        break;
                    case ShootType.Slip:
                        PlayerParticle.circleEffect(playerPos, ShootType.Slip);
                        break;
                    default:
                        PlayerParticle.circleEffect(playerPos, ShootType.Normal);
                        break;
                }
            }
        }
    }
}
