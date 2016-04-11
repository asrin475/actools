﻿using System;
using System.IO;
using System.Linq;
using AcManager.Tools.AcManagersNew;
using AcManager.Tools.Managers.Directories;
using AcManager.Tools.Objects;
using AcTools.Utils;

namespace AcManager.Tools.Managers {
    public class TracksManager : AcManagerNew<TrackObject> {
        public static TracksManager Instance { get; private set; }

        public static TracksManager Initialize() {
            if (Instance != null) throw new Exception("already initialized");
            return Instance = new TracksManager();
        }

        public override BaseAcDirectories Directories => AcRootDirectory.Instance.TracksDirectories;

        public override TrackObject GetDefault() {
            return base.GetById("imola") ?? base.GetDefault();
        }

        public override TrackObject GetById(string id) {
            return base.GetById(id.Contains('/') ? id.Split('/')[0] : id);
        }

        public TrackBaseObject GetLayoutById(string id) {
            if (!id.Contains('/')) return base.GetById(id);
            return base.GetById(id.Split('/')[0])?.GetLayoutById(id);
        }

        protected override TrackObject CreateAcObject(string id, bool enabled) {
            return new TrackObject(this, id, enabled);
        }

        private static readonly string[] WatchedFileNames = {
            @"preview.png",
            @"outline.png",
            @"ui_track.json"
        };

        protected override bool ShouldSkipFile(string objectLocation, string filename) {
            if (base.ShouldSkipFile(objectLocation, filename)) return true;
            if (WatchedFileNames.Contains(Path.GetFileName(filename).ToLowerInvariant())) {
                return false;
            }

            var relative = FileUtils.GetRelativePath(filename, objectLocation);

            var splitted = FileUtils.Split(relative);
            if (!string.Equals(splitted[0], "ui", StringComparison.OrdinalIgnoreCase) || splitted.Length > 3) {
                return true;
            }

            return false;
        }
    }
}
