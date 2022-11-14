using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorRandomPositionUtil : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectPrefab;
    public GameObject objectPrefab2;
    public float radius = 0.2f;
    
    protected Vector2 GetRandomPosition(){
        return Random.insideUnitCircle * radius + (Vector2)transform.position;
    }

    protected Quaternion Random2DRotation(){
        return Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    protected virtual GameObject GetObject(){
        return Instantiate(objectPrefab);
    }

    public void CreateObject(){
        Vector2 position = GetRandomPosition();
        GameObject impactObject = GetObject();
        impactObject.transform.position = position;
        impactObject.transform.rotation = Random2DRotation();
    }
    protected virtual GameObject GetObject2(){
        return Instantiate(objectPrefab2);
    }

    public void CreateObject2(){
        Vector2 position = GetRandomPosition();
        GameObject impactObject = GetObject2();
        impactObject.transform.position = position;
        impactObject.transform.rotation = Random2DRotation();
    }
}
