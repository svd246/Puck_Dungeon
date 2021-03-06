﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{

    public bool isSelected;
    public bool canShoot = true;
    public bool canSelect;
    public bool isAiming = false;


    Rigidbody2D rgbody2D;
    SpriteRenderer spriteRen;
    ParticleSystem colParticles;


    public Text txtPHealth;
    public Text txtPDamage;
    public Text txtPSpeed;
    public Text txtMoveManaCost;

    public float pHealth = 10;
    public float pDamage = 5;
    public float baseMana = 10;
    public float pMana = 10;
    public float baseSpeed = 8;
    public float speed = 10;
    public float maxSpeed = 15;
    public float startMana = 10;
    public float baseMoveCost = 5;
    public float totalMoveCost;

    public gameControllerScriptNew gcScript;

    public GameObject actionUI;
    public GameObject[] playerObjs;
    public GameObject aimArrow;

    // Start is called before the first frame update
    void Start()
    {
        rgbody2D = GetComponent<Rigidbody2D>();
        spriteRen = this.gameObject.transform.Find("puckSprite/characterSprite").GetComponent<SpriteRenderer>();
        gcScript = GameObject.Find("GameController").GetComponent<gameControllerScriptNew>();
        actionUI = GameObject.Find("actionUIHolder");
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        aimArrow = this.gameObject.transform.Find("puckSprite/aimArrow").gameObject;
        actionUI.SetActive(false);
        isSelected = false;
        colParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        aim();
        updateUI();
        updateSprite();
        moveCost();
    }

    

    private void updateSprite()
    {
        //if the player cannot be selected and it is their turn
        if(canSelect == false){
            //make the charactersprite grey
            spriteRen.color = Color.grey;
        //if the player can be selected or it is not their turn
        }else if(canSelect == true){
            spriteRen.color = Color.white;
        }
    }

    //attempt to select the player
    private void OnMouseDown(){
        //if the player can be selected
        if(canSelect == true){
            //and it is the players turn
            if(gcScript.playerTurn == true){
                //select the player
                isSelected = true;
            }
        }
    }

    private void aim()
    {
        //if the player can be selected
        if(canSelect == true){
            //and it is the players turn
            if(gcScript.playerTurn == true){
                //and the player is selected
                if(isSelected == true){
                    //and the player is aiming
                    if(isAiming == true){
                        //activate the aim arrow
                        aimArrow.SetActive(true);
                        
                        //rotate the aimarrow towards the mouse
                        Vector3 mousePos = Input.mousePosition;
                        mousePos.z = 5.23f;

                        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
                        mousePos.x = mousePos.x - objectPos.x;
                        mousePos.y = mousePos.y - objectPos.y;

                        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                        aimArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                        //start checking to see if we have shot every frame
                        checkShoot();

                        //give us the ability to shoot
                        canShoot = true;
                    }

                    //if we right click
                    if(Input.GetMouseButtonDown(1)){
                            //deselect the player
                            isSelected = false;
                            canShoot = false;
                            isAiming = false;
                    }
                }
                else
                {
                    canShoot = false;
                    isAiming = false;
                    isSelected = false;
                    aimArrow.SetActive(false);
                }
            }
        }
        
    }

    public void doAim(){
        if(pMana >= totalMoveCost){
            isAiming = true;
        }
    }


    private void updateUI(){
        txtPHealth.text = "" + pHealth;
        txtPDamage.text = "" + pDamage;
        txtPSpeed.text = "" + speed;
        txtMoveManaCost.text = "" + totalMoveCost;
        if(isSelected == true && isAiming == false)
        {
            actionUI.SetActive(true);
        }
        if(isSelected == false || isAiming == true)
        {
            actionUI.SetActive(false);
        }
        if(totalMoveCost > pMana){
            txtMoveManaCost.color = Color.red;
        }else{
            txtMoveManaCost.color = Color.black;
        }
    }

    private void moveCost(){
        if(speed < baseSpeed){
            totalMoveCost = 5;
        }else{
            totalMoveCost = baseMoveCost + (speed - baseSpeed);
        }
    }

    private void checkShoot()
    {
        if(Input.GetMouseButtonDown(0) && canShoot == true){
            doShoot();
            canShoot = false;
        }
    }

    private void doShoot()
    {
        rgbody2D.velocity = aimArrow.transform.right * speed;
        isSelected = false;
        isAiming = false;
        pMana -= totalMoveCost;
        aimArrow.SetActive(false);
        if(pMana < 1){
            canSelect = false;
        }
    }

    public void addDmg()
    {
        pDamage += 1;
    }

    public void addHealth()
    {
        pHealth += 1;
    }

    public void checkHealth()
    {
        if(pHealth < 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        colParticles.Play();
        if(gcScript.playerTurn == false){
            if(col.gameObject.tag == "enemy")
            {
                pHealth -= col.gameObject.GetComponent<damageScript>().damage;
                checkHealth();
            }
        }
        // if(col.gameObject.tag == "enemy" && this.gameObject.activeSelf == true)
        // {
        //     Time.timeScale = .1f;
        //     Invoke("timeReset", .02f);
        // }
    }

    public void IncreaseSpeed(){
        if(speed < maxSpeed){
            speed++;
        }
    }

    public void DecreaseSpeed(){
        if(speed > 0){
            speed--;
        }
    }

    // void timeReset(){
    //     Time.timeScale = 1f;
    // }
}
