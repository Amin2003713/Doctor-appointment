namespace App.Domain.ValueObjects;

[Flags]
public enum PaymentMethods
{
    None          = 0,
    Cash          = 1,
    Card          = 2,
    BankTransfer  = 4,
    OnlineGateway = 8,
    Insurance     = 16
}