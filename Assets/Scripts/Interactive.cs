using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    public string interactText = "E to Use";

    public UnityEvent OnUse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        OnUse.Invoke();
        Debug.Log( "Interact" );
    }
}
