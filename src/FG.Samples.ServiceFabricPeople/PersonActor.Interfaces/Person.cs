using System.Runtime.Serialization;

namespace PersonActor.Interfaces
{
	[DataContract]
	public class Person
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Title { get; set; }
	}
}