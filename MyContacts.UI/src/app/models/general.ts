export class ApiResult<T>
{
  public Error: string;
  public Result: T;
}

export class BearerDetails
{
  public Token: string;
  public ExpirationDate: Date;
}

export class Contact {
  public Id: string;
  public Username: string;
  public Email: string;
  public PhoneNumber: string;
}

export class Login {
  public Username: string;
  public Password: string;
}
