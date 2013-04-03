using System.Collections.Generic;
using System.Linq;

namespace EveWindowsPhone.Controllers.ApplicationBarController {
	public class ApplicationBarPageCollection : IEnumerable<ApplicationBarPage> {
		private Dictionary<string, ApplicationBarPage> pages; 


		public ApplicationBarPageCollection() {
			this.pages = new Dictionary<string, ApplicationBarPage>();
		}


		public ApplicationBarPage this[string name] {
			get { return this.pages[name]; }
			set { this.pages[name] = value; }
		}

		public ApplicationBarPage this[int index] {
			get { return this.pages.Values.ElementAt(index); }
		}

		public ApplicationBarPage GetPageAt(int index) {
			return this.pages.Values.ElementAt(index);
		}

		public int Count {
			get { return this.pages.Count; }
		}

		public bool Add(string name, ApplicationBarPage page) {
			if (this.pages.ContainsKey(name))
				return false;

			this.pages.Add(name, page);
			return true;
		}

		public bool Contains(string name) {
			return this.pages.ContainsKey(name);
		}

		public void Clear() {
			this.pages.Clear();
		}

		#region IEnumerable<ApplicationBarPage

		public IEnumerator<ApplicationBarPage> GetEnumerator() {
			return new ApplicationBarPageCollectionEnumerator(this);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		#endregion
	}
}