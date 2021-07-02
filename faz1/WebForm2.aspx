<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="faz1.WebForm2" %>

<%@ Register Assembly="DevExpress.Web.v21.1, Version=21.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<script src="https://code.jquery.com/jquery-2.2.4.min.js" integrity="sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44=" crossorigin="anonymous"></script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   
    <form id="form1" runat="server">
         <dx:ASPxGridView OnHtmlRowPrepared="grid_HtmlRowPrepared" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" ID="grid"
            Width="100%" KeyFieldName="ID_USER"  OnRowDeleting="grid_RowDeleting1" runat="server">

            <Columns>
                <dx:GridViewCommandColumn ShowDeleteButton="true" VisibleIndex="0"></dx:GridViewCommandColumn>
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
    </form>
</body>
</html>
