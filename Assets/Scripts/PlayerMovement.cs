using System.Text;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float Speed = 12f;

    //Variables to apply gravity and ground player
    public Transform groundCheck;
    public float gravity = -9.81f;
    Vector3 velocity;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    //Reference to Projectile script to change weapons
    public Projectile projectile;


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 Move = transform.right * X + transform.forward * Z;
        controller.Move(Move * Speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponChange(Water)"))
        {
            ChangeToWater();
        }

        if (other.CompareTag("WeaponChange(Normal)"))
        {
            ChangeToNormal();
        }
        
    }

    public void ChangeToWater()
    {
        Debug.Log("HitWeaponChangeWater");
        projectile.Normal = false;
        projectile.Water = true;
    }
     
    public void ChangeToNormal()
    {
        Debug.Log("HitWeaponChangeNormal");
        projectile.Normal = true;
        projectile.Water = false;
    }

}
