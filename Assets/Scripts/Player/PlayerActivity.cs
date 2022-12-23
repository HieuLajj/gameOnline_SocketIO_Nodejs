using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent OnShoot = new UnityEvent();
    public GameObject objectGameobject;
    private ObjectPool objectPool;
    [SerializeField] private AudioClip music1;

    private void Start() {
        gun.SwitchGunWeapon(selectedGun);
        objectPool = objectGameobject.GetComponent<ObjectPool>();
    }

    // public void SwitchWeapon(){
    //     if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f){
    //         ++selectedGun;
    //         if(selectedGun >= allGuns.Length){
    //             selectedGun = 0;
    //         }
    //         SwitchGun();
    //     }else if( Input.GetAxisRaw("Mouse ScrollWheel") < 0f){
    //         --selectedGun;
    //         if(selectedGun <0){
    //             selectedGun = allGuns.Length -1;
    //         }
    //         SwitchGun();
    //     }
    // }
    public void SwitchGun(){
        gun.SwitchGunWeapon(selectedGun);
        NetworkManager.instance.GetComponent<NetworkManager>().ComandSelectedGuns(selectedGun);
    }
    public void ChangeWeapon(int selectedGun){
        this.selectedGun = selectedGun;
        gun.SwitchGunWeapon(selectedGun);
    }
    public void Shoot(int team){
        if (NetworkManager.instance.statusRoom != "Game") return;
        Debug.Log("faewf");
        OnShoot?.Invoke();
        AudioManager.Instance.PlaySFX(music1);
        for(var i = 0; i<=this.selectedGun; i++){
            GameObject bullet = objectPool.GetPooledObject();
            if(bullet!=null){
                bullet.transform.position = bulletSpawn.GetChild(i).position;
                bullet.transform.rotation = bulletSpawn.GetChild(i).rotation;
                Bullet b = bullet.GetComponent<Bullet>();
                b.a = selectedGun;
                b.Bung();
                b.playerFrom = gameObject;
                b.teamPlayerFrom = team;
                bullet.SetActive(true);
                StartCoroutine(HideBullet(bullet,3));
            }
            //var bullet = Instantiate(bulletPrefab, bulletSpawn.GetChild(i).position, bulletSpawn.GetChild(i).rotation) as GameObject;
            // Bullet b = bullet.GetComponent<Bullet>();
            // b.a = selectedGun;
            // b.playerFrom = gameObject;
            // b.teamPlayerFrom = team;
            //Destroy(bullet, 5.0f);
        }
        //NetworkManager.instance.GetComponent<NetworkManager>().CommandShoot();
    }

    private IEnumerator HideBullet(GameObject bullet, float duration){
        yield return new WaitForSeconds(duration);
        bullet.SetActive(false);
    }
}
