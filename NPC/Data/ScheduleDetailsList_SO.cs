using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScheduleDetailsList_SO", menuName = "NPC Schedule/ScheduleDetailsList")]
public class ScheduleDetailsList_SO : ScriptableObject
{
    public List<ScheduleDetails> scheduleDetailsList = new List<ScheduleDetails>();
}
