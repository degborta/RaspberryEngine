using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RaspberryEngine.Assets
{
	public class AssetsManager
	{
		public bool HighRes;

		private ContentManager _contentManager;

		private List<Texture2D> _textures;
		private List<SpriteFont> _fonts;
		private List<SoundEffect> _sounds;
		private List<Song> _songs;
		private List<Video> _videos;
		private List<LoadableAsset> _assets;

		public AssetsManager(ContentManager contentManager)
		{
			_contentManager = contentManager;
			_textures = new List<Texture2D>();
			_fonts = new List<SpriteFont>();
			_sounds = new List<SoundEffect>();
			_songs = new List<Song>();
			_videos = new List<Video>();

			_assets = new List<LoadableAsset>();
		}

		public void RemoveScreensAssets(List<LoadableAsset> screenAssets)
		{
			//Update the usage counter on all assets currently loaded
			foreach (LoadableAsset assetToRemove in screenAssets) {
				foreach (LoadableAsset asset in _assets)
				{
					if (assetToRemove.Key == asset.Key)
						asset.Usage--;
				}
			}

			//Remove the assets that are not being used
			for (int i = _assets.Count - 1; i < 0; i--) {
				if (_assets[i].Usage == 0)
					_assets.Remove(_assets[i]);
			}
		}

		public void AddScreensAssets(List<LoadableAsset> screenAssets)
		{
			//Add assets that has not ben loaded
			foreach (LoadableAsset assetToAdd in screenAssets)
			{
				bool exist = false;

				//If asset is loaded add one to the asstes Usage counter
				foreach (LoadableAsset asset in _assets)
					if (assetToAdd.Key == asset.Key)
					{ exist = true; asset.Usage++; }

				//Load the asset that is not loaded yet
				if (!exist)
				{
					string filePath;

					//Create a copy of the loadableAsset
					LoadableAsset newAsset = new LoadableAsset(assetToAdd.Key, assetToAdd.FileName, assetToAdd.Type);
					newAsset.Usage = 1;

					//load the asset
					switch (assetToAdd.Type)
					{
						case AssetType.Texture:

							filePath = newAsset.FileName;

							if (HighRes)
								filePath += "@2x";

							newAsset.Index = _textures.Count; // Write the index of the Texture
							_textures.Add(_contentManager.Load<Texture2D>(filePath));
							break;

						case AssetType.Font:

							filePath = newAsset.FileName;

							if (HighRes)
								filePath += "@2x";

							newAsset.Index = _fonts.Count; // Write the index of the Font
							_fonts.Add(_contentManager.Load<SpriteFont>(filePath));
							break;

						case AssetType.Sound:

							newAsset.Index = _sounds.Count; // Write the index of the Sound
							_sounds.Add(_contentManager.Load<SoundEffect>(newAsset.FileName));
							break;

						case AssetType.Song:

							newAsset.Index = _songs.Count; // Write the index of the Song
							_songs.Add(_contentManager.Load<Song>(newAsset.FileName));
							break;

						case AssetType.Video:

							newAsset.Index = _videos.Count; // Write the index of the Video
							_videos.Add(_contentManager.Load<Video>(newAsset.FileName));
							break;
					}
					//Add asset to our assets list
					_assets.Add(newAsset);
				}

			}
		}

		/// <summary>
		/// Be carefull with this. It will unload all content from the contentmanager and clear everything in this AssetsManager.
		/// Dont try to use any contents after this without calling AddScreenAssets() first.
		/// </summary>
		public void Unload()
		{
			_contentManager.Unload();

			_textures.Clear();
			_fonts.Clear();
			_sounds.Clear();
			_songs.Clear();
			_videos.Clear();

			_assets.Clear();
		}

		public Object GetAsset(string Key)
		{
			foreach (LoadableAsset asset in _assets)
			{
				if (Key == asset.Key)
					switch (asset.Type)
					{
						case AssetType.Texture:
							return _textures[asset.Index];

						case AssetType.Font:
							return _fonts[asset.Index];

						case AssetType.Sound:
							return _sounds[asset.Index];

						case AssetType.Song:
							return _songs[asset.Index];

						case AssetType.Video:
							return _videos[asset.Index];
					}
			}
			return null;
		}

	}
}
