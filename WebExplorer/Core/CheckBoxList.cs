using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebExplorer
{
    public static class HtmlControls
    {
        public enum Layoutt
        {
            Table = 0,
            Flow = 1
        }

        public enum Direction
        {
            Horizontal = 0,
            Vertical = 1
        }

        public enum RepeatColumns
        {
            OneColumn = 1,
            TwoColumns = 2,
            ThreeColumns = 3,
            FourColumns = 4,
            FiveColumns = 5,
            SixColumns = 6
        }
        public static MvcHtmlString CheckBoxListHtml(this HtmlHelper helper, 
            IDictionary<string, int> items, 
            CheckBoxListSettings settings,
            object htmlAttributes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            TagBuilder HtmlBody = GenerateHtmlMarkup_OuterTag(settings.CheckBoxListLayout, new RouteValueDictionary(htmlAttributes));

            int iMod = items.Count % (int)settings.CheckBoxListRepeatColumns;
            int iterationsCount = items.Count / (int)settings.CheckBoxListRepeatColumns + (iMod == 0 ? 0 : 1);
            for (var i = 0; i < iterationsCount; i++)
            {
                stringBuilder.Append(GenerateHtml_BeginRow(settings.CheckBoxListLayout));
                foreach (KeyValuePair<string, int> item in items.Where((item, index) =>
                    settings.CheckBoxListDirection == Direction.Horizontal ?
                        index / (int)settings.CheckBoxListRepeatColumns == i
                    :
                        (index - i) % iterationsCount == 0))
                {
                    stringBuilder.AppendFormat("{0} {1} {2} {3}",
                        GenerateHtmlMarkup_CheckBox(item, settings),
                        GenerateHtml_MiddleRow(settings.CheckBoxListLayout),
                        GenerateHtmlMarkup_Label(item),
                        GenerateHtml_MiddleRow(settings.CheckBoxListLayout));
                }
                stringBuilder.Append(GenerateHtml_EndRow(settings.CheckBoxListLayout));
            }

            HtmlBody.InnerHtml = stringBuilder.ToString();
            return new MvcHtmlString(HtmlBody.ToString(TagRenderMode.Normal));
        }
        public static string GenerateHtml_BeginRow(Layoutt cbl_Layout)
        {
            switch (cbl_Layout)
            {
                case Layoutt.Table:
                    return "<tr><td>";
                case Layoutt.Flow:
                    return "";
                default:
                    return "";
            }
        }

        public static string GenerateHtml_MiddleRow(Layoutt cbl_Layout)
        {
            switch (cbl_Layout)
            {
                case Layoutt.Table:
                    return "</td><td>";
                case Layoutt.Flow:
                    return "";
                default:
                    return "";
            }
        }

        public static string GenerateHtml_EndRow(Layoutt cbl_Layout)
        {
            switch (cbl_Layout)
            {
                case Layoutt.Table:
                    return "</td></tr>";
                case Layoutt.Flow:
                    return "<br />";
                default:
                    return "";
            }
        }

        public static TagBuilder GenerateHtmlMarkup_OuterTag(Layoutt cbl_Layout, IDictionary<string, object> htmlAttributes)
        {
            string htmlTag = string.Empty;
            switch (cbl_Layout)
            {
                case Layoutt.Flow:
                    htmlTag = "div";
                    break;
                case Layoutt.Table:
                    htmlTag = "table";
                    break;
            }

            TagBuilder tagBuilder = new TagBuilder(htmlTag);
            tagBuilder.MergeAttributes(htmlAttributes);
            return tagBuilder;
        }

        public static string GenerateHtmlMarkup_CheckBox(KeyValuePair<string, int> item, CheckBoxListSettings settings)
        {
            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", settings.CheckBoxListName);
            tagBuilder.MergeAttribute("value", item.Value.ToString());

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        public static string GenerateHtmlMarkup_Label(KeyValuePair<string, int> item)
        {
            TagBuilder tagBuilder = new TagBuilder("label");

            tagBuilder.SetInnerText(item.Key);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }
        public class CheckBoxListSettings
        {
            public string CheckBoxListName = "SelectedCheckBoxListItems";

            public Layoutt CheckBoxListLayout = Layoutt.Table;
            public Direction CheckBoxListDirection = Direction.Horizontal;
            public RepeatColumns CheckBoxListRepeatColumns = RepeatColumns.SixColumns;
        }
    }
}