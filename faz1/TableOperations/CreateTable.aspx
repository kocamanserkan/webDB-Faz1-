<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CreateTable.aspx.cs" Inherits="faz1.TableOperations.CreateTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <link href="../vendors/customCss.css" rel="stylesheet" />
    <style>
        #ckbBos {
            box-shadow: inherit !important;
        }

        #cbNew {
            box-shadow: inherit !important;
        }

        #column-list {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 100%;
        }

            #column-list li {
                margin: 0 3px 3px 3px;
                padding: 0.4em;
                padding-left: 1.5em;
                font-size: 1.4em;
                height: 18px;
            }

        .vl {
            position: absolute;
            border-left: 6px solid #808080;
            height: 90%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cls-block offset2" style="width: 70%">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Kolon Bilgileri</h3>
            </div>
            <div class="panel-body">
                <div class="row-fluid">
                    <form id="myForm">
                        <div class="span12">
                            <div class="row-fluid">
                                <label for="txttb">Tablo Adı*</label>
                                <asp:TextBox ClientIDMode="static" MaxLength="40" ID="txtTableName" Width="250px" placeholder="Tablo Adı" CssClass="input-small" runat="server" />
                              
                                <hr />
                            </div>
                            <br />
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>Kolon Adı*</th>
                                        <th>Kolon Veri Tipi</th>
                                        <th>Boş Geçilebilir</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th>
                                            <asp:TextBox MaxLength="20" ID="txtKolonAd" ClientIDMode="static" placeholder="Kolon Adı" CssClass="input-small" runat="server" /><br />
                                            <th>
                                                <asp:DropDownList ClientIDMode="static" Width="150px" CssClass="input-small" ID="ddlDataType" runat="server">
                                                    <asp:ListItem Value="string" Text="Metin" />
                                                    <asp:ListItem Value="int" Text="Tamsayı" />
                                                    <asp:ListItem Value="decimal" Text="Ondalık" />
                                                    <asp:ListItem Value="datetime" Text="Tarih ve Saat" />
                                                </asp:DropDownList>
                                            </th>
                                        <th>
                                            <asp:CheckBox Checked="true" ClientIDMode="static" Style="box-shadow: inherit" ID="ckbBos" runat="server" />
                                        </th>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:Button ClientIDMode="static" data-toggle="tooltip" OnClientClick="return validateColumn()" title="first tooltip" Text="Ekle" OnClick="btnAddColumn_Click" ID="btnAddColumn" CssClass="btn btn-success" runat="server" />
                        </div>
                    </form>
                    <div class="span6">
                    </div>
                </div>
            </div>
            <div class="panel-footer">
            </div>
        </div>
        <div id="gridPanel" class="panel panel-primary">
            <div class="panel-heading">
                <div class="row-fluid">
                    <div class="span6">
                        <h3 class="panel-title">Eklenen Kolonlar</h3>
                    </div>
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span12">
                                <asp:Button ID="btnDeleteAllColumns" ClientIDMode="static" OnClientClick="return deleteAllColumns()" Style="float: right" OnClick="btnDeleteAllColumns_Click" Text="Tüm Kolonları Temizle" CssClass="btn btn-danger" runat="server" />
                                <%--                                <button style="float: right" class="btn btn-danger" type="submit"><i class="icon-trash"></i>Hepsini Sil</button>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="cls-block">
                    <asp:GridView ID="grdMyTable" ClientIDMode="static" OnRowDataBound="grdMyTable_RowDataBound" OnRowUpdating="grdMyTable_RowUpdating" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" OnRowEditing="grdMyTable_RowEditing1">
                        <Columns>
                            <asp:TemplateField HeaderText="Düzen">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditGrid" ClientIDMode="static" CssClass="btn btn-warning" Text="Düzenle" runat="server" CommandName="Edit" />
                                    <asp:LinkButton ID="btnDelGridCol" ClientIDMode="static" CssClass="btn btn-danger" Text="Sil" runat="server" OnClick="OnDelete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ClientIDMode="static" ID="update" CssClass="btn btn-success" OnClientClick="return validateGrid(this)" Text="Güncelle" runat="server" CommandName="Update" />
                                    <asp:LinkButton CssClass="btn btn-danger" Text="Vazgeç" runat="server" OnClick="OnCancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Kolon Adı">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_KolonAd" runat="server" Text='<%#Eval("kolonAdi") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_KolonAd" CssClass="bu" ToolTip="Kolon Adı" ClientIDMode="static" Style="border-color: red;" runat="server" Width="80px" Text='<%#Eval("kolonAdi") %>'></asp:TextBox>

                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Kolon Veri Tipi">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_KolonData" runat="server" Text='<%#Eval("kolonData") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList Style="border-color: red;" runat="server" ID="ddlDataTypeNew" Width="160px">
                                        <asp:ListItem Value="string" Text="Metin" />
                                        <asp:ListItem Value="int" Text="Tamsayı" />
                                        <asp:ListItem Value="decimal" Text="Ondalık" />
                                        <%--    <asp:ListItem Value="date" Text="Tarih ve Saat" />--%>
                                        <asp:ListItem Value="datetime" Text="Tarih ve Saat" />
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Boş Bırakılabilirlik">
                                <ItemTemplate>
                                    <asp:Label ID="lblNullable" runat="server" Text='<%#Eval("nullable") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox Style="border-color: red; box-shadow: inherit" ClientIDMode="static" ID="cbNew" runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="panel-footer">
                <br />
                <asp:Button Style="float: right" OnClientClick="return IsGridValid();" ID="btnOnayla" ClientIDMode="static" OnClick="btnOnayla_Click" Text="Onayla" CssClass="btn btn-success" runat="server" />
                <br />
                <br />
            </div>
        </div>

    </div>
    <script src="../vendors/alert.js"></script>
    <script src="../vendors/alertFunc.js"></script>
    <script type="text/javascript">


        $('#btnOnayla').click(function () {

            var tableName = $('#txtTableName').val();
            var numericReg = /^[a-zA-Z_][a-zA-Z0-9_]*$/;
            if (!numericReg.test(tableName)) {
                /* alert('Tablo adınızı kontrol ediniz');*/

                return false;
            }

        })


        $('#txtTableName').keyup(function () {
            $('span.error-keyup-1').hide();
            var inputVal = $(this).val();
            var numericReg = /^[a-zA-Z_][a-zA-Z0-9_]*$/;
            if (!numericReg.test(inputVal) && inputVal != '') {
                $(this).after('<span style="color:red" class="error error-keyup-1">Hatalı Tablo Adı </span>');
                $('#btnAddColumn').attr('disabled', 'disabled')
                $('#txtKolonAd').attr('disabled', 'disabled')
                //$('#btnDeleteAllColumns').attr('disabled', 'disabled')
                //$('#btnOnayla').attr('disabled', 'disabled')
                //$('#btnEditGrid').attr('style', 'display:none')
                //$('#btnDelGridCol').attr('style', 'display:none')
                $('#gridPanel').attr('style', 'display:none')

            } else {
                $('#btnAddColumn').removeAttr('disabled');
                $('#txtKolonAd').removeAttr('disabled');
                //$('#btnDeleteAllColumns').removeAttr('disabled');
                //$('#btnOnayla').removeAttr('disabled');
                //$('#btnEditGrid').removeAttr('style');
                //$('#btnDelGridCol').removeAttr('style');
                $('#gridPanel').removeAttr('style');

            }
        });

        $('#txtKolonAd').keyup(function () {

            $('span.error-keyup-1').hide();
            var inputVal = $(this).val();
            var numericReg = /^[a-zA-Z_][a-zA-Z0-9_]*$/;
            if (!numericReg.test(inputVal) && inputVal != '') {
                $(this).after('<span style="color:red" class="error error-keyup-1">Hatalı Kolon Adı </span>');
                $('#btnAddColumn').attr('disabled', 'disabled')
                $('#txtTableName').attr('disabled', 'disabled')
                $('#gridPanel').attr('style', 'display:none')

            } else {
                $('#btnAddColumn').removeAttr('disabled');
                $('#txtTableName').removeAttr('disabled');
                $('#gridPanel').removeAttr('style');
            }

        });
        function columnIsExist() {

            var texttocheck = '';
            var kolonName = document.getElementById("txtKolonAd").value;
            $("#grdMyTable tr td").each(function () {
                texttocheck += this.innerText.trim();

            });
            if (kolonName.length > 0) {

                if (texttocheck.toLowerCase().includes(kolonName.toLowerCase())) {


                    alert('Eklemek istediğiniz kolon mevcut. (' + kolonName + ')');
                    return false;
                }

                else {
                    return true;
                }
            }
            else {
                alertMsg('Kolon adı boş bırakılamaz', 'no');
                /*alert("Kolon adı boş bırakılamaz");*/
                return false;
            }
        }
        function IsGridValid() {

            var texttocheck = '';
            var errorMsg = [];
            var tableName = document.getElementById("txtTableName").value;
            $("#grdMyTable tr td").each(function () {
                texttocheck += this.innerText.trim();

            });
            if (tableName === '') {
                let msg = 'Tablo adı boş bırakılamaz. \n';
                errorMsg.push(msg);

            }
            else {
                var numericReg = /^[a-zA-Z_][a-zA-Z0-9_]*$/;
                if (!numericReg.test(tableName)) {
                    alertMsg('Tablo adınızı kontrol ediniz', 'no');
                  /*  alert('Tablo adınızı kontrol ediniz');*/

                    return false;
                }
            }

            if (texttocheck.length == 0) {

                let msg = 'Tablo oluşturmak için kolon eklemeliniz.\n';
                errorMsg.push(msg);
            }
            if (errorMsg.length == 0) {
                return true;
            }
            else {
                var alertMssg = '';
                for (var i = 0; i < errorMsg.length; i++) {
                    alertMssg += errorMsg[i] + ' ';
                }
                alertMsg(alertMssg, 'no');
               /* alert(alertMssg);*/
                return false;
            }


        }
        function deleteAllColumns() {

            var texttocheck = '';
            var errorMsg = [];
            var tableName = document.getElementById("txtTableName").value;
            $("#grdMyTable tr td").each(function () {
                texttocheck += this.innerText.trim();
            });
            if (texttocheck.length == 0) {
                alert('Kolon eklemelisiniz.')
                return false;

            }
            else {

                return confirm('Tüm kolonları silmek üzeresiniz. Onaylıyor musunuz?')
            }
        }

        function validateColumn() {
            var kolonName = document.getElementById("txtKolonAd").value;
            if (kolonName === "") {
               /* alert("Kolon Adı Boş geçilemez.");*/
                alertMsg('Kolon Adı Boş Geçilemez.', 'no');
                return false;
            }
            else {
                return true;
            }
        }
        function validateGrid(ctrl) {

            $('span.error-keyup-1').hide();

            var numericReg = /^[a-zA-Z_][a-zA-Z0-9_]*$/;

            var grdRow = ctrl.parentNode.parentNode;
            var grdControl = grdRow.getElementsByTagName("input");
            // edit için İçeride dönen
            for (var i = 0; i < grdControl.length; i++) {
                if (grdControl[i].empid = 'txt_KolonAd') {
                    if (grdControl[i].value === "") {
                        alertMsg(grdControl[i].title + ' zorunludur.', 'no');
                    /*    alert(grdControl[i].title + ' zorunludur.');*/
                        return false;
                    }
                    else if (!numericReg.test(grdControl[i].value) ) {

                        alertMsg('Hatalı Kolon Güncellemesi. Kolon Adınızı Kontrol Ediniz' , 'no');

                        return false;
                    }

                    else {

                        var texttocheck = '';
                        $("#grdMyTable tr td").each(function () {
                            texttocheck += this.innerText.trim();
                        });
                        if (grdControl[i].value.length > 0) {
                            if (texttocheck.toLowerCase().includes(grdControl[i].value.toLowerCase())) {
                               /* alert('Eklemek istediğiniz kolon mevcut. (' + grdControl[i].value + ')');*/
                                alertMsg('Eklemek istediğiniz kolon mevcut. (' + grdControl[i].value + ')', 'no');
                                return false;
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            alertMsg('Kolon adı boş bırakılamaz', 'no');
                            /*alert("Kolon adı boş bırakılamaz");*/
                            return false;
                        }


                    }
                }
            }

        }
    </script>

    <script>
        $('#btnCreateTable').click(function () {

            $('#btnCreateTableSQL').click();
        })

    </script>
</asp:Content>
