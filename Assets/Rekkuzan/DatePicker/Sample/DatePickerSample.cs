using Rekkuzan.DatePicker;
using UnityEngine;

public class DatePickerSample : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI Result;
    public void OnDateClicked()
    {
        NativeDatePicker.Instance.RequestDatePicker(e =>
        {
            Debug.Log(e);
            Result.text = string.Format("{0:yyyy-mm-dd}", e);
        });
    }
}
