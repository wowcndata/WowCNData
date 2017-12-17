using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Models
{
    public class NavBarModel
    {
        public string Brand;
        public List<LinkModel> SubItems;
    }
    public class LinkModel
    {
        public string Text;
        public string Url;
        public string Active;
        public string Enabled;
        public LinkModel(string text, string url, string active, string enabled)
        {
            Text = text;
            Url = url;
            Active = active;
            Enabled = enabled;
        }
        public LinkModel()
        {

        }
    }
}