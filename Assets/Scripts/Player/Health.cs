using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public HealthBar healthBar;
    public  const int maxHealth = 100;
    public bool destroyOnDeath;
    public int currentHealth = maxHealth;
    public bool isEnemy = false;
    //public RectTransform healthBar;
    private bool isLocalPlayer;
    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        isLocalPlayer = pc.isLocalPlayer;
        if(isLocalPlayer){
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(GameObject playerFrom, int amount){
        currentHealth -= amount;
        //Debug.Log("da bi trung dan"+currentHealth);
        //OnChangeHealth();
        NetworkManager.instance.GetComponent<NetworkManager>().CommandHealthChange(playerFrom, this.gameObject, amount);
    }

    public void OnChangeHealth( int currentHealth){
        this.currentHealth = currentHealth;
        healthBar.SetHealth((currentHealth));
        if(currentHealth <= 0){
            gameObject.SetActive(false);
            if(destroyOnDeath){
                Destroy(gameObject);
            }else{
                currentHealth = maxHealth;
                Respawn();
            }
            pc.setAvtivePlayer = false;
        }
    }

    void Respawn(){
        if(isLocalPlayer){
            Vector3 spawnPoint = Vector3.zero;
            Quaternion spawnRotation = Quaternion.Euler(0,180,0);
            transform.position = spawnPoint;
            transform.rotation = spawnRotation;
        }
    }

}
