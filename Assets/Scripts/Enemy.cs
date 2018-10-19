using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] bool isGuidedProjectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int health = 100;
    [SerializeField] int scoreValue = 10;
    [SerializeField] float minTimeBetweenShots = 0.5f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject explosionAnimation;
    [SerializeField] SoundManager soundManager;

    private float shotCounter;
    private GameObject playerObj;
    private Player player;
    private float rotateSpeed = 10f;

    void Start ()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        FindPlayer();
    }

    private void FindPlayer()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            player = playerObj.GetComponent<Player>();
        }
    }

    void Update () {
        if (player)
        {
            CountDownToFire();
        }
	}

    private void CountDownToFire()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        if (isGuidedProjectile)
        {
            FireGuidedProjectile();
        }
        else
        {
            FireStraightProjectile();
        }
        soundManager.TriggerEnemyShotSFX(gameObject);
    }

    private void FireStraightProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    private void FireGuidedProjectile()
    {
        Vector2 dir = player.transform.position - transform.position;
        Quaternion rotation = GetRotationToward(dir);
        FireRotatedProjectileTowardTarget(dir, rotation);
    }

    private static Quaternion GetRotationToward(Vector2 dir)
    {
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        return rotation;
    }

    private void FireRotatedProjectileTowardTarget(Vector2 dir, Quaternion rotation)
    {
        GameObject guidedProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        guidedProjectile.transform.rotation = Quaternion.Slerp(guidedProjectile.transform.rotation, 
                                                                rotation, rotateSpeed * Time.deltaTime);
        Vector2 moveDirection = (dir).normalized * projectileSpeed;
        guidedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        health -= damageDealer.GetDamage();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameSession.instance.AddToScore(scoreValue);
        Destroy(gameObject);
        soundManager.TriggerEnemyDeadSFX(gameObject);
        TriggerExplosionVFX();
    }

    private void TriggerExplosionVFX()
    {
        GameObject explosion = Instantiate(explosionAnimation, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }
}
