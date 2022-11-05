using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public IItem item;
    public string id;
    public SpriteRenderer spriteRenderer;
    public void SwitchItem(int h){
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch(h){
            case 0:
                item = new Bandage();
                break;
            case 1:
                item = new Energy();
                break;
            default:
                item = new Energy();
                break;
        }
        spriteRenderer.sprite = item.ShowSprite();
    }
    void Start()
    {
        id = gameObject.name;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        
        item.Effect(other.gameObject);
        NetworkManager.instance.CommandItemServer(gameObject.name);
    }
}
