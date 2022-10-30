using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemServer : MonoBehaviour
{
    // Start is called before the first frame update
    public string id;
    void Start()
    {
        id = gameObject.name;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        NetworkManager.instance.CommandItemServer(gameObject.name);
    }
}
