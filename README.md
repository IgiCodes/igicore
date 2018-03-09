# igicore
Base framework for GTAV FiveM roleplaying servers but in C#


This repo is an absolute mess right now. Purely using it as online storage and for ease of collaboration when interacting with my stream twitch.tv/igicodes

I'll tidy this up when the framework is more production worthy.


# Development
Clone this repo inside your FiveM server's ``resources`` directory and build the project in Visual Studio 2017.

Edit your ``server.cfg`` file to include the following lines below your existing configuration:

```
set mysql_connection "server=localhost;database=fivem;user id=root;password=password;charset=utf8"

start igicore
```

Note: You may need to manually preconfigure your MySQL server to default the character set to Unicode. For MariaDB add ``--character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci`` to the server arguments.
