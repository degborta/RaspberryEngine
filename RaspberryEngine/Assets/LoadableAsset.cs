namespace RaspberryEngine.Assets {
	public class LoadableAsset {
		public string FileName { get; set; }
		public string Key { get; set; }
		public AssetType Type { get; set; }

		public int Index { get; set; }
		public int Usage { get; set; }

		public LoadableAsset(string key, string fileName, AssetType type) {
			Key = key;
			FileName = fileName;
			Type = type;
		}
	}
}
