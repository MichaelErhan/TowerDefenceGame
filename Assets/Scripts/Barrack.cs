using UnityEngine;
using System.Collections;

public class Barracks : MonoBehaviour
{
    public GameObject archerPrefab; 
    public Transform spawnPoint;    

    private void Start()
    {
        StartCoroutine(SpawnArcherWithDelay(15f));
    }

    private IEnumerator SpawnArcherWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(archerPrefab, spawnPoint.position, spawnPoint.rotation);

        StartCoroutine(SpawnArcherWithDelay(15f));
    }
}