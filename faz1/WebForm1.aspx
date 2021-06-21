<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="faz1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="hero">
                <div class="row-fluid">
                    <div class="span6">
                        <div class="cls-block offset3">
                            <asp:FileUpload runat="server" ID="fileUploadBtn" />
                              <asp:Button ID="btnOpen" OnClick="btnOpen_Click" ClientIDMode="static" Text="Open" CssClass="btn btn-success" runat="server" />
                            <asp:Button  ID="btnClose" ClientIDMode="static" Text="Close" CssClass="btn btn-success" runat="server" />

                            <asp:GridView ID="grdExcel" runat="server"></asp:GridView>

                        </div>
                    </div>
                    <div class="span6">
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
