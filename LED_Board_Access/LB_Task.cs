using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LED_Board_Access
{
public class LB_Task
    {

    private Theme theme;
    private TaskScheduler.LB_DateTime timeFrom;
    private TaskScheduler.LB_DateTime timeTo;

    public TaskScheduler.LB_DateTime TimeFrom
    {
        get { return timeFrom; }
        set { timeFrom = value; }
    }
    public TaskScheduler.LB_DateTime TimeTo
    {
        get { return timeTo; }
        set { timeTo = value; }
    }
    public string ThemeName
    {
        get;
        set;
    }
    private static int id = 0;
    private int task_id = id;
    public int TaskID
    {
        get { return task_id; }
    }
    public LB_Task()
    {
        id++;
    }
    public LB_Task(TaskScheduler.LB_DateTime TimeFrom, TaskScheduler.LB_DateTime TimeTo, Theme theme)
    {
        id++;
        this.theme = theme;
        this.TimeFrom = TimeFrom;
        this.TimeTo = TimeTo;
    }
    public void SetTheme(Theme theme)
    {
        this.theme = theme;
    }
    public void SetTimeFrom(TaskScheduler.LB_DateTime time)
    {
        TimeFrom = time;
    }
    public void SetTimeTo(TaskScheduler.LB_DateTime time)
    {
        TimeTo = time;
    }
    public void SetTimeFrom(int h,int m)
    {
        timeFrom.hour = h;
        timeFrom.minute = m;
    }
    public void SetTimeTo(int h, int m)
    {
        timeTo.hour = h;
        timeTo.minute = m;
    }
    public void SetDay(DayOfWeek day)
    {
        timeFrom.day = day;
        timeTo.day = day;
    }
   /* public override bool Equals(Object obj)
    {
        LB_Task task = obj as LB_Task;
        if (task == null) return false;
        return TaskID == task.TaskID;
    }
    public override int GetHashCode()
    {
        return this.TaskID;
    }*/
    }
}
