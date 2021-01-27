using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSenser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Stop=false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Wall")
        {
            Stop = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Wall")
        {
            Stop = false;
        }
    }
}
