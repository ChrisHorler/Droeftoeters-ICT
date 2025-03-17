# functional login pagina

contains:
- canvas
	- login elements like buttons and inputs
	- loginPage script
- MainManager
	- api connector script
	- MainManager script
	
## loginpage script

Dit script hendelt de validatie van de invoer, het afhandelen van de login request, en register request.

Bij startup checkt ie met `MainManager script` en `api connector script` of dat je bent ingelogd (hiervoor moet nog een endpoint komen op de api)

ClickButton(string) is hoe de 2 buttons communiceren met de logica, Bij het invoer van "Register" wordt de invoer gebruikt om te registreren. Bij al het andere wordt er ingelogd.

SetPasswordValue(string) en SetUsernameValue(string) is hoe de invoer velden aangeven aan de logica wat de value is van de 2 velden, dit gebeurt bij de `on value changed` event van de input field.

## mainManager script

dit script zorgt ervoor dat data bewaard blijft als je switcht tussen scenes, hiervan bestaat er max 1. Dit wordt nu gebruikt zodat je vanaf elke scene een AUTH request kan sturen naar de api.

## apiconnector script

`public string baseUrl = "";` voor de correcte api address.

HandleResponse(string, string) is voor het testen van het afhandelen van je api request, dit logged de response van de api.

SendGetRequest(string, Action<string, string>) stuurt een GET

SendAuthGetRequest(string, Action<string, string>) stuurt een authorized GET met de hulp van het mainmanager script.

SendAuthDeleteRequest(string, Action<string, string>) stuurt een authorized DELETE met behulp van mainmanager script

SendPostRequest(string, string, Action<string, string>) stuurt een POST, vergeet de body niet te serializen naar een string.

sentAuthPutRequest(string, string, Action<string, string>) stuurt een authorized PUT naar de api met behulp van de mainmanager script, vergeet niet het serializen van de body naar n string

handleLoginError(string, string, bool) dit is een functie die je kan gebruiken om te checken of een error is omdat je niet meer bent ingelogd, en dat ie dan automatisch probeert in te loggen door je refresh token. 

SendAuthPostRequest(string, Action<string, string>) stuurt een authorized POST met behulp van het mainmanagerscript, vergeet niet de body te serializen naar een string

 