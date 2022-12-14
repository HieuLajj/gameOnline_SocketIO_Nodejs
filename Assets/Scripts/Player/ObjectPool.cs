using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 15;
    [SerializeField] private GameObject bulletPrefab;
    void Start()
    {

        for(int i=0; i< amountToPool; i++){
            GameObject obj = Instantiate(bulletPrefab, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }      
    }

    public GameObject GetPooledObject(){
        for(int i =0; i< pooledObjects.Count; i++){
            if(!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        return null;
    }

}
