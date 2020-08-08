using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NavTutorial {
    public class ConnectedWaypoint : WayPoint {

        #region Variables
        /// <summary>
        /// The radius in which things can connect to this.
        /// </summary>
        protected float connectivityRadius = 50f;

        /// <summary>
        /// The connections this class has.
        /// </summary>
        List<ConnectedWaypoint> connections;
        #endregion

        public void Start()
        {
            //Grab all waypoint objects in scene.
            GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

            //Create a list of waypoints I can refer to later.
            connections = new List<ConnectedWaypoint>();

            //Check if they're a connected waypoint.
            for (int i = 0; i < allWaypoints.Length; i++)
            {
                ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

                //i.e. we found a waypoint
                if (nextWaypoint != null)
                {
                    if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                    {
                        connections.Add(nextWaypoint);
                    }
                }
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, connectivityRadius);
        }

        public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
        {
            if (connections.Count == 0)
            {
                //No waypoints?  Return null and complain.
                Debug.LogError("Insufficient waypoint count.", gameObject);
                return null;
            }
            else if (connections.Count == 1 && connections.Contains(previousWaypoint))
            {
                //Only one waypoint and it's the previous one?  Just use that.
                return previousWaypoint;
            }
            else
            {
                ConnectedWaypoint nextWaypoint;
                int nextIndex = 0;

                do
                {
                    nextIndex = UnityEngine.Random.Range(0, connections.Count);
                    nextWaypoint = connections[nextIndex];
                } while (nextWaypoint == previousWaypoint);

                return nextWaypoint;
            }
        }
    } 
}
