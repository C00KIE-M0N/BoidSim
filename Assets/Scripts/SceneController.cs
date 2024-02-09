using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Boid BoidPrefab;
    public int SpawnBoids = 100;

    private List<Boid> boids;

    void Start()
    {
        boids = new List<Boid>();

        for (int i = 0; i < SpawnBoids; i++)
        {
            SpawnBoid(BoidPrefab.gameObject, i);
        }
    }

    void Update()
    {
        foreach (Boid boid in boids)
        {
            boid.SimulateMovement(boids, Time.deltaTime);
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmindex)
    {
        var boidInstance = Instantiate(prefab);
        boidInstance.transform.localPosition += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        boids.Add(boidInstance.GetComponent<Boid>());
    }
}
