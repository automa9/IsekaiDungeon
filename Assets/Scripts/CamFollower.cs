using UnityEngine;

public class CamFollower : MonoBehaviour
{
    #region Private Fields

    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField]
    private bool followOnStart = false;

    [Tooltip("The Smoothing for the camera to follow the target")]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    // cached transform of the target
    Transform cameraTransform;

    // maintain a flag internally to reconnect if target is lost or camera is switched
    bool isFollowing;

    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;


    #endregion

    //public Transform lookAt;

    #region MonoBehaviour Callbacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase
    /// </summary>
    void Start()
    {
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }


    void FixedUpdate()
    {
        // The transform target may not destroy on level load, 
        // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }

        // only follow is explicitly declared
        if (isFollowing)
        {
            Follow();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Raises the start following event. 
    /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
    /// </summary>
    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        // we don't smooth anything, we go straight to the right camera shot
        Cut();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Follow the target smoothly
    /// </summary>
    void Follow()
    {

        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, this.transform.position + centerOffset, smoothSpeed);

        //cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position +this.transform.TransformVector(cameraOffset), smoothSpeed*Time.deltaTime);
        cameraTransform.position = smoothedPosition;
        //cameraTransform.LookAt(this.transform.position + centerOffset);

    }


    void Cut()
    {
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, this.transform.position, smoothSpeed);

        //cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position +this.transform.TransformVector(cameraOffset), smoothSpeed*Time.deltaTime);
        cameraTransform.position = smoothedPosition;
        //cameraTransform.LookAt(this.transform.position + centerOffset);
    }
    #endregion


    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(this.transform);

        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
        //transform.LookAt(this.transform, Vector3.left);
    }
}
