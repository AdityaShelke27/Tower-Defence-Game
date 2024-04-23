using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed;
    public float zoomSpeed;
    public float panBorderThickness = 10;
    public Transform maxPos;
    public Transform minPos;
    private int mousePosx;
    private int mousePosy;
    private void OnEnable()
    {
        GameManager.s_GameOver += DisableScript;
    }
    private void OnDisable()
    {
        GameManager.s_GameOver -= DisableScript;
    }
    void Update()
    {
        if(Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            mousePosy = 1;
        }
        else if(Input.mousePosition.y <= panBorderThickness)
        {
            mousePosy = -1;
        }
        else
        {
            mousePosy = 0;
        }

        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            mousePosx = 1;
        }
        else if (Input.mousePosition.x <= panBorderThickness)
        {
            mousePosx = -1;
        }
        else
        {
            mousePosx = 0;
        }
        MoveCamera();
    }

    void MoveCamera()
    {
        float hor = System.Math.Sign(Input.GetAxisRaw("Horizontal") + mousePosx) * panSpeed * Time.deltaTime;
        float ver = System.Math.Sign(Input.GetAxisRaw("Vertical") + mousePosy) * panSpeed * Time.deltaTime;
        float zoom = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;

        Vector3 moveDir = new Vector3(ver, 0, -hor) + transform.forward * zoom;
        transform.Translate(moveDir, Space.World);
        Vector3 camPos = transform.position;
        camPos.x = Mathf.Clamp(camPos.x, minPos.position.x, maxPos.position.x);
        camPos.y = Mathf.Clamp(camPos.y, minPos.position.y, maxPos.position.y);
        camPos.z = Mathf.Clamp(camPos.z, minPos.position.z, maxPos.position.z);
        transform.position = camPos;
        //CREATE A UI TO CHANGE THE PANSPEED AND ZOOMSPEED
    }

    void DisableScript(int none)
    {
        this.enabled = false;
    }
}
