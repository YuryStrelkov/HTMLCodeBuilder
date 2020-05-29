using HTMLCodeBuilder.TaggedElements;
using System;
using System.Collections.Generic;

namespace HTMLCodeBuilder.HTMLelements
{
    public class HTMLTable:TagElement
    {
        public int W { get; private set; }

        public int H { get; private set; }

        private int records;

        private string CSS;

        private List<int> rows;

        private Dictionary<int, List<int>> cells;

        private TagElementsGroup table;

        public ITagElement getCellData(int i, int j)
        {
            return table.GetElement(getCellID(i, j));
        }

        public int getCellID(int i, int j)
        {
            if (H < i + 1)
            {
                return table.RootID;
            }
            if (W < j)
            {
                return table.RootID;
            }

            return cells[rows[i]][j];
        }

        public void appendRecord(string val)
        {
            if ( records%W == 0)
            {
                addRow();
            }

            setRecord(records/W, records%W, val);

            records++;
        }

        public void setRecord(int i, int j, string val)
        {
            getCellData(i, j).InnerString = val;
        }

        public override string ExpandOpenTag(int tab)
        {
            TabLevel = tab;

            code.Append(GetTab(TabLevel));

            code.Append(OpenTag);

            foreach (string key in elementSettings.Keys)
            {
                if (key.StartsWith("#") || key.StartsWith("."))
                {
                    continue;
                }
                code.Append(" ");
                code.Append(key);
                code.Append(" = ");
                code.Append('"');
                code.Append(elementSettings[key]);
                code.Append('"');
            }
            code.Append(">");
            code.Append("\n");
            return code.ToString();
        }

        public override string ExpandCloseTag(int tab)
        {
            return table.BuildCode(TabLevel) +  GetTab(TabLevel) + CloseTag;
        }

        private void addHeadersRow(string [] headedrs)
        {
            table.AddElementParam(table.RootID, "##style", CSS);

            W = headedrs.Length;

            H++;

            int tr = table.AddElement(new HTMLElement("<tr>", "</tr>"), table.RootID);

            rows.Add(tr);

            cells.Add(tr, new List<int>());

            for (int i = 0; i < W; i++)
            {
                int td = table.AddElement(new HTMLElement("<th>", "</th>"), tr);
                cells[tr].Add(td);
                table.GetElement(td).InnerString = headedrs[i];
                table.AddElementParam(td, "class", "table-header-cell");
            }
            records = W;
        }

        private void addRow()
        {
            table.AddElementParam(table.RootID, "##style", CSS);

            H++;

            int tr = table.AddElement(new HTMLElement("<tr>", "</tr>"), table.RootID);

            rows.Add(tr);

            cells.Add(tr, new List<int>());
            
            for (int i = 0; i < W; i++)
                {
                    int td = table.AddElement(new HTMLElement("<td>", "</td>"), tr);
                    cells[tr].Add(td);
                    table.AddElementParam(td, "class", "table-cell");
                }
    
        }

        private HTMLTable(string opentag, string closetag) : base(opentag, closetag)
        {
            cells = new Dictionary<int, List<int>>();

            table = new TagElementsGroup(this);
        }

        public  HTMLTable(string [] headers):base("<table>", "</table>")
        {
            AddParam("class", OpenTag.Substring(1, OpenTag.Length - 2));

            OpenTag = OpenTag.Remove(OpenTag.Length - 1, 1); ;
            
            Tag = OpenTag.Substring(1, OpenTag.Length - 1);

            W = headers.Length;

            cells = new Dictionary<int, List<int>>();

            rows = new List<int>();

            table = new TagElementsGroup(new HTMLElement("<tbody>", "</tbody>"));

            table.AddElementParam(table.RootID, "class", "table-data");

            AddParam( "id", "table-" + new Random().NextDouble().GetHashCode().ToString());

            string style_width = "width : " + (100.0 / headers.Length).ToString().Replace(',', '.') + "%";

            CSS        = GetParam("id")+ "+width:100%;border-collapse: collapse;text-align : center;+" +
                         GetParam("id") + " td+border:1px solid " + HTMLElements.HTMLsettings.GetSetting(HTMLSettings.TableBoderColor) + ";" + style_width + ";+" +
                         GetParam("id") + " th+border:1px solid " + HTMLElements.HTMLsettings.GetSetting(HTMLSettings.TableBoderColor) + ";";

            AddParam( "##style", CSS);

            addHeadersRow(headers);
        }

        public override string ToString()
        {
            return OpenTag + ">" + " " + InnerString + " " + CloseTag;
        }

        public override ITagElement Copy()
        {
            TagElementsGroup table_ = table.GetSubListCopy(table.RootID);
            HTMLTable element = (HTMLTable)table_.GetElement(table_.RootID);
            return element;
        }
    }
}
