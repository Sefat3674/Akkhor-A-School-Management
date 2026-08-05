namespace Akkhor.Domain.Enums;

public enum CaseType
{
    Civil,
    Criminal,
    Family,
    Property,
    Corporate,
    LaborLaw,
    Other
}

public enum CaseStatus
{
    Preparing,
    Filed,
    AwaitingEvidence,
    InHearing,
    Judgment,
    Closed,
    Archived
}

public enum CasePriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum InvoiceStatus
{
    Unpaid,
    PartiallyPaid,
    Paid,
    Overdue
}

