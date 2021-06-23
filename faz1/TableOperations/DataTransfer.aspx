<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataTransfer.aspx.cs" Inherits="faz1.TableOperations.DataTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <link href="../vendors/customCss.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container cls-block offset2" style="width: 75%">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h2>Excel Veri Transferi</h2>
            </div>
            <div class="panel-body">
                <div class="row-fluid">
                    <div class="span12">
                        <label>Veri Transfer Edilecek Tablo</label>
                        <asp:DropDownList ID="ddlSelectedTable" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSelectedTable_SelectedIndexChanged" runat="server">
                            <asp:ListItem Text="Tablo Seçiniz" Value="1" />
                        </asp:DropDownList>
                    </div>

                    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
                    <%--                        <asp:Button Text="Transfer Et" CssClass="btn" ID="btnTransfer" OnClick="btnTransfer_Click" runat="server" />--%>
                </div>
            </div>
        </div>
        <div class="panel panel-primary">
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class=" cls-block span12">
                    <asp:Panel Visible="false" ID="pnlGrd" runat="server">
                        <h2>
                            <asp:Label ID="lblTableName" Text="text" runat="server" /></h2>
                        <asp:GridView OnRowDataBound="grdTable_RowDataBound" CssClass="table table-hover" ID="grdTable" runat="server">
                        </asp:GridView>

                    </asp:Panel>
                </div>
            </div>
            <asp:Panel Visible="false" ID="pnlExcelUpload" runat="server">
                <div class="row-fluid">
                    <div class=" cls-block span12">
                        <h2>Excel Verisi</h2>
                       
                        <asp:GridView ID="grdExcel" CssClass="table table-hover" runat="server" OnRowDataBound="grdExcel_RowDataBound">
                        </asp:GridView>

                        <asp:UpdatePanel ID="up1" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:FileUpload ID="fuImgPath" CssClass="btn" runat="server" />
                                <asp:Button ID="btnUpload" CssClass="btn btn-info" runat="server" Text="Yükle" OnClick="btnTransfer_Click" />
                                <br />

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                         <asp:DropDownList Visible="false" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlExcelSheets_SelectedIndexChanged" ID="ddlExcelSheets" runat="server">
                          
                        </asp:DropDownList>

                        <asp:Label Style="float: right" ID="lblMsg" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Button Visible="false" OnClick="btnFinal_Click" ID="btnFinal" Style="float: right" Text="Aktar" CssClass="btn btn-success" runat="server" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>


    <script src="../vendors/alert.js"></script>
    <script src="../vendors/alertFunc.js"></script>
</asp:Content>
