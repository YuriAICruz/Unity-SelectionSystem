using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using Vuforia;

namespace Graphene.SelectionSystem
{
    [RequireComponent(typeof(Camera))]
    public class ScreenListener : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _lastPos;

        [SerializeField] private InputWrapper _input;

        public event Action<Ray> UpdatePos, LClickPos;
        public event Action<Vector3> Drag;

        public LayerMask Mask;

        private Queue<ISelectable> _selectablesOver = new Queue<ISelectable>();
        private ISelectable _lastOver;
        private List<ISelectable> _selected = new List<ISelectable>();
        private RaycastHit[] _hits;

        private void Awake()
        {
            _input.Init();
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

            DelegateRays(ray);

            RaycastFromRay(ray);

            CalculateDrag(pos);

            _lastPos = pos;
        }

        private void CalculateDrag(Vector3 pos)
        {
            if (!Input.GetMouseButton(0)) return;
            
            var delta = 
                _camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, _camera.nearClipPlane)) -
                _camera.ScreenToWorldPoint(new Vector3(_lastPos.x, _lastPos.y, _camera.nearClipPlane));
            
            foreach (var sel in _selected)
            {
                sel.OnDrag(delta, pos-_lastPos);
            }

            Drag?.Invoke(delta);
        }

        private void RaycastFromRay(Ray ray)
        {
            _hits = Physics.RaycastAll(ray, _camera.farClipPlane, Mask);

            if (_hits.Length > 0)
            {
                var lst = _hits.ToList().FindAll(x => x.collider.GetComponent<ISelectable>() != null).OrderBy(x => x.distance).ToArray();
                var first = true;
                for (var i = 0; i < lst.Length; i++)
                {
                    var slt = lst[i].collider.GetComponent<ISelectable>();

                    if (first)
                    {
                        first = false;
                        if (_lastOver != slt)
                        {
                            UnOverSelectables();
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (_input.Add)
                            {
                                slt.OnSelect(lst[i].point);
                                _selected.Add(slt);
                            }
                            else if (_input.Subtract)
                            {
                                if (_selected.Contains(slt))
                                {
                                    var k = _selected.FindIndex(x => x == slt);

                                    if (k >= 0)
                                    {
                                        _selected[k].OnDeselect();
                                        _selected.RemoveAt(k);
                                    }
                                }
                            }
                            else
                            {
                                slt.OnSelect(lst[i].point);
                                if (!_selected.Contains(slt))
                                    Deselect();
                                _selected.Add(slt);
                            }
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
                if (lst.Length == 0)
                {
                    UnOverSelectables();
                }
            }
            else
                UnOverSelectables();
        }

        private void DelegateRays(Ray ray)
        {
            UpdatePos?.Invoke(ray);

            if (Input.GetMouseButtonDown(0))
            {
                LClickPos?.Invoke(ray);
            }
        }

        private void Deselect()
        {
            foreach (var sel in _selected)
            {
                sel.OnDeselect();
            }

            _selected = new List<ISelectable>();
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