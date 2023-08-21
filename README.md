# SteamActivityTracker
A simple background service that polls the Steam Web API for your activity to record individual sessions, rather than only lifetime total playtime.

## Getting Started
This project runs using local resources, so it must be built and executed locally.

First, replace the `SteamId` in the `appsettings.json` file with your Steam Account ID.

Then, ensure your User Secrets file for the main `SteamActivityTracker` project resembles the following:
```
{
    "ConnectionStrings": {
        "Default": "<YOUR LOCAL DB CONNECTION STRING HERE>"
    },
    "SteamClientOptions": {
        "Key": "<YOUR STEAM ACCOUNT API KEY HERE>"
    }
}
```