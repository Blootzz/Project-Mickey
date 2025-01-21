using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool controlsEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetControlsEnabled(bool enabled, GameObject callingObject)
    {
        //print(callingObject.name);
        controlsEnabled = enabled;
    }
}
