﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowSpellScript : MonoBehaviour
{
    public bool canPlace;
    public bool canFireArrow;
    public GameObject aimArrow;
    public GameObject arrowPuckPrefab;
    playerScript pScript;
    gameControllerScriptNew gcScript;

    private void Start() 
    {
        canFireArrow = false;
        canPlace = false;
        pScript = GetComponent<playerScript>();
        gcScript = GameObject.Find("GameController").GetComponent<gameControllerScriptNew>();
    }

    private void Update() {
        if(pScript.canSelect == true){
            //and it is the players turn
            if(gcScript.playerTurn == true){
                //and the player is selected
                if(pScript.isSelected == true)
                {
                    if(canPlace == true)
                    {
                        aimArrow.SetActive(true);
                        canFireArrow = true;
                        
                        //rotate the aimarrow towards the mouse
                        Vector3 mousePos = Input.mousePosition;
                        mousePos.z = 5.23f;

                        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
                        mousePos.x = mousePos.x - objectPos.x;
                        mousePos.y = mousePos.y - objectPos.y;

                        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                        aimArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    }
                    //if we right click
                    if(Input.GetMouseButtonDown(1)){
                            //deselect the player
                            pScript.isSelected = false;
                            pScript.canShoot = false;
                    }
                }
                else
                {
                }
            }
        }
        checkPlace();
    }

    public void doArrow()
    {
        canPlace = true;
    }

    public void checkPlace()
    {
        if(canFireArrow == true){
            if(Input.GetMouseButtonDown(0)){
                GameObject arrowPuck = Instantiate(arrowPuckPrefab, new Vector3(aimArrow.transform.position.x + 1,aimArrow.transform.position.y, -2), aimArrow.transform.rotation);
                Rigidbody2D arrowPuckRigidBody = arrowPuck.GetComponent<Rigidbody2D>();
                arrowPuckRigidBody.velocity = arrowPuck.transform.right * 10;
            }
        }
    }
        
}
