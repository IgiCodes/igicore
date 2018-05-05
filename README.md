# igicore
[![License](https://img.shields.io/github/license/Igirisujin/igicore.svg)](LICENSE)

Complete base framework for GTAV FiveM roleplay servers built entirely in managed C#.
This project aims to replace existing FiveM server resources with a single managed framework to build upon.

This project is primarily developed live on [my Twitch stream](https://www.twitch.tv/igicodes) with the help and input of viewers. Planned features and progress are tracked on [Trello](https://trello.com/b/cGGQ5tmV/igicore).

**Currently a work in progress**

## Development
Building the project will require [Visual Studio 2017](https://www.visualstudio.com/) and [Node.js](https://nodejs.org/) to be installed. A MySQL database is required for storage, [MariaDB](https://mariadb.org/) is recommended.

This resource currently replaces *all* stock server resources; make sure you remove them from your configuration. The server will always try to load ``sessionmanager``, even if it is not in your configuration, so you must delete or rename the resource.

1. Clone this repo inside your FiveM server's ``resources`` directory:
  ```sh
  git clone https://github.com/Igirisujin/igicore.git
  cd igicore
  ```

2. Build the project in Visual Studio.

4. Install interface dependencies:
  ```sh
  cd Interface
  npm install
  ```

5. Build interface:
  ```sh
  npm run build
  ```

5. Edit your ``server.cfg`` file to include the following line below your existing configuration:
  ```
  exec resources\igicore\igicore.cfg
  ```

6. Edit ``igicore.cfg`` with your database connection information as needed.

Note: You may need to manually preconfigure your MySQL server to default the character set to Unicode. For MariaDB add ``--character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci`` to the server arguments.
