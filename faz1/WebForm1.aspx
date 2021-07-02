<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="faz1.WebForm1" %>
   <%@ Register Assembly="DevExpress.Web.v21.1, Version=21.1.3.0, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="cls-block offset1">


        <%--  <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False"
            KeyFieldName="ID" OnRowUpdating="ASPxGridView1_RowUpdating" Width="588px" OnRowDeleting="ASPxGridView1_RowDeleting" OnRowInserting="ASPxGridView1_RowInserting">
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="True" ShowDeleteButton="True" ShowNewButton="True"/>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Data" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsDetail ShowDetailRow="True" />
            <Templates>
                <DetailRow>
                    <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="ID" OnBeforePerformDataSelect="ASPxGridView2_BeforePerformDataSelect"
                        OnRowUpdating="ASPxGridView1_RowUpdating" Width="100%">
                        <Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="True"/>
                            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="MasterID" ReadOnly="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Data" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </DetailRow>
            </Templates>
        </dx:ASPxGridView>--%>



        <dx:ASPxGridView OnHtmlRowPrepared="grid_HtmlRowPrepared" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" ID="grid"
            Width="100%" KeyFieldName="ID_USER" OnRowDeleting="grid_RowDeleting1" runat="server">

            <Columns>
                <dx:GridViewDataTextColumn FieldName="NAME" Caption="Ad">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LASTNAME" Caption="Soyad">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EMAIL_USER" Caption="Email">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="USERNAME" Caption="Kullanıcı Adı">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>
</asp:Content>
