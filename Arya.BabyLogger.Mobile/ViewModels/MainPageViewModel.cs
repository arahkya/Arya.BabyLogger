using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;
using Arya.BabyLogger.Shared.Feed;

namespace Arya.BabyLogger.Mobile.ViewModels;

public class MainPageViewModel
{
    private static readonly HttpClient HttpClient = new();

    public class BabyEvent
    {
        public required string EventType { get; set; }
        public required string SubTitle { get; set; }
        public required string Title { get; set; }
    }

    public ObservableCollection<BabyEvent> BabyEvents { get; set; } = [];
    // public MainPageViewModel()
    // {
    //     // Sample data
    //     var now = DateTime.Now;

    //     BabyEvents.Add(new BabyEvent { EventType = "ป้อนนม", EventTime = now.AddMinutes(-20) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เปลี่ยนผ้าอ้อม", EventTime = now.AddMinutes(-40) });
    //     BabyEvents.Add(new BabyEvent { EventType = "งีบ", EventTime = now.AddHours(-1.5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "นอนคว่ำ", EventTime = now.AddHours(-2) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ขวดนม", EventTime = now.AddHours(-2.5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เรอ", EventTime = now.AddHours(-3) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เล่น", EventTime = now.AddHours(-3.5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เดินเล่น", EventTime = now.AddHours(-4) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ป้อนนม", EventTime = now.AddHours(-5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เปลี่ยนผ้าอ้อม", EventTime = now.AddHours(-5.5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "งีบ", EventTime = now.AddHours(-6) });
    //     BabyEvents.Add(new BabyEvent { EventType = "อาบน้ำ", EventTime = now.AddHours(-7) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ห่อตัว", EventTime = now.AddHours(-7.5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "วัดอุณหภูมิ", EventTime = now.AddHours(-8) });
    //     BabyEvents.Add(new BabyEvent { EventType = "วิตามินดี", EventTime = now.AddHours(-8.5) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ชั่งน้ำหนัก", EventTime = now.AddHours(-9) });
    //     BabyEvents.Add(new BabyEvent { EventType = "พบแพทย์", EventTime = now.AddHours(-10) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ฉีดวัคซีน", EventTime = now.AddHours(-11) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ชิมอาหารบด", EventTime = now.AddHours(-12) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ย้ายลงเตียง", EventTime = now.AddHours(-13) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ตื่น", EventTime = now.AddHours(-14) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ยืดเส้น", EventTime = now.AddHours(-15) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ป้อนนมระหว่างหลับ", EventTime = now.AddHours(-16) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ป้อนนมเที่ยงคืน", EventTime = now.AddHours(-18) });
    //     BabyEvents.Add(new BabyEvent { EventType = "งีบในรถเข็น", EventTime = now.AddHours(-19) });
    //     BabyEvents.Add(new BabyEvent { EventType = "นั่งคาร์ซีท", EventTime = now.AddHours(-20) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เปลี่ยนชุด", EventTime = now.AddHours(-21) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ปั๊มนม", EventTime = now.AddHours(-22) });
    //     BabyEvents.Add(new BabyEvent { EventType = "กอดอุ่น", EventTime = now.AddHours(-23) });
    //     BabyEvents.Add(new BabyEvent { EventType = "อ่านนิทาน", EventTime = now.AddHours(-24) });
    //     BabyEvents.Add(new BabyEvent { EventType = "กล่อมเพลง", EventTime = now.AddHours(-25) });
    //     BabyEvents.Add(new BabyEvent { EventType = "ป้อนนมเช้ามืด", EventTime = now.AddHours(-26) });
    //     BabyEvents.Add(new BabyEvent { EventType = "เปลี่ยนผ้าอ้อม", EventTime = now.AddHours(-27) });
    // }


    public async void LoadEvents()
    {
        var events = await ListFeedResponseAsync();

        BabyEvents.Clear();
        foreach (var babyEvent in events)
        {
            BabyEvents.Add(babyEvent);
        }
    }

    private async Task<List<BabyEvent>> ListFeedResponseAsync()
    {
        const string UrlEndpoint = "http://localhost:5001/api/feed";

        var response = await HttpClient.GetFromJsonAsync<ListFeedResponse>(UrlEndpoint);
        if (response?.Items == null || response.Items.Count == 0)
        {
            return [];
        }

        var events = new List<BabyEvent>(response.Items.Count);
        foreach (var item in response.Items)
        {
            events.Add(new BabyEvent { Title = item.Title, SubTitle = item.Time.ToString("d MMM yyyy HH:mm", new CultureInfo("th-TH")), EventType = item.Type });
        }

        return events;
    }
}
