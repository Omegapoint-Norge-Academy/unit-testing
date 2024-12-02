using ConsumerBank.Services.Contracts;
using ConsumerBank.Services.DbObjects;
using Moq;
using RandomTestValues;
using Xunit;

namespace ConsumerBank.Services.UnitTests
{
    public class LoanerServicesTests
    {
        private readonly Mock<IDatabase> _databaseServiceMock;
        private readonly LoanerService _service;

        public LoanerServicesTests()
        {
            _databaseServiceMock = new Mock<IDatabase>();
            _service = new LoanerService(_databaseServiceMock.Object);
        }

        // Bruker [Theory] i steden for [Fact] fordi Theory lar deg angi input-parametere med InlineData-attributter.
        // Tester for både true og false for å verifisere at det er resultatet fra creditProvider som returneres.
        [Theory]
        [InlineData(1000, true)]
        [InlineData(90000000, false)]
        public async Task Apply_ValidRequest_ShouldReturnCreditEvaluationResult(int amount, bool expectedEvaluationResult)
        {
            // Arrange
            var request = RandomValue.Object<LoanRequest>();
            request.Amount = amount;
            
            // Act
            var actualResult = await _service.Apply(request);

            // Assert
            Assert.Equal(expectedEvaluationResult, actualResult);
        }

        // Merk at alle tester er delt i 3:
        // Arrange - inneholder all kode for oppsett.
        // Act - trigger koden som skal testes. Typisk bare en linje kode her.
        // Assert - her er alle Assert og Verify-statements. I denne delen avgjøres det om testen har feilet eller ikke.

        [Fact]
        public async Task Apply_ValidPersonData_PersonIsStoredInDb()
        {
            // Arrange
            var request = RandomValue.Object<LoanRequest>();

            // Act
            await _service.Apply(request);

            // Assert
            AssertPersonDataWasSaved(firstName: request.Person.FirstName);
        }

        [Fact]
        public async Task Apply_ValidPersonData_AddressIsStoredInDb()
        {
            // Arrange
            var request = RandomValue.Object<LoanRequest>();

            // Act
            await _service.Apply(request);

            // Assert
            AssertAddressWasSaved(street: request.Person.CustomerAddress.Street);
        }

        [Fact]
        public async Task Apply_ValidPersonData_LoanWasUpdated()
        {
            // Arrange
            var personId = RandomValue.Int(minPossibleValue: 1);
            SetupPersonIdReturned(personId);

            // Lar RandomValue.Object populere alt, og presiserer så de parameterene som har direkte påvirkning på testresultatet.
            var request = RandomValue.Object<LoanRequest>();
            request.Person.Id = personId;
            request.Amount = 100;

            // Act
            await _service.Apply(request);

            // Assert
            AssertLoanWasUpdated(personId, request.Amount);
        }

        private void AssertLoanWasUpdated(int personId, int requestAmount)
        {
            _databaseServiceMock.Verify(x => x.UpdateLoan(personId, requestAmount));
        }

        private void SetupPersonIdReturned(int personId)
        {
            _databaseServiceMock
                .Setup(x => x.SavePerson(It.IsNotNull<PersonEntity>()))
                .ReturnsAsync(personId);
        }

        private void AssertAddressWasSaved(string street)
        {
            // Kan legge til sjekk på flere adresseparametere her om ønskelig.
            _databaseServiceMock.Verify(x => x.SaveAddress(It.Is<AddressEntity>(address => address.Street == street)));
        }

        private void AssertPersonDataWasSaved(string firstName)
        {
            _databaseServiceMock
                .Verify(x => x.SavePerson(
                    It.Is<PersonEntity>(personEntity => personEntity.FirstName == firstName)));
        }
    }
}