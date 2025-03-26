---
tags:
  - author/martijn
---
Dit is een lijst met alle requirements, alleen afvinken wanneer besproken met Chris of Martijn.

## Applicatie

### Main

[source](https://brightspace.avans.nl/d2l/le/lessons/201949/topics/1562097)

Jullie ontwikkelen een 2D-applicatie die voldoet aan de volgende algemene criteria:

- [ ] **Applicatie Functionaliteit**
    - [ ] De applicatie bevat algemene informatie en visuals die aansluiten op de patiëntreis en het behandeltraject van het kind (zie Cases).
    - [ ] De applicatie bevat gepersonaliseerde informatie die is afgestemd op de patiëntgegevens en de persoonlijke behandelmomenten en dokter-afspraken van het kind.
    - [ ] De applicatie moet de afgesproken must-have functionaliteiten bevatten en correct werken.
    - [ ] Extra should-have of could-have functionaliteiten verhogen de beoordeling.

- [ ] **2D Grafische Interface**
    - [ ] De gebruikersinterface moet aansluiten op het vooraf bepaalde UX-ontwerp, dat is gevalideerd bij de opdrachtgever.
    - [ ] Interacties, animaties en visuals dragen bij aan een intuïtieve en aantrekkelijke ervaring en sluiten aan bij de behoeften en belevingswereld van kinderen uit een bepaalde leeftijdscategorie.
    - [ ] De applicatie is toegankelijk voor gebruikers met een beperking.
    
- [ ] **Secure Backend**
    - [ ] De applicatie bevat een veilige backend API met RESTful endpoints.
    - [ ] De backend moet goed beveiligd zijn, met correct geïmplementeerde authenticatie en autorisatie.
    
- [ ] **Tests**
    - [ ] Er moeten geautomatiseerde unit- en acceptatietesten zijn uitgevoerd.
    
- [ ] **Deployment**
    - [ ] De applicatie moet geautomatiseerd gedeployed worden via een CI/CD ontwikkelstraat.

[rubric](https://brightspace.avans.nl/d2l/lp/rubrics/preview.d2l?ou=201949&rubricId=29005&originTool=quicklinks)

[inlever link](https://brightspace.avans.nl/d2l/lms/dropbox/user/folder_submit_files.d2l?ou=201949&isprv=0&db=159860&grpid=241025&cfql=1)

### Gebruikersscenario

[source](https://brightspace.avans.nl/d2l/le/lessons/201949/topics/1580846)

- [ ] **Aanmelden bij de App (Must have)**  
    - [ ] **Opening van de App:**  
        - [ ] De ouder opent de app op hun smartphone of tablet.  
        - [ ] Op het welkomstscherm is de naam van de app zichtbaar en wordt een korte uitleg gegeven over het doel van de app, bijvoorbeeld: “De app helpt jou en je kind het behandeltraject volgen.”  
    - [ ] **Account Aanmaken/Inloggen:**  
        - [ ] Als de ouder al een account heeft, kan deze inloggen met hun gegevens (gebruikersnaam en wachtwoord).  
        - [ ] Nieuwe gebruikers kunnen zich aanmelden met hun e-mailadres.  
        - [ ] Na registratie komt de gebruiker op een introductiescherm waar het gebruik van de app in een paar zinnen wordt uitgelegd.  
    - [ ] **Patiëntinformatie invoeren:**  
        - [ ] De ouder wordt gevraagd om enkele gegevens over het kind in te voeren:  
            - [ ] Naam van het kind  
            - [ ] Geboortedatum  
            - [ ] Behandelplan: (“Route A: Neusamandelen” of “Route B: Keelamandelen”)  
            - [ ] Naam van de arts of specialist  
            - [ ] Eerste afspraak of behandelingdatum (indien al bekend)  
        - [ ] Deze gegevens worden opgeslagen en gebruikt voor de gepersonaliseerde ervaring. (Route A of B met afspraakinformatie)  
    - [ ] **Introductie van de App:**  
        - [ ] Het kind wordt gepresenteerd met een korte uitleg over hoe de app hen gaat helpen tijdens het behandeltraject (eventueel met een animatie).  
        - [ ] De ouder wordt aangemoedigd om samen met het kind de app verder te verkennen en om de eerste stappen te zetten.  

- [ ] **Persoonlijke Behandeltraject en tijdlijn (Must have)**  
    - [ ] **Behandeltraject Overzicht:**  
        - [ ] Na het invoeren van de gegevens komt het kind (en de ouder) terecht in de visuele weergave van hun persoonlijke behandeltraject.  
        - [ ] De app toont een visuele tijdlijn (route, plattegrond of activiteitenoverzicht) met alle belangrijke stappen van het behandeltraject (bijv. afspraak met arts, voorbereiden, operatie, herstel, follow-up).  
        - [ ] De tijdlijn bevat een route-aanduiding (bijv. “Route A: Neusamandelen” of “Route B: Keelamandelen”) en geeft duidelijk aan welke stappen al zijn doorlopen en welke nog komen en waar belangrijke beslismomenten zitten.  
    - [ ] **Persoonlijk Plan en Planning:**  
        - [ ] Op basis van het ingevoerde behandelplan ziet de gebruiker een specifieke route (A of B) en de bijbehorende planning.  
        - [ ] De planning toont de geplande afspraken, zoals de datum van de operatie, controle-afspraken, en herstelafspraken.  
    - [ ] **Kleur en Visuele Weergave:**  
        - [ ] De tijdlijn is visueel aantrekkelijk en maakt gebruik van vrolijke kleuren, iconen, en animaties om de voortgang aan te geven.  

- [ ] **Voorlichting en Uitleg (Must have)**
    - [ ] **Educatieve inhoud:**
        - [ ] Op basis van het gekozen behandeltraject biedt de app verschillende bronnen voor voorlichting:
            - [ ] Webvideo's waarin (delen van) een behandeling wordt uitgelegd.
            - [ ] Tekst en afbeeldingen die de stappen van de operatie visueel weergeven.
            - [ ] Links naar specifieke webpagina's met relevante informatie in browser.
    - [ ] **Interactiviteit en Betrokkenheid:**
        - [ ] Het kind kan de video's samen met de ouder bekijken, waarbij er vragen gesteld kunnen worden via de app om te zorgen voor een actieve deelname.
        - [ ] Er zijn ook kleine animaties of geluiden die de interactie met het kind versterken.

- [ ] **Het Kind krijgt/Kiest een Avatar**
    - [ ] **Avatar (must have):**
        - [ ] Zodra de basisinformatie is ingevoerd, krijgt het kind een persoonlijke avatar die hen vertegenwoordigt in de app.
        - [ ] De app biedt een verscheidenheid aan avatars, bijvoorbeeld dieren, superhelden, of speelse karakters die aansluiten bij het behandeltraject.
        - [ ] Met de avatar doorloopt het kind het gehele behandeltraject.
    - [ ] **Avatar kiezen (nice to have):**
        - [ ] Het kind kan zelf een avatar kiezen uit een selectie avatars.

- [ ] **Dagboekfunctie en notities**
    - [ ] **Dagboek bijhouden (must have):**
        - [ ] De app biedt een dagboekfunctie waarmee het kind en de ouder notities kunnen maken tijdens het behandeltraject.
        - [ ] Ze kunnen bijvoorbeeld hun gevoelens bijhouden voor en na de operatie, hun herstelproces, en het effect van pijnmedicatie.
        - [ ] Of notities invullen over eet- en drinkinname en eventuele ongemakken of pijnklachten na de operatie.
    - [ ] **Foto's toevoegen:**
        - [ ] Het kind kan, samen met de ouder, foto's toevoegen om de voortgang te documenteren, zoals een foto van het kind op de dag van de operatie, of een foto van hun herstel.

- [ ] **Stickers en personalisatie**
    - [ ] **Stickers Plakken:**
        - [ ] Het kind kan stickers toevoegen om hun ervaring in de app op te vrolijken. Deze stickers kunnen een beloning zijn voor het voltooien van bepaalde taken, bijvoorbeeld een sticker voor het "voltooien van de voorbereidingen voor de operatie" of een sticker voor het "herstellen na de operatie".
    - [ ] **Motivatie en Positieve Berichten:**
        - [ ] De app biedt regelmatig positieve berichten en herinneringen, zoals "Goed gedaan!" of "Vergeet je knuffel niet mee te nemen!" of "Nog twee dagen tot je afspraak".

- [ ] **Herstel en opvolging**
    - [ ] **Herstelmonitoring (must have):**
        - [ ] Na de operatie biedt de app hersteladvies en herinneringen aan pijnmedicatie of rusttijden.
    - [ ] **Follow-Up Afspraken:**
        - [ ] De app stuurt herinneringen voor follow-up afspraken.
        - [ ] Biedt een functie waarmee ouders eenvoudig vervolgafspraken kunnen maken of aanpassen.
    - [ ] **Terug naar het Dagelijkse Leven (must have):**
        - [ ] De app geeft tips voor het terugkeren naar dagelijkse activiteiten, zoals school.
        - [ ] Biedt begeleiding over het omgaan met post-operatieve symptomen (zoals keelpijn na de behandeling).
## Documentatie

[source](https://brightspace.avans.nl/d2l/le/lessons/201949/topics/1562104)

- [ ] **Projectplan**  
    - [ ] Beschrijf de context, het probleem en de doelstellingen van het project.  
    - [ ] Definieer stakeholders en hun rol binnen het project.  
    - [ ] Stel een risicoanalyse en mijlpalenplanning op, waarbij Scrum als werkwijze wordt toegepast.  

- [ ] **Epics en User Stories (Neem op in het projectplan)**  
    - [ ] Formuleer functionele, toegankelijkheids- en security-eisen.  
    - [ ] Werk deze systematisch uit in epics en user stories met acceptatiecriteria.  
    - [ ] Prioriteer de user stories en koppel ze aan de projectplanning.  

- [ ] **UX-Ontwerp**  
    - [ ] Ontwikkel personas, een moodboard en wireframes die aansluiten op de doelgroep.  
    - [ ] Zorg voor een intuïtief ontwerp met aandacht voor toegankelijkheid volgens WCAG-richtlijnen.  

- [ ] **Software-Ontwerp**  
    - [ ] Documenteer de architectuur en belangrijkste componenten.  
    - [ ] Gebruik visuele diagrammen om interacties en afhankelijkheden te verduidelijken.  
    - [ ] Onderbouw de gekozen architectuurprincipes.  

- [ ] **Testplan en -rapportage**  
    - [ ] Beschrijf de teststrategie en testmethoden.  
    - [ ] Stel testscenario’s op en valideer deze bij de opdrachtgever.  
    - [ ] Rapporteer testresultaten (en analyseer verbeterpunten).  

[inlever link](https://brightspace.avans.nl/d2l/lms/dropbox/user/folder_submit_files.d2l?ou=201949&isprv=0&db=159861&grpid=241025&cfql=1)

[rubric](https://brightspace.avans.nl/d2l/lp/rubrics/preview.d2l?ou=201949&rubricId=29220&originTool=quicklinks)

## Individuele projectverantwoording (Persoonlijk)

[source](https://brightspace.avans.nl/d2l/le/lessons/201949/topics/1562105)

- [ ] **SDLC Verantwoording**  
    - [ ] Beschrijf hoe jullie projectgroep het Software Development Life Cycle (SDLC)-proces heeft doorlopen binnen het project.  
    - [ ] Ga in op elke fase en licht daarbij toe welke keuzes en verbeteringen zijn gemaakt.  

- [ ] **SCRUM Verantwoording**  
    - [ ] Leg uit hoe jullie projectgroep de SCRUM-methodiek heeft toegepast bij de uitvoer van het project.  
    - [ ] Beschrijf de SCRUM-rollen, producten en communicatietools die jullie binnen het team hebben toegepast en benoem specifiek jouw rol, verantwoordelijkheden en input daarbij.  
    - [ ] Neem bewijsmateriaal op van uitgevoerde SCRUM-onderdelen, zoals bijvoorbeeld de wekelijkse retrospectives (Visuele templates + post-its).  

- [ ] **Eigen bijdrage aan de applicatie**  
    - [ ] Toon aan welke onderdelen van de 2D grafische applicatie en/of documentatie jij hebt ontwikkeld.  
    - [ ] Gebruik daarvoor concrete bewijzen, zoals codevoorbeelden, documentfragmenten, of commit-geschiedenis en pull requests. Denk ook aan een screenshot van de door jou uitgevoerde taken in de planningstool (SCRUM-board).  

- [ ] **Eigen bijdrage aan het samenwerkingsproces**  
    - [ ] Leg uit hoe jij hebt bijgedragen aan de samenwerking binnen het team.  
    - [ ] Geef concrete voorbeelden, waarin je aangeeft wat je hebt bijgedragen en wat je daarvan geleerd hebt.  
    - [ ] Relateer dit aan het door jou (aan het begin van de periode) opgestelde persoonlijke leerdoel.  
    - [ ] En aan de lesonderdelen van het vak professionele vaardigheden, waaronder de Belbin-rollen, assertiviteitstest, puntenverdeling team (sterktes).  
    - [ ] Onderbouw je verhaal met bewijsmateriaal, zoals gespreksverslagen of ontvangen (/gevraagde) teamfeedback.  

### Documentvorm

De gevraagde documentvorm voor deze opdracht is vrij. Dat wil zeggen dat je zelf mag bepalen in welke vorm je de individuele verantwoording inlevert. Dit kan bijvoorbeeld in een Word-, PowerPoint- of Online Whiteboard-document. Zolang het bestand door ons maar te openen is en het inhoudelijk maar voldoet aan de opdracht en de criteria in de rubric. Eventueel kan je hiervoor de gegeven PowerPoint-template gebruiken.  

[inlever punt](https://brightspace.avans.nl/d2l/lp/rubrics/preview.d2l?ou=201949&rubricId=29228&originTool=quicklinks)

[rubric](https://brightspace.avans.nl/d2l/lp/rubrics/preview.d2l?ou=201949&rubricId=29228&originTool=quicklinks)