---
tags:
  - author/martijn
  - front-end
  - back-end
  - feature/login
---
## Theory

The login page is simple by concept, but there is more underwater, I made a flowchart canvas to show the flow. this way you could debug it if something needs to change.

![[Login Flowchart.canvas|Login Flow]]
## code etc

In deze scene ziet het er zo ongeveer uit:
- **canvas**
	- login elements like buttons and inputs
	- loginPage script
- **MainManager**
	- api connector script
	- MainManager script

Hier is de uitleg van de classes/scripts:

### LoginScript Class

Dit script hendelt de validatie van de invoer, het afhandelen van de login request, en register request.

Bij startup checkt ie met `MainManager script` en `api connector script` of dat je bent ingelogd (hiervoor moet nog een endpoint komen op de api)

Als je niet bent ingelogd, haalt ie gelijk een nieuwe session token op als dat mogenlijk is waardoor je toch automatisch ingelogd wordt.

`ClickButton(string)` is hoe de buttons communiceren met de logica, Vul 1 van de volgende waarden in:
- `ParentRegister`
- `ParentLogin`
- `ChildLogin`

Als je iets anders invult wordt er een error gelogd wanneer je klikt op de knop.

`SetPasswordValue(string)` en `SetUsernameValue(string)` is hoe de invoer velden aangeven aan de logica wat de value is van de 2 velden, dit gebeurt bij de `on value changed` event van de input field.
Als je een 2e password veld hebt, dan gebruik je `SetSecondPasswordValue(string)`, dit 2e wachtwoord veld is voor bij het registreren om te kijken of de wachtwoorden het zelfde zijn.

Het loginpagescript heeft 11 public fields die je kan / moet assignen in unity:
- `parentRegisterErrorMessageLabel` en `parentLoginErrorMessageLabel` en `childLoginErrorMessageLabel` is waar hij het error bericht op gaat plakken.
- `parentRegisterUsernameField` en `parentLoginUsernameField` en `childLoginUsernameField` is het username invoer veld.
- `parentRegisterPasswordField` en `parentLoginPasswordField` en `childLoginPasswordField` is het wachtwoord invoer veld.
- `parentRegisterSecondPasswordField` is optioneel, dit wordt gebruikt voor als je registratie formulier 2 wachtwoord velden heeft. 
- `defaulSceneAfterLogin` is de scene die geopend wordt wanneer je bent ingelogd.

Als je je afvraagt waarom er zoveel fields zijn, en ik dit niet via methods doe: Unity is fucked. :/

Verder hebben we nog de volgende method:

```cs
EmptyLoginFormFields() {}
```

Deze method moet gebruikt worden wanneer je switcht tussen de login, register en parent/child forms.

### MainManager Class

dit script zorgt ervoor dat data bewaard blijft als je switcht tussen scenes, hiervan bestaat er max 1. Dit wordt nu gebruikt zodat je vanaf elke scene een AUTH request kan sturen naar de api.
Verder haalt dit ook de login credentials op aan het begin uit je bestanden, en onthoudt hij die gegevens.

`LoginDataSaveLocation` is het pad waar de login van de user wordt opgeslagen.

### ApiConnecter Class

`public string baseUrl = "";` voor de correcte api address.
`public string defaultLoginScene = "TestFunctionalLogin";` is om aan te geven welke scene de login scene is.

```cs
/// <summary>
/// The method that connects the frontend with the backend.
/// </summary>
/// <param name="path">The path that you put behind the baseurl of the api.</param>
/// <param name="protocol">POST GET PUT or DELETE</param>
/// <param name="authorized">wether you need to be logged in for this endpoint or not.</param>
/// <param name="callback">the method that will get called with the result of the api request.</param>
/// <param name="body">the body that you provide for an PUT and POST request. JSON string.</param>
/// <param name="autoLogin">whether we send the user to the login page if we get an unauthorized error. If enabled it will try to auto login the user again.</param>
/// <returns></returns>
public IEnumerator SendRequest(string path, HttpMethod protocol, bool authorized, Action<string, string> callback, string body = "", bool autoLogin = true) {}
```

this method is the method you use to connect the app to the [[API]]. Here is an example of how to implement it:

```cs
apiConnecter.SendRequest("account/checkAccessToken", HttpMethod.GET, true, (string response, string error) =>
	{
		if (error == null)
		{
			Debug.Log("yay");
		}
		else {
			Debug.Log("errorrrrr");
		}
	}, "", false);
```

verder heeft deze classe ook nog een `HandleResponse(string, string)` om te loggen wat voor response je krijgt van de backend.

Verder zijn er nog meer methods die onderwater aangeroepen worden door `SendRequest()`, maar die zijn allemaal private.

### Support Classes Etc

Here are a few support classes I created for helping with the main logic. 
#### LoginResponse

```cs
public class LoginResponse
{
    public string tokenType { get; set; }
    public string accessToken { get; set; }
    public int expiresIn { get; set; }
    public string refreshToken { get; set; }
}
```

This class is used in the `ApiConnecter`, `LoginScript` and `MainManager` classes.
It is initiated in `ApiConnecter.cs`.

It is used so that we can save the login session, re-authenticate etc. This has all the data we need to restart the session.

#### HttpMethod ENUM

```cs
public enum HttpMethod
{
    POST,
    PUT,
    GET,
    DELETE
}
```

This is used in the `ApiConnecter.SendRequest()`, it is initiated in `ApiConnector.cs`.
The use is to differentiate between the request you want to send.

#### Validator

```cs
public class Validator
{
    public static bool IsValidPassword(string password) {}
	public static bool IsValidEmail(string email) {}
}
```

This class is used in the `LoginScript` class and initiated in `LoginScript.cs`.
This class is used to validate the login mail and password.
