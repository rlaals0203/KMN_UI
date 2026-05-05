using UnityEngine.EventSystems;

namespace KMN.UI.Interaction
{
    public delegate void OnClickEvent(IClickable clickable);
    
    public interface IClickable
    {
        public void OnClick(PointerEventData eventData) { }
    }
}