using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    Player ppl;
    [Tooltip("1=up  2=down  3=left  4=right")]
    public int hitNum;
    void Start()
    {
        ppl = transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            ppl.HitGround(hitNum);
        }
    }
}
