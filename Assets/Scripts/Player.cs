using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Vector2 padding = new Vector2(1f, 1f);
    [SerializeField] int health = 300;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFireRate = 0.5f;

    private SoundManager soundManager;
    private Coroutine firingRoutine;
    private float minX, maxX, minY, maxY;
    private bool isFiring;
    private Level level;

    void Start () {
        SetUpMoveBounderies();
        level = FindObjectOfType<Level>();
        soundManager = GameObject.FindGameObjectWithTag("Sound Manager").GetComponent<SoundManager>();
	}

    private void SetUpMoveBounderies()
    {
        var gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0)).x + padding.x;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0)).x - padding.x;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0)).y + padding.y;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1)).y - padding.y;

    }

    void Update () {
        Move();
        Fire();
	}

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newLimitedXPos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
        var newLimitedYPos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);
        transform.position = new Vector2(newLimitedXPos, newLimitedYPos);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1") && !isFiring)
        {
            firingRoutine = StartCoroutine(FireContinuously());
            isFiring = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingRoutine);
            isFiring = false;
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            soundManager.TriggerPlayerShotSFX(gameObject);
            yield return new WaitForSeconds(projectileFireRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        soundManager.TriggerPlayerDeadSFX(gameObject);
        level.LoadGameOver();
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(soundManager.GetPlayerDeadSFX().length);
        level.LoadGameOver();
    }

    public int GetHealth()
    {
        return Mathf.Max(health, 0);
    }
}
