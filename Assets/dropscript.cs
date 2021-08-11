using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "dropzone")
        {
           if(this.name == "Character")
            {
                Debug.Log("Loss");
            }
           else if(this.name == "Ai")
            {
                Debug.Log("Win");
            }
           else
            {
                return;
            }
        }
    }
}
