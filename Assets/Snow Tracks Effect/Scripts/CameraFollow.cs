using UnityEngine;

public enum UpdateMode
{
    Update, FixedUpdate, LateUpdate
}

public class CameraFollow : MonoBehaviour
{
    //[SerializeField] UpdateMode updateMode;
	[SerializeField] Transform target;
    [SerializeField] float SmoothTime = 0.1f;

	private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    // private float yVelocity, zVelocity  = 0;

    private bool followTarget = false;

    private void Start()
    {
        if (target == null)
            return;

        offset = transform.position - target.position;
        followTarget = true;
    }

    private void FixedUpdate()
    {
        if (target == null && !followTarget)
            return;

       /* transform.position = new Vector3(target.position.x + offset.x,
           Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref yVelocity, SmoothTime),
           Mathf.SmoothDamp(transform.position.z, target.position.z + offset.z, ref zVelocity, SmoothTime));*/
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, SmoothTime);
    }

    public void SetTarget(Transform trans)
    {
        target = trans;
        offset = transform.position - target.position;
    }

    public void PauseFollowing()
    {
        followTarget = false;
    }

    public void ResumeFollowing()
    {
        followTarget = true;
    }
}
