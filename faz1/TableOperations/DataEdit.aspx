<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataEdit.aspx.cs" Inherits="faz1.TableOperations.DataEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <link href="../vendors/customCss.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="myModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 runat="server" id="myModalLabel">Tablo Adı</h3>
        </div>
        <div class="modal-body">
            <div class="row-fluid">
                <div class="span6">
                    <label>Oluşturulma Tarihi</label>
                </div>
                 <div class="span6">
                     <asp:Label ID="lblCreateDate" Text="text" runat="server" />
                </div>
            </div>
            <hr />
             <div class="row-fluid">
                <div class="span6">
                    <label>Son Güncelleme Tarihi</label>
                </div>
                 <div class="span6">
                      <asp:Label ID="lblModifyDate" Text="text" runat="server" />
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button class="btn" data-dismiss="modal" aria-hidden="true">Kapat</button>
        </div>
    </div>


    <div class="container offset2" style="width:75%">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h2>Tablo Güncelleme Ekranı</h2>
            </div>
                    <div class="panel-body">
                <div class="row-fluid">
                    <label>Güncellenecek Tablo</label>
                    <asp:DropDownList ID="ddlTable" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged" AutoPostBack="true" runat="server">
                        <asp:ListItem Text="Tablo Seçiniz" Value="1" />
                    </asp:DropDownList>
                    
                       <a href="#myModal" id="modalBtn" visible="false" runat="server" style="float: right; " role="button" class="btn btn-info" data-toggle="modal">Tablo Bilgilerini Göster</a>
                    <br />
                    <br />
                    <br />
                    <br />
                </div>
                         <asp:Panel Visible="false" ID="pnlEdit" runat="server">
                <div style="width: 100%; height: 300px; overflow: scroll;">
                    <asp:GridView CssClass="table table-bordered" ID="TableGridView"
                        OnRowEditing="TableGridView_RowEditing"
                        OnRowCancelingEdit="TableGridView_RowCancelingEdit"
                        OnRowUpdating="TableGridView_RowUpdating"
                        OnRowDeleting="TableGridView_RowDeleting"
                        OnPageIndexChanging="TableGridView_PageIndexChanging"
                        OnRowDataBound="TableGridView_RowDataBound"
                        runat="server" AutoGenerateColumns="false">
                    </asp:GridView>
                </div>
            </div>
            <div class="panel-footer">
            </div>
                   </asp:Panel>
        </div>
    </div>
    <asp:Panel ID="MsgPanel" Visible="false" runat="server">
        <asp:Label ID="msg_lbl" Text="text" runat="server" />
    </asp:Panel>
   
    <script src="../vendors/alert.js"></script>
    <script src="../vendors/alertFunc.js"></script>

</asp:Content>
