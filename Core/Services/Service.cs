using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IgiCore.Core.Services
{
    public abstract class Service : IService
    {
        public Dictionary<string, Delegate> Events { get; } = new Dictionary<string, Delegate>();

        public abstract void Initialise();

        public void HandleEvent(string eventName, Action action) { this.Events.Add(eventName, action); }

        public void HandleEvent<T1>(string eventName, Action<T1> action) { this.Events.Add(eventName, action); }

        public void HandleEvent<T1, T2>(string eventName, Action<T1, T2> action) { this.Events.Add(eventName, action); }

        public void HandleEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) { this.Events.Add(eventName, action); }

        public void HandleJsonEvent<T>(string eventName, Action<T> action) { this.Events.Add(eventName, new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)))); }

        public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action)
        {
            this.Events.Add(
                eventName,
                new Action<string, string>(
                    (j1, j2) =>
                        action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2))));
        }

        public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
        {
            this.Events.Add(
                eventName,
                new Action<string, string, string>(
                    (j1, j2, j3) => action(
                        JsonConvert.DeserializeObject<T1>(j1),
                        JsonConvert.DeserializeObject<T2>(j2),
                        JsonConvert.DeserializeObject<T3>(j3))));
        }
    }
}
