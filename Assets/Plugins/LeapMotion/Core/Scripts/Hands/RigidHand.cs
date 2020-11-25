/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2020.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

namespace Leap.Unity {
    /** A physics model for our rigid hand made out of various Unity Collider. */
    public class RigidHand : SkeletalHand {

        /** Transform object for the palm object of this hand. */
        public Transform cube;
        public Transform car;
        public override ModelType HandModelType {
            get {
                return ModelType.Physics;
            }
        }
        public float filtering = 0.5f;

        public override bool SupportsEditorPersistence() {
            return true;
        }

        public override void InitHand() {
            base.InitHand();
        }

        public override void UpdateHand() {

            for (int f = 0; f < fingers.Length; ++f) {
                if (fingers[f] != null) {
                    fingers[f].UpdateFinger();
                }
            }

            if (palm != null) {
                Rigidbody palmBody = palm.GetComponent<Rigidbody>();
                if (palmBody) {
                    palmBody.MovePosition(GetPalmCenter());
                    palmBody.MoveRotation(GetPalmRotation());
                } else {
                    palm.position = GetPalmCenter();
                    palm.rotation = GetPalmRotation();

                }
                Debug.Log(string.Format("We're getting the palm's new position and rotation: \n{0:0.000000}, {1:0.000000}, {2:0.000000}; {3:0.000000}, {4:0.000000}, {5:0.000000}", palm.position.x, palm.position.y, palm.position.z, palm.rotation.eulerAngles.x, palm.rotation.eulerAngles.y, palm.rotation.eulerAngles.z));
            }

            if (forearm != null) {
                // Set arm dimensions.
                CapsuleCollider capsule = forearm.GetComponent<CapsuleCollider>();
                if (capsule != null) {
                    // Initialization
                    capsule.direction = 2;
                    forearm.localScale = new Vector3(1f / transform.lossyScale.x, 1f / transform.lossyScale.y, 1f / transform.lossyScale.z);

                    // Update
                    capsule.radius = GetArmWidth() / 2f;
                    capsule.height = GetArmLength() + GetArmWidth();
                }

                Rigidbody forearmBody = forearm.GetComponent<Rigidbody>();
                if (forearmBody) {
                    forearmBody.MovePosition(GetArmCenter());
                    forearmBody.MoveRotation(GetArmRotation());
                } else {
                    forearm.position = GetArmCenter();
                    forearm.rotation = GetArmRotation();
                }
                //Debug.Log(string.Format("We're getting the forearm's new position and rotation: {0:0.000000}, {1:0.000000}, {2:0.000000}", forearm.position.x, forearm.position.y, forearm.position.z));

            }

            if (cube != null) {
                //Rigidbody cubeBody = cube.GetComponent<Rigidbody>();
                //Rigidbody palmBody = palm.GetComponent<Rigidbody>();
                cube.position = GetPalmCenter() + new Vector3(0, 0, 0.2f);
                cube.rotation = GetPalmRotation();


                //Rigidbody carBody = car.GetComponent<Rigidbody>();
                //carBody.AddForce()

                Debug.Log("Cube!!!!");

            } else {
                Debug.Log("null!!!!");
            }

        }
    }
}