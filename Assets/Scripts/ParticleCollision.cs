using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEvents;   // パーティクル衝突に関する情報

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject obj)
    {
        if (obj.name == "Head")
        {
            //Debug.Log(collisionEvents[i].intersection);
            particle.GetCollisionEvents(obj, collisionEvents);

            foreach(var collisionEvent in collisionEvents)
            {
                Vector3 pos = collisionEvent.intersection;
                obj.GetComponent<MeshCreate>().Hit(pos);
            }

        }
    }
}
