﻿using System;

namespace Poiesis.App.Base.LoadedEvent
{
    public class DelegateLoadedAction : ILoadedAction
    {
        public Action LoadedActionDelegate { get; set; }

        public DelegateLoadedAction(Action action)
        {
            LoadedActionDelegate = action;
        }

        public void ContentControlLoaded()
        {
            LoadedActionDelegate?.Invoke();
        }
    }
}
