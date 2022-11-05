using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
}

public interface IItem{
    Sprite ShowSprite();
    void Effect(GameObject player);
}
public class Bandage : IItem{
    private Sprite spriteItem;
    public Sprite ShowSprite(){
        spriteItem = Resources.Load<Sprite>("Load/Item/item_life");
        return spriteItem;
    }
    public void Effect(GameObject player){
        Health health = player.GetComponent<Health>();
        if (health.currentHealth == 100) return;
        int hp = 20;
        NetworkManager.instance.CommandHealthPlus(player.name, hp);
    }
}

public class Energy : IItem{
    private Sprite spriteItem;
    public Sprite ShowSprite(){
        spriteItem = Resources.Load<Sprite>("Load/Item/item_potion");
        return spriteItem;
    }
    public void Effect(GameObject player){
        Debug.Log(player.name);
        EnergyManager energyManager = player.GetComponent<EnergyManager>();
        energyManager.ChangeEnergy(20);
    }
}
