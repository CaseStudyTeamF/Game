using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class WarpWall_Control : MonoBehaviour
{

    //このゲートが出口になる際の入口
    private GameObject entranceGate = null;

    //このゲートが入口になる際の出口
    [Tooltip("このゲートに入った際の出口")]
    public GameObject exitGate = null;

    //出口の角度に対して補正する角度(deg)
    [Tooltip("出口の角度に対して補正する角度(deg)")]
    public float exitAngle=0;

    //このゲートが出口になる際の入口のコントローラ
    private WarpWall_Control entranceGate_Control;

    //このゲートが入口になる際の出口のコントローラ
    private WarpWall_Control exitGate_Control;



    private List<Collider2D> warpObjectList=new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        exitGate_Control = exitGate.GetComponent<WarpWall_Control>();
        exitGate_Control.SetEntranceGate(transform.gameObject);
    }
    public void SetEntranceGate(GameObject entranceGateObj)
    {
        entranceGate = entranceGateObj;
        entranceGate_Control = entranceGate.GetComponent<WarpWall_Control>();
    }



    public IEnumerator DelayCoroutine(float s, Action action)
    {
        yield return new WaitForSeconds(s);
        action?.Invoke();
    }

    public void WarpStart(Collider2D collision)
    {

        collision.transform.position = exitGate.transform.position;

        // ワープするオブジェクトのRigidbody
        var warpRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        //出口から排出されるオブジェクトの向き
        float exitAngle = ((-transform.localEulerAngles.z + exitGate.transform.localEulerAngles.z +180) % 360);


        Debug.Log(exitAngle);
        //ベクトルを3次元に
        var objectVelocity = new Vector3(warpRigidbody.velocity.x, warpRigidbody.velocity.y, 0 );

        // 回転値
        var quaternion = Quaternion.Euler(new Vector3(0, 0, exitAngle/2));
        
        // 行列を生成
        var matrix = Matrix4x4.TRS(new Vector3(0,0,0), quaternion, new Vector3(1, 1, 1));

        objectVelocity = matrix.MultiplyPoint(objectVelocity);

        //向きを更新したベクトルを代入
        warpRigidbody.velocity = new Vector2(objectVelocity.x,objectVelocity.y);

    }

    public bool WarpCheck(Collider2D collision)
    {
        return warpObjectList.Contains(collision);
    }

    public void WarpEnd(Collider2D collision)
    {
        warpObjectList.Remove(collision);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!entranceGate_Control.WarpCheck(collision) && !WarpCheck(collision))
        {
            warpObjectList.Add(collision);

            StartCoroutine(DelayCoroutine(0.02f, () =>
            {
                WarpStart(collision);
            }));
            

            //3秒後にワープを許可する
            StartCoroutine(DelayCoroutine(3, () =>
            {
                WarpEnd(collision);
            }));
        }

    }


}
