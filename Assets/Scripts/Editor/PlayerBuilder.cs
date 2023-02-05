using System;
using System.Collections.Generic;
using System.IO;
using Unity.SharpZipLib.Zip;
using Unity.SharpZipLib.Zip.Compression;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace MoonGale.Editor
{
    internal static class PlayerBuilder
    {
        private const string WindowsWindowsExtension = "exe";
        private const string AndroidExecutableExtension = "apk";
        private const string BurstDebugInformationSuffix = "_BurstDebugInformation_DoNotShip";

        private const Deflater.CompressionLevel ArchiveCompressionLevel =
            Deflater.CompressionLevel.BEST_COMPRESSION;

        private const string BuildArchiveExtension = "zip";

        /// <returns>
        /// Report containing build results of given <paramref name="settings"/>.
        /// </returns>
        public static BuildReport Build(BuildSettings settings)
        {
            var playerOptions = CreateBuildPlayerOptions(settings);
            var buildReport = BuildPipeline.BuildPlayer(playerOptions);

            DeleteBurstDebugInformationDirectory(settings);

            if (settings.IsArchivePlayer)
            {
                Archive(settings);
            }

            return buildReport;
        }

        private static BuildPlayerOptions CreateBuildPlayerOptions(BuildSettings settings)
        {
            return new BuildPlayerOptions
            {
                scenes = GetEnabledScenePaths(),
                locationPathName = GetBuildPath(settings),
                options = CreateBuildOptions(settings),
                target = settings.BuildTarget
            };
        }

        private static string[] GetEnabledScenePaths()
        {
            var scenePaths = new List<string>();
            var scenes = EditorBuildSettings.scenes;

            foreach (var scene in scenes)
            {
                if (scene.enabled == false)
                {
                    continue;
                }

                var scenePath = scene.path;
                scenePaths.Add(scenePath);
            }

            return scenePaths.ToArray();
        }

        private static BuildOptions CreateBuildOptions(BuildSettings settings)
        {
            var options = BuildOptions.None;

            if (settings.IsDevelopmentBuild)
            {
                options |= BuildOptions.Development;
            }

            if (settings.IsAutoRunPlayer)
            {
                options |= BuildOptions.AutoRunPlayer;
            }

            return options;
        }

        private static void DeleteBurstDebugInformationDirectory(BuildSettings settings)
        {
            var buildPath = GetBuildPath(settings);
            var buildDirectoryPath = Path.GetDirectoryName(buildPath);
            if (string.IsNullOrWhiteSpace(buildDirectoryPath))
            {
                return;
            }

            var burstDebugDirectoryName = $"{Path.GetFileNameWithoutExtension(buildPath)}" +
                                          $"{BurstDebugInformationSuffix}";

            var burstDebugDirectoryPath = Path.Combine(buildDirectoryPath, burstDebugDirectoryName);
            if (Directory.Exists(burstDebugDirectoryPath))
            {
                Directory.Delete(burstDebugDirectoryPath, true);
            }
        }

        private static void Archive(BuildSettings settings)
        {
            var buildPath = GetBuildPath(settings);
            var buildDirectoryPath = Path.GetDirectoryName(buildPath);
            var archiveFilePath = GetArchiveFilePath(settings);

            Archive(buildDirectoryPath, archiveFilePath);
        }

        private static string GetBuildPath(BuildSettings settings)
        {
            var parts = new List<string> { settings.BuildName };

            if (settings.IsAppendVersionToBuildName)
            {
                parts.Add(settings.BuildVersion);
            }

            var platform = GetBuildPlatformName(settings.BuildTarget);
            if (settings.IsAppendPlatformToBuildName)
            {
                parts.Add(platform);
            }

            if (settings.IsAppendDateToBuildName)
            {
                parts.Add(settings.BuildDateTimeString);
            }

            var buildFileExtension = GetFileExtension(settings.BuildTarget);
            var buildFileName = $"{string.Join(settings.BuildNameDelimiter, parts)}." +
                                $"{buildFileExtension}";

            var buildDirectory = settings.BuildDirectory;

            if (string.IsNullOrWhiteSpace(buildDirectory) == false)
            {
                return Path.Combine(buildDirectory, platform, buildFileName);
            }

            return Path.Combine(platform, buildFileName);
        }

        private static string GetArchiveFilePath(BuildSettings settings)
        {
            var parts = new List<string> { settings.BuildName };

            if (settings.IsAppendVersionToArchiveName)
            {
                parts.Add(settings.BuildVersion);
            }

            if (settings.IsAppendPlatformToArchiveName)
            {
                var targetName = GetBuildPlatformName(settings.BuildTarget);
                parts.Add(targetName);
            }

            if (settings.IsAppendDateToArchiveName)
            {
                parts.Add(settings.BuildDateTimeString);
            }

            var archiveFileName = $"{string.Join(settings.BuildNameDelimiter, parts)}.{BuildArchiveExtension}";
            var archiveDirectory = settings.ArchiveDirectory;

            if (string.IsNullOrWhiteSpace(archiveDirectory) == false)
            {
                return Path.Combine(archiveDirectory, archiveFileName);
            }

            return archiveFileName;
        }

        private static string GetBuildPlatformName(BuildTarget target)
        {
            return target.ToString();
        }

        private static string GetFileExtension(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows64:
                    return WindowsWindowsExtension;
                case BuildTarget.Android:
                    return AndroidExecutableExtension;
                default:
                    throw new Exception($"Unsupported target: {target}");
            }
        }

        private static void Archive(string sourceDirectoryPath, string destinationFilePath)
        {
            var fastZip = new FastZip
            {
                CreateEmptyDirectories = true,
                CompressionLevel = ArchiveCompressionLevel
            };

            fastZip.CreateZip(destinationFilePath, sourceDirectoryPath, true, null);
        }
    }
}
