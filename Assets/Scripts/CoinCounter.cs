using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    Text coinNum;
    // Start is called before the first frame update
    void Start()
    {
        coinNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        coinNum.text = PlayerMove.Coins.ToString();
    }
}
