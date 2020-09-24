using UnityEngine;

public class enemyHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Debug.Log("BulletHitEnemy");
            Destroy(gameObject);
        }
    }


}
