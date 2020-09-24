using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour

{
    //Projectile Type
    public bool Normal = true;
    public bool Water = false;


    //bullet
    public GameObject bulletNormal;
    public GameObject bulletWater;

    //bullet force
    public float shootForce, upwardForce;

    //projectile stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public float timeBetweenShootingWater, timeBetweenShotsWater;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public bool allowButtonHoldWater;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;
    public GameObject Player;

    //Graphics
    public GameObject muzzleFlash;
    public TMP_Text ammoUI;

    //Bug fixing
    public bool allowInvoke = true;



    


    private void Awake()
    {
        //Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display when it is created
        ammoUI.text = bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap;

    }

    void MyInput()
    {
        if (Normal == true)
        {
            //Check if allowed to hold down button and take corresponding input
            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            //Reloading
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

            //Reload automatically when trying to shoot without ammo
            if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();


            //Shooting
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                //Set bullets shot to 0
                bulletsShot = 0;

                ShootNormal();
            }
        }
        
        if (Water == true)
            //Check if allowed to hold down button and take corresponding input
            if (allowButtonHoldWater) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();


        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            ShootWater();
        }
    }

    void ShootNormal()
    {
        readyToShoot = false;

        //Spawns bullets and addds force in direction the player is facing
        GameObject currentBullet = Instantiate(bulletNormal, attackPoint.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody>().AddForce(Player.transform.forward * shootForce * Time.deltaTime, ForceMode.VelocityChange);
        Debug.Log("bullet Spawned");

        //Instantiate muzzle flash
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        
        bulletsLeft--;
        bulletsShot++;


        //Invoke resetShot function (if not already invoked) Invoke calls function after variable time float
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting); //Calls ResetShot function after 'timeBetweenShooting' assigned value
            allowInvoke = false;
        }

        // if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    void ShootWater()
    {
        readyToShoot = false;

        //Spawns bullets and addds force in direction the player is facing
        GameObject currentBullet = Instantiate(bulletWater, attackPoint.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody>().AddForce(Player.transform.forward * shootForce * Time.deltaTime, ForceMode.VelocityChange);
        Debug.Log("bullet Spawned");

        //Instantiate muzzle flash
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;


        //Invoke resetShot function (if not already invoked) Invoke calls function after variable time float
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting); //Calls ResetShot function after 'timeBetweenShooting' assigned value
            allowInvoke = false;
        }

        // if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }



    void OnCollisionEnter(Collision collisionInfo)
    {
        //Destroys bullet once it collides with an object
        if (collisionInfo.collider.tag == "Enemy")
        {
            Debug.Log("Collision with Enemy");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Collision with non-enemy");
            Destroy(this.gameObject);
        }



    }



    void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}



































//--------- This code was previously in the Shoot function, and can be used to impliment raycast aiming and bullet spread ----------//


////Find the exact hit position using a raycast
//Ray ray = fpsCam.ScreenPointToRay(Input.mousePosition); //Ray in middle of screen
//RaycastHit hit;

////Check if ray hits something
//Vector3 targetPoint;
//if (Physics.Raycast(ray, out hit))
//{
//    targetPoint = hit.point;
//    Debug.Log("Ray hit something");
//}
//else
//{
//    targetPoint = ray.GetPoint(100);
//}
////{
////    Physics.Raycast(ray, out hit);
////    targetPoint = hit.point;
////    Debug.Log("RayDidntHit");
////}
////  targetPoint = ray.GetPoint(75); //Just a point far away from the player

////Calculate direction from attackpoint to targetPoint
//Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

////Calculate spread
//float x = Random.Range(-spread, spread);
//float y = Random.Range(-spread, spread);

////Calculate new direction with spread
//Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

////Instantiate Bullet
//GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
//currentBullet.transform.forward = directionWithSpread.normalized;

////Add forces to bullets
//currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
//currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

//Vector3 spawnPos = attackPoint.position;