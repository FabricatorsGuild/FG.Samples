using System;

namespace PersonActor.Diagnostics
{
	public interface IActorLogger
	{
		void ActorActivated(bool firstActivation);
		void ActorDeactivated();
		IDisposable ReadState(string stateName);
		IDisposable WriteState(string stateName);
		void ActorHostInitializationFailed(Exception ex);
	}
}