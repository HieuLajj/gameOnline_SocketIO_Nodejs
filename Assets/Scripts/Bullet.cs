using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerFrom;
    private void OnCollisionEnter2D(Collision2D other) {
        var hit = other.gameObject;
        var health = hit.GetComponent<Health>();
        if(health != null){
            health.TakeDamage(playerFrom,10);
        }
        Destroy(gameObject);
    }
}
