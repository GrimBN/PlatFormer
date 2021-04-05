using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            FindObjectOfType<LevelController>().StarGet();
            gameObject.SetActive(false);
        }
    }
}
