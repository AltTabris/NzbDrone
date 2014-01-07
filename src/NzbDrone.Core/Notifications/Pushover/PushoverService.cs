﻿using NzbDrone.Core.Messaging.Commands;
using RestSharp;
using NzbDrone.Core.Rest;

namespace NzbDrone.Core.Notifications.Pushover
{
    public interface IPushoverProxy
    {
        void SendNotification(string title, string message, string apiKey, string userKey, PushoverPriority priority);
    }

    public class PushoverProxy : IPushoverProxy, IExecute<TestPushoverCommand>
    {
        private const string URL = "https://api.pushover.net/1/messages.json";

        public void SendNotification(string title, string message, string apiKey, string userKey, PushoverPriority priority)
        {
            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.AddParameter("token", apiKey);
            request.AddParameter("user", userKey);
            request.AddParameter("title", title);
            request.AddParameter("message", message);
            request.AddParameter("priority", (int)priority);

            client.ExecuteAndValidate(request);
        }

        public void Execute(TestPushoverCommand message)
        {
            const string title = "Test Notification";
            const string body = "This is a test message from NzbDrone";

            SendNotification(title, body, message.ApiKey, message.UserKey, (PushoverPriority)message.Priority);
        }
    }
}
