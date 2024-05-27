using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    private const float maxLength = 1f;
    private const string propName = "_MainTex";

    GameObject player;

    [SerializeField]
    private Vector2 offset;

    private Material material;


    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Image>() is Image i)
        {
            material = i.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (material)
        {
            // x��y�̒l��0 �` 1�Ń��s�[�g����悤�ɂ���
            var x = Mathf.Repeat(Time.time, maxLength);
            //var y = Mathf.Repeat(Time.time, maxLength);
            var offset = new Vector2(x, 0);
            material.SetTextureOffset(propName, offset);
        }

    }

    private void OnDestroy()
    {
        // �Q�[������߂���Ƀ}�e���A����Offset��߂��Ă���
        if (material)
        {
            material.SetTextureOffset(propName, Vector2.zero);
        }
    }
}
