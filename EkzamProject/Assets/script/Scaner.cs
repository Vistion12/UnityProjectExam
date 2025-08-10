using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] private float scanRadius = 10;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, scanRadius);
        Gizmos.DrawWireSphere(transform.position, scanRadius);

    }

    public Queue<Resource> Scan(Queue<Resource> resources)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, scanRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Resource resource) &&
               !resource.checkedResources())
            {
                resources.Enqueue(resource);
                resource.takedResources();
            }
        }
        return resources;
    }
}