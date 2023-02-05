using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MoonGale.Editor
{
    internal static class BuildMenuItems
    {
        [MenuItem("/Tools/Build And Run Windows")]
        private static void BuildAndRunWindows()
        {
            var buildSettings = new BuildSettings(BuildTarget.StandaloneWindows64)
            {
                IsAppendVersionToBuildName = true,
                IsAutoRunPlayer = true,
                BuildName = Application.productName,
                BuildNameDelimiter = " "
            };

            var buildReport = PlayerBuilder.Build(buildSettings);
            LogBuildReport(buildReport);
        }

        [MenuItem("/Tools/Build And Archive Windows")]
        private static void BuildWindows()
        {
            var buildSettings = new BuildSettings(BuildTarget.StandaloneWindows64)
            {
                IsAppendVersionToBuildName = true,
                IsAppendVersionToArchiveName = true,
                IsArchivePlayer = true,
                BuildName = Application.productName,
                BuildNameDelimiter = " "
            };

            var buildReport = PlayerBuilder.Build(buildSettings);
            LogBuildReport(buildReport);
        }

        private static void LogBuildReport(BuildReport report)
        {
            var summary = report.summary;
            var result = summary.result;

            var resultString = GetBuildResultString(summary.totalErrors, result);
            Debug.Log(
                ""
                + $"\nBuild for {summary.platform.ToString()} {resultString}!"
                + $"\nOutput Path: {summary.outputPath}"
                + $"\nDuration: {summary.totalTime.ToString()}"
                + $"\nWarnings: {summary.totalWarnings.ToString()}"
                + $"\nErrors: {summary.totalErrors.ToString()}"
                + $"\nSize: {((int)(summary.totalSize / 1024f / 1024f)).ToString()} MB"
            );
        }

        private static string GetBuildResultString(int errorCount, BuildResult result)
        {
            if (errorCount > 0)
            {
                return "<color=red><b>Failed</b></color>";
            }

            switch (result)
            {
                case BuildResult.Succeeded:
                    return "<color=cyan><b>Succeeded</b></color>";
                case BuildResult.Failed:
                    return "<color=red><b>Failed</b></color>";
                case BuildResult.Cancelled:
                    return "<color=grey><b>Canceled</b></color>";
                default:
                    return $"<color=grey><b>{result.ToString()}</b></color>";
            }
        }
    }
}
