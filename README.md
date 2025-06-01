# x365-automation
Automating the x365 Token Claim Process

After downloading the files, navigate to the 'Publish' folder and double-click the 'x365auto.exe' file to run the application.

Before launching the executable, make sure to configure the user login details in the 'appsettings.json' file, as shown below.

```
{
    "AppSettings": {
      "LoginUrl": "https://x365.ai/login",
      "ActionUrl": "https://x365.ai/quantum",
      "email": "Testuser@gmail.com",
      "password": "Crypto@12345",
      "intervalInMinutes": "10",
      "closePopups": "true"
    }
  }
```
  
'intervalInMinutes' defines how often the flow should run continuously, with the interval specified in minutes using this parameter.

### Prerequisites

1. Install the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. Ensure an active internet connection
3. Have your x365 account credentials ready
4. Use a Windows machine

