namespace JodosServer.Models
{
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    }

    public enum MatakCodesEnum
    {
        Ramala=1
    }

    public enum PayStatus
    {
        Paid=1,
        InProgress=2,
        Cancelled=3,
        Used = 4,
        Overflow = 5
    }

    public enum OperationTypeP
    {
        NewCard = 1,
        Renew = 9
    }

    public enum FatherCodes
    {
        PayStatus = 1,
        OperationType = 2
    }

    public enum SearchTypes
    {
        AgraID = 1,
        CitizenID = 2
    }
}