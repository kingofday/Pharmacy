using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Pharmacy.Domain;

namespace Pharmacy.Dashboard
{
    [HtmlTargetElement("nested-view")]
    public class NestedView : TagHelper
    {
        public string TagId { get; set; }
        public string AddText { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string SearchText { get; set; }
        public string ShowAllText { get; set; }
        public List<NestedItem> Items { get; set; }
        public string GetItemsUrl { get; set; }
        public string EditUrl { get; set; }
        public string DeleteUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("nested-view");
            var rootActions = new TagBuilder("div");
            rootActions.AddCssClass("root-actions");
            rootActions.InnerHtml.AppendHtml($"<input id='input-search' type='text' class='form-control' placeholder='{SearchText}' /><div class='root-btns'>");
            rootActions.InnerHtml.AppendHtml($"<button class='btn btn-info' id='btn-search'>{SearchText}</button>");
            if (!string.IsNullOrWhiteSpace(AddText))
                rootActions.InnerHtml.AppendHtml($"<button class='btn btn-primary' id='btn-add-root'>{AddText}</button>");
            rootActions.InnerHtml.AppendHtml($"<button class='btn btn-warning' id='btn-show-all' data-open='false'>{ShowAllText}</button></div>");
            wrapper.InnerHtml.AppendHtml(rootActions);
            wrapper.Attributes.Add("id", TagId);
            wrapper.Attributes.Add("data-add-text", AddText);
            wrapper.Attributes.Add("data-edit-text", EditText);
            wrapper.Attributes.Add("data-edit-url", EditUrl);
            wrapper.Attributes.Add("data-delete-text", DeleteText);
            wrapper.Attributes.Add("data-delete-url", DeleteUrl);
            //var filterRow = new TagBuilder("div");
            //filterRow.AddCssClass("nested-items-filter");
            //var input = new TagBuilder("input");
            //input.AddCssClass("input-search");
            var mainUl = new TagBuilder("ul");
            mainUl.AddCssClass("main-nested-ul");
            TagBuilder Appender(NestedItem currentItem)
            {
                var li = new TagBuilder("li");
                var childs = Items.Where(x => x.ParentId == currentItem.Id).OrderByDescending(x => x.OrderPrority).ToList();
                var hasChild = childs.Any();
                li.InnerHtml.AppendHtml($"<div class='item' data-id='{currentItem.Id}'>{(hasChild ? "<span class='sign'></span>" : string.Empty)}<span class='name'>{currentItem.Name}</span></div>");
                if (hasChild)
                {
                    li.InnerHtml.AppendHtml($"<ul class='submenu'>");
                    foreach (var child in childs)
                        li.InnerHtml.AppendHtml(Appender(child));
                    li.InnerHtml.AppendHtml($"</ul>");
                }
                return li;
            }
            foreach (var item in Items.Where(x => x.ParentId == null).OrderByDescending(x => x.OrderPrority).ToList())
                mainUl.InnerHtml.AppendHtml(Appender(item));
            wrapper.InnerHtml.AppendHtml(mainUl);
            output.Content.AppendHtml(wrapper);
        }
    }
}
