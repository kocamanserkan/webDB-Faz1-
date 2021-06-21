<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Register.aspx.cs" Inherits="faz1.Register" %>

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
                    <legend style="text-align: center"><strong>Kullanıcı Kayıt Formu</strong> </legend>

                    <div runat="server" id="alertMsg" hidden class="alert alert-success">
                        <a class="close" data-dismiss="alert" href="#">×</a><a>
                    </div>
                 
                    <form accept-charset="UTF-8">
                        <asp:TextBox ID="txtName" CssClass="span4" placeholder="Ad*" runat="server" required />
                        <asp:TextBox ID="txtLastName" CssClass="span4" placeholder="Soyad*" runat="server" required />
                        <asp:TextBox ID="txtEmail" TextMode="email" CssClass="span4" placeholder="E-mail*" runat="server" required />
                        <asp:TextBox ID="txtUserName" CssClass="span4" placeholder="Kullanıcı Adı*" runat="server" required />
                        <asp:TextBox TextMode="password" ID="txtPassword" CssClass="span4" placeholder="Parola*" runat="server" required />
                        <asp:TextBox TextMode="password" ID="txtConfirmPassword" CssClass="span4" placeholder="Parola Tekrar*" runat="server" required />
                        <br />
                        <div class="registrationFormAlert" style="color: red;" id="CheckPasswordMatch"></div>
                        <%--<button type="submit" name="submit" class="btn btn-info btn-block">Giriş!</button><br />--%>
                        <asp:Button ID="btnRegister" OnClick="btnRegister_Click" CssClass="btn btn-info btn-block" type="submit" Text="Kayıt Ol" runat="server" />
                        <br />
                        <br />
                        <a href="/Login.aspx" class="pull-right">Giriş Ekranı</a>
                    </form>
                </div>
            </div>
        </div>
        <script>
            // Password check operations
            function checkPasswordMatch() {
                var password = $("#txtPassword").val();
                var confirmPassword = $("#txtConfirmPassword").val();
                if (password != confirmPassword) {
                    $("#CheckPasswordMatch").html("Parolalar uyuşmuyor!");
                }
                else if (password == confirmPassword) {
                    $("#CheckPasswordMatch").html("");
                }

            }
            $(document).ready(function () {
                var confirmPassword = $("#txtConfirmPassword")
                confirmPassword.keyup(checkPasswordMatch)
                var password = $("#txtPassword")
                password.keyup(checkPasswordMatch)

            });
        </script>
    </form>
</body>
</html>
