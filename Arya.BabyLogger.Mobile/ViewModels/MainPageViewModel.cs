using System.Collections.ObjectModel;

namespace Arya.BabyLogger.Mobile.ViewModels;

public class MainPageViewModel
{
    public class BabyEvent
    {
        public required string EventType { get; set; }
        public DateTime EventTime { get; set; }
    }

    public ObservableCollection<BabyEvent> BabyEvents { get; set; } = [];
    public MainPageViewModel()
    {
        // Sample data
        var now = DateTime.Now;

        BabyEvents.Add(new BabyEvent { EventType = "Feeding", EventTime = now.AddMinutes(-20) });
        BabyEvents.Add(new BabyEvent { EventType = "Diaper Change", EventTime = now.AddMinutes(-40) });
        BabyEvents.Add(new BabyEvent { EventType = "Nap", EventTime = now.AddHours(-1.5) });
        BabyEvents.Add(new BabyEvent { EventType = "Tummy Time", EventTime = now.AddHours(-2) });
        BabyEvents.Add(new BabyEvent { EventType = "Bottle", EventTime = now.AddHours(-2.5) });
        BabyEvents.Add(new BabyEvent { EventType = "Burping", EventTime = now.AddHours(-3) });
        BabyEvents.Add(new BabyEvent { EventType = "Play", EventTime = now.AddHours(-3.5) });
        BabyEvents.Add(new BabyEvent { EventType = "Walk", EventTime = now.AddHours(-4) });
        BabyEvents.Add(new BabyEvent { EventType = "Feeding", EventTime = now.AddHours(-5) });
        BabyEvents.Add(new BabyEvent { EventType = "Diaper Change", EventTime = now.AddHours(-5.5) });
        BabyEvents.Add(new BabyEvent { EventType = "Nap", EventTime = now.AddHours(-6) });
        BabyEvents.Add(new BabyEvent { EventType = "Bath", EventTime = now.AddHours(-7) });
        BabyEvents.Add(new BabyEvent { EventType = "Swaddle", EventTime = now.AddHours(-7.5) });
        BabyEvents.Add(new BabyEvent { EventType = "Temperature Check", EventTime = now.AddHours(-8) });
        BabyEvents.Add(new BabyEvent { EventType = "Vitamin D", EventTime = now.AddHours(-8.5) });
        BabyEvents.Add(new BabyEvent { EventType = "Weight Check", EventTime = now.AddHours(-9) });
        BabyEvents.Add(new BabyEvent { EventType = "Doctor Visit", EventTime = now.AddHours(-10) });
        BabyEvents.Add(new BabyEvent { EventType = "Vaccination", EventTime = now.AddHours(-11) });
        BabyEvents.Add(new BabyEvent { EventType = "Solid Tasting", EventTime = now.AddHours(-12) });
        BabyEvents.Add(new BabyEvent { EventType = "Crib Transfer", EventTime = now.AddHours(-13) });
        BabyEvents.Add(new BabyEvent { EventType = "Wake", EventTime = now.AddHours(-14) });
        BabyEvents.Add(new BabyEvent { EventType = "Stretch", EventTime = now.AddHours(-15) });
        BabyEvents.Add(new BabyEvent { EventType = "Dream Feed", EventTime = now.AddHours(-16) });
        BabyEvents.Add(new BabyEvent { EventType = "Midnight Feeding", EventTime = now.AddHours(-18) });
        BabyEvents.Add(new BabyEvent { EventType = "Stroller Nap", EventTime = now.AddHours(-19) });
        BabyEvents.Add(new BabyEvent { EventType = "Car Seat Ride", EventTime = now.AddHours(-20) });
        BabyEvents.Add(new BabyEvent { EventType = "Outfit Change", EventTime = now.AddHours(-21) });
        BabyEvents.Add(new BabyEvent { EventType = "Pumping", EventTime = now.AddHours(-22) });
        BabyEvents.Add(new BabyEvent { EventType = "Snuggle", EventTime = now.AddHours(-23) });
        BabyEvents.Add(new BabyEvent { EventType = "Story Time", EventTime = now.AddHours(-24) });
        BabyEvents.Add(new BabyEvent { EventType = "Lullaby", EventTime = now.AddHours(-25) });
        BabyEvents.Add(new BabyEvent { EventType = "Early Morning Feed", EventTime = now.AddHours(-26) });
        BabyEvents.Add(new BabyEvent { EventType = "Diaper Change", EventTime = now.AddHours(-27) });
    }
}