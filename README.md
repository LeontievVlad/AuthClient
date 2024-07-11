# AuthClient
Configuration and launch of the client part of Console application for authentication and notification

1. Cloning the repository:
git clone https://github.com/LeontievVlad/AuthClient
cd your-repository-folder

2. Installation of dependencies:
Open a terminal in the root directory of the console application and run the command:
dotnet restore

3. Starting the client part:
Run the command to start the client:
dotnet run
The client application will wait for the authentication data to be entered and display the received notifications.

Using the program
1. Starting the server part:
Make sure the backend is up and running.

2. Starting the client part:
Start the console application. 
1. Enter the number 1 for authentication, 2 for registration
  1. Enter a username and password for authentication.
	1. Enter the number 1-3 for the corresponding actions
  2. Enter a username, email and password for registration.
	1. Enter the number 1-3 for the corresponding actions
3. Receiving notifications:
After successful login, the client program will display received notifications in real time.

Notes and comments
The app uses SignalR to deliver real-time notifications.
Additional settings can be added to configuration files (appsettings.json) to configure server and database settings.