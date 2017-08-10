<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="~/LoginPage.aspx.vb" Inherits="VisitelWeb.Visitel.LoginPageControl"
    Theme="redmond" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="~/UserControl/Header.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Footer" Src="~/UserControl/Footer.ascx" %>
<%@ Register TagPrefix="uc3" TagName="UCTopBarLoginPanel" Src="~/UserControl/UCTopBarLoginPanel.ascx" %>
<%@ Register TagPrefix="uc4" TagName="UCSlidingContents" Src="~/UserControl/UCSlidingContents.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Visitel : Login</title>
    <link href="Style/VisitelStyle.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/redmond/thickbox.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="formLogin" runat="server">
    <div id="viewableregion">
        <hr id="header_stripe" />
        <uc3:UCTopBarLoginPanel ID="UCTopBarLoginPanel1" runat="server" />
        <div class="fullHeaderRegion">
            <div id="header" class="displayRegion">
                <uc1:Header ID="htHeaderTitle" runat="server" />
                <marquee behavior="alternate" direction="right"><p><font size="4" face="verdana">This site is under development...</font></p> </marquee>
            </div>
            <div id="PageHeader" class="displayRegion">
                <div class="DivPageHeader">
                    <asp:Label ID="lblPageHeader" runat="server" Text="Login"></asp:Label>
                </div>
                <div class="PageLastUpdate DivPageLastUpdate">
                </div>
                <div class="PageUserName DivPageUserName">
                    <asp:Label ID="lblUser" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
        <div id="PageMainContent" class="displayRegion">
            <div class="DivMainContentError">
                <div id="divErrMesg" class="ui-widget divErrMesg-ui-widget" runat="server">
                    <div class="ui-state-error ui-corner-all" style="padding: 0pt 0.7em;">
                        <span class="ui-icon ui-icon-alert" style="margin-right: 0.3em;"></span>
                        <asp:Label ID="lblErrMsg" runat="server" EnableTheming="false" EnableViewState="false">
                        </asp:Label>
                    </div>
                </div>
                <uc4:UCSlidingContents ID="UCSlidingContents1" runat="server" />
                <div class="SecurityGuidelines DivSecurityGuidelines">
                    <div class="SectionDiv-header SecurityGuidelinesHeader">
                        Security Guidelines</div>
                    <div>
                        <asp:Image ID="Image1" runat="server" CssClass="ImagePath" />
                        Do NOT share your password with anyone else.<br />
                        <br />
                        <asp:Image ID="Image2" runat="server" CssClass="ImagePath" />
                        Do NOT use the save password option on your computer.<br />
                        <br />
                        <asp:Image ID="Image3" runat="server" CssClass="ImagePath" />
                        Do NOT write down your password or reveal it to anyone<br />
                        <br />
                        <asp:Image ID="Image4" runat="server" CssClass="ImagePath" />
                        If your Password has been stolen or compromised, change password immediately.<br />
                        <br />
                        <asp:Image ID="Image5" runat="server" CssClass="ImagePath" />
                        Change your password regularly. We recommend 30 days.<br />
                        <br />
                        <asp:Image ID="Image6" runat="server" CssClass="ImagePath" />
                        Few wrong password retries will lock your User ID
                    </div>
                </div>
                <div class="DivLoginPanel">
                    <asp:Panel ID="Login1" CssClass="login LoginPanel" runat="server" DefaultButton="ButtonLogin">
                        <div class="loginContent">
                            <div class="SectionDiv-header LoginHeaderCaption">
                                Login</div>
                            <div class="loginFields LoginDiv">
                                <div class="newRow">
                                    <div class="DivLoginImage">
                                        <img src="Images/lock_n_key.jpg" class="LoginImage" />
                                    </div>
                                </div>
                                <div class="DivLoginControl">
                                    <div class="newRow DivLabelUserName">
                                        User Name
                                    </div>
                                    <div class="newRow DivUserName">
                                        <asp:TextBox ID="TextBoxUserName" runat="server" CssClass="TextBoxUserName" Text="anjan"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="TextBoxUserName"
                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="newRow">
                                        Password
                                    </div>
                                    <div class="newRow">
                                        <%-- <asp:TextBox ID="TextBoxPassword" runat="server" Width="200Px" TextMode="Password"></asp:TextBox>--%>
                                        <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="TextBoxPassword" Text="abcd@W987"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="TextBoxPassword"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="newRow DivFailureText">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </div>
                                <div class="newRow DivButtonLogin">
                                    <asp:Button ID="ButtonLogin" runat="server" CssClass="ButtonLogin" Text="Log In" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div id="footer" class="displayFooterRegion">
                <uc2:Footer ID="FooterTitle1" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
