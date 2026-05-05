using UnityEngine.EventSystems;

namespace KMN.UI.Interaction
{
    public interface IDraggable
    {
        void OnDragStart(PointerEventData eventData);
        void OnDrag(PointerEventData eventData) { }
        void OnDragEnd(PointerEventData eventData);
    }
}