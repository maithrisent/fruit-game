using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;
    
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public GameManager gameManager;
    
    // void Start()
    // {
        
    // }

    void Update()
    {
        health = gameManager.health;
        maxHealth = gameManager.maxHealth;

    for (int i = 0; i < hearts.Length; i++) //hearts game object is initialized in Unity Editor, and objects have been assigned
    {
        if (i < health)
        {
            hearts[i].sprite = fullHeart; //if full health the sprite should be full heart
        }
        else
        {
            hearts[i].sprite = emptyHeart; //everytime the player loses the sprite should be empty heart
        }
        hearts[i].enabled = i < maxHealth; //tells us how many hearts in total are present in the game
    }
    }
}
