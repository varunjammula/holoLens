/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class Tracker : MonoBehaviour, ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;
        private Vector3 temp;
        private GameObject line;
        #endregion // PRIVATE_MEMBER_VARIABLES

        public TextMesh messageMesh;
        //public GameObject arrow;
        public GameObject linePrefab;

        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            temp = mTrackableBehaviour.transform.position;
            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found at: " + getTimeStamp());
            messageMesh.text = mTrackableBehaviour.TrackableName + " found at: " + temp + " on " + getTimeStamp();

            //arrow.SetActive(true);
            //arrow.transform.position = temp;

            //arrow.transform.rotation = mTrackableBehaviour.transform.rotation;
            //arrow.transform.rotation = new Quaternion(0, 90, 0, 0);
            Vector3 position = new Vector3(temp.x, 0, temp.z);
            line = Instantiate(linePrefab, position, Quaternion.identity);
            //line.transform.rotation = mTrackableBehaviour.transform.rotation;
            //OnTrackingLost();
        }

        private void AddObject(Vector3 position)
        {

        }

        private string getTimeStamp()
        {
            return ((long)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
        }

        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost !");
            messageMesh.text = "trackable lost.";
            //arrow.SetActive(false);
        }

        #endregion // PRIVATE_METHODS
    }
}
