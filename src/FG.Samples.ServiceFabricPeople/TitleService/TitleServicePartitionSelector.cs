using System.Text;
using FG.Common.Utils;

namespace TitleService
{
	public class TitleServicePartitionSelector
	{
		public static long GetPartition(string title)
		{
			var keyValue = (long)CRC64.ToCRC64(Encoding.UTF8.GetBytes(title));
			return keyValue;
		}
	}
}