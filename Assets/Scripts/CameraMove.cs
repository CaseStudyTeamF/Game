using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // スクロールの両端
    [SerializeField] float LeftBorder = 0;
    [SerializeField] float RightBorder = 0;
    [SerializeField] float TopBorder = 0;
    [SerializeField] float BottomBorder = 0;

    // カメラ
    [SerializeField] GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPlayer();

        Vector3 newRotation = transform.eulerAngles;

        newRotation.x = Mathf.Clamp(-newRotation.x, -0.1f, 0.1f);
        newRotation.y = Mathf.Clamp(-newRotation.y, -0.1f, 0.1f);
        newRotation.z = Mathf.Clamp(-newRotation.z, -0.1f, 0.1f);

        transform.eulerAngles = newRotation;
    }

    void FollowPlayer()
    {
        // X距離
        float delta_X = playerObj.transform.position.x - transform.position.x;
        float moveDistanceX = Mathf.Clamp(delta_X, -1.0f, 1.0f);
        float newPosX = Mathf.Clamp(moveDistanceX + transform.position.x, LeftBorder, RightBorder);

        // Y距離
        float delta_Y = playerObj.transform.position.y - transform.position.y;
        float moveDistanceY = Mathf.Clamp(delta_Y, -1.0f, 1.0f);
        float newPosY = Mathf.Clamp(moveDistanceY + transform.position.y, BottomBorder, TopBorder);


        Vector3 newPos = new Vector3(newPosX, newPosY, transform.position.z);
        transform.position = newPos;
    }

    public void ShakeCamera(Vector3 rotation)
    {
        transform.eulerAngles = rotation;
    }
}
