using System;
using System.Collections.Generic;

namespace EveWindowsPhone.Controllers.ApplicationBarController {
	public class ApplicationBarPageCollectionEnumerator : IEnumerator<ApplicationBarPage> {
		// Variables
		private ApplicationBarPageCollection collection;
		private int index;


		public ApplicationBarPageCollectionEnumerator(ApplicationBarPageCollection collection) {
			// Inform about null collection argument passed
			if (collection == null)
				System.Diagnostics.Debug.WriteLine("Given collection is null. Creating empty collection.");

			// Initialize variables
			this.collection = collection ?? new ApplicationBarPageCollection();
			this.index = 0;
		}


		public ApplicationBarPage Current {
			get { return this.collection.GetPageAt(this.index); }
		}

		object System.Collections.IEnumerator.Current {
			get { return this.Current; }
		}

		public bool MoveNext() {
			// Get index into range of collection
			this.index = Math.Min(this.collection.Count - 1, Math.Max(0, this.index));

			// Check if index can be increased by one
			if (this.index + 1 >= this.collection.Count)
				return false;

			this.index += 1;
			return true;
		}

		public void Reset() {
			this.index = 0;
		}

		public void Dispose() {
			this.collection = null;
			this.index = 0;
		}
	}
}