# FUTWatcher

This is a personal project developed with the purpose to keep track of investments in the Ultimate Team mode regarding the FIFA series. It still contains some small bugs, if you want to contribute please contact me.


## Content

It contains:
  - API developed with .NET Core; 
  - WebApp developed with Angular 2/4/5; 
  - Chrome Extension (not fully developed) in order to get players from the FIFA WebApp;
  - Runs with a PostgreSQL database;
  
## Instructions
  
In order to run the project proceed as described:
  - Clone the repository;
  - API (you need to have [.NET Core](https://www.microsoft.com/net/download/windows) installed) 
    - Go to the API folder
    - Run ```dotnet ef database update``` to run the migrations
    - In order to populate the database:
        - Start the API with ```dotnet run``` 
        - Call the following endpoint (with Postman, for instance): http://localhost:5000/api/Home/PopulateDB
        - This will take a while since it calls the FIFA API do get all the players currently in the game...
  - Web App (you need to have [Angular cli](https://cli.angular.io/) and npm installed)
    - Run ```npm install``` in order to install the packages
    - Run ```ng serve```
    - Access in your browser http://localhost:4200
