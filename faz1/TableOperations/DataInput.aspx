<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataInput.aspx.cs" Inherits="faz1.TableOperations.DataInput" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../vendors/customCss.css" rel="stylesheet" />
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
    <div class="span12 offset2" style="width: 75%">
        <div class="panel panel-primary ">
            <div class="panel-heading">
                <h2>Tablo Veri Girişi</h2>
            </div>
            <div class="panel-body">
                <asp:Label Text="Veri girilecek Tablo" runat="server" /><br />
                <br />
                <asp:DropDownList ID="ddlTables" AutoPostBack="true" AppendDataBoundItems="true" OnTextChanged="ddlTables_TextChanged" runat="server">
                    <asp:ListItem Text="Tablo Seçiniz" Value="1" />
                </asp:DropDownList>
                    <a href="#myModal"  visible="false" runat="server" id="btnInfoModal" style="float: right" role="button" class="btn btn-info" data-toggle="modal">Tablo Bilgilerini Göster</a>

                <br />
                <br />
                <br />
                <div class="row-fluid">
                    <asp:GridView ID="grdTable" Visible="false" OnRowDataBound="grdTable_RowDataBound" AutoGenerateColumns="true" CssClass="table table-hover" runat="server">
                    </asp:GridView>
                    <br />
                </div>
                <div class="row-fluid">
                    <asp:Panel Visible="false" CssClass="table offset4" Width="600px" ID="myPel" runat="server">
                    </asp:Panel>
                </div>
            </div>
            <div class="panel-footer">
                <asp:Panel ID="pnlFooterBtns" Visible="false" runat="server">
                    <br />
                    <asp:Button ClientIDMode="static" Text="Kaydet" style="float:right" ID="btnSave" CssClass="btn btn-success" OnClick="btnSave_Click" runat="server" />
                    <br />
                    <br />
                </asp:Panel>
            </div>
        </div>
        <div hidden id="toast">
            <div id="img">Icon</div>
            <div id="desc">A notification message..</div>
        </div>
    </div>
    <script src="../vendors/alert.js"></script>
    <script src="../vendors/alertFunc.js"></script>
</asp:Content>
