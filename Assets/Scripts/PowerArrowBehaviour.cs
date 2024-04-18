using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PowerArrowBehaviour : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drawUpdate(Vector2 vector)
    {
        // ���̃T�C�Y�ύX
        float size = vector.magnitude / 500;
        Vector2 scale = new Vector2(size, size);

        transform.localScale = scale;

        // �p�x
        float angle = Mathf.Atan2(vector.y, vector.x);
        transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

        // ���W
        float x = Mathf.Cos(angle) * size * 7;
        float y = Mathf.Sin(angle) * size * 7;
        Vector2 pos = new Vector2(x, y);
        transform.localPosition = pos;



    }

}
