namespace MeldRDP.ViewModels {
	using MeldRDP.Models;

	public class EndPointGroupViewModel : ViewModelBase {

		public string Name { get; set; }
		public bool IsVirtual { get; }

		public EndPointGroupViewModel(string name, bool isVirtual) {
			Name = name;
			IsVirtual = isVirtual;
		}

		public ConnectionGroup GetGroup() {
			return new ConnectionGroup(Name = this.Name);
		}
	}
}
