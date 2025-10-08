public class DomainException : Exception
{
    public string Code { get; }
    public DateTime OccurredOn { get; }

    protected DomainException(string message, string code) : base(message)
    {
        Code = code;
        OccurredOn = DateTime.UtcNow;
    }

    protected DomainException(string message, string code, Exception innerException)
        : base(message, innerException)
    {
        Code = code;
        OccurredOn = DateTime.UtcNow;
    }
}
public class AccessDeniedDomainException : DomainException
{
    public AccessDeniedDomainException(string message) 
        : base(message, "ACCESS_ERROR") { }
}
public class ConflictDomainException : DomainException
{
    public ConflictDomainException(string message)
        : base(message, "CONFLICT_ERROR") { }
}

public class NotFoundDomainException : DomainException
{
    public NotFoundDomainException(string message)
        : base(message, "NOT_FOUND_ERROR") { }
}

//----------------------------------------------------------------------
public class UserDomainException : DomainException
{
    public UserDomainException(string message)
        : base(message, "USER_ERROR") { }

    public UserDomainException(string message, Exception innerException)
        : base(message, "USER_ERROR", innerException) { }
}

public class WorkingGroupDomainException : DomainException
{
    public WorkingGroupDomainException(string message) 
        : base(message, "WORKGROUP_ERROR") { }
}

public class CandidateDomainException : DomainException
{
    public CandidateDomainException(string message)
        : base(message, "CANDIDATE_ERROR") { }
}



public class UnitOfWorkDomainException : DomainException
{
    public UnitOfWorkDomainException(string message)
        : base(message, "UNIT_OF_WORK_ERROR") { }
}

public class EmployeeDomainException : DomainException
{
    public EmployeeDomainException(string message)
        : base(message, "EMPLOYEE_ERROR") { }
}

public class VerificationDomainException : DomainException
{
    public VerificationDomainException(string message)
        : base(message, "VERIFICATION_ERROR") { }
}