using System;
using UnityEngine;

namespace Snow
{
    public class MirrorCam : MonoBehaviour
    {
        [SerializeField] protected Camera cam;

        private void Awake()
        {
            cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));;
        }
    }
}
