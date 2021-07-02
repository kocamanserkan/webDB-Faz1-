<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="faz1.UserDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="bootstrap/js/bootstrap.js"></script>
    <link href="vendors/customCss.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="Server"></asp:ScriptManager>
    <div id="myModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 runat="server" id="myModalLabel">Profil Düzenleme Ekranı</h3>
        </div>
        <div class="modal-body">
            <div class="row-fluid">
                <div class="span6">
                    <label>Ad</label>
                </div>
                <div class="span6">
                    <asp:TextBox ID="txtNewName" runat="server" />
                </div>
            </div>
            <hr />
            <div class="row-fluid">
                <div class="span6">
                    <label>Soyad</label>
                </div>
                <div class="span6">
                    <asp:TextBox ID="txtNewLastName" runat="server" />
                </div>
            </div>
            <hr />
            <div class="row-fluid">
                <div class="span6">
                    <label>Email</label>
                </div>
                <div class="span6">
                    <asp:TextBox TextMode="Email" ID="txtNewEmail" runat="server" />
                </div>
            </div>
            <hr />
        </div>
        <div class="modal-footer">
              <button class="btn" data-dismiss="modal" aria-hidden="true">Kapat</button>
            <asp:Button Text="Güncelle" style="height:auto" class="btn btn-success" OnClick="btnUpdatePerson_Click" ID="btnUpdatePerson" runat="server" />
        </div>
    </div>
    <div class="container">
        <div class="well span8 offset2">
               <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Kullanıcı Bilgisi</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid">
                                <div class="span3">
                                    <img runat="server" ID="imgProfilePhoto" class="img-circle"
                                        src="https://lh5.googleusercontent.com/-b0-k99FZlyE/AAAAAAAAAAI/AAAAAAAAAAA/eu7opA4byxI/photo.jpg?sz=100"
                                        alt="User Pic"><br />
                                    <br />
                                </div>
                                <div class="span6">

                                    <table class="table table-condensed table-responsive table-user-information">
                                        <tbody>
                                            <tr>
                                                <td>Ad:</td>

                                                <td>
                                                    <asp:Label Text="text" ID="lblUserAD" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>Soyad:</td>
                                                <td>
                                                    <asp:Label Text="text" ID="lbluserSoyad" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>E-mail</td>
                                                <td>
                                                    <asp:Label Text="text" ID="lblUserMail" runat="server" /></td>
                                            </tr>
                                               <tr>
                                                <td>Rol</td>
                                                <td>
                                                    <asp:Label Text="text" ID="lblRole" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>Tablo Sayım</td>
                                                <td>
                                                    <asp:Label ID="lblTableCount" Text="text" runat="server" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <br />
                            <a href="#myModal" style="float: right" role="button" class="btn btn-info" data-toggle="modal">Bilgilerimi Güncelle</a>
                            <br />
                            <br />
                                  
                             

                        </div>
                   </div>
        </div>
    </div>
    <script src="vendors/alert.js"></script>
    <script src="vendors/alertFunc.js"></script>
</asp:Content>
