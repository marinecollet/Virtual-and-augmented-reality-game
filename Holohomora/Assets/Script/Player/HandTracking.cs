using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class HandTracking : MonoBehaviour
{
    /// <summary>
    /// HandDetected tracks the hand detected state.
    /// Returns true if the list of tracked hands is not empty.
    /// </summary>
    public bool HandDetected
    {
        get { return trackedHands.Count > 0; }
    }

    public GameObject TrackingObject;

    private HashSet<uint> trackedHands = new HashSet<uint>();
    private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
    private GestureRecognizer gestureRecognizer;
    private uint activeId;

    void Awake()
    {
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
        gestureRecognizer.Tapped += GestureRecognizerTapped;
        gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;
        gestureRecognizer.HoldCompleted += GestureRecognizer_HoldCompleted;
        gestureRecognizer.HoldCanceled += GestureRecognizer_HoldCanceled;
        gestureRecognizer.StartCapturingGestures();
    }


    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        trackedHands.Add(id);
        activeId = id;

        var obj = Instantiate(TrackingObject) as GameObject;

        obj.transform.localRotation = this.transform.GetChild(0).rotation;


        Vector3 pos;

        if (args.state.sourcePose.TryGetPosition(out pos))
        {
            obj.transform.localPosition = pos;
        }


        trackingObject.Add(id, obj);

    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        uint id = args.state.source.id;
        Vector3 pos;
        Quaternion rot;

        if (args.state.source.kind == InteractionSourceKind.Hand)
        {
            if (trackingObject.ContainsKey(id))
            {
                if (args.state.sourcePose.TryGetPosition(out pos))
                {
                    trackingObject[id].transform.localRotation = this.transform.GetChild(0).rotation;
                    trackingObject[id].transform.localPosition = pos;
                }
                if (args.state.sourcePose.TryGetRotation(out rot))
                {
                    trackingObject[id].transform.rotation = rot;
                }
            }
        }
    }

    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        if (trackedHands.Contains(id))
        {
            trackedHands.Remove(id);
        }

        if (trackingObject.ContainsKey(id))
        {
            var obj = trackingObject[id];
            trackingObject.Remove(id);
            Destroy(obj);
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
        }
    }



    void ChangeObjectColor(GameObject obj, Color color)
    {
        var rend = obj.GetComponentInChildren<Renderer>();
        if (rend)
        {
            rend.material.color = color;
            Debug.LogFormat("Color Change: {0}", color.ToString());
        }
    }

    private void GestureRecognizer_HoldStarted(HoldStartedEventArgs args)
    {
        uint id = args.source.id;
        if (trackingObject.ContainsKey(activeId))
        {
        }
    }

    private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
    {
        uint id = args.source.id;
        if (trackingObject.ContainsKey(activeId))
        {
        }
    }

    private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)
    {
        uint id = args.source.id;
        if (trackingObject.ContainsKey(activeId))
        {
        }
    }

    private void GestureRecognizerTapped(TappedEventArgs args)
    {
        uint id = args.source.id;
        if (trackingObject.ContainsKey(activeId))
        {
        }
    }

    void OnDestroy()
    {
        InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;

        gestureRecognizer.Tapped -= GestureRecognizerTapped;
        gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;
        gestureRecognizer.HoldCompleted -= GestureRecognizer_HoldCompleted;
        gestureRecognizer.HoldCanceled -= GestureRecognizer_HoldCanceled;
        gestureRecognizer.StopCapturingGestures();
    }
}