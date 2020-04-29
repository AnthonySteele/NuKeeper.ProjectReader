using System;

namespace NuKeeper.ProjectReader.Logging
{

#pragma warning disable CA1716 // Identifiers should not match keywords
    public interface INuKeeperLogger
    {
        void Error(string message, Exception? ex = null);
        void Minimal(string message);
        void Normal(string message);
        void Detailed(string message);
    }
}
