using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterHashtagSearch
{
    public partial class SiteMaster : MasterPage
    {
        private static readonly string consumerKey = "D8JoOQq4AUw5GiOZXkGUfE8NG";
        private static readonly string consumerSecret = "zXe1MWge8rGcY8pHqBfqrpqspQUlBaMXRYDWOI01G6gsUyw7r6";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SetCredentials();

            InitializeControls();
        }

        private void SetCredentials()
        {
            if (Auth.Credentials == null && Auth.ApplicationCredentials == null)
            {
                var verifyCode = Request["oauth_verifier"];
                var token = Request["oauth_token"];
                var authId = Request["authorization_id"];

                if (string.IsNullOrWhiteSpace(verifyCode) == false && string.IsNullOrWhiteSpace(authId) == false)
                {
                    var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(verifyCode, authId);
                    Auth.SetCredentials(userCredentials);
                }
            }
            else
            {
                var userCredentials = Auth.Credentials ?? Auth.ApplicationCredentials;
                Auth.SetCredentials(userCredentials);
            }
        }

        private void InitializeControls()
        {
            if (Auth.Credentials != null)
            {
                loginLink.Visible = false;
                var userCredentials = Auth.Credentials;
                var authenticatedUser = Tweetinvi.User.GetAuthenticatedUser(userCredentials);

                screenNameLabel.Text = $"@{authenticatedUser.ScreenName}";
                userImage.ImageUrl = authenticatedUser.ProfileImageUrl;
                userImage.Visible = true;

            }
            else
            {
                loginLink.Visible = true;
                screenNameLabel.Text = string.Empty;
                userImage.ImageUrl = string.Empty;
                userImage.Visible = false;
            }
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            var appCreds = new ConsumerCredentials(consumerKey, consumerSecret);
            
            var redirect = $"http://{Request.Url.Authority}";

            var authenticationContext = AuthFlow.InitAuthentication(appCreds, redirect);

            HttpContext.Current.Response.Redirect(authenticationContext.AuthorizationURL);
        }

        protected void logoutLink_Click(object sender, EventArgs e)
        {
            Auth.Credentials = null;
            Auth.ApplicationCredentials = null;
            
            Session.RemoveAll();
            Response.Redirect("#");
        }
    }
}