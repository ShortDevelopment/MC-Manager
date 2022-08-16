using Newtonsoft.Json;
using System;
using static ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads.SubscribePackage;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads
{

    public class SubscribePackage : Package<SubscribePayload>
    {

        public SubscribePackage(string eventName)
        {
            Header.ID = Guid.NewGuid().ToString();
            Header.Type = "commandRequest";
            Header.Purpose = "subscribe";

            Payload = new SubscribePayload();
            Payload.EventName = eventName;
        }

        public class SubscribePayload
        {
            [JsonProperty("eventName")]
            public string EventName { get; set; }
        }

        /// <summary>
        /// <see href="https://gist.github.com/jocopa3/5f718f4198f1ea91a37e3a9da468675c">Source</see>
        /// </summary>
        public static string[] WellKnownEvents { get; } = new[]{
            "AdditionalContentLoaded",
            "AgentCommand",
            "AgentCreated",
            "ApiInit",
            "AppPaused",
            "AppResumed",
            "AppSuspended",
            "AwardAchievement",
            "BlockBroken",
            "BlockPlaced",
            "BoardTextUpdated",
            "BossKilled",
            "CameraUsed",
            "CauldronUsed",
            "ChunkChanged",
            "ChunkLoaded",
            "ChunkUnloaded",
            "ConfigurationChanged",
            "ConnectionFailed",
            "CraftingSessionCompleted",
            "EndOfDay",
            "EntitySpawned",
            "FileTransmissionCancelled",
            "FileTransmissionCompleted",
            "FileTransmissionStarted",
            "FirstTimeClientOpen",
            "FocusGained",
            "FocusLost",
            "GameSessionComplete",
            "GameSessionStart",
            "HardwareInfo",
            "HasNewContent",
            "ItemAcquired",
            "ItemCrafted",
            "ItemDestroyed",
            "ItemDropped",
            "ItemEnchanted",
            "ItemSmelted",
            "ItemUsed",
            "JoinCanceled",
            "JukeboxUsed",
            "LicenseCensus",
            "MascotCreated",
            "MenuShown",
            "MobInteracted",
            "MobKilled",
            "MultiplayerConnectionStateChanged",
            "MultiplayerRoundEnd",
            "MultiplayerRoundStart",
            "NpcPropertiesUpdated",
            "OptionsUpdated",
            "performanceMetrics",
            "PackImportStage",
            "PlayerBounced",
            "PlayerDied",
            "PlayerJoin",
            "PlayerLeave",
            "PlayerMessage",
            "PlayerTeleported",
            "PlayerTransform",
            "PlayerTravelled", // Too many messages!!!
            "PortalBuilt",
            "PortalUsed",
            "PortfolioExported",
            "PotionBrewed",
            "PurchaseAttempt",
            "PurchaseResolved",
            "RegionalPopup",
            "RespondedToAcceptContent",
            "ScreenChanged",
            "ScreenHeartbeat",
            "SignInToEdu",
            "SignInToXboxLive",
            "SignOutOfXboxLive",
            "SpecialMobBuilt",
            "StartClient",
            "StartWorld",
            "TextToSpeechToggled",
            "UgcDownloadCompleted",
            "UgcDownloadStarted",
            "UploadSkin",
            "VehicleExited",
            "WorldExported",
            "WorldFilesListed",
            "WorldGenerated",
            "WorldLoaded",
            "WorldUnloaded"
        };
    }
}
