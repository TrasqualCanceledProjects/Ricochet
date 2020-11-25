using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    RaycastHit hit;
    private void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 1f);
        if(hit.collider != null && hit.collider.CompareTag("Door"))
        {
            Destroy(hit.collider.gameObject);
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
}
