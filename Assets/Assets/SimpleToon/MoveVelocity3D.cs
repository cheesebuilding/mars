using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity3D : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed = 50f;

    private Vector3 velocityVector;
    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody.velocity = velocityVector * moveSpeed;
    }

    public void Disable() {
        this.enabled = false;
        rigidbody.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}