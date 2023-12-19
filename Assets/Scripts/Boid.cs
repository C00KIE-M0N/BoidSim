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
            if (_distance < LookRange)
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
        Vector3 _faceAwayDirection = Vector3.zero;

        //loop over all other boids in scene, skip if itself.
        foreach (Boid _boid in BoidsInScene)
        {
            float _distance = Vector3.Distance(_boid.transform.position, transform.position);

            if (_distance < AvoidRange)
            {
                _faceAwayDirection = _faceAwayDirection + (transform.position - _boid.transform.position);
            }
        }

        _faceAwayDirection = _faceAwayDirection.normalized;

        //calculate new move direction to include avoidance.
        Direction = Direction + AvoidStrength * _faceAwayDirection / (AvoidStrength + 1);
        Direction = Direction.normalized;
    }

    private void AlignWithBoids()
    {
        Vector3 _directionSum = Vector3.zero;
        int _boidCount = 0;

        foreach (Boid _boid in BoidsInScene)
        {
            float _distance = Vector3.Distance(_boid.transform.position, transform.position);
            if (_distance < CohesionRange) 
            {
                _directionSum += _boid.Direction;
                _boidCount++;
            }
        }

        Vector3 _directionAverage = _directionSum / _boidCount;
        _directionAverage = _directionAverage.normalized;

        float deltaTimeStrength = CohesionStrength * Time.deltaTime;
        Direction = Direction + deltaTimeStrength * _directionAverage / (deltaTimeStrength + 1);
        Direction = Direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveToCenterOfFlock();
        //AvoidNearbyBoids();
        //AlignWithBoids();

        transform.Translate(Direction * (Speed * Time.deltaTime));
        transform.LookAt(Direction);
    }
}
