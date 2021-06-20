namespace API.Interfaces
{
	public class NotificationMessageDTO
	{
		public string EndpointDetails { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; }
	}
}