using UnityEngine;

namespace Graphene.SelectionSystem
{
    public interface ISelectable
    {
        void OnOver(Vector3 point);
        void OnOut();
        void OnClick(Vector3 point);
    }
}