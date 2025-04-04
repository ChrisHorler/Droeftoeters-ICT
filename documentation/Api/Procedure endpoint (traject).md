---
tags:
  - author/thomas
  - back-end
---
# Beschrijving
De procedure controller bestuurt de data van alle trajecten. Het word gebruikt als je de trajecten wilt ophalen, aanpassen of verwijderen.

Zie [[Api/Algemene api informatie#Data checks|data checks]] voor wat er allemaal word gedaan voordat de data naar de datalaag worden gestuurd.

# (Database)model
Het procedure model bestaat uit de volgende properties:
- Id : string \[Primary key, required]
	- De string is een uuid/guid. De reden dat het niet als guid word opgeslagen is omdat met een string werken in de meeste instanties voor minder conversion issues zorgt op een database level
- Title : string \[Required]
	- De titel van het traject
- Description : string \[Optional]
	- De beschrijving van het traject, kan leeg zijn
- ProcedureItems : List\<[[Api/Procedure endpoint (traject)|ProcedureItem]]> \[Optional]
	- De lijst van onderliggende traject stappen word hierin terug gegeven bij een read request. Hierin data zetten zal niks doen op een write request

# Endpoints
Alle endpoints waar een traject word terug gestuurd zal ook de onderliggende trajectstappen hebben in de ProcedureItems lijst.

## ReadAll : IActionresult\(List\<Procedure>)
### Request
```
GET: /api/Procedure/all
```
**Body**: Leeg
### Returnwaarden 
**OkObjectResult (200)**: Als alles goed is verlopen zal het een OK terug sturen met de lijst van alle trajecten.

**BadRequestResult (400)**: Als er iets maar fout gaat zal er een error gethrowed worden. zoals besproken in [[Api/Algemene api informatie#Geen foutmelding informatie in response|beveiliging]], zal hier geen informatie over worden terug gestuurd in de response, maar deze word wel gelogd.

### Beschrijving
ReadAll stuurt een lijst terug met alle trajecten in de database. Er word over iedere traject heen gelooped om de onderliggende stappen toe te voegen.

## Read(string id) : IActionresult\(Procedure)
### Request
```
GET: /api/Procedure/all
```
**Body**: Leeg
### Returnwaarden 
**OkObjectResult (200)**: Als alles goed is verlopen zal het een OK terug sturen met het gevonden traject

**BadRequestResult (400)**: Als er iets maar fout gaat zal er een error gethrowed worden. zoals besproken in [[Api/Algemene api informatie#Geen foutmelding informatie in response|beveiliging]], zal hier geen informatie over worden terug gestuurd in de response, maar deze word wel gelogd.

### Beschrijving
Read zoekt een specifieke traject op gebaseerd op welke id je hem meegeeft. Als er niks gevonden word, zal er een

## Write(Procedure procedure) : IActionresult\(bool)
### Request
```
POST: /api/Procedure/
```
**Body**: [[#(Database)model|Procedure model]]
### Returnwaarden 
**OkObjectResult (200)**: Als alles goed is verlopen zal het een OK terug sturen met een boolean

**BadRequestResult (400)**: Als er iets maar fout gaat zal er een error gethrowed worden. zoals besproken in [[Api/Algemene api informatie#Geen foutmelding informatie in response|beveiliging]], zal hier geen informatie over worden terug gestuurd in de response, maar deze word wel gelogd.

### Beschrijving
Write schrijft een procedure naar de database. Er word verwacht dat de id guid in de request al word meegegeven. Deze moet valide en uniek zijn.


