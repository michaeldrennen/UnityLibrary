using UnityEngine;

public class AimAtTarget : MonoBehaviour {
    public  Transform  target;          // Target Object
    public  float      aimSpeed = 5.0f; // Speed at Which I lock on the Target
    private Quaternion _newRotation;
    private float      _orientTransform;
    private float      _orientTarget;


    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        _orientTransform = transform.position.x;
        _orientTarget    = target.position.x;
        // To Check on which side is the target , i.e. Right or Left of this Object
        if (_orientTransform > _orientTarget) {
            // Will Give Rotation angle , so that Arrow Points towards that target
            _newRotation = Quaternion.LookRotation(transform.position - target.position, -Vector3.up);
        }
        else {
            _newRotation = Quaternion.LookRotation(transform.position - target.position, Vector3.up);
        }

        // Here we have to freeze rotation along X and Y Axis, for proper movement of the arrow
        _newRotation.x = 0.0f;
        _newRotation.y = 0.0f;

        // Finally rotate and aim towards the target direction using Code below
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * aimSpeed);
    }
}
