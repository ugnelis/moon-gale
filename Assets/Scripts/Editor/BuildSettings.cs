using System;
using UnityEditor;
using UnityEngine;

namespace MoonGale.Editor
{
    internal struct BuildSettings
    {
        private const string DefaultBuildName = "moon-gale";
        private const string DefaultBuildNameDelimiter = "-";
        private const string DefaultBuildDirectory = "Builds";
        private const string DefaultArchiveDirectory = "Builds";
        private const string DefaultBuildDateTimeFormat = "yyyyMMddHHmm";

        public string BuildName { get; set; }

        public string BuildNameDelimiter { get; set; }

        public string BuildVersion { get; set; }

        public BuildTarget BuildTarget { get; }

        public string BuildDateTimeString => BuildDateTime.ToString(DateTimeFormat);

        public string DateTimeFormat { private get; set; }

        public DateTime BuildDateTime { private get; set; }

        public string BuildDirectory { get; set; }

        public string ArchiveDirectory { get; set; }

        public bool IsDevelopmentBuild { get; set; }

        public bool IsAutoRunPlayer { get; set; }

        public bool IsArchivePlayer { get; set; }

        public bool IsAppendVersionToBuildName { get; set; }

        public bool IsAppendPlatformToBuildName { get; set; }

        public bool IsAppendDateToBuildName { get; set; }

        public bool IsAppendVersionToArchiveName { get; set; }

        public bool IsAppendPlatformToArchiveName { get; set; }

        public bool IsAppendDateToArchiveName { get; set; }

        public BuildSettings(BuildTarget buildTarget)
        {
            BuildName = DefaultBuildName;
            BuildNameDelimiter = DefaultBuildNameDelimiter;
            BuildVersion = Application.version;
            BuildTarget = buildTarget;

            BuildDirectory = DefaultBuildDirectory;
            ArchiveDirectory = DefaultArchiveDirectory;

            BuildDateTime = DateTime.Now;
            DateTimeFormat = DefaultBuildDateTimeFormat;

            IsDevelopmentBuild = false;
            IsAutoRunPlayer = false;
            IsArchivePlayer = false;

            IsAppendVersionToBuildName = false;
            IsAppendPlatformToBuildName = false;
            IsAppendDateToBuildName = false;

            IsAppendVersionToArchiveName = false;
            IsAppendPlatformToArchiveName = false;
            IsAppendDateToArchiveName = false;
        }
    }
}
