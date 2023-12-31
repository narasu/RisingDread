using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public bool HasCollision { get; private set; } = false;
    private bool hadCollision;
    private List<Collider> colliders = new();
    private Collider col;
        
    private void Start()
    {
        col = GetComponent<Collider>();
        HasCollision = colliders.Count > 0;
    }
        
    private void OnTriggerStay(Collider _other)
    {
        if (colliders.Contains(_other))
        {
            return;
        }
        colliders.Add(_other);
            
        HasCollision = true;
        if (!hadCollision)
        {
            hadCollision = true;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
            
        if (colliders.Contains(_other))
        {
            colliders.Remove(_other);
        }

        if (colliders.Count == 0)
        {
            HasCollision = false;
            if (hadCollision)
            {
                hadCollision = false;
            }
        }
    }

    public float GetNearestDistance()
    {
        if (colliders.Count == 0)
        {
            return .0f;
        }
        float distance = Mathf.Infinity;

        foreach (Collider t in colliders)
        {
            float colliderTop = t.bounds.max.y;
            float diff = transform.position.y - colliderTop;
            if (diff < distance)
            {
                distance = diff;
            }
        }

        if (distance < col.bounds.size.y && distance > .0f)
        {
            return distance;
        }

        return .0f;
    }

}