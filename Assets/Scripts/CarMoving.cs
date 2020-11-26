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
    private Vector3 basePosition;
    private Vector3 oriPosition;

    private bool fistHolding = false;
    private bool cubeScaled = false;
    private bool shouldApplyForce = false;
    public float fistThreshold = 0.09f;

    private int _palmOpenCountMax = 10;
    private int palmOpenCount;



    // Start is called before the first frame update
    void Start() {
        //controller = _leapProvider.GetLeapController();
        basePosition = palm.position;
        oriPosition = transform.position;
        palmOpenCount = _palmOpenCountMax;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space is pressed, reverting the Moving Car's location.");
            transform.position = oriPosition;
        }
        

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

        if (!fistHolding) {
            palmOpenCount--;
        } else {
            palmOpenCount = _palmOpenCountMax;
        }

        shouldApplyForce = palmOpenCount > 0;


        if (shouldApplyForce && !cubeScaled) {
            debugCube.localScale *= 1.5f;
            cubeScaled = true;
        } else if (!shouldApplyForce && cubeScaled) {
            // ! update base location when the first unfolding FixedUpdate is called.
            
            debugCube.localScale /= 1.5f;
            cubeScaled = false;
        }

        if (shouldApplyForce) {
            // TODO: Apply force here to the car
            Rigidbody carBody = GetComponent<Rigidbody>();
            if (carBody) {
                var acceleration = (palm.position - basePosition) * 75f;
                acceleration.y = 0; // ! explicitly remove the y components
                carBody.AddForce(acceleration, ForceMode.Acceleration);
                Debug.Log(string.Format("Adding acceleration to car in direction: {0}, {1}, {2}", acceleration.x, acceleration.y, acceleration.z));
            }
        } else {
            basePosition = palm.position;
        }

        Debug.Log(string.Format("fistDegree is {0}, are we holding our fist? {1}", fistDegree, fistHolding));
    }
}
