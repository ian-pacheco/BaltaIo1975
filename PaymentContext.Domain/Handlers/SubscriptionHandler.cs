using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>,
        IHandler<CreateCreditCardSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }
       
        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Assinatura realizada com sucesso");
            }

            //Verifica documento no banco
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF ja esta cadastrado");

            //Verifica email no banco
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este Email ja esta cadastrado");

            //Gerar novos VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode);

            //Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupa as validaçoes
            AddNotifications(name, document, email, student, subscription, payment);

            // Checar as notificaçoes
            if (Invalid)
                return new CommandResult(false, "Não foi possível realir sua assinatura");

            //Salvar as Informaçoes
            _repository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem Vindo ao meu site", "Assinatura criada com sucesso");

            //Retornar informaçoes
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            // Fail Fast Validations

            //Verifica documento no banco
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF ja esta cadastrado");

            //Verifica email no banco
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este Email ja esta cadastrado");

            //Gerar novos VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode);

            //Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupa as validaçoes
            AddNotifications(name, document, email, student, subscription, payment);

            // Checar as notificaçoes
            if (Invalid)
                return new CommandResult(false, "Não foi possível realir sua assinatura");

            //Salvar as Informaçoes
            _repository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem Vindo ao meu site", "Assinatura criada com sucesso");

            //Retornar informaçoes
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreateCreditCardSubscriptionCommand command)
        {
            // Fail Fast Validations

            //Verifica documento no banco
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF ja esta cadastrado");

            //Verifica email no banco
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este Email ja esta cadastrado");

            //Gerar novos VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode);

            //Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new CreditCardPayment(
                command.CardHolderName,
                command.CardNumber,
                command.LastTransactionNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupa as validaçoes
            AddNotifications(name, document, email, student, subscription, payment);

            // Checar as notificaçoes
            if (Invalid)
                return new CommandResult(false, "Não foi possível realir sua assinatura");

            //Salvar as Informaçoes
            _repository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem Vindo ao meu site", "Assinatura criada com sucesso");

            //Retornar informaçoes
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

    }
}