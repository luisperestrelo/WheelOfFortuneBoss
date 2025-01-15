using UnityEngine;
using UnityEngine.UI;

public class UISegment : MonoBehaviour
{
    public int Index { get; private set; }
    public Image SegmentImage { get; private set; }
    public Button SegmentButton { get; private set; }
    private UIWheel uiWheel;

    public void Initialize(int index, Sprite icon, UIWheel uiWheel)
    {
        Index = index;
        SegmentImage = GetComponent<Image>();
        SegmentImage.sprite = icon;
        SegmentButton = GetComponent<Button>();
        this.uiWheel = uiWheel;

        SegmentButton.onClick.AddListener(OnSegmentClick);
        SegmentImage.enabled = true;
    }

    private void OnSegmentClick()
    {
        uiWheel.OnSegmentSelected(Index);
    }
}
