using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public float currentPlayerHealth;
    private float maxPlayerHealth;
    [SerializeField] private int regenRate = 1;
    private bool canRegen;

    [Header("Effects")]
    [SerializeField] private Image hurtEffect;
    [SerializeField] private float hurtTimer = 0.1f;

    [Header("Heal Timer")]
    [SerializeField] private float healCooldown;
    private float maxHealCooldown;
    [SerializeField] private bool startCooldown;

    private void Start()
    {
        maxPlayerHealth = currentPlayerHealth;
        maxHealCooldown = healCooldown;
    }

    IEnumerator HurtFlash()
    {
        hurtEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(hurtTimer);
        hurtEffect.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (currentPlayerHealth >= 0)
        {
            canRegen = false;
            StartCoroutine(HurtFlash());
            healCooldown = maxHealCooldown;
            startCooldown = true;

            currentPlayerHealth -= damage;

            if (currentPlayerHealth <= 0)
            {
                Debug.Log("You Lose!");
            }
        }
    }

    private void Update()
    {
        if (startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if (currentPlayerHealth <= maxPlayerHealth - 0.01)
            {
                currentPlayerHealth += Time.deltaTime * regenRate;
            }
            else
            {
                currentPlayerHealth = maxPlayerHealth;
                healCooldown = maxHealCooldown;
                canRegen = false;
            }
        }
    }
}
