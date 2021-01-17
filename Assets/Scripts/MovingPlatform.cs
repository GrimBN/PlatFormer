using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [Tooltip("Element 0 should be 0,0 (+/- .5 if even no. of tiles) and the center of the platform should be at the starting point of the respective Path tilemap.\nElement 1 should be the difference" +
             " between the start and second point, Element 2 should be the difference between the second and third point in Path and so on for Element 3,4,5,...")]
    [SerializeField] List<Vector2>  waypoints;
    [SerializeField] bool isOneWay = false;
    [SerializeField] bool isStopAtEnd = false;
    List<Vector2> reverseWaypoints;
    bool isPlaying = false;

    Rigidbody2D platformRigidbody;

    void Start()
    {        
        GetComponent<Tilemap>().CompressBounds();
        platformRigidbody = GetComponent<Rigidbody2D>();
        reverseWaypoints = new List<Vector2>(waypoints);
        reverseWaypoints.Reverse();
        ResetPlatformPosition();
    }

    public void SetIsPlaying(bool value)
    {
        isPlaying = value;
    }

    public void ResetPlatformPosition()
    {
        if(waypoints[0] != null)
        {
            platformRigidbody.position = waypoints[0];
        }
    }

    public IEnumerator MovePlatform()
    {
        //platformRigidbody.position = waypoints[0];
        ResetPlatformPosition();

        while (isPlaying)
        {            
            foreach (Vector2 waypoint in waypoints)
            {
                if(waypoint == waypoints[0]) { continue; }
                Vector2 difference = (waypoint - platformRigidbody.position);
                difference.Normalize();
                
                while (difference == (waypoint - platformRigidbody.position).normalized)
                {                    
                    platformRigidbody.position += new Vector2(moveSpeed * Time.deltaTime * difference.x, moveSpeed * Time.deltaTime * difference.y);                    
                    yield return null;
                }
                platformRigidbody.position = waypoint;
            }

            if (!isOneWay)
            {
                foreach (Vector2 waypoint in reverseWaypoints)
                {

                    if (waypoint == reverseWaypoints[0]) { continue; }
                    Vector2 difference = (waypoint - platformRigidbody.position);
                    difference.Normalize();

                    while (difference == (waypoint - platformRigidbody.position).normalized)
                    {
                        platformRigidbody.position += new Vector2(moveSpeed * Time.deltaTime * difference.x, moveSpeed * Time.deltaTime * difference.y);
                        yield return null;
                    }
                    platformRigidbody.position = waypoint;

                }
            }

            if(isStopAtEnd)
            {
                break;
            }
        }        
    }

    
}
