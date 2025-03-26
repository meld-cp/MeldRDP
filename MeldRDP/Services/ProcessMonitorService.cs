namespace MeldRDP.Services {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading;

	public class ProcessMonitorService {
		private readonly Timer timer;

		private readonly Dictionary<Process, Action> processes = [];
		private bool IsChecking;

		public ProcessMonitorService(TimeSpan checkFrequency) {
			this.timer = new Timer(callback: _ => this.CheckProcesses(), state: null, dueTime: TimeSpan.Zero, period: checkFrequency);
		}

		public void MonitorOnExit(Process process, Action OnExitAction) {
			this.processes.Add(process, OnExitAction);
		}

		private void CheckProcesses() {
			if (this.IsChecking) {
				return;
			}
			this.IsChecking = true;
			try {
				var processes = this.processes.Select(x => (Process: x.Key, OnExitAction: x.Value)).ToArray();
				foreach (var (proc, onExitCallback) in processes) {
					if (proc.HasExited) {
						this.processes.Remove(proc);
						onExitCallback();
					}
				}
			} finally {
				this.IsChecking = false;
			}
		}

	}
}
