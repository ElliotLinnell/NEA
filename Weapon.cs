using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject collider;
    public float damage = 10f;

    private ThirdPersonController thirdPersonController;

    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();

        if (thirdPersonController == null)
        {
            Debug.LogError("ThirdPersonController component not found on the GameObject.");
        }
    }

    public void Attack()
    {
        if (thirdPersonController != null && thirdPersonController.isAttacking == true)
        {
            if (collider != null && collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}