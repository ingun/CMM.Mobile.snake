using CMM.Mobile.snake;

public class TouchEffect : RoutingEffect
{
    public event TouchActionEventHandler TouchAction;

    public TouchEffect() : base("MyCompany.TouchEffect")
    {
    }

    public bool Capture { set; get; }

    public void OnTouchAction(Element element, TouchActionEventArgs args)
    {
        TouchAction?.Invoke(element, args);
    }
}

public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

public class TouchActionEventArgs : EventArgs
{
    public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
    {
        Id = id;
        Type = type;
        Location = location;
        IsInContact = isInContact;
    }

    public long Id { get; set; }
    public TouchActionType Type { get; set; }
    public Point Location { get; set; }
    public bool IsInContact { get; set; }
}

public enum TouchActionType
{
    Pressed,
    Moved,
    Released,
    Cancelled
}
