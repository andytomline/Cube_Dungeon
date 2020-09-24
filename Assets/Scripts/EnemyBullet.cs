using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        
        
        Destroy(this.gameObject);
    }
}
