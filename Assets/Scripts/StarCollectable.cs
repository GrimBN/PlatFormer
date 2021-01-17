using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollectable : MonoBehaviour
{

   // BoxCollider2D boxCollider2D;

    void Start()
    {
        //boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            FindObjectOfType<LevelController>().StarGet();
            gameObject.SetActive(false);
        }
    }
}
