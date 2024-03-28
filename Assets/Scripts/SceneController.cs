using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Boid BoidPrefab;
    public Boid BoidPrefab2;
    public Boid BoidPrefab3;
    public int SpawnBoids = 100;

    public float BoidSimArea;

    private List<Boid> boids;

    void Start()
    {
        boids = new List<Boid>();

        for (int i = 0; i < SpawnBoids; i++)
        {
            SpawnBoid(BoidPrefab.gameObject, 0);
        }
        for (int i = 0; i < SpawnBoids; i++)
        {
            SpawnBoid(BoidPrefab2.gameObject, 1);
        }
        for (int i = 0; i < SpawnBoids; i++)
        {
            SpawnBoid(BoidPrefab3.gameObject, 2);
        }
    }

    void Update()
    {
        foreach (Boid boid in boids)
        {
            boid.SimulateMovement(boids, Time.deltaTime);

            var boidpos = boid.transform.position;

            if (boidpos.x > BoidSimArea)
            {
                boidpos.x -= BoidSimArea * 2;
            }
            else if (boidpos.x < -BoidSimArea)
            {
                boidpos.x += BoidSimArea * 2;
            }

            if (boidpos.y > BoidSimArea)
            {
                boidpos.y -= BoidSimArea * 2;
            }
            else if (boidpos.y < -BoidSimArea)
            {
                boidpos.y += BoidSimArea * 2;
            }

            if (boidpos.z > BoidSimArea)
            {
                boidpos.z -= BoidSimArea * 2;
            }
            else if (boidpos.z < -BoidSimArea)
            {
                boidpos.z += BoidSimArea * 2;
            }

            boid.transform.position = boidpos;
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmindex)
    {
        var boidInstance = Instantiate(prefab);
        boidInstance.transform.localPosition += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        boidInstance.transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        var boidcontroller = boidInstance.GetComponent<Boid>();
        boidcontroller.SwarmIndex = swarmindex;
        boids.Add(boidInstance.GetComponent<Boid>());
    }
}
