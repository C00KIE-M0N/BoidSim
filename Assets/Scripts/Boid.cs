using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
   public int SwarmIndex { get; set; }
    public float NoClumpingRadius;
    public float LocalAreaRadius;
    public float Speed;
    public float SteeringSpeed;
    
    public void SimulateMovement(List<Boid> other, float time)
    {
        //default vars
        var steering = Vector3.zero;

        //seperation vars
        Vector3 seperationdirection = Vector3.zero;
        int seperationCount = 0;

        //alignment vars
        Vector3 alignmentdirection = Vector3.zero;
        int alignmentCount = 0;

        //cohesion vars
        Vector3 cohesionsdirection = Vector3.zero;
        int cohesionCount = 0;

        foreach (Boid _boid in other)
        {
            //skip self
            if (_boid == this)
            {
                continue;
            }

            var distance = Vector3.Distance(_boid.transform.position, this.transform.position);

            //identify local neighbor
            if (distance < NoClumpingRadius)
            {
                seperationdirection += _boid.transform.position - transform.position;
                seperationCount++;
            }

            if (distance < LocalAreaRadius)
            {
                alignmentdirection += _boid.transform.forward;
                alignmentCount++;

                cohesionsdirection += _boid.transform.forward - transform.position;
                cohesionCount++;
            }
        }

        //calculate average
        if (seperationCount > 0)
        {
            seperationdirection /= seperationCount;
        }

        //flip and normalize
        seperationdirection = -seperationdirection.normalized;

        if (alignmentCount > 0)
        {
            alignmentdirection /= alignmentCount;
        }

        if (cohesionCount > 0)
        {
            cohesionsdirection /= cohesionCount;
        }

        cohesionsdirection -= transform.position;

        //apply steering
        steering += seperationdirection.normalized;
        steering += alignmentdirection.normalized;
        steering += cohesionsdirection.normalized;

        if (steering != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);
        }

        //move
        transform.position += transform.TransformDirection(new Vector3(0, 0, Speed)) * time;
    }
}
