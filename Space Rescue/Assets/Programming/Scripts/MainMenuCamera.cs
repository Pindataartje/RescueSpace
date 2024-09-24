using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] Transform _camera;

    [SerializeField] bool _forward;

    [SerializeField] float _speed;

    void Update()
    {
        if (_forward)
        {
            _camera.transform.Rotate(new Vector3(0, _speed * Time.deltaTime, 0));
        }
        else
        {
            _camera.transform.Rotate(new Vector3(0, -_speed * Time.deltaTime, 0));
        }
    }
}
