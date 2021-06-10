namespace API.Errors
{
  public class HttpException
  {
    public HttpException(int statusCode, string message = "Internal Server Error", string details = null)
    {
      this.statusCode = statusCode;
      this.message = message;
      this.details = details;
    }

    public int statusCode { get; set; }
    public string message { get; set; }
    public string details { get; set; }
  }
}