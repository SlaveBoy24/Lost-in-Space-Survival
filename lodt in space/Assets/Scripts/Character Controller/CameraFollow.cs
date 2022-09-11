using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private string _statement;
    [SerializeField] private Transform _target;

    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;

    // swipes in building mode
    [SerializeField] private float _builidngCamMoveSpeed;
    private Vector3 _startedPosition;
    [SerializeField] private bool _isMobile;
    [SerializeField] private bool _isSwiping;
    [SerializeField] private Vector2 _tapPosition;
    [SerializeField] private Vector2 _swipeDelta;

    public void SetState(string state)
    {
        _statement = state;
    }

    private void Start()
    {
        _isMobile = Application.isMobilePlatform;

        _target = GameObject.FindGameObjectWithTag("MainCharacter").transform;
    }

    private void LateUpdate()
    {
        if (_statement == "")
        {
            Vector3 desiredPosition = _target.position + _offset;
            desiredPosition.y = _offset.y;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * _speed);
            transform.position = smoothedPosition;
        }
        else
        {
            if (!_isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isSwiping = true;
                    _tapPosition = Input.mousePosition;
                    _startedPosition = transform.position;
                }
                else if (Input.GetMouseButtonUp(0))
                { 
                    ResetSwipe();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        _isSwiping = true;
                        _tapPosition = Input.GetTouch(0).position;
                        _startedPosition = transform.position;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        ResetSwipe();
                    }
                }
            }

            CheckSwipe();
        }
    }

    private void CheckSwipe()
    {
        _swipeDelta = Vector2.zero;


        if (_isSwiping)
        {
            if (!_isMobile)
            {
                Debug.Log("Swiping");

                _swipeDelta = (Vector2)Input.mousePosition - _tapPosition;

                Vector3 move = _startedPosition;
                move.x -= _swipeDelta.x * _builidngCamMoveSpeed;
                move.z -= _swipeDelta.y * _builidngCamMoveSpeed;

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, move, Time.deltaTime * _speed);
                transform.position = smoothedPosition;
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    _swipeDelta = Input.GetTouch(0).position - _tapPosition;

                    Vector3 move = _startedPosition;
                    move.x += _swipeDelta.x;
                    move.z += _swipeDelta.y;

                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, move, Time.deltaTime * _speed);
                    transform.position = smoothedPosition;
                }
            }
        }
    }

    private void ResetSwipe()
    {
        _isSwiping = false;

        _tapPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
