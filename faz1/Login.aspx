<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Login.aspx.cs" Inherits="faz1.Login" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <style>
        body {
            background-image: url(vendors/images/map-bg.svg);
            background-repeat: no-repeat;
            background-position: center center;
            background-color: #126cb1;
            background-size: cover;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <div class="container">
            <asp:HiddenField ID="hdn_lblIP" runat="server" />
            <div style="margin-top: 130px" class="row">
                <div class="span4 offset4 well">
                    <legend style="text-align: center"><strong>Faz-1 Projesi </strong></legend>
                    <div runat="server" id="alertMsg" hidden>
                        <a class="close" data-dismiss="alert" href="#">×</a> <a>
                    </div>
                    <form accept-charset="UTF-8">
                        <asp:TextBox ID="txtUserName" CssClass="span4" placeholder="Kullanıcı Adı" runat="server" required />
                        <asp:TextBox TextMode="password" ID="txtPassword" CssClass="span4" placeholder="**********" runat="server" required />
                        <asp:Button ID="btnLogin" CssClass="btn btn-info btn-block" type="submit" OnClick="btnLogin_Click" Text="Giriş" runat="server" />
                        <br />
                        <label class="checkbox">
                            <asp:CheckBox ID="ckbRemember" runat="server" />
                            Beni Hatırla
                        </label>
                        <br />
                        <a href="/Register.aspx" class="pull-right">Kayıt Ol</a>
                    </form>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
