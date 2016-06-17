using UnityEngine;

public class VirtualStick
{
    private bool isDigital;
    private float _x;
    private float _y;
    private string xAxisName;
    private string yAxisName;
    private float deadZone;
    private float sensitivity;
    private bool invertX;
    private bool invertY;

    public float x
    {
        get
        {
            return _x;
        }
    }

    public float y
    {
        get
        {
            return _y;
        }
    }

    public void Update()
    {
        _x = Input.GetAxis(xAxisName);
        _y = Input.GetAxis(yAxisName);
        if (Mathf.Abs(_x) + Mathf.Abs(_y) < deadZone)
        {
            _x = 0;
            _y = 0;
        }
        _x *= sensitivity * sensitivity * Mathf.Abs(_x / sensitivity);
        _y *= sensitivity * sensitivity * Mathf.Abs(_y / sensitivity);
        if (invertX == true)
        {
            _x *= -1;
        }
        if (invertY == true)
        {
            _y *= -1;
        }
    }

    public VirtualStick(string _xAxisName, string _yAxisName, float _deadZone, float _sensitivity, bool _invertX, bool _invertY)
    {
        xAxisName = _xAxisName;
        yAxisName = _yAxisName;
        deadZone = _deadZone;
        sensitivity = _sensitivity;
        invertX = _invertX;
        invertY = _invertY;
    }

}