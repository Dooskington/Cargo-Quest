﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDefenseComponent : MonoBehaviour
{
    public float hull = 100.0f;
    public float maxHull = 100.0f;
    public float shield = 100.0f;
    public float maxShield = 100.0f;
    public float rechargeFrequency = 5.0f;
    public float rechargeAmount = 5.0f;
    public Slider hullSlider;
    public Slider shieldSlider;
    public AudioEvent crashAudio;

    private float lastRechargeTime;
    private float lastSpeed;
    private Rigidbody2D shipRigidbody;

    public void Repair()
    {
        hull = maxHull;
    }

    public void DamageShield(float damage)
    {
        float overlap = damage - shield;

        if (overlap > 0)
        {
            shield = 0.0f;
            hull -= overlap;
        }
        else
        {
            shield -= damage;
        }
    }

    private void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();

        hullSlider.maxValue = maxHull;
        hullSlider.value = hull;

        shieldSlider.maxValue = maxShield;
        shieldSlider.value = shield;
    }

    private void Update()
    {
        lastSpeed = shipRigidbody.velocity.magnitude;

        if ((Time.time - lastRechargeTime) >= rechargeFrequency)
        {
            Recharge();
        }

        hull = Mathf.Clamp(hull, 0.0f, maxHull);
        shield = Mathf.Clamp(shield, 0.0f, maxShield);

        UpdateUI();
    }

    private void UpdateUI()
    {
        hullSlider.maxValue = maxHull;
        shieldSlider.maxValue = maxShield;

        hullSlider.value = Mathf.Lerp(hullSlider.value, hull, 2.5f * Time.deltaTime);
        shieldSlider.value = Mathf.Lerp(shieldSlider.value, shield, 2.5f * Time.deltaTime);
    }

    private void Recharge()
    {
        lastRechargeTime = Time.time;
        shield += rechargeAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mine"))
        {
            return;
        }

        if (lastSpeed < 1.5f)
        {
            return;
        }

        float damage = 5.0f * lastSpeed;
        hull -= damage;
        crashAudio.Play(transform.position, (damage / maxHull) * 3);
    }
}
