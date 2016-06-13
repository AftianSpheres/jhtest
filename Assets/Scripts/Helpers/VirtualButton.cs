using UnityEngine;

public class VirtualButton
{
    private bool _isKeyDown;
    private bool _isKeyUp;
    private bool _isPressed;
    private float min;
    private float max;
    public bool BtnDown
    {
        get
        {
            return _isKeyDown;
        }
    }
    public bool BtnUp
    {
        get
        {
            return _isKeyUp;
        }
    }
    public bool Pressed
    {
        get
        {
            return (_isPressed);
        }
    }
    private bool isAxis;
    private bool isNegative;
    private KeyCode key;
    private string axis;

    public VirtualButton(KeyCode _key)
    {
        key = _key;
        isAxis = false;
    }

    public VirtualButton(string _axis, bool _isNegative, float _min, float _max)
    {
        axis = _axis;
        isAxis = true;
        isNegative = _isNegative;
    }

    public void Update()
    {
        if (isAxis == true)
        {
            if ((isNegative == true && Input.GetAxis(axis) < min) || (isNegative == false && Input.GetAxis(axis) > min))
            {
                if (_isPressed == false)
                {
                    _isKeyDown = true;
                    _isKeyUp = false;
                }
                else
                {
                    _isKeyDown = false;
                }
                _isPressed = true;
            }
            else
            {
                if (_isPressed == true)
                {
                    _isKeyUp = true;
                    _isKeyDown = false;
                }
                else
                {
                    _isKeyUp = false;
                }
                _isPressed = false;
            }
        }
        else
        {
            _isKeyDown = Input.GetKeyDown(key);
            _isKeyUp = Input.GetKeyUp(key);
            _isPressed = Input.GetKey(key);
        }
    }
}