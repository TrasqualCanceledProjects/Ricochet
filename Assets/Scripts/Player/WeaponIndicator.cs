using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIndicator : MonoBehaviour
{
    WeaponControl weapon;

    private void Awake()
    {
        weapon = FindObjectOfType<WeaponControl>();
    }

    void Update()
    {
        transform.LookAt(new Vector3(weapon.transform.position.x, transform.position.y, weapon.transform.position.z));   
    }
}
