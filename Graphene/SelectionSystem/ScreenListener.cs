using System;
using System.Collections.Generic;
using System.Linq;
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

        private Queue<ISelectable> _selectablesOver = new Queue<ISelectable>();
        private ISelectable _lastOver;
        private RaycastHit[] _hits;

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

            _hits = Physics.RaycastAll(ray, _camera.farClipPlane, Mask);

            if (_hits.Length > 0)
            {
                var lst = _hits.ToList().OrderBy(x => x.distance).ToArray();
                var clear = false;
                for (int i = 0; i < lst.Length; i++)
                {
                    var slt = lst[i].collider.GetComponent<ISelectable>();

                    if (slt != null)
                    {
                        var first = i == 0;

                        if (first)
                        {
                            if (_lastOver != slt)
                            {
                                Debug.Log("UnOverSelectables");
                                UnOverSelectables();
                            }
                            if (Input.GetMouseButtonDown(0))
                            {
                                slt.OnClick(lst[i].point);
                            }
                            else
                            {
                                slt.OnOver(lst[i].point);
                                if (!_selectablesOver.Contains(slt))
                                    _selectablesOver.Enqueue(slt);
                                _lastOver = slt;
                            }
                        }
                        else
                        {
                            slt.OnPassThrough(lst[i].point);
                        }
                    }
                    else clear = true;
                }
                if (clear)
                {
                    UnOverSelectables();
                }
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