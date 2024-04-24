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

        float moveMax = -0.05f;

        newRotation.x = Mathf.Clamp(-newRotation.x, -moveMax, moveMax);
        newRotation.y = Mathf.Clamp(-newRotation.y, -moveMax, moveMax);
        newRotation.z = Mathf.Clamp(-newRotation.z, -moveMax, moveMax);

        transform.eulerAngles = newRotation;
    }

    void FollowPlayer()
    {
        float moveMax = 0.5f;

        // X距離
        float delta_X = playerObj.transform.position.x - transform.position.x;
        float moveDistanceX = Mathf.Clamp(delta_X, -moveMax, moveMax);
        float newPosX = Mathf.Clamp(moveDistanceX + transform.position.x, LeftBorder, RightBorder);

        // Y距離
        float delta_Y = playerObj.transform.position.y - transform.position.y;
        float moveDistanceY = Mathf.Clamp(delta_Y, -moveMax, moveMax);
        float newPosY = Mathf.Clamp(moveDistanceY + transform.position.y, BottomBorder, TopBorder);


        Vector3 newPos = new Vector3(newPosX, newPosY, transform.position.z);
        transform.position = newPos;
    }

    public void ShakeCamera(Vector3 rotation)
    {
        transform.eulerAngles = rotation;
    }
}
