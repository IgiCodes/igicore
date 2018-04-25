# igicore
Base framework for GTAV FiveM roleplaying servers build in C#.

This repo is an absolute mess right now. Purely using it as online storage and for ease of collaboration when interacting with my stream [twitch.tv/igicodes](https://www.twitch.tv/igicodes).
I'll tidy this up when the framework is more production worthy.

## Development
This resource replaces ALL stock server resources; make sure you remove them from your configuration. The server will always try to load ``sessionmanager``, even if it is not in your configuration, so you must delete or rename the resource.

Clone this repo inside your FiveM server's ``resources`` directory and build the project in Visual Studio 2017.

Edit your ``server.cfg`` file to include the following line below your existing configuration:

```
exec resources\igicore\igicore.cfg
```

Set the options as needed in ``igicore.cfg`` .

Note: You may need to manually preconfigure your MySQL server to default the character set to Unicode. For MariaDB add ``--character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci`` to the server arguments.
