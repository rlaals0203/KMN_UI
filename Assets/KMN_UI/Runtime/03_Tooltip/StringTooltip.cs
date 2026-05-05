using TMPro;
using UnityEngine;

namespace KMN.UI.Tooltip
{
    public class StringTooltip : BaseTooltip<string>
    {
        [SerializeField] private TextMeshProUGUI text;
        
        protected override void ShowTooltip(string data)
        {
            text.text = data;
        }
    }
}