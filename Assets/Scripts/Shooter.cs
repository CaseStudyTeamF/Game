using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] Sprite NomalShooter;
    [SerializeField] Sprite SpaceShooter;
    [SerializeField] Sprite RubberShooter;
    [SerializeField] Sprite IceShooter;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(PlayerMove.shootType)
        {
            case ShootType.Normal:
                sr.sprite = NomalShooter;
                break;
            case ShootType.Anti_Gravity:
                sr.sprite = SpaceShooter;
                break;
            case ShootType.SuperBall:
                sr.sprite = RubberShooter;
                break;
            case ShootType.Slip:
                sr.sprite = IceShooter;
                break;
        }
    }
}
