<%@ Page Title="Admin Paneli" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelofAdmin.aspx.cs" Inherits="faz1.Administration.PanelofAdmin" %>

<%@ Register Assembly="DevExpress.Web.v21.1, Version=21.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <link href="../vendors/customCss.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <style type="text/css">
        .input-icons i {
            position: absolute;
        }

        .input-icons {
            width: 100%;
        }

        .icon {
            padding: 10px;
            min-width: 40px;
        }

        .input-field {
            width: 80%;
        }


        .auto-style1 {
            margin-right: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        function myFunction(s, e) {
            pnlCB.PerformCallback();
            var command = e.command;
            if (command === "UPDATEEDIT") {

                alertMsg('Kayıt Güncellendi', 'yes')

            }
            else if (command === "DELETEROW") {

                alertMsg('Kayıt Silindi', 'yes')
            }

        }
        function okFunc(s, e) {
            alert(e.result);
        }

        $(document).ready(function () {
            var delay = (function () {
                var timer = 0;
                return function (callback, ms) {
                    clearTimeout(timer);
                    timer = setTimeout(callback, ms);
                };
            })();


            $('#txtUserName').keyup(function () {
                delay(function () {
                    if ($('#txtUserName').val() != '') {
                        ShowDetails();

                    }
                    else {
                        $(".fa").remove();
                    }

                }, 1200);
            });
        });


        function ShowDetails() {
            var checkUserName = $('#txtUserName').val();
            Callback1.PerformCallback(checkUserName);
        }

        jQuery.fn.insertAt = function (index, element) {
            var lastIndex = this.children().size();
            if (index < 0) {
                index = Math.max(0, lastIndex + 1 + index);
            }
            this.append(element);
            if (index < lastIndex) {
                this.children().eq(index).before(this.children().last());
            }
            return this;
        }

        function OnCallbackComplete(s, e) {

            var detailsContainer = $('#txtResult')
            var username = $('#txtUserName')
            var divResult = $('#divResult')
            /* detailsContainer.html(e.result)*/
            var no = '<i style="margin-left: 160px; margin-top: 6px; color: red;" class="fa fa-ban" aria-hidden="true"></i>'
            var yes = '<i style="margin-left: 160px; margin-top: 6px;color: green;" class="fa fa-check" aria-hidden="true"></i>'
            if (e.result === "ok") {
                $(".fa").remove();
                $("#divResult").insertAt(0, yes);
            }
            else {
                $(".fa").remove();
                alert('Kullanıcı Adı Müsait Değil ' + username.val() + '')
                $("#divResult").insertAt(0, no);
            }
        }

        $(document).ready(function () {
            $('#btnInfoModal').click(function () {
                var ddl = $('#ddlRole');
                pnlCB.PerformCallback();
            });
        });

        $(document).ready(function () {

            $('#ddlRole').change(function () {

                alert('asd');


                var myHidden = document.getElementById('<%= hfDDLSelected.ClientID %>');
                myHidden.value = this.value

                /*alert(myHidden.value)*/
                /*alert(this.value);*/
                /*  hf.val(this.value);*/
                /*   alert(myHidden.value)*/

            })
        });

        function ddlEndCallBack() {
            $('#ddlRole').change(function () {

                var myHidden = document.getElementById('<%= hfDDLSelected.ClientID %>');
                myHidden.value = this.value


            })
        }

    </script>

    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback1"
        OnCallback="ASPxCallback1_Callback">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>

  

    <div id="myModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 runat="server" id="myModalLabel">Yeni Kullanıcı Ekle</h3>
        </div>
        <div style="margin-top: 20px; margin-bottom: 20px; margin-left: 15px" class="modal-body">

            <div class="row-fluid">
                <div class="span6">
                    Adı
                    <br />
                    <asp:TextBox required="true" ID="txtName" runat="server" />
                </div>
                <div class="span6">
                    Kullanıcı Adı 
                    <br />
                    <label id="txtResult"></label>
                    <div id="divResult" class="input-icons">
                        <asp:TextBox required="true" ID="txtUserName" CssClass="input-field" ClientIDMode="static" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span6">
                    Soyadı
                    <br />
                    <asp:TextBox required="true" ID="txtLastName" runat="server" />
                </div>
                <div class="span6">
                    Email
                    <br />
                    <asp:TextBox TextMode="Email" ID="txtEmail" runat="server" />
                </div>
            </div>
            <div class="row-fluid">
                <div class="span6">
                    Rolü
                    <br />
                    <dx:ASPxCallbackPanel ID="clbPanelDDL" ClientInstanceName="pnlCB" OnCallback="clbPanelDDL_Callback" runat="server" Width="200px">
                        <ClientSideEvents EndCallback="ddlEndCallBack" />
                        <PanelCollection>
                            <dx:PanelContent runat="server">
                                <asp:DropDownList ID="ddlRole" CssClass="required" ClientIDMode="static" runat="server">
                                </asp:DropDownList>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>

                </div>
                <div class="span6">
                    Parola
                    <br />
                    <asp:TextBox required="true" ID="txtPass" TextMode="Password" runat="server" />
                </div>
            </div>

        </div>
        <div class="modal-footer">
            <dx:ASPxCheckBox ID="checkSendMail" Style="float: left" Text="Onay Maili Gönderilsin" runat="server"></dx:ASPxCheckBox>
            <button class="btn" data-dismiss="modal" aria-hidden="true">Kapat</button>
            <dx:ASPxButton ID="btnSaveNewUser" Text="Kaydet" CssClass="btn btn-success" OnClick="btnSaveNewUser_Click" runat="server"></dx:ASPxButton>
        </div>
    </div>
    <div class="container cls-block offset2">
        <asp:HiddenField runat="server" ID="hfDDLSelected" ClientIDMode="static" />
        <br />
        <dx:ASPxGridView OnDataBound="grdUsers_DataBound" ID="grdUsers" ClientIDMode="Static" KeyFieldName="ID_USER"
            Width="100%" runat="server" Theme="Moderno"
            OnRowUpdating="grdUsers_RowUpdating" OnRowDeleting="grdUsers_RowDeleting"
            OnHtmlDataCellPrepared="grdUsers_HtmlDataCellPrepared" AutoGenerateColumns="False" CssClass="auto-style1">
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            <SettingsBehavior AllowSort="true" AllowDragDrop="false" ConfirmDelete="true" />
            <SettingsCommandButton>
                <DeleteButton Text="Sil"></DeleteButton>
                <EditButton Text="Düzenle"></EditButton>
                <NewButton Text="Yeni"></NewButton>
                <UpdateButton Text="Güncelle"></UpdateButton>
                <CancelButton Text="Vazgeç"></CancelButton>
                <ClearFilterButton Text="Temizle"></ClearFilterButton>
            </SettingsCommandButton>
            <SettingsText  ConfirmDelete="Kayıt Silinmek Üzere Onaylıyor Musunuz?" EmptyDataRow="Görüntülenecek Kayıt Yok" />
            <StylesPager >

            </StylesPager>
            <Columns>
                <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" VisibleIndex="0" ShowClearFilterButton="True">
                    <HeaderCaptionTemplate>
                        <a href="#myModal" id="btnInfoModal" role="button" style="color: white;" class="btn btn-success" data-toggle="modal">Yeni Kişi Ekle</a>
                    </HeaderCaptionTemplate>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="NAME" VisibleIndex="1" Caption="Ad" SortIndex="1" SortOrder="Ascending">
                    <EditFormSettings VisibleIndex="0" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LASTNAME" VisibleIndex="2" Caption="Soyad">
                    <EditFormSettings VisibleIndex="1" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EMAIL_USER" VisibleIndex="3" Caption="Email">
                    <EditFormSettings VisibleIndex="2" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="USERNAME" VisibleIndex="4" Caption="Kullanıcı Adı">
                    <EditFormSettings VisibleIndex="3" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn FieldName="ROLE" PropertiesComboBox-TextField="TEXTROLES" VisibleIndex="5" Caption="Rol">
                    <EditFormSettings VisibleIndex="4" />
                </dx:GridViewDataComboBoxColumn>
            </Columns>
            <ClientSideEvents EndCallback="myFunction" />
            <SettingsLoadingPanel Text="Yükleniyor" />
        </dx:ASPxGridView>
    </div>
    <script>

</script>
    <script src="../vendors/alert.js"></script>
    <script src="../vendors/alertFunc.js"></script>
</asp:Content>
