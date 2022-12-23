using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankItem : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text win;
    public TMP_Text lose;
    public void SetItem(string _name, string _lose, string _win){
        name.text = _name;
        win.text = _win;
        lose.text = _lose;
    }
}
