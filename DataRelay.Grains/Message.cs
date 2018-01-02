namespace DataRelay.Grains
{
	public class Message
	{
		public string PayloadType { get; }

		public Message(string payloadType)
		{
			PayloadType = payloadType;
		}
	}
}