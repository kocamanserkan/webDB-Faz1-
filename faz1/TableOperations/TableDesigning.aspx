<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TableDesigning.aspx.cs" Inherits="faz1.TableOperations.TableDesigning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <link href="../vendors/customCss.css" rel="stylesheet" />
    <style>
        #cbkNewColumn {
            box-shadow: inherit !important;
        }
    </style>
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
                    <label>Kolon Adı</label>
                </div>
                <div class="span6">
                    <asp:TextBox ID="txtNewColumnName" ClientIDMode="static" runat="server" />
                </div>
            </div>
            <hr />
            <div class="row-fluid">
                <div class="span6">
                    <label>Veri Tipi</label>
                </div>
                <div class="span6">
                    <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="ddlNewColumnData">
                        <asp:ListItem Value="varchar" Text="Metin" />
                        <asp:ListItem Value="int" Text="Tam Sayı" />
                        <asp:ListItem Value="decimal" Text="Ondalık" />
                        <%--<asp:ListItem Value="date" Text="Tarih" />--%>
                        <asp:ListItem Value="datetime" Text="Tarih ve Saat" />
                    </asp:DropDownList>
                </div>
            </div>
            <hr />
            <div style="display: none" class="row-fluid">
                <div class="span6">
                    <label>Boş Geçilebilirlik</label>
                </div>
                <div class="span6">
                    <asp:CheckBox ID="cbkNewColumn" Enabled="false" ClientIDMode="static" Checked="true" runat="server" />
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button class="btn" data-dismiss="modal" aria-hidden="true">Kapat</button>
            <asp:Button Style="height: auto;" Text="Kolon Ekle" ClientIDMode="static" ID="addColumn" OnClick="addColumn_Click" CssClass="btn btn-success" runat="server" />
        </div>
    </div>
    <div class="hero">
        <div class="span11 offset2" style="width: 75%">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h2>Tablo Tasarım Ekranı</h2>
                </div>
                <div class="panel-body">
                    <div class="row-fluid">
                        <div class="span6">
                            <label>Düzenlenecek Tablo</label>
                            <asp:DropDownList AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" ID="ddlTables" runat="server">
                                <asp:ListItem Text="Tablo Seçiniz" Value="1" />

                            </asp:DropDownList>
                        </div>
                        <div class="span6">
                            <br />
                            <asp:Panel Visible="false" ID="pnlAlter" runat="server">
                            </asp:Panel>
                        </div>
                        <br />
                        <br />
                        <br />
                        <br />
                        <div class="row-fluid">
                            <asp:GridView AutoGenerateColumns="false" ID="grdModifyTable" CssClass="table table-bordered" runat="server"
                                OnRowEditing="grdModifyTable_RowEditing"
                                OnRowDataBound="grdModifyTable_RowDataBound"
                                OnRowUpdating="grdModifyTable_RowUpdating"
                                ClientIDMode="static">

                                <Columns>
                                    <asp:TemplateField HeaderText="Düzen">
                                        <ItemTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" Text="Düzenle" runat="server" CommandName="Edit" />
                                            <asp:LinkButton OnClientClick="dellColumn()" CssClass="btn btn-danger" OnClick="OnDelete" Text="Sil" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton OnClientClick="return validateGrid(this)" CssClass="btn btn-success" Text="Güncelle" runat="server" CommandName="Update" />
                                            <asp:LinkButton CssClass="btn btn-danger" OnClick="OnCancel" Text="Vazgeç" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Kolon Adı">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_KolonAd" runat="server" Text='<%#Eval("COLUMN_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_KolonAd" required="true" ToolTip="Kolon Adı" ClientIDMode="static" Style="border-color: red;" runat="server" Width="80px" Text='<%#Eval("COLUMN_NAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Kolon Veri Tipi">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_KolonData" runat="server" Text='<%#Eval("DATA_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList AppendDataBoundItems="true" Style="border-color: red;" runat="server" ID="ddlDataTypeNew" Width="160px">
                                                <asp:ListItem Value="varchar" Text="Metin" />
                                                <asp:ListItem Value="int" Text="Tam Sayı" />
                                                <asp:ListItem Value="decimal" Text="Ondalık" />
                                                <%-- <asp:ListItem Value="date" Text="Tarih" />--%>
                                                <asp:ListItem Value="datetime" Text="Tarih ve Saat" />
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div runat="server" id="pnlFooter" visible="false" class="panel-footer">
                    <br />

                    <asp:Button Style="float: right" OnClientClick="deleteTable()" Text="Tabloyu Sil" ID="btnDeleteTable" OnClick="btnDeleteTable_Click" CssClass="btn btn-danger" runat="server" />
                    <a href="#myModal" style="float: right; margin-right: 5px; padding: 9px;" role="button" class="btn btn-info" data-toggle="modal">Tabloya Kolon Ekle</a>

                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
    <script src="../vendors/alert.js"></script>
    <script src="../vendors/alertFunc.js"></script>
    <script>
        function validateGrsid(ctrl) {
            var grdRow = ctrl.parentNode.parentNode;
            var grdControl = grdRow.getElementsByTagName("input");
            // edit için İçeride dönen
            for (var i = 0; i < grdControl.length; i++) {
                if (grdControl[i].empid = 'txt_KolonAd') {
                    if (grdControl[i].value === "") {
                        alert(grdControl[i].title + ' zorunludur.');
                        return false;
                    }
                    else {

                        var texttocheck = '';
                        $("#grdModifyTable tr td").each(function () {
                            texttocheck += this.innerText.trim();
                        });
                        if (grdControl[i].value.length > 0) {
                            if (texttocheck.toLowerCase().includes(grdControl[i].value.toLowerCase())) {
                                alert('Eklemek istediğiniz kolon mevcut. (' + grdControl[i].value + ')');
                                return false;
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            alert("Kolon adı boş bırakılamaz");
                            return false;
                        }


                    }
                }
            }

        }

        function validateGrid(ctrl) {

            if (SaveChanges()) {
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
                        else if (!numericReg.test(grdControl[i].value)) {

                            alertMsg('Hatalı Kolon Güncellemesi. Kolon Adınızı Kontrol Ediniz', 'no');

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
            else {
                return false;
            }


        }
       
        function deleteTable() {

            return confirm('Tabloyu silmek üzeresiniz. Onayladığınız taktirde tablo içindeki veriler ile birlikte silinecektir.')

        }
        function dellColumn() {

            return confirm('Tablonun bir kolonunu silmek üzeresiniz. Kolon içerisindeki bilgiler ile birlikte silinecektir. Onaylıyor musunuz?')

        }
        $('#txtNewColumnName').keyup(function () {

            $('span.error-keyup-1').hide();
            var inputVal = $(this).val();
            var numericReg = /^[a-zA-Z_][a-zA-Z0-9_]*$/;
            if (!numericReg.test(inputVal) && inputVal != '') {
                $('#addColumn').css("display", "none");
                $(this).before('<span style="color:red" class="error error-keyup-1">Hatalı Kolon Adı </span>');
            }
            else {
                $('#addColumn').css("display", "initial");
            }

        });

        function SaveChanges() {

            return confirm('Tabloya ait kolonların veri yapısını değiştirmek kayıtları tamamen silebilir. Onaylıyor musunuz?')

        }
        $('#addColumn').click(function () {
         var inputVal = $('#txtNewColumnName').val();
            if (inputVal == '') {
                alertMsg('Kolon adı boş bırakılamaz.', 'no');
                return false;
            }
            else {
                return true;
            }

        })
    </script>
</asp:Content>
