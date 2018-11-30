using UnityEngine;

namespace Graphene.SelectionSystem
{
    public class DebugLineDir : MonoBehaviour
    {
        private ScreenListener _screenListener;
        public float Distance;

        private void Awake()
        {
            _screenListener = FindObjectOfType<ScreenListener>();

            _screenListener.UpdatePos += UpdatePos;
            _screenListener.LClickPos += LClickPos;
        }

        private void LClickPos(Ray ray)
        {
            Debug.DrawRay(ray.origin, ray.direction*Distance, Color.red);
        }


        private void UpdatePos(Ray ray)
        {
            Debug.DrawRay(ray.origin, ray.direction*Distance, Color.green);
        }
    }
}