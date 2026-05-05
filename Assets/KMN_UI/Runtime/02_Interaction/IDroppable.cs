using UnityEngine.EventSystems;

namespace KMN.UI.Interaction
{
    public interface IDroppable
    {
        void OnDrop(PointerEventData eventData);
    }
}