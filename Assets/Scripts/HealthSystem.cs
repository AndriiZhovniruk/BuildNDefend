using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    private int healthAmount;
    [SerializeField] private int healthAmountMax;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }
    public void Damage(int DamageAmount)
    {
        healthAmount -= DamageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (IsDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }
    public bool IsDead()
    {
        return healthAmount == 0;
    }
    public int GetHealthAmount()
    {
        return healthAmount;
    }
    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
    public bool IsFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    public void SetHealAmountMax(int healthAmountMax, bool updateHealthAmount)
    {
        this.healthAmountMax = healthAmountMax;
        if (updateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }
    }
    public void SetHealAmount(int hp)
    {
        healthAmount = hp;
    }
}