using UnityEngine;

namespace Graphene.SelectionSystem
{
    [RequireComponent(typeof(Renderer))]
    public class DrawRayToShader : MonoBehaviour, ISelectable
    {
        private Material _material;
        private Vector3 _lastPos;
        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        public void OnOver(Vector3 point)
        {
            _lastPos = point;
            SetPoint(point);
        }

        public void OnOut()
        {
            
        }

        public void OnSelect(Vector3 point)
        {
            
        }

        public void OnDeselect()
        {
            
        }

        public void OnPassThrough(Vector3 point)
        {
            SetPoint(point);
        }

        public void OnDrag(Vector3 delta, Vector3 deltaRaw)
        {
            //delta = transform.TransformDirection(delta);
            Debug.DrawRay(_lastPos, ScreenToWorld(-delta), Color.magenta, 1);
        }

        [SerializeField] private float _ratio;
        private Vector3 ScreenToWorld(Vector3 delta)
        {
            var dist = (transform.position - Camera.main.transform.position).magnitude;

            return delta.normalized;//*_ratio*dist;
        }

        private void SetPoint(Vector3 point)
        {
            _lastPos = point;
            _material.SetVector("_Pos", new Vector4(point.x, point.y, point.z, 0));
        }
    }
}