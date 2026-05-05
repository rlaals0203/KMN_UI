using UnityEngine;

namespace KMN.UI.ContextMenu
{
    [CreateAssetMenu(fileName = "ContextMenuSO", menuName = "SO/ContextMenu", order = 10)]
    public class ContextMenuSO : ScriptableObject
    {
        public BaseContextMenu menu;
    }
}