using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerReadyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _readyText;
    
    public void SetReadyState(bool ready)
    {
        if(ready)
        {
            _readyText.color = Color.green;
            _readyText.text = "READY";
        }
        else
        {
            _readyText.color = Color.black;
            _readyText.text = "NOT READY";
        }
    }
}
