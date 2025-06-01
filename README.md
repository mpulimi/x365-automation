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
