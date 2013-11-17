using System.Collections;
using System.Web;

namespace Halo.Utilities
{
    public abstract class BaseParameterPasser
    {
        private string url = string.Empty;

        public abstract string this[string name] { get; set; }

        public abstract ICollection Keys { get; }

        public string Url
        {
            get
            {
                return url.Replace("+", " ");
            }
            set
            {
                url = value;
            }
        }

        public BaseParameterPasser()
        {
            if (HttpContext.Current != null)
            {
                url = HttpContext.Current.Request.Url.ToString();
            }
        }

        public BaseParameterPasser(string passedUrl)
        {
            url = passedUrl;
        }

        public virtual void PassParameters()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Redirect(Url, true);
            }
        }
    }  /// End of Class
}
