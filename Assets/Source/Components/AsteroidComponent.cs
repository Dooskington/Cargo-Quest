﻿using Assets.Source.Data;
using Assets.Source.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidComponent : MonoBehaviour
{
    public GameObject orePrefab;
    public Ore[] ores;
    public float health;

    private Ore oreType;
    private int minHealth = 2;
    private int maxHealth = 6;
    private int oreCount;
    private int minOre = 3;
    private int maxOre = 10;
    private float minScale = 0.75f;
    private float maxScale = 1.75f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            ProjectileComponent projectile = collider.gameObject.GetComponent<ProjectileComponent>();
            TakeDamage(projectile.damage);
        }
    }

    private void Awake()
    {
        float distanceFromCenter = Vector3.Distance(transform.position, Vector3.zero);

        float dropRateSum = 0.0f;
        foreach (Ore ore in ores)
        {
            if ((distanceFromCenter >= ore.minDistance) && (distanceFromCenter < ore.maxDistance))
            {
                dropRateSum += ore.dropRate;
            }
        }

        float rand = Random.Range(1.0f, dropRateSum);
        foreach (Ore ore in ores)
        {
            if ((distanceFromCenter >= ore.minDistance ) && (distanceFromCenter < ore.maxDistance))
            {
                rand -= ore.dropRate;
                if (rand <= 0.0f)
                {
                    oreType = ore;
                    break;
                }
            }
        }

        //Debug.Log("distance = " + distanceFromCenter + ", dropRateSum = " + dropRateSum + ", oreType = " + oreType.oreName);

        oreCount = Random.Range(minOre, maxOre + 1);

        float scalar = oreCount.Map(minOre, maxOre, minScale, maxScale);
        transform.localScale = transform.localScale * scalar;

        health = (int) oreCount.Map(minOre, maxOre, minHealth, maxHealth);
    }

    private void Start()
    {
        //GetComponent<SpriteRenderer>().sprite = oreType.sprite;
    }

    private void TakeDamage(float amount)
    {
        health -= amount;
        
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        for (int i = 0; i < oreCount; i++)
        {
            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity) as GameObject;
            ore.GetComponent<OreComponent>().Ore = oreType;
        }

        Destroy(gameObject);
    }
}
