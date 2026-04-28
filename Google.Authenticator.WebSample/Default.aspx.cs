using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Google.Authenticator.WebSample
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSecret.Text))
            {
                txtSecret.Text = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            }

            this.lblSecretKey.Text = txtSecret.Text;

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("我 & You", "user@example.com", txtSecret.Text, false, 10);

            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupInfo.ManualEntryKey;

            this.imgQrCode.ImageUrl = qrCodeImageUrl;
            this.lblManualSetupCode.Text = manualEntrySetupCode;
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            TimeSpan tolerance = new TimeSpan(3000);
            var result = tfa.ValidateTwoFactorPIN(txtSecret.Text, this.txtCode.Text, timeTolerance: tolerance);

            if (result)
            {
                this.lblValidationResult.Text = this.txtCode.Text + " is a valid PIN at UTC time " + DateTime.UtcNow.ToString();
                this.lblValidationResult.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                this.lblValidationResult.Text = this.txtCode.Text + " is not a valid PIN at UTC time " + DateTime.UtcNow.ToString();
                this.lblValidationResult.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
