using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Sistema_BD_Clinica_Patologica
{
    public class CustomContentLoader : INavigationContentLoader
    {
        private String loginPage, secureFolder;

        private PageResourceContentLoader loader = new PageResourceContentLoader();

        public IAsyncResult BeginLoad(Uri targetUri, Uri currentUri, AsyncCallback userCallback, object asyncState)
        {
            Uri oldUri = targetUri;
            if (!App.UserIsAuthenticated)
            {
                // Redirect the request to the login page.
                targetUri = new Uri(LoginPage, UriKind.Relative);
                /*
                if ((System.IO.Path.GetDirectoryName(targetUri.ToString()).Trim('\\') ==
                SecuredFolder) && (targetUri.ToString() != LoginPage))
                {
                    // Redirect the request to the login page.
                    targetUri = new Uri(LoginPage, UriKind.Relative);
                }
                */
            }


            return loader.BeginLoad(targetUri, currentUri, userCallback, asyncState);
        }

        public bool CanLoad(Uri targetUri, Uri currentUri)
        {
            return loader.CanLoad(targetUri, currentUri);
        }

        public void CancelLoad(IAsyncResult asyncResult)
        {
            loader.CancelLoad(asyncResult);
        }

        public LoadResult EndLoad(IAsyncResult asyncResult)
        {
            return loader.EndLoad(asyncResult);
        }

        public String LoginPage
        {
            get { return loginPage; }
            set { loginPage = value; }
        }

        public String SecuredFolder
        {
            get { return secureFolder; }
            set { secureFolder = value; }
        }
    }
}
