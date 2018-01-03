using System;

namespace DataRelay.Grains
{
	public class Message
	{
		public Guid Id { get; }
		public string PayloadType { get; }

		public Message(string payloadType)
		{
			Id = Guid.NewGuid();
			PayloadType = payloadType;
		}
	}
}