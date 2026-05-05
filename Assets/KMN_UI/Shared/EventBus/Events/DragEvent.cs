using UnityEngine;

namespace KMN.EventsBus
{
    public struct DragEvent : IEvent
    {
        public Sprite Sprite { get; }
        public bool IsDragStart { get; }

        public DragEvent(Sprite sprite, bool isDragStart)
        {
            Sprite = sprite;
            IsDragStart = isDragStart;
        }
    }
}