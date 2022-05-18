namespace Atrasti.Utils
{
    public static class EmailUtils
    {
        public static string ConfirmAccountTemplate(this string registerToken)
        {
            return $@"
<table width=""100%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
   <!-- START HEADER/BANNER -->
   <tbody>
      <tr>
         <td align=""center"">
            <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
               <tbody>
                  <tr>
                     <td align=""center"" valign=""top"" style=""background-color: #343a40"">
                     <table class=""col-600"" width=""600"" height=""200"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
                        <tbody>
                           <tr>
                              <td height=""40""></td>
                           </tr>
                           <tr>
                              <td align=""center"" style=""line-height: 0px;"">
                                 <img style=""display:block; line-height:0px; font-size:0px; border:0px;"" src=""https://www.atrasti.com/img/atrasti_logo.png"" width=""118"" height=""27"" alt=""logo"">
                              </td>
                           </tr>
                           <tr>
                              <td height=""50""></td>
                           </tr>
                        </tbody>
                     </table>
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
      <!-- END HEADER/BANNER -->
      <!-- START 3 BOX SHOWCASE -->
      <tr>
         <td align=""center"">
            <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""margin-left:20px; margin-right:20px; border-left: 1px solid #dbd9d9; border-right: 1px solid #dbd9d9;"">
               <tbody>
                  <tr>
                     <td height=""35""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Raleway', sans-serif; font-size:22px; font-weight: bold; color:#2a3a4b;"">Your registration was successful!</td>
                  </tr>
                  <tr>
                     <td height=""10""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Lato', sans-serif; font-size:14px; color:#757575; line-height:24px; font-weight: 300; padding: 0 10px 0 10px"">
                        We're excited to have you get started. First, you need to confirm your account. Just press the button below.
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
            <tr>
         <td align=""center"">
            <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""margin-left:20px; margin-right:20px; border-left: 1px solid #dbd9d9; border-right: 1px solid #dbd9d9;"">
               <tbody>
                  <tr>
                     <td height=""35""></td>
                  </tr>
                  <tr>
                     <td align=""center"" height=""30"" style=""font-family: 'Open Sans', Arial, sans-serif; font-size:13px; color: #ffffff;text-decoration: none;height: 30px;background: #29B6F6;display: inline-block;text-align: center;line-height: 30px;width: 40%;margin-left:30%;"">
                        <a href=""{registerToken}"" style=""color:#ffffff; text-decoration: none"">Confirm Account</a>
                     </td>
                  </tr>
                  <tr>
                     <td height=""10""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Lato', sans-serif; font-size:14px; color:#757575; line-height:24px; font-weight: 300; padding: 0 10px 0 10px"">
                        If that doesn't work, copy and paste the following link in your browser:
                     </td>
                  </tr>
                  <tr>
                     <td height=""10""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Lato', sans-serif; font-size:14px; color:#757575; line-height:24px; font-weight: 300; padding: 0 10px 0 10px;max-width: 580px"">
                        {registerToken}
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
      <tr>
         <td height=""5""></td>
      </tr>
      <!-- END 3 BOX SHOWCASE -->
      <!-- START READY FOR NEW PROJECT -->
      <tr>
         <td align=""center"">
            <table align=""center"" width=""600"" border=""0"" cellspacing=""0"" cellpadding=""0"" style="" border-left: 1px solid #dbd9d9; border-right: 1px solid #dbd9d9;"">
               <tbody>
                  <tr>
                     <td height=""50""></td>
                  </tr>
                  <tr>
                     <td align=""center"" bgcolor=""#34495e"">
                        <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
                           <tbody>
                              <tr>
                                 <td height=""35""></td>
                              </tr>
                              <tr>
                                 <td align=""left"" style=""font-family: 'Lato', sans-serif; font-size:12px; color:#fff; line-height: 1px; font-weight: 300; padding: 0 35px 0 35px"">
                                    Atrasti. A part of Imesum © 2020 Copyright. All rights reserved. 
                                 </td>
                              </tr>
                              <tr>
                                 <td height=""40""></td>
                              </tr>
                           </tbody>
                        </table>
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
      <!-- END READY FOR NEW PROJECT -->						
   </tbody>
</table>";
        }
        
                public static string ResetPasswordTemplate(this string resetToken)
        {
            return $@"
<table width=""100%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
   <!-- START HEADER/BANNER -->
   <tbody>
      <tr>
         <td align=""center"">
            <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
               <tbody>
                  <tr>
                     <td align=""center"" valign=""top"" style=""background-color: #343a40"">
                     <table class=""col-600"" width=""600"" height=""200"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
                        <tbody>
                           <tr>
                              <td height=""40""></td>
                           </tr>
                           <tr>
                              <td align=""center"" style=""line-height: 0px;"">
                                 <img style=""display:block; line-height:0px; font-size:0px; border:0px;"" src=""https://www.atrasti.com/img/atrasti_logo.png"" width=""118"" height=""27"" alt=""logo"">
                              </td>
                           </tr>
                           <tr>
                              <td height=""50""></td>
                           </tr>
                        </tbody>
                     </table>
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
      <!-- END HEADER/BANNER -->
      <!-- START 3 BOX SHOWCASE -->
      <tr>
         <td align=""center"">
            <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""margin-left:20px; margin-right:20px; border-left: 1px solid #dbd9d9; border-right: 1px solid #dbd9d9;"">
               <tbody>
                  <tr>
                     <td height=""35""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Raleway', sans-serif; font-size:22px; font-weight: bold; color:#2a3a4b;"">Reset your password!</td>
                  </tr>
                  <tr>
                     <td height=""10""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Lato', sans-serif; font-size:14px; color:#757575; line-height:24px; font-weight: 300; padding: 0 10px 0 10px"">
                        Just press the button below and you'll be on your way to regain access to your account.
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
            <tr>
         <td align=""center"">
            <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""margin-left:20px; margin-right:20px; border-left: 1px solid #dbd9d9; border-right: 1px solid #dbd9d9;"">
               <tbody>
                  <tr>
                     <td height=""35""></td>
                  </tr>
                  <tr>
                     <td align=""center"" height=""30"" style=""font-family: 'Open Sans', Arial, sans-serif; font-size:13px; color: #ffffff;text-decoration: none;height: 30px;background: #29B6F6;display: inline-block;text-align: center;line-height: 30px;width: 40%;margin-left:30%;"">
                        <a href=""{resetToken}"" style=""color:#ffffff; text-decoration: none"">Reset password</a>
                     </td>
                  </tr>
                  <tr>
                     <td height=""10""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Lato', sans-serif; font-size:14px; color:#757575; line-height:24px; font-weight: 300; padding: 0 10px 0 10px"">
                        If that doesn't work, copy and paste the following link in your browser:
                     </td>
                  </tr>
                  <tr>
                     <td height=""10""></td>
                  </tr>
                  <tr>
                     <td align=""center"" style=""font-family: 'Lato', sans-serif; font-size:14px; color:#757575; line-height:24px; font-weight: 300; padding: 0 10px 0 10px;max-width: 580px"">
                        {resetToken}
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
      <tr>
         <td height=""5""></td>
      </tr>
      <!-- END 3 BOX SHOWCASE -->
      <!-- START READY FOR NEW PROJECT -->
      <tr>
         <td align=""center"">
            <table align=""center"" width=""600"" border=""0"" cellspacing=""0"" cellpadding=""0"" style="" border-left: 1px solid #dbd9d9; border-right: 1px solid #dbd9d9;"">
               <tbody>
                  <tr>
                     <td height=""50""></td>
                  </tr>
                  <tr>
                     <td align=""center"" bgcolor=""#34495e"">
                        <table class=""col-600"" width=""600"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"">
                           <tbody>
                              <tr>
                                 <td height=""35""></td>
                              </tr>
                              <tr>
                                 <td align=""left"" style=""font-family: 'Lato', sans-serif; font-size:12px; color:#fff; line-height: 1px; font-weight: 300; padding: 0 35px 0 35px"">
                                    Atrasti. A part of Imesum © 2020 Copyright. All rights reserved. 
                                 </td>
                              </tr>
                              <tr>
                                 <td height=""40""></td>
                              </tr>
                           </tbody>
                        </table>
                     </td>
                  </tr>
               </tbody>
            </table>
         </td>
      </tr>
      <!-- END READY FOR NEW PROJECT -->						
   </tbody>
</table>";
        }
    }
}