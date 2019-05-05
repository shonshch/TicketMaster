using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Threading.Tasks;
using GogoKit;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using TaskExtensions = GogoKit.TaskExtensions;

namespace TicketMasterApp
{
    class EvenGenrator
    {
        // Access to Viagogo API
        private readonly IViagogoClient _viagogoClient = new ViagogoClient(
            new ProductHeaderValue("TicketMaster"),
            "TaRJnBcw1ZvYOXENCtj5",
            "ixGDUqRA5coOHf3FQysjd704BPptwbk6zZreELW2aCYSmIT8XJ9ngvN1MuKV");

        public async Task ReturnChapestTicket()
        {
            var searchRequest = new SearchResultRequest();
            // Set type of search to category (to find artist)
            searchRequest.Parameters.Add("type", "category");
            var searchResults = await _viagogoClient.Search.GetAsync("tomorrowland", searchRequest);
            var category =
                await _viagogoClient.Hypermedia.GetAsync<Category>(searchResults.Items.First().CategoryLink);
            var eventsRequest = new EventRequest();
            // Pagination for events request
            eventsRequest.Parameters.Add("page_size", "100000");
            // Retrieve all events
            var allEvents =await _viagogoClient.Events.GetAllByCategoryAsync((int) category.Id, eventsRequest);
            var currentEvents = RelevantEventsInfo(allEvents, new DateTime(2019, 7, 25),
                new DateTime(2019, 7, 29), 700);
            System.Console.WriteLine(currentEvents[0].LocalWebPageLink.HRef);
        }

        private List<Event> RelevantEventsInfo(IReadOnlyList<Event> events, DateTime minDate, DateTime maxDate,
            int price)
        {
            // Create Hashtable list to store all relevant values for each event
            List<Event> eventsInfo = new List<Event>();
            for (int i = 0; i < events.Count; i++)
            {
                // Store current event in variable local to loop
                var Event = events[i];

                // If event doesn't pass filtering, skip this iteration of the loop
                if (minDate.Ticks > Event.StartDate.Value.DateTime.Ticks ||
                    maxDate.Ticks < Event.StartDate.Value.DateTime.Ticks ||
                    price < Event.MinTicketPrice.Amount ||
                    !Event.Name.Contains("Full Madness"))
                {
                    continue;
                }

                // Create Hashtable in eventsInfo list
                eventsInfo.Add(Event);

            }

            return eventsInfo;
        }
    }
}