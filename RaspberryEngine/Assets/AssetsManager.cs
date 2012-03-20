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

        private ContentManager Content;

        private List<Texture2D> Textures;
        private List<SpriteFont> Fonts;
        private List<SoundEffect> Sounds;
        private List<Song> Songs;
        private List<Video> Videos;
        private List<LoadableAsset> Assets;

        public AssetsManager(ContentManager contentManager)
        {
            Content = contentManager;
            Textures = new List<Texture2D>();
            Fonts = new List<SpriteFont>();
            Sounds = new List<SoundEffect>();
            Songs = new List<Song>();
            Videos = new List<Video>();

            Assets = new List<LoadableAsset>();
        }

        public void RemoveScreensAssets(List<LoadableAsset> ScreenAssets)
        {
            //Update the usage counter on all assets currently loaded
            foreach (LoadableAsset assetToRemove in ScreenAssets)
                foreach (LoadableAsset asset in Assets)
                {
                    if (assetToRemove.Key == asset.Key)
                        asset.Usage--;
                }

            //Remove the assets that are not being used
            for(int i = Assets.Count-1; i < 0; i--)
                if (Assets[i].Usage == 0)
                    Assets.Remove(Assets[i]);
        }

        public void AddScreensAssets(List<LoadableAsset> ScreenAssets)
        {
            //Add assets that has not ben loaded
            foreach (LoadableAsset assetToAdd in ScreenAssets)
            {
                bool exist = false;

                //If asset is loaded add one to the asstes Usage counter
                foreach (LoadableAsset asset in Assets)
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

                            newAsset.Index = Textures.Count; // Write the index of the Texture
                            Textures.Add(Content.Load<Texture2D>(filePath));
                            break;

                        case AssetType.Font:
                            filePath = newAsset.FileName;

                            if (HighRes)
                                filePath += "@2x";

                            newAsset.Index = Fonts.Count; // Write the index of the Font
                            Fonts.Add(Content.Load<SpriteFont>(filePath));
                            break;

                        case AssetType.Sound:

                            newAsset.Index = Sounds.Count; // Write the index of the Sound
                            Sounds.Add(Content.Load<SoundEffect>(newAsset.FileName));
                            break;

                        case AssetType.Song:

                            newAsset.Index = Songs.Count; // Write the index of the Song
                            Songs.Add(Content.Load<Song>(newAsset.FileName));
                            break;

                        case AssetType.Video:

                            newAsset.Index = Videos.Count; // Write the index of the Video
                            Videos.Add(Content.Load<Video>(newAsset.FileName));
                            break;
                    }
                    //Add asset to our assets list
                    Assets.Add(newAsset);
                }

            }
        }

        /// <summary>
        /// Be carefull with this. It will unload all content from the contentmanager and clear everything in this AssetsManager.
        /// Dont try to use any contents after this without calling AddScreenAssets() first.
        /// </summary>
        public void Unload()
        {
            Content.Unload();

            Textures.Clear();
            Fonts.Clear();
            Sounds.Clear();
            Songs.Clear();
            Videos.Clear();

            Assets.Clear();
        }

        public Object GetAsset(string Key)
        {
            foreach (LoadableAsset asset in Assets)
            {
                if (Key == asset.Key)
                    switch (asset.Type)
                    {
                        case AssetType.Texture:
                            return Textures[asset.Index];

                        case AssetType.Font:
                            return Fonts[asset.Index];

                        case AssetType.Sound:
                            return Sounds[asset.Index];

                        case AssetType.Song:
                            return Songs[asset.Index];

                        case AssetType.Video:
                            return Videos[asset.Index];
                    }
            }
            return null;
        }

    }
}
