using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.Document = "99999999999";
            command.Email = "hello@balta.io2";
            command.BarCode = "123456789";
            command.BoletoNumber = "123456";
            command.PaymentNumber = "12345";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "WAYNE CORP";
            command.PayerDocument = "12345678900";
            command.PayerDocumentType = Domain.Enums.EDocumentType.CPF;
            command.PayerEmail = "batman@dc.com";
            command.Street = "bobos";
            command.Number = "0";
            command.Neighborhood = "outras";
            command.City = "Gotham";
            command.State = "SP";
            command.Country = "USA";
            command.ZipCode = "1245678";

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}
