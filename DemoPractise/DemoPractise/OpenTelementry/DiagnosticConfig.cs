using System.Diagnostics;

namespace DemoPractise.OpenTelementry;

internal static class DiagnosticConfig
{
    internal static readonly ActivitySource Source = new("webhooks-api");
}
