This is a school project that I have done a couple years ago.

It was basicly to demonstrate the use of some of the core technologies of the .NET Framework.

This project is based on a server/client architecture.

The solution includes:

- A Bomberman 2D game based on the XNA game engine.

- A WCF duplex communication module for client/server interaction.

- An Entity Framework module for data access and storage.

- A simple server host application.

- A WPF UI that allows players to : subscribe,
				    manage accounts and friendships,
				    chat and hold audio conforence calls,
				    launch bomberman game with up to 4 players per room.




############### IMPORTANT NOTE ###################################################################################################################################

In order to have this solution work correctly, you should use Visual Studio 2010 and avoid using any higher version.

The reason for this is that the XNA game engine isn't supported on the latest versions of Visual Studio.

You need to also make sure that the XNA Framework is installed on every client machine.



############## SETUP #############################################################################################################################################

To run this project and play with your friends, you must:

1) Have a SQLServer instance up and running on your machine.

2) Create a database called "BombermanOnlineDatabase" on your SQLServer instance.

3) Run the BomberRepository/BomberDataModel.edmx.sql script on your SQLServer instance.

4) Add the connection string of the newly created BombermanOnlineDatabase to the BomberServer/App.config file.

5) Make sure the enpoint address in Bomberserver/App.config file is using an available port and change "localhost" by your actual IP address on your network.

6) Make sur the endpoint address in BomberBetaClient/App.config is the same as the one in Bomberserver/App.config.

7) Recompile the solution.

8) Run the server BomberServer/bin/Debug (or Release)/BomberServer.exe

9) Distribute the client BomberBetaClient/bin/Debug (or Release) and run it using BomberBetaClient.exe

10) Finally enjoy!


##################################################################################################################################################################