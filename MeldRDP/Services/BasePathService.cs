namespace MeldRDP.Services {
	using System.Collections.Generic;
	using System.IO;

	public abstract class BasePathService(string basePath) {

		protected bool BasePathExists() {
			return Directory.Exists(basePath);
		}

		protected void EnsureDirectoryExists() {
			if (!Directory.Exists(basePath)) {
				Directory.CreateDirectory(basePath);
			}
		}

		protected string BuildFilePath(string relFilename) {
			return Path.Combine(basePath, relFilename);
		}

		protected IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption) {
			return Directory.EnumerateFiles(
				path: basePath,
				searchPattern: searchPattern,
				searchOption: searchOption
			);
		}
	}
}
