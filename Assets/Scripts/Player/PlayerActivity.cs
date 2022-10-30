using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActivity : MonoBehaviour
{
    public GameObject weaponGun;
    [SerializeField] 
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private Transform centerGun;
    public int selectedGun;
    public Gun[] allGuns;
    public Gun gun;

    private void Start() {
        gun.SwitchGunWeapon(selectedGun);
    }

    public void SwitchWeapon(){
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f){
            ++selectedGun;
            if(selectedGun >= allGuns.Length){
                selectedGun = 0;
            }
            SwitchGun();
        }else if( Input.GetAxisRaw("Mouse ScrollWheel") < 0f){
            --selectedGun;
            if(selectedGun <0){
                selectedGun = allGuns.Length -1;
            }
            SwitchGun();
        }
    }
    public void SwitchGun(){
        gun.SwitchGunWeapon(selectedGun);
        NetworkManager.instance.GetComponent<NetworkManager>().ComandSelectedGuns(selectedGun);
    }
    public void ChangeWeapon(int selectedGun){
        this.selectedGun = selectedGun;
        gun.SwitchGunWeapon(selectedGun);
    }
    public void Shoot(int team){
       var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
       Bullet b = bullet.GetComponent<Bullet>();
       b.a = selectedGun;
       b.playerFrom = gameObject;
       b.teamPlayerFrom = team;
       b.GetComponent<Rigidbody2D>().velocity = (bulletSpawn.position - centerGun.position) * 6;
       Destroy(bullet, 5.0f);
    }
}
