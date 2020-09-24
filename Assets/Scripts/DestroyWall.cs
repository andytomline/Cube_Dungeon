using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public GameObject wallDestroyedEffect;
    public Transform thisWall;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Instantiate(wallDestroyedEffect, thisWall.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
