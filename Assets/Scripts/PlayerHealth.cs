using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth = 100;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text HPText;

    public float iframes = 2.0f;
    private float iframesTimer = 0f;
    private GameObject playerHitbox;
    private SpriteRenderer sprite;

    private void Start()
    {
        currentHealth = maxHealth;
        setMaxHealth(maxHealth);
        iframesTimer = iframes;
        playerHitbox = GameObject.FindGameObjectWithTag("PlayerHitbox");
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(iframesTimer < iframes) //Invulnerable during this time
        {
            playerHitbox.SetActive(false);
            iframesTimer += Time.deltaTime;
        }
        else
        {
            playerHitbox.SetActive(true);
        }
    }

    public void takeDamage(int damage)
    {
        GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>().resetMultiplier();
        iframesTimer = 0f;
        StartCoroutine(Flash(sprite, iframes, 0.2f));

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

    IEnumerator Flash(SpriteRenderer sprite, float duration, float rate)
    {
        for (int n = 0; n < duration/rate/2; n++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(rate);
            sprite.color = Color.white;
            yield return new WaitForSeconds(rate);
        }
    }

}
