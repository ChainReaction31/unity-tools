using UnityEngine;

public class FPSFlyCam : MonoBehaviour
{
    private CharacterController controller;
    private float yaw = 0f;
    private float pitch = 0f;
    
    public float Speed = 5f;
    public float viewSpeedH = 2f;
    public float viewSpeedV = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = 0f;
        // check for "jump/crouch"
        if (Input.GetButton("Jump"))
        {
            vertical = 10f;
        }else if (Input.GetButton("Crouch"))
        {
            vertical = -10f;
        }
        // Todo: Break vertical movement out from horizontal movement
        Vector3 move =  new Vector3(Input.GetAxis("Horizontal"), vertical, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);
        move = move * Speed;
        controller.Move(move * Time.deltaTime * Speed);
        
        // Mouse Look Controls
        yaw += viewSpeedH * Input.GetAxis("Mouse X");
        pitch -= viewSpeedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
