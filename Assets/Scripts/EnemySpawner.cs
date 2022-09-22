using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public GameObject spawnPoint;
    public int numberOfEnemies;
    [HideInInspector]
    public List<SpawnPoint> enemySpawnPoints;
    void Start()
    {
        for(int i = 0; i < numberOfEnemies; i++){
            var spawnPosition = new Vector2(Random.Range(-8f,8f), 0f);
            Quaternion spawnRotation =  Quaternion.Euler(0f,0f,0f);
            SpawnPoint enemySpawinPoint = (Instantiate(spawnPoint, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            enemySpawnPoints.Add(enemySpawinPoint);
        }
        SpawnEnemies();
        
    }

    public void SpawnEnemies(){
        int i=0;
        foreach(SpawnPoint sp in enemySpawnPoints){
            Vector2 position = sp.transform.position;
            Quaternion rotation = sp.transform.rotation;
            GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;
            newEnemy.name = i + "";
            playercontroller pc = newEnemy.GetComponent<playercontroller>();
            pc.isLocalPlayer = false;
            Health h = newEnemy.GetComponent<Health>();
            h.currentHealth = 100;
            h.destroyOnDeath = true;
            //h.OnChangeHealth();
            h.isEnemy = true;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
