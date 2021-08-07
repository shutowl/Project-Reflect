using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int currentHealth = 100;
    public int maxHealth = 100;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text HPText;

    private void Start()
    {
        currentHealth = maxHealth;
        setMaxHealth(maxHealth);
    }

    public void takeDamage(int damage)
    {
        GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>().resetMultiplier();

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            setHealth(currentHealth);

            if(currentHealth <= 0)
            {
                Debug.Log("Player's health has reached 0!");
                SceneManager.LoadScene("Test Arena");
            }
        }
    }

    public void setHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        HPText.text = "" + currentHealth;
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);

        HPText.text = "" + maxHealth;
    }
}
