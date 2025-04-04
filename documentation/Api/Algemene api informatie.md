---
tags:
  - author/thomas
  - back-end
---
# Beveiliging

## Geen foutmelding informatie in response
badrequests zullen nooit een body hebben

# Data checks
Iedere controller endpoint zal een aantal dingen checken voordat het naar de data laag word verstuurd.

Alle controllers hebben een check om te kijken of alle uuid/guid strings geparsed kunnen worden naar een Guid datatype.

Alle **Read(all)**(get) endpoints checken of het resultaat van de data laag request niet null is. 
Als dit null is betekent het dat er niks gevonden is met de gegeven conditie/parameter. Daarnaast kan een lijst wel leeg zijn in het geval dat er niks in de database staat.

Alle **Write**(post/put/delete) endpoints checken of het resultaat true of false is. Een true betekent dat er 1 of meer rij in de database is aangepast.  
Daarnaast word er ook gecheckt of een entiteit met dezelfde id wel of niet bestaat om duplicates te voorkomen/proberen te schrijven naar een niet bestaande entiteit