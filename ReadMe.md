# Oppgaver APP Unit Testing

Her er en kort beskrivelse av oppgavene som følger med kurset

## Oppgave 1 - Enhetstesting

Forsøk å opprette enhetstester for følgende klasser:

**ConsumerBank.Services:**
- LoanerService.cs
- (Prøv gjerne på Database.cs også, men den er ikke enkel. Enhetstester her kommer neppe til å gi mye verdi.)

**ConsumerBank.Api:**
- Controllers/LoansController.cs

 Bruk valgfritt mocking framework, men en av disse anbefales:

- [Moq](https://github.com/devlooped/moq)
- [Nsubstitute](https://nsubstitute.github.io/)
- [FakeItEasy](https://fakeiteasy.github.io/)

Jeg har brukt Moq mest, så jeg kan gi mest assistanse her. Men de andre skal kunne gå greit de og.

## Oppgave 2 - Integrasjonstesting

Forsøk å opprette integrasjonstester for følgende klasser:

**ConsumerBank.Services:**
- Database.cs

Det er en fordel å ha [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio) installert nå, for å kunne se detaljene i databasen.

## Oppgave 3 - Regresjonstesting

Forsøk å skrive regresjonstester for API'et: POST https://localhost:5269/loans/apply. 

For å starte API'et lokalt åpner du et kommandovindu og går til mappen der \ConsumerBankApi\ConsumerBankApi.csproj befinner seg. Start APIet med kommandoen `dotnet run`. 

Gå til https://localhost:5269/swagger for detaljerte opplysninger om APIet.

## Oppgave 4 - Alt på en gang (hvis tid)

Nå er tiden kommet til å teste hele TDD-sirkelen!

- Skriv en eller flere tester.
- Kjør alle testene. Verifiser at testen(e) fra forrige punkt feiler.
- Skriv den enklest mulige kode som får testen til å passere.
- Kjør alle testene igjen. Verifiser at testen(e) fra punkt 1 nå kjører, og ingen andre tester feiler.
- Refaktorer.

Du skal nå opprette nye endepunkt i LoansController. Velg ett eller flere (eller finn på noe selv!):
- GET /loans/list: Lister ut hva en person har i lån. Input PersonApi. Skal returnere lånebeløpet ([dbo].[Loans].[Amount]), eller HTTP 204 om personen ikke har noe lån.
- GET /loans/persons/{id}: Lister ut personopplysninger. HTTP 204 om personen ikke finnes i databasen.
- GET /loans/persons/{id}/address: Returnerer ut personens adresse. HTTP 204 om personen ikke finnes i databasen.
- ....