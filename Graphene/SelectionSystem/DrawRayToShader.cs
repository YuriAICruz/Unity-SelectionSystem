﻿using UnityEngine;

namespace Graphene.SelectionSystem
{
    [RequireComponent(typeof(Renderer))]
    public class DrawRayToShader : MonoBehaviour, ISelectable
    {
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        public void OnOver(Vector3 point)
        {
            SetPoitn(point);
        }

        public void OnOut()
        {
            
        }

        public void OnClick(Vector3 point)
        {
            
        }

        public void OnPassThrough(Vector3 point)
        {
            SetPoitn(point);
        }

        private void SetPoitn(Vector3 point)
        {
            _material.SetVector("_Pos", new Vector4(point.x, point.y, point.z, 0));
        }
    }
}