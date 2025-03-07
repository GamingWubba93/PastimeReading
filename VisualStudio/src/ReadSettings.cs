﻿using System;
using System.IO;
using MelonLoader;
using ModSettings;
using UnityEngine;

namespace PastimeReading
{
    internal class ReadSettings : JsonModSettings
    {
        [Section("General settings")]

        [Name("Current page")]
        [Description("Current page. Use to quickly navigate the book. \n\nCurrently there's no way to get actual number of pages for your book, so it's set to 999.")]
        [Slider(1f, 999f, 500)]
        public int currentPage = 1;

        [Name("Font size")]
        [Description("Page font size. Default: 16")]
        [Slider(14f, 24f)]
        public int fontSize = 16;

        [Name("RTL language")]
        [Description("Enable right-to-left way of writing. \n\nThis option also enables Arabic diacritics and makes the book open to the right. \n\nSupports Farsi, Arabic and Hebrew. \n\nLTR languages break when turning this on. No fix for that.")]
        public bool enableRTL = false;

        [Name("Text alignment")]
        [Description("Default: Justified. \n\nOption is there in case your language looks bad with it. Or if you want to be fancy when reading poetry")]
        [Choice(new string[]
        {
            "Justified",
            "Left",
            "Right",
            "Centered"
        })]
        public int textAlignment;

        [Section("Controls")]

        [Name("Keybinding")]
        [Description("The key you press to take out the book. Press again to open it. Holster to close.")]
        public KeyCode openKeyCode = KeyCode.Alpha5;

        [Section("Customization")]

        [Name("Book texture")]
        [Description("Book appearance. \n\nYou can modify textures yourself. They should be here ...Mods/pastimeReading/textures/")]
        [Choice(new string[]
        {
            "variant_A",
            "variant_B",
            "variant_C",
            "variant_D",
            "variant_E",
        })]
        public int bookTexture;

        [Section("Reload")]

        [Name("Reload book from text file?")]
        [Description("Check this to reload book from file when pressing CONFIRM.")]
        public bool reloadBook;

        public static bool settingsChanged;

        protected override void OnConfirm()
		{


            ReadSettings.settingsChanged = true;

            base.OnConfirm();

            if (ReadMain.hands == null) return;


            // reload stuff when pressin confirm


            // RTL switch
            if (PageManager.currentlyRTL != Settings.options.enableRTL)
            {
                PageManager.InitPages("rtl");
                Settings.options.reloadBook = true;
            }

            // book reload
            if (Settings.options.reloadBook)
			{
                PageManager.InitPages("read");
                PageManager.InitPages("split");
                Settings.options.currentPage = 1;
                PageManager.InitPages("setup");
                Settings.options.reloadBook = false;
            }

            // book texture

            if (PageManager.currentBookTexture != Settings.options.bookTexture)
            {
                ReadInstance.UpdateBookTexture();
                PageManager.InitPages("setup");
            }

            // change page

            if (PageManager.currentPage != Settings.options.currentPage)
            {
                PageManager.currentPage = Settings.options.currentPage;
                PageManager.InitPages("page");
                PageManager.InitPages("setup");
            }


            // font change
            if (PageManager.currentFontSize != (float)Settings.options.fontSize)
            {
                PageManager.InitPages("font");
                PageManager.InitPages("split");
                PageManager.InitPages("page");
                PageManager.InitPages("setup");
            }

            if (PageManager.currentAlignment != Settings.options.textAlignment)
            {
                PageManager.InitPages("font");
            }
        }
	}
}
