using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{

    public TextMeshProUGUI TextUI;

    public void SetText(string text, string text2 = null) {
        TextUI.text = text + "\n" + text2;
    }
}
