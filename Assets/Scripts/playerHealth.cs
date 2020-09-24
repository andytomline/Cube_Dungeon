using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class playerHealth : MonoBehaviour
{
    GameObject Player;
    public int currentHealth = 5;
    public int maximumHealth = 5;

    public TMP_Text healthDisplay;

    private void Update()
    {
        healthDisplay.text = currentHealth.ToString() + " / " + maximumHealth.ToString();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            currentHealth--;
        }
        if (collision.gameObject.tag.Equals("Enemy Ranged Attack"))
        {
            
            currentHealth--;
        }
        if (collision.gameObject.tag.Equals("Enemy Ranged Attack (Fire)"))
        {
            currentHealth--;
            currentHealth--;
        }
    }


}
