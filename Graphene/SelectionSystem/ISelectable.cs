using UnityEngine;

namespace Graphene.SelectionSystem
{
    public interface ISelectable
    {
        void OnOver(Vector3 point);
        void OnOut();
        void OnSelect(Vector3 point);
        void OnDeselect();
        void OnPassThrough(Vector3 point);
        void OnDrag(Vector3 delta, Vector3 deltaRaw);
    }
}