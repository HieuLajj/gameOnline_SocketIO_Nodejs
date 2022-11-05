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
        // for(var i = panelLobby.childCount-1; i>=0; i--){
        //         Destroy(panelLobby.GetChild(i).gameObject);
        // }
        for(var i = 0; i<=this.selectedGun; i++){
            //var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
            var bullet = Instantiate(bulletPrefab, bulletSpawn.GetChild(i).position, bulletSpawn.GetChild(i).rotation) as GameObject;
            Bullet b = bullet.GetComponent<Bullet>();
            b.a = selectedGun;
            b.playerFrom = gameObject;
            b.teamPlayerFrom = team;
            Destroy(bullet, 5.0f);
        }
    }
}
