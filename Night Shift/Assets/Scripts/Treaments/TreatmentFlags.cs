using UnityEngine;
using System.Collections;

public static class TreatmentFlags
{
    public static bool isHemorhagieOccupied { get; set;}
    public static bool isPsychologyOccupied { get; set; }
    public static bool isVomitoriumOccupied { get; set; }
    public static bool isSurgeryOccupied { get; set; }
    public static bool isExorcismOccupied { get; set; }

    public static readonly Vector3 VomitoriumPosition = new Vector3(-9.0f, 7.25f, -1.0f);
    public static readonly Vector3 SurgeryPosition = new Vector3(-1.0f, 7.25f, -1.0f);
    public static readonly Vector3 PsychologyPosition = new Vector3(12.0f, 3.5f, -1.0f);
    public static readonly Vector3 HemorhagiePosition = new Vector3(12.0f, -5.0f, -1.0f);
    public static readonly Vector3 ExorcismPosition = new Vector3(0.0f, -12.25f, -1.0f);
}
 