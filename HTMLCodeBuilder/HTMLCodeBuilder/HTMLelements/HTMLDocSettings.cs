using System;
using System.Collections.Generic;

namespace HTMLCodeBuilder.HTMLelements
{
    public enum HTMLSettings
    {
        FontSize = 0,
        ParagraphFontSize = 1,
        ContentFontSize = 2,
        ParagraphTitleFontSize = 3,

        PageWigth = 4,
        PageHeight = 5,
        PageLeftIndent = 6,
        PageContentWidth = 7,
        PageContentHeight = 8,
        PageTopIndent = 9,

        PageBackgroundColor = 10,
        ContentBackgroundColor = 11,
        TableBoderColor = 12,

        TitleFontSize = 13,

        ParagraphFontColor = 14,
        ContentFontColor = 15,
        ContentHoverColor = 16,
        ParagraphTitleFontColor = 17,
    }
    public class HTMLDocSettings
    {
        private readonly Dictionary<HTMLSettings, string> settings;

        public void Save(string pach)
        {
            string settingsStr = "";

            foreach (HTMLSettings setting in settings.Keys)
            {
                settingsStr += setting.ToString()+" "+ ((int)(setting)).ToString() + " "+settings[setting]+"\n";
            }

            System.IO.File.WriteAllText(pach,settingsStr);
        }

        public void Load(string pach)
        {
            string[] lines = System.IO.File.ReadAllLines(pach);

            string[] splitLine;

            for (int i=0;i<lines.Length ;i++)
            {
                splitLine = lines[i].Split(' ');
                SetSetting((HTMLSettings)int.Parse(splitLine[1]), splitLine[2]);
            }
        }

        public string GetSetting(HTMLSettings setting)
        {
            if (settings.ContainsKey(setting))
            {
                return settings[setting];
            }
            return "";
        }

        public void SetSetting(HTMLSettings setting, string val)
        {
            if (settings.ContainsKey(setting))
            {
                settings[setting] = val;
                return;
            }
            settings.Add(setting, val);
        }

        private void UseDefault()
        {
            SetSetting(HTMLSettings.ContentFontSize, "14pt");
            SetSetting(HTMLSettings.FontSize, "14pt");
            SetSetting(HTMLSettings.ParagraphFontSize, "14pt");
            SetSetting(HTMLSettings.ParagraphTitleFontSize, "20pt");


            SetSetting(HTMLSettings.PageHeight, "300mm");
            SetSetting(HTMLSettings.PageWigth, "200mm");

            SetSetting(HTMLSettings.PageLeftIndent, "15mm");
            SetSetting(HTMLSettings.PageContentWidth, "180mm");
            SetSetting(HTMLSettings.PageTopIndent, "15mm");
            SetSetting(HTMLSettings.PageContentHeight, "270мм");
            SetSetting(HTMLSettings.TitleFontSize, "30pt");

            /*
                PageBackgroundColor = 10,
        ContentBackgroundColor = 11,
        TableBoderColor = 12,ContentHoverColor
             */
            SetSetting(HTMLSettings.PageBackgroundColor, "rgba(205,205,205,255)");
            SetSetting(HTMLSettings.ContentHoverColor, "rgba(225,225,225,255)");
            SetSetting(HTMLSettings.ContentBackgroundColor, "rgba(250,250,250,255)");
            SetSetting(HTMLSettings.TableBoderColor, "rgba(200,200,200,255)");
            /*
            ParagraphFontColor = 14,
            ContentFontColor = 15,
            ContentHoverColor = 16,
            ParagraphTitleFontColor = 17,
             */
            SetSetting(HTMLSettings.ParagraphFontColor, "rgba(0,0,0,255)");
            SetSetting(HTMLSettings.ContentFontColor, "rgba(0,0,0,255)");
            SetSetting(HTMLSettings.ContentHoverColor, "rgba(225,225,225,255)");
            SetSetting(HTMLSettings.ParagraphTitleFontColor, "rgba(0,0,0,255)");

        }

        public HTMLDocSettings()
        {
            settings = new Dictionary<HTMLSettings, string>();
            UseDefault();
            try {
                Load("defaulSettings.txt");
            } catch(Exception e)
            {
                Save("defaulSettings.txt");
                Console.Write("Something wrong with document settings file...\n");
                Console.Write("Default settings will be aplied...\n");
            }
        }
    }
}
