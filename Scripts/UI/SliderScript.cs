using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    public Text text;
    public void OnValueChanged()
    {
        GameManager.instance.boardSize = (int)gameObject.GetComponent<Slider>().value;
        text.text = ((int)gameObject.GetComponent<Slider>().value).ToString();
    }
}
