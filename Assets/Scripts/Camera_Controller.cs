using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    //Shake variables
    [SerializeField]
    private float shake_intensity;
    private Vector3 move;
    private int[] dir = new int[2];

    [SerializeField]
    private float shake_duration;
    private float shake_time = 0;
    public  bool  canShake   = false;

    //Camera
    private new Camera camera;
    private Vector3 ori_pos;


    void Start()
    {
        dir[0]  = -1;
        dir[1]  = 1;

        camera  = Camera.main;
        ori_pos = camera.transform.localPosition;
        move    = ori_pos;
    }


    public void Reset_Shake()
    {
        shake_time = 0;
        camera.transform.localPosition = ori_pos;
    }

    public void CameraShake()
    {
        if(shake_time < shake_duration)
        {
            move.x = shake_intensity * Mathf.PerlinNoise(Time.time, 0) * dir[Random.Range(0, 2)];
            move.y = shake_intensity * Mathf.PerlinNoise(0, Time.time) * dir[Random.Range(0, 2)];

            camera.transform.localPosition = ori_pos + move;

            shake_time += Time.deltaTime;
        }
        else
        {
            Reset_Shake();
            canShake = false;
        }
    }
}
