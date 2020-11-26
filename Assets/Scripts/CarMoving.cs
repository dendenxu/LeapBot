using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class CarMoving : MonoBehaviour {

    // ? Should we use this?
    //[SerializeField] private LeapServiceProvider _leapProvider;
    //private Controller controller;

    // ! we're using the third bone to recognize if the hand is currently closed
    [SerializeField] private Transform palm;
    [SerializeField] private Transform[] bone3s;
    [SerializeField] private Transform debugCube;

    private bool fistHolding = false;
    private bool cubeScaled = false;
    public float fistThreshold = 0.085f;


    // Start is called before the first frame update
    void Start() {
        //controller = _leapProvider.GetLeapController();
    }

    // Update is called once per frame
    void Update() {
        //if (controller != null && controller.IsConnected) {
        //    Debug.Log("The Controller is connected");
        //    Frame now = controller.Frame();
        //    Frame prev = controller.Frame(1);
        //    Debug.Log(string.Format("Getting: {0}", now));

        //} else {
        //    Debug.LogWarning("Check your Controller connection");
        //}

    }

    void FixedUpdate() {
        float fistDegree = 0f;
        Vector3 position = palm.position - 0.035f * palm.up - 0.015f * palm.forward;
        Quaternion rotation = palm.rotation;

        Rigidbody debugCubeBody = debugCube.GetComponent<Rigidbody>();

        if (debugCubeBody) {
            debugCubeBody.MovePosition(position);
            debugCubeBody.MoveRotation(rotation);
        } else {
            debugCube.position = position;
            debugCube.rotation = rotation;
        }



        for (int i = 0; i < bone3s.Length; i++) {
            float distance = Vector3.Distance(bone3s[i].position, position);
            //Debug.Log(string.Format("Getting current distance {0}", distance));
            fistDegree += distance * distance;
        }

        fistDegree = Mathf.Sqrt(fistDegree);

        fistHolding = fistDegree < fistThreshold;

        if (fistHolding && !cubeScaled) {
            debugCube.localScale *= 1.5f;
            cubeScaled = true;
        } else if (!fistHolding && cubeScaled) {
            debugCube.localScale /= 1.5f;
            cubeScaled = false;
        }

        Debug.Log(string.Format("fistDegree is {0}, are we holding our fist? {1}", fistDegree, fistHolding));
    }
}
