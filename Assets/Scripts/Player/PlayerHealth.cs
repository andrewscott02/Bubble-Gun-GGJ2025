using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 5;

    public int CurrentHealth { get; private set; }

    [SerializeField]
    private GameObject deathFX;

    [SerializeField]
    private Image hitImage;
    [SerializeField]
    private float hitOpacity;
    [SerializeField]
    private float opacityResetInterval;
    [SerializeField]
    private float opacityResetAmount;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            Color newColour = hitImage.color;
            newColour.a = hitOpacity;
            hitImage.color = newColour;
            StartCoroutine(IResetOpacity(opacityResetInterval));
        }
    }

    private IEnumerator IResetOpacity(float delay)
    {
        yield return new WaitForSeconds(delay);

        Color newColour = hitImage.color;
        newColour.a = Mathf.Clamp(hitImage.color.a - opacityResetAmount, 0, 1);
        hitImage.color = newColour;

        if (newColour.a < 0)
            StartCoroutine(IResetOpacity(delay));
    }

    private void Die()
    {
        Color newColour = hitImage.color;
        newColour.a = hitOpacity;
        hitImage.color = newColour;

        Instantiate(deathFX, transform.position, Quaternion.identity);

        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        StartCoroutine(ILoadGameOverScreen(1.5f));
    }

    private IEnumerator ILoadGameOverScreen(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("GameOverScreen");
    }
}