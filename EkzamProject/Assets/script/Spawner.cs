using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private Resource prefabResource;
    [SerializeField] private Transform container;
    [SerializeField] private Transform min;
    [SerializeField] private Transform max;

    private WaitForSeconds wait;
    private Vector3 spawnPosition;
    private void Start()
    {
        wait = new WaitForSeconds(delay);
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        while (enabled)
        {
            spawnPosition = new Vector3(
                Random.Range(min.position.x, max.position.x),
                0.41f,
                Random.Range(min.position.z, max.position.z)
            );

            // Исправлено: ресурсы создаются в контейнере
            var resource = Instantiate(
                prefabResource,
                spawnPosition,
                Quaternion.identity,
                container
            );

            yield return wait;
        }
    }
}