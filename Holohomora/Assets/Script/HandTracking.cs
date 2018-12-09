using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class HandTracking : MonoBehaviour {
    /// <summary>
    /// HandDetected tracks the hand detected state.
    /// Returns true if the list of tracked hands is not empty.
    /// </summary>
    public bool HandDetected
    {
        get { return trackedHands.Count > 0; }
    }

    public GameObject TrackingObject;
    //public TextMesh StatusText;
    public Text StatusText;
    public Color DefaultColor = Color.green;
    public Color TapColor = Color.blue;
    public Color HoldColor = Color.red;


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
        StatusText.text = "READY\n"+ StatusText.text;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        var obj = Instantiate(TrackingObject) as GameObject;
    //        obj.transform.SetParent(this.transform.GetChild(0));
    //        obj.transform.localRotation = Quaternion.identity;

    //        Vector3 pos;


    //        obj.transform.localPosition = new Vector3(0.0f, -0.1f, 0.6f);

    //        StatusText.text = "obj created at pos " + new Vector3(0.0f, -0.1f, 0.6f) + "\n" + StatusText.text;


    //        trackingObject.Add(1, obj);
    //    }
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        var obj = trackingObject[1];
    //        trackingObject.Remove(1);
    //        Destroy(obj);
    //        StatusText.text = "obj destroyed \n" + StatusText.text;
    //    }
    //}
    
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
        //StatusText.text = "HoldStarted - Kind:" + args.source.kind.ToString() + " - Id:" + id;
        if (trackingObject.ContainsKey(activeId))
        {
            ChangeObjectColor(trackingObject[activeId], HoldColor);
            //StatusText.text += "-TRACKED";
        }
    }

    private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
    {
        uint id = args.source.id;
        //StatusText.text = "HoldCompleted - Kind:" + args.source.kind.ToString() + " - Id:" + id;
        if (trackingObject.ContainsKey(activeId))
        {
            ChangeObjectColor(trackingObject[activeId], DefaultColor);
            //StatusText.text += "-TRACKED";
        }
    }

    private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)
    {
        uint id = args.source.id;
        //StatusText.text = "HoldCanceled - Kind:" + args.source.kind.ToString() + " - Id:" + id;
        if (trackingObject.ContainsKey(activeId))
        {
            ChangeObjectColor(trackingObject[activeId], DefaultColor);
            //StatusText.text += "-TRACKED";
        }
    }

    private void GestureRecognizerTapped(TappedEventArgs args)
    {
        uint id = args.source.id;
        //StatusText.text = "Tapped - - Kind:" + args.source.kind.ToString() + " - Id:" + id;
        if (trackingObject.ContainsKey(activeId))
        {
            ChangeObjectColor(trackingObject[activeId], TapColor);
            //StatusText.text += "-TRACKED";
        }
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

        //var obj = Instantiate(TrackingObject) as GameObject;
        //obj.transform.SetParent(this.transform);
        //obj.transform.localRotation = this.transform.localRotation;

        var obj = Instantiate(TrackingObject) as GameObject;
        obj.transform.SetParent(this.transform.GetChild(0));
        obj.transform.localRotation = Quaternion.identity;

        Vector3 pos;

        if (args.state.sourcePose.TryGetPosition(out pos))
        {
            obj.transform.localPosition = pos + new Vector3(0f, 0f, 0.5f);
        }
        StatusText.text = "obj created at pos " + pos+ "\n" + StatusText.text;


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
                    trackingObject[id].transform.localPosition = pos + new Vector3(0f,0f,0.5f);
                }
                StatusText.text = "obj updated at pos " + pos+ "\n" + StatusText.text;
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
            StatusText.text = "obj destroyed \n" + StatusText.text;
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
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
