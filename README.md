# SteamActivityTracker
A simple background service that polls the Steam Web API for your activity to record individual sessions, rather than only lifetime total playtime.

## Getting Started
This project runs using local resources, so it must be built and executed locally.

First, replace the `SteamId` config in the `appsettings.json` file with your Steam Account ID.

Then, replace the `ConnectionStrings:Default` config in the `appsettings.Development.json` file with your local database connection string.

Lastly, ensure your User Secrets file for the main `SteamActivityTracker` project resembles the following:
```
{
    "SteamClientOptions": {
        "Key": "<YOUR STEAM ACCOUNT API KEY HERE>"
    }
}
```