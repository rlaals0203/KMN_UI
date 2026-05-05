using UnityEngine.EventSystems;

namespace KMN.UI.Interaction
{
    public interface IHoverable
    {
        public void OnHoverEnter(PointerEventData eventData);
        public void OnHoverExit(PointerEventData eventData);
    }
}