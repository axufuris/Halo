using System.Web;

namespace Halo.Utilities
{
    public class SessionParameterPasser : BaseParameterPasser
    {
        public SessionParameterPasser() : base() { }
        public SessionParameterPasser(string url) : base(url) { }

        public override string this[string name]
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session[name].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session[name] = value;
                }
            }
        }

        public override System.Collections.ICollection Keys
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session.Keys;
                }
                else
                {
                    return null;
                }
            }
        }
    }   /// End of Class
}
