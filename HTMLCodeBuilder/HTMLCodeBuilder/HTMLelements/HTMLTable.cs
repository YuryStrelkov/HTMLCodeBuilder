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
            return table.getElement(getCellID(i, j));
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

        public override string expandOpenTag(int tab)
        {
            TabLevel = tab;

            code.Append(getTab(TabLevel));

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

        public override string expandCloseTag(int tab)
        {
            return table.buildCode(TabLevel) +  getTab(TabLevel) + CloseTag;
        }

        private void addHeadersRow(string [] headedrs)
        {
            table.addElementParam(table.RootID, "##style", CSS);

            W = headedrs.Length;

            H++;

            int tr = table.addElement(new HTMLElement("<tr>", "</tr>"), table.RootID);

            rows.Add(tr);

            cells.Add(tr, new List<int>());

            for (int i = 0; i < W; i++)
            {
                int td = table.addElement(new HTMLElement("<th>", "</th>"), tr);
                cells[tr].Add(td);
                table.getElement(td).InnerString = headedrs[i];
                table.addElementParam(td, "class", "table-header-cell");
            }
            records = W;
        }

        private void addRow()
        {
            table.addElementParam(table.RootID, "##style", CSS);

            H++;

            int tr = table.addElement(new HTMLElement("<tr>", "</tr>"), table.RootID);

            rows.Add(tr);

            cells.Add(tr, new List<int>());
            
            for (int i = 0; i < W; i++)
                {
                    int td = table.addElement(new HTMLElement("<td>", "</td>"), tr);
                    cells[tr].Add(td);
                    table.addElementParam(td, "class", "table-cell");
                }
    
        }

        private HTMLTable(string opentag, string closetag) : base(opentag, closetag)
        {
            cells = new Dictionary<int, List<int>>();

            table = new TagElementsGroup(this);
        }

        public  HTMLTable(string [] headers):base("<table>", "</table>")
        {
            appendParam("class", OpenTag.Substring(1, OpenTag.Length - 2));

            OpenTag = OpenTag.Remove(OpenTag.Length - 1, 1); ;
            
            Tag = OpenTag.Substring(1, OpenTag.Length - 1);

            W = headers.Length;

            cells = new Dictionary<int, List<int>>();

            rows = new List<int>();

            table = new TagElementsGroup(new HTMLElement("<tbody>", "</tbody>"));

            table.addElementParam(table.RootID, "class", "table-data");

            appendParam( "id", "table-" + new Random().NextDouble().GetHashCode().ToString());

            string style_width = "width : " + (100.0 / headers.Length).ToString().Replace(',', '.') + "%";

            CSS        = getParam("id")+ "+width:100%;border-collapse: collapse;text-align : center;+" +
                         getParam("id") + " td+border:1px solid " + HTMLElements.HTMLsettings.getSetting(HTMLSettings.TableBoderColor) + ";" + style_width + ";+" +
                         getParam("id") + " th+border:1px solid " + HTMLElements.HTMLsettings.getSetting(HTMLSettings.TableBoderColor) + ";";

            appendParam( "##style", CSS);

            addHeadersRow(headers);
        }

        public override string ToString()
        {
            return OpenTag + ">" + " " + InnerString + " " + CloseTag;
        }

        public override ITagElement Copy()
        {
            TagElementsGroup table_ = table.getSubListCopy(table.RootID);
            HTMLTable element = (HTMLTable)table_.getElement(table_.RootID);
            return element;
        }
    }
}
