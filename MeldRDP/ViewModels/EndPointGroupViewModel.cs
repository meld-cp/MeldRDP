namespace MeldRDP.ViewModels {
	using MeldRDP.Models;

	public class EndPointGroupViewModel : ViewModelBase {

		public string Name { get; set; }
		public ConnectionGroupType GroupType { get; }

		public EndPointGroupViewModel(ConnectionGroupType groupType, string name) {
			this.GroupType = groupType;
			this.Name = name;
		}

		public ConnectionGroup GetGroup() {
			return new ConnectionGroup(Type: this.GroupType, Name: this.Name);
		}
	}
}
