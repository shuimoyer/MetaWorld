using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		  UnityEngine.Debug.Log("ffff");
      ThreePlayerDDZ ddz = new ThreePlayerDDZ();
      UnityEngine.Debug.Log(ddz);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
