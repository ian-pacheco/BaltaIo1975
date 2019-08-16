using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class PayPalPayment : Payment
    {
        public PayPalPayment(
            string transactionCode,
            DateTime paidDate,
            DateTime expireDate,
            decimal total,
            decimal totalPayment,
            string payer,
            Document document,
            Address addres,
            Email email) : base(
                paidDate,
                expireDate,
                total,
                totalPayment,
                payer,
                document,
                addres,
                email)
        {
            TransactionCode = transactionCode;
        }

        public string TransactionCode { get; private set; }
    }
}