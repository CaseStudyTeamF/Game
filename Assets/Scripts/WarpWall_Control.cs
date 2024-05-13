using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class WarpWall_Control : MonoBehaviour
{

    //���̃Q�[�g���o���ɂȂ�ۂ̓���
    private GameObject entranceGate = null;

    //���̃Q�[�g�������ɂȂ�ۂ̏o��
    [Tooltip("���̃Q�[�g�ɓ������ۂ̏o��")]
    public GameObject exitGate = null;

    //�o���̊p�x�ɑ΂��ĕ␳����p�x(deg)
    [Tooltip("�o���̊p�x�ɑ΂��ĕ␳����p�x(deg)")]
    public float exitAngle=0;

    //���̃Q�[�g���o���ɂȂ�ۂ̓����̃R���g���[��
    private WarpWall_Control entranceGate_Control;

    //���̃Q�[�g�������ɂȂ�ۂ̏o���̃R���g���[��
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

        // ���[�v����I�u�W�F�N�g��Rigidbody
        var warpRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        //�o������r�o�����I�u�W�F�N�g�̌���
        float exitAngle = ((-transform.localEulerAngles.z + exitGate.transform.localEulerAngles.z +180) % 360);


        Debug.Log(exitAngle);
        //�x�N�g����3������
        var objectVelocity = new Vector3(warpRigidbody.velocity.x, warpRigidbody.velocity.y, 0 );

        // ��]�l
        var quaternion = Quaternion.Euler(new Vector3(0, 0, exitAngle/2));
        
        // �s��𐶐�
        var matrix = Matrix4x4.TRS(new Vector3(0,0,0), quaternion, new Vector3(1, 1, 1));

        objectVelocity = matrix.MultiplyPoint(objectVelocity);

        //�������X�V�����x�N�g������
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
            

            //3�b��Ƀ��[�v��������
            StartCoroutine(DelayCoroutine(3, () =>
            {
                WarpEnd(collision);
            }));
        }

    }


}
