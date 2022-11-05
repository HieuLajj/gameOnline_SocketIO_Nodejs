using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    private int currentEnergy;
    // Start is called before the first frame update
    [SerializeField]
    private Slider energySlider;
     private bool isLocalPlayer;
    private PlayerController pc;
    void Start()
    {
        pc = GetComponent<PlayerController>();
        isLocalPlayer = pc.isLocalPlayer;
        if(!isLocalPlayer){
            energySlider.gameObject.SetActive(false);
        }
        currentEnergy = 0;
        energySlider.value = currentEnergy;
    }

    public void ChangeEnergy(int h){
        currentEnergy += h;
        if (currentEnergy == 100) {
            currentEnergy=0;
            NetworkManager.instance.ComandSelectedGuns(0);
        }
        energySlider.value = currentEnergy;
    }
}
