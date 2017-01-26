﻿using Assets.Source.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidComponent : MonoBehaviour
{
    public GameObject orePrefab;
    public Ore[] ores;
    public float health = 100.0f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            ProjectileComponent projectile = collider.gameObject.GetComponent<ProjectileComponent>();
            TakeDamage(projectile.damage);
        }
    }

    private void Start()
    {
        float minScale = 0.75f;
        float maxScale = 1.75f;
        float rand = Random.Range(minScale, maxScale);
        Vector3 randomScale = new Vector3(rand, rand, 1.0f);
        transform.localScale = randomScale;

        GetComponent<SpinComponent>().speed = Random.Range(-20.0f, 20.0f);
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
        Ore oreType = ores[Random.Range(0, ores.Length)];
        int oreAmount = Random.Range(5, 10);
        for (int i = 0; i < oreAmount; i++)
        {
            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity) as GameObject;
            ore.GetComponent<OreComponent>().Ore = oreType;
        }

        Destroy(gameObject);
    }
}