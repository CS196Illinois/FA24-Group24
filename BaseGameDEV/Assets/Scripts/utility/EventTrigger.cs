using System;

public static class EventTrigger 
//This should handle all script triggered event.
{
    //Create a delegate for the event
    public delegate void UpdateTextEventHandler(string text);

    //Create an event based on the delegate
    public static event UpdateTextEventHandler OnUpdateText;

    public static event UpdateTextEventHandler OnUpdateInv;

    public static event UpdateTextEventHandler OnUpdateupdate;

    //A method to invoke the event
    public static void TriggerUpdateText(string newText, string newInv) 
    {
        OnUpdateText?.Invoke(newText);
        OnUpdateInv?.Invoke(newInv);
        
    }
    
    //Update minigametext hopefully
    public static void TriggerUpdateText(string oneText) {
        OnUpdateupdate?.Invoke(oneText);
    }

}