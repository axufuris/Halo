using System.Collections;
using System.Web;

namespace Halo.Utilities
{
    public class UrlParameterPasser : BaseParameterPasser
    {
        private SortedList localQueryString = null;

        public UrlParameterPasser() : base() { }
        public UrlParameterPasser(string url) : base(url) { }

        /// <summary>
        /// This will redirect to the URL including the parameters added.
        /// </summary>
        public override void PassParameters()
        {
            if (localQueryString != null && localQueryString.Count > 0)
            {
                bool firstOne = true;

                if (base.Url.IndexOf("?") == -1)
                {
                    base.Url += "?";
                }
                else
                {
                    base.Url += "&";
                }

                foreach (DictionaryEntry o in localQueryString)
                {
                    if (!firstOne)
                    {
                        base.Url += "&";
                    }
                    else
                    {
                        firstOne = false;
                    }

                    base.Url += string.Concat(HttpContext.Current.Server.UrlEncode(o.Key.ToString()), "=",
                                                HttpContext.Current.Server.UrlEncode(o.Value.ToString()));
                }
            }

            base.PassParameters();
        }

        /// <summary>
        /// Gives you the full url.
        /// </summary>
        /// <returns>String of the full URL</returns>
        public string FullUrl()
        {
            if (localQueryString.Count > 0)
            {
                bool firstOne = true;

                if (base.Url.IndexOf("?") == -1)
                {
                    base.Url += "?";
                }
                else
                {
                    base.Url += "&";
                }

                foreach (DictionaryEntry o in localQueryString)
                {
                    if (!firstOne)
                    {
                        base.Url += "&";
                    }
                    else
                    {
                        firstOne = false;
                    }

                    base.Url += string.Concat(HttpContext.Current.Server.UrlEncode(o.Key.ToString()), "=",
                                                HttpContext.Current.Server.UrlEncode(o.Value.ToString()));
                }
            }

            return base.Url;
        }

        public override string this[string name]
        {
            get
            {
                if (localQueryString == null)
                {
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Request.QueryString[name];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return localQueryString[name].ToString();
                }
            }
            set
            {
                if (localQueryString == null)
                {
                    localQueryString = new SortedList();
                }

                if ((localQueryString[name]) == null)
                {
                    localQueryString.Add(name, value);
                }
                else
                {
                    localQueryString[name] = value;
                }
            }
        }

        public override ICollection Keys
        {
            get
            {
                if (localQueryString == null)
                {
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Request.QueryString.Keys;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return localQueryString.Keys;
                }
            }
        }
    }   /// End of Class
}
