using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    [SerializeField] private FixedJoystick _aimFJ;
    public bool isLocalPlayer = false;
    private float waitTime = 1/24f;
    private Vector3 mousepos;
    Rect screenRect;
    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    
    
    }
    void Update()
    {
        if(screenRect.Contains(Input.mousePosition) && isLocalPlayer){
            //Vector3 mousepos = Input.mousePosition;
            Vector3 gunposition = Camera.main.WorldToScreenPoint(transform.position);
            // mousepos.x = mousepos.x - gunposition.x;
            // mousepos.y = mousepos.y - gunposition.y;
            mousepos.x = _aimFJ.Horizontal;
            mousepos.y = _aimFJ.Vertical;
            if (mousepos.x == 0 && mousepos.y==0 ) return;
            float gunangle = Mathf.Atan2(mousepos.y, mousepos.x) * Mathf.Rad2Deg;
            if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x){
                transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunangle));
               // NetworkManager.instance.ComandRotateWeapon(new Vector3(180f, 0f, -gunangle));
                StartCoroutine(SendRotate(waitTime, new Vector3(180f, 0f, -gunangle)));
            }else{
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunangle));
               // NetworkManager.instance.ComandRotateWeapon(new Vector3(0f, 0f, gunangle));
                StartCoroutine(SendRotate(waitTime, new Vector3(0f, 0f, gunangle)));
            } 
        }       
    }
    private IEnumerator SendRotate(float waitTime, Vector3 rotateVector){
        yield return new WaitForSeconds(waitTime);
        NetworkManager.instance.ComandRotateWeapon(rotateVector);
    }
}
