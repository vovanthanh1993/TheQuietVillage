using UnityEngine;

public class FollowTransformPos : MonoBehaviour
{
    [SerializeField] private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = target.rotation;
    }
}
