using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public List<Boid> BoidsInScene = new List<Boid>();

    [SerializeField] private float Speed;
    
    [SerializeField] private float LookRange;
    [SerializeField] private float MoveStrength;

    [SerializeField] private float AvoidRange;
    [SerializeField] private float AvoidStrength;

    [SerializeField] private float CohesionRange;
    [SerializeField] private float CohesionStrength;

    private Vector3 Direction;

    private void MoveToCenterOfFlock()
    {
        Vector3 _directionSum = transform.position;
        int _boidCount = 0;

        //add all boid positions in range, then divide by the amount of boids to get avarage position of objects.
        foreach (Boid _boid in BoidsInScene)
        {
            float _distance = Vector3.Distance(gameObject.transform.position, _boid.gameObject.transform.position);
            if (_distance <= LookRange)
            {
                _directionSum += _boid.transform.position;
                _boidCount++;
            }
        }

        if (_boidCount == 0)
        {
            return;
        }

        //get normalized value of avarage position.
        Vector3 _directionAverage = _directionSum / _boidCount;
        _directionAverage = _directionAverage.normalized;
        Vector3 _faceDirection = (_directionAverage - transform.position).normalized;

        //move direction of this boid to normalized position.
        float _turnStrength = MoveStrength * Time.deltaTime;
        Direction = Direction + _turnStrength * _faceDirection / (_turnStrength + 1);
        Direction = Direction.normalized;
    }

    private void AvoidNearbyBoids()
    {
        //loop over all other boids in scene, skip if itself.
        //if boid is in avoid range, subtract its position from our position to get the direction facing away and normalize it.
        //calculate new move direction to include avoidance.
    }

    private void AlignWithBoids()
    {
        //loop over other boids. If in range, add direction to the local sum.
        //caculate avarage look direction according to sum.
        //Calculate new direction to inlude alignment.
    }

    // Update is called once per frame
    void Update()
    {
        MoveToCenterOfFlock();
        AvoidNearbyBoids();
        AlignWithBoids();

        transform.Translate(Direction * (Speed * Time.deltaTime));
    }
}
