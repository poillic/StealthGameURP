using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Back : MonoBehaviour
{
    public string words;
    public TextMeshProUGUI label;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        label.SetText( words );
    }
}
