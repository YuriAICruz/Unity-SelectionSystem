using UnityEngine;

namespace Graphene.SelectionSystem
{
    [RequireComponent(typeof(Renderer))]
    public class Outline : MonoBehaviour, ISelectable
    {
        private Material _material;

        public Color NormalColor, SelectedColor;
        
        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _material.SetInt("_Outline", 0);
            _material.SetColor("_OutlineColor", NormalColor);
        }
        
        public void OnOver(Vector3 point)
        {
            _material.SetInt("_Outline", 1);
        }

        public void OnOut()
        {
            _material.SetInt("_Outline", 0);
        }

        public void OnSelect(Vector3 point)
        {
            _material.SetColor("_OutlineColor", SelectedColor);
        }

        public void OnDeselect()
        {
            _material.SetInt("_Outline", 0);
            _material.SetColor("_OutlineColor", NormalColor);
        }

        public void OnPassThrough(Vector3 point)
        {
            //_material.SetInt("_Outline", 0);
        }

        public void OnDrag(Vector3 delta, Vector3 deltaRaw)
        {
        }
    }
}