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

        var leaderboid = other[0];
        var leaderangle = 180f;

        foreach (Boid _boid in other)
        {
            //skip self
            if (_boid == this)
            {
                continue;
            }

            var distance = Vector3.Distance(_boid.transform.position, transform.position);

            //identify local neighbor
            if (distance < NoClumpingRadius)
            {
                seperationdirection += _boid.transform.position - transform.position;
                seperationCount++;
            }

            if (distance < LocalAreaRadius && _boid.SwarmIndex == SwarmIndex)
            {
                alignmentdirection += _boid.transform.forward;
                alignmentCount++;

                cohesionsdirection += _boid.transform.forward - transform.position;
                cohesionCount++;

                var angle = Vector3.Angle(_boid.transform.position - transform.position, transform.forward);
                if (angle < leaderangle && angle < 90f)
                {
                    leaderboid = _boid;
                    leaderangle = angle;
                }
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
        steering += seperationdirection.normalized * 0.5f;
        steering += alignmentdirection.normalized * 0.34f;
        steering += cohesionsdirection.normalized * 0.16f;

        if (leaderboid != null)
        {
            steering += (leaderboid.transform.position - transform.position).normalized * 0.5f;
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, LocalAreaRadius, LayerMask.GetMask("Obstacle")))
        {
            steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;
        }

        if (steering != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);
        }

        //move
        transform.position += transform.TransformDirection(new Vector3(0, 0, Speed)) * time;
    }
}
