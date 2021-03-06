﻿using System;

namespace AcTools.Utils.Helpers {
    public class ActionAsDisposable : IDisposable {
        public static IDisposable Empty { get; } = new ActionAsDisposable(null);

        private readonly Action _action;

        public ActionAsDisposable(Action action) {
            _action = action;
        }

        public void Dispose() {
            _action?.Invoke();
        }
    }
}