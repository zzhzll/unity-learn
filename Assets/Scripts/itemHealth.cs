using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemHealth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            controller.changeHealth(1);
            Destroy(gameObject);
        }
    }
}
