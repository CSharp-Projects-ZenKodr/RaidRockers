using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavTutorial {
    public class WayPoint : MonoBehaviour {
        #region Variables
        /// <summary>
        /// The Radius that the gizmo will draw in.
        /// </summary>
        [SerializeField]
        protected float debugDrawRadius = 1.0f;
        #endregion

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
        }
    } 
}