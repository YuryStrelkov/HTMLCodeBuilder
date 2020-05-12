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
        private Dictionary<HTMLSettings, string> settings;

        public void save(string pach)
        {
            string settingsStr = "";

            foreach (HTMLSettings setting in settings.Keys)
            {
                settingsStr += setting.ToString()+" "+ ((int)(setting)).ToString() + " "+settings[setting]+"\n";
            }

            System.IO.File.WriteAllText(pach,settingsStr);
        }

        public void load(string pach)
        {
            string[] lines = System.IO.File.ReadAllLines(pach);

            string[] splitLine;

            for (int i=0;i<lines.Length ;i++)
            {
                splitLine = lines[i].Split(' ');
                setSetting((HTMLSettings)int.Parse(splitLine[1]), splitLine[2]);
            }
        }

        public string getSetting(HTMLSettings setting)
        {
            if (settings.ContainsKey(setting))
            {
                return settings[setting];
            }
            return "";
        }

        public void setSetting(HTMLSettings setting, string val)
        {
            if (settings.ContainsKey(setting))
            {
                settings[setting] = val;
                return;
            }
            settings.Add(setting, val);
        }

        private void useDefault()
        {
            setSetting(HTMLSettings.ContentFontSize, "14pt");
            setSetting(HTMLSettings.FontSize, "14pt");
            setSetting(HTMLSettings.ParagraphFontSize, "14pt");
            setSetting(HTMLSettings.ParagraphTitleFontSize, "20pt");


            setSetting(HTMLSettings.PageHeight, "300mm");
            setSetting(HTMLSettings.PageWigth, "200mm");

            setSetting(HTMLSettings.PageLeftIndent, "15mm");
            setSetting(HTMLSettings.PageContentWidth, "180mm");
            setSetting(HTMLSettings.PageTopIndent, "15mm");
            setSetting(HTMLSettings.PageContentHeight, "270мм");
            setSetting(HTMLSettings.TitleFontSize, "30pt");

            /*
                PageBackgroundColor = 10,
        ContentBackgroundColor = 11,
        TableBoderColor = 12,ContentHoverColor
             */
            setSetting(HTMLSettings.PageBackgroundColor, "rgba(205,205,205,255)");
            setSetting(HTMLSettings.ContentHoverColor, "rgba(225,225,225,255)");
            setSetting(HTMLSettings.ContentBackgroundColor, "rgba(250,250,250,255)");
            setSetting(HTMLSettings.TableBoderColor, "rgba(200,200,200,255)");
            /*
            ParagraphFontColor = 14,
            ContentFontColor = 15,
            ContentHoverColor = 16,
            ParagraphTitleFontColor = 17,
             */
            setSetting(HTMLSettings.ParagraphFontColor, "rgba(0,0,0,255)");
            setSetting(HTMLSettings.ContentFontColor, "rgba(0,0,0,255)");
            setSetting(HTMLSettings.ContentHoverColor, "rgba(225,225,225,255)");
            setSetting(HTMLSettings.ParagraphTitleFontColor, "rgba(0,0,0,255)");

        }

        public HTMLDocSettings()
        {
            settings = new Dictionary<HTMLSettings, string>();
            useDefault();
            try {
                load("defaulSettings.txt");
            } catch(Exception e)
            {
                save("defaulSettings.txt");
                Console.Write("Something wrong with document settings file...\n");
                Console.Write("Default settings will be aplied...\n");
            }
        }
    }
}
