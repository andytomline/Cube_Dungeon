using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name);
        Destroy(this.gameObject);
    }
}

