using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace Graphene.SelectionSystem
{
    [RequireComponent(typeof(Camera))]
    public class ScreenListener : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _lastPos;

        public event Action<Ray> UpdatePos, LClickPos;

        public LayerMask Mask;
        private RaycastHit _hit;

        private Queue<ISelectable> _selectablesOver = new Queue<ISelectable>();
        private ISelectable _lastSelected;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            ListenMouse();
        }

        private void ListenMouse()
        {
            var pos = Input.mousePosition;

            var ray = _camera.ScreenPointToRay(new Vector3(pos.x, pos.y, _camera.nearClipPlane));

            UpdatePos?.Invoke(ray);

            if (Input.GetMouseButtonDown(0))
            {
                LClickPos?.Invoke(ray);
            }

            if (Physics.Raycast(ray, out _hit, _camera.farClipPlane, Mask))
            {
                var slt = _hit.collider.GetComponent<ISelectable>();
                if (slt != null)
                {
                    if (_lastSelected != slt)
                        UnOverSelectables();
                    _lastSelected = slt;
                    if (Input.GetMouseButtonDown(0))
                        slt.OnClick(_hit.point);
                    else
                    {
                        slt.OnOver(_hit.point);
                        _selectablesOver.Enqueue(slt);
                    }
                }
                else
                    UnOverSelectables();
            }
            else
                UnOverSelectables();

            _lastPos = pos;
        }

        private void UnOverSelectables()
        {
            for (int i = 0; i < _selectablesOver.Count; i++)
            {
                _selectablesOver.Dequeue().OnOut();
            }
        }
    }
}