﻿using System.Collections.Generic;
using System.Linq;
using NLog;
using NzbDrone.Common.EnsureThat;

namespace NzbDrone.Common.Eventing
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Logger _logger;
        private readonly IEnumerable<IHandle> _handlers;

        public EventAggregator(Logger logger, IEnumerable<IHandle> handlers)
        {
            Ensure.That(() => handlers).HasItems();
            _logger = logger;
            _handlers = handlers;
        }

        public void Publish<TEvent>(TEvent message) where TEvent : IEvent
        {
            _logger.Trace("Publishing {0}", message.GetType().Name);

            foreach (var handler in _handlers.OfType<IHandle<TEvent>>())
            {
                _logger.Trace("{0} => {1}", message.GetType().Name, handler.GetType().Name);
                handler.Handle(message);
            }
        }
    }
}