﻿namespace AmbientSounds.Services
{
    /// <summary>
    /// Interface for app setting properties.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// URL to access the catalogue.
        /// </summary>
        string CatalogueUrl { get; }

        /// <summary>
        /// API key for telemetry.
        /// </summary>
        string TelemetryApiKey { get; }

        /// <summary>
        /// Determines if previous state should be loaded.
        /// Default true.
        /// </summary>
        bool LoadPreviousState { get; set; }

        /// <summary>
        /// The Client ID used for logging into a Microsoft Account.
        /// </summary>
        string MsaClientId { get; set; }

        /// <summary>
        /// The URL for the cloud sync file.
        /// </summary>
        string CloudSyncFileUrl { get; set; }
    }
}
