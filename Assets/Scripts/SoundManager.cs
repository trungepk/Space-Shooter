using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] AudioClip enemyShotSFX, enemyDeadSFX, playerShotSFX, playerDeadSFX;
    [SerializeField] [Range(0,1)] float enemyShotVolume = 1f;
    [SerializeField] [Range(0,1)] float enemyDeadVolume = 1f;
    [SerializeField] [Range(0,1)] float playerShotVolume = 1f;
    [SerializeField] [Range(0,1)] float playerDeadVolume = 1f;

    public AudioClip GetPlayerDeadSFX()
    {
        return playerDeadSFX;
    }

    public void TriggerEnemyShotSFX(GameObject enemy)
    {
        if (!enemyShotSFX) { return; }
        AudioSource.PlayClipAtPoint(enemyShotSFX, enemy.transform.position, enemyShotVolume);
    }

    public void TriggerEnemyDeadSFX(GameObject enemy)
    {
        if (!enemyDeadSFX) { return; }
        AudioSource.PlayClipAtPoint(enemyDeadSFX, enemy.transform.position, enemyDeadVolume);
    }

    public void TriggerPlayerShotSFX(GameObject player)
    {
        if (!playerShotSFX) { return; }
        AudioSource.PlayClipAtPoint(playerShotSFX, player.transform.position, playerShotVolume);
    }

    public void TriggerPlayerDeadSFX(GameObject player)
    {
        if (!playerDeadSFX) { return; }
        AudioSource.PlayClipAtPoint(playerDeadSFX, player.transform.position, playerDeadVolume);
    }
}
