using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void ShouldReturnBadRequestWhenCNPJIsInvalid()
        {
            var doc = new Document("123", Domain.Enums.EDocumentType.CNPJ);
            Assert.IsTrue(doc.Invalid);
        }

        [TestMethod]
        public void ShouldReturnOKWhenCNPJIsValid()
        {
            var doc = new Document("46770880000186", Domain.Enums.EDocumentType.CNPJ);
            Assert.IsTrue(doc.Valid);
        }

        [TestMethod]
        public void ShouldReturnBadRequestWhenCPFIsInvalid()
        {
            var doc = new Document("123", Domain.Enums.EDocumentType.CPF);
            Assert.IsTrue(doc.Invalid);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("42018933086")]
        [DataRow("46024714041")]
        [DataRow("61362876097")]
        public void ShouldReturnOKWhenCPFIsValid(string cpf)
        {
            var doc = new Document(cpf, Domain.Enums.EDocumentType.CPF);
            Assert.IsTrue(doc.Valid);
        }
    }
}
