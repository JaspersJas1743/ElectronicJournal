using System.Threading.Tasks;

namespace ElectronicJournal.Utilities.Api.ConnectionChecker
{
	public interface IConnectionChecker
	{
		Task<bool> CheckConnection();
	}
}
