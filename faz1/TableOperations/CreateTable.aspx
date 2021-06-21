<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CreateTable.aspx.cs" Inherits="faz1.TableOperations.CreateTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <script src="../vendors/alert.js"></script>
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
                                <%--<asp:RegularExpressionValidator 
                                    ErrorMessage="Hatalı Tablo Adı" 
                                     ForeColor="red"
                                     ControlToValidate="txtTableName"
                                     ValidationExpression="^[\p{L}_][\p{L}\p{N}@$#_]{0,127}$" 
                                    runat="server" />--%>
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
                                                    <%--<asp:ListItem Value="date" Text="Tarih ve Saat" />--%>
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
    <script type="text/javascript">


        //$(document).ready(function () {
        //    $("a[id*=update]").click(function () {
        //        var valid = true;
        //        var errorString = "";
        //        var allInputs = $(this).closest("tr").find("input[type=text][id*=grdMyTable]");
        //        allInputs.each(function (index, item) {
        //            if ($.trim(item.value) == "") {
        //                errorString += " Please provide a value for " +
        //                    $(this).closest("table").find("th:eq(" + $(item).parent()[0].cellIndex + ")").html() + "\n";
        //            }
        //        });
        //        if (errorString != "") {
        //            alert(errorString);
        //            return false;
        //        }
        //    });
        //})




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
                //$('#btnDeleteAllColumns').attr('disabled', 'disabled')
                //$('#btnOnayla').attr('disabled', 'disabled')
                //$('#btnEditGrid').attr('disabled', 'disabled')
                //$('#btnDelGridCol').attr('disabled', 'disabled')
                $('#gridPanel').attr('style', 'display:none')

            } else {
                $('#btnAddColumn').removeAttr('disabled');
                $('#txtTableName').removeAttr('disabled');
                //$('#btnDeleteAllColumns').removeAttr('disabled');
                //$('#btnOnayla').removeAttr('disabled');
                //$('#btnEditGrid').removeAttr('disabled');
                //$('#btnDelGridCol').removeAttr('disabled');
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

        "function" != typeof Object.create && (Object.create = function (t) { function o() { } return o.prototype = t, new o }), function (t, o) { "use strict"; var i = { _positionClasses: ["bottom-left", "bottom-right", "top-right", "top-left", "bottom-center", "top-center", "mid-center"], _defaultIcons: ["success", "error", "info", "warning"], init: function (o) { this.prepareOptions(o, t.toast.options), this.process() }, prepareOptions: function (o, i) { var s = {}; "string" == typeof o || o instanceof Array ? s.text = o : s = o, this.options = t.extend({}, i, s) }, process: function () { this.setup(), this.addToDom(), this.position(), this.bindToast(), this.animate() }, setup: function () { var o = ""; if (this._toastEl = this._toastEl || t("<div></div>", { "class": "jq-toast-single" }), o += '<span class="jq-toast-loader"></span>', this.options.allowToastClose && (o += '<span class="close-jq-toast-single">&times;</span>'), this.options.text instanceof Array) { this.options.heading && (o += '<h2 class="jq-toast-heading">' + this.options.heading + "</h2>"), o += '<ul class="jq-toast-ul">'; for (var i = 0; i < this.options.text.length; i++)o += '<li class="jq-toast-li" id="jq-toast-item-' + i + '">' + this.options.text[i] + "</li>"; o += "</ul>" } else this.options.heading && (o += '<h2 class="jq-toast-heading">' + this.options.heading + "</h2>"), o += this.options.text; this._toastEl.html(o), this.options.bgColor !== !1 && this._toastEl.css("background-color", this.options.bgColor), this.options.textColor !== !1 && this._toastEl.css("color", this.options.textColor), this.options.textAlign && this._toastEl.css("text-align", this.options.textAlign), this.options.icon !== !1 && (this._toastEl.addClass("jq-has-icon"), -1 !== t.inArray(this.options.icon, this._defaultIcons) && this._toastEl.addClass("jq-icon-" + this.options.icon)) }, position: function () { "string" == typeof this.options.position && -1 !== t.inArray(this.options.position, this._positionClasses) ? "bottom-center" === this.options.position ? this._container.css({ left: t(o).outerWidth() / 2 - this._container.outerWidth() / 2, bottom: 20 }) : "top-center" === this.options.position ? this._container.css({ left: t(o).outerWidth() / 2 - this._container.outerWidth() / 2, top: 20 }) : "mid-center" === this.options.position ? this._container.css({ left: t(o).outerWidth() / 2 - this._container.outerWidth() / 2, top: t(o).outerHeight() / 2 - this._container.outerHeight() / 2 }) : this._container.addClass(this.options.position) : "object" == typeof this.options.position ? this._container.css({ top: this.options.position.top ? this.options.position.top : "auto", bottom: this.options.position.bottom ? this.options.position.bottom : "auto", left: this.options.position.left ? this.options.position.left : "auto", right: this.options.position.right ? this.options.position.right : "auto" }) : this._container.addClass("bottom-left") }, bindToast: function () { var t = this; this._toastEl.on("afterShown", function () { t.processLoader() }), this._toastEl.find(".close-jq-toast-single").on("click", function (o) { o.preventDefault(), "fade" === t.options.showHideTransition ? (t._toastEl.trigger("beforeHide"), t._toastEl.fadeOut(function () { t._toastEl.trigger("afterHidden") })) : "slide" === t.options.showHideTransition ? (t._toastEl.trigger("beforeHide"), t._toastEl.slideUp(function () { t._toastEl.trigger("afterHidden") })) : (t._toastEl.trigger("beforeHide"), t._toastEl.hide(function () { t._toastEl.trigger("afterHidden") })) }), "function" == typeof this.options.beforeShow && this._toastEl.on("beforeShow", function () { t.options.beforeShow() }), "function" == typeof this.options.afterShown && this._toastEl.on("afterShown", function () { t.options.afterShown() }), "function" == typeof this.options.beforeHide && this._toastEl.on("beforeHide", function () { t.options.beforeHide() }), "function" == typeof this.options.afterHidden && this._toastEl.on("afterHidden", function () { t.options.afterHidden() }) }, addToDom: function () { var o = t(".jq-toast-wrap"); if (0 === o.length ? (o = t("<div></div>", { "class": "jq-toast-wrap" }), t("body").append(o)) : (!this.options.stack || isNaN(parseInt(this.options.stack, 10))) && o.empty(), o.find(".jq-toast-single:hidden").remove(), o.append(this._toastEl), this.options.stack && !isNaN(parseInt(this.options.stack), 10)) { var i = o.find(".jq-toast-single").length, s = i - this.options.stack; s > 0 && t(".jq-toast-wrap").find(".jq-toast-single").slice(0, s).remove() } this._container = o }, canAutoHide: function () { return this.options.hideAfter !== !1 && !isNaN(parseInt(this.options.hideAfter, 10)) }, processLoader: function () { if (!this.canAutoHide() || this.options.loader === !1) return !1; var t = this._toastEl.find(".jq-toast-loader"), o = (this.options.hideAfter - 400) / 1e3 + "s", i = this.options.loaderBg, s = t.attr("style") || ""; s = s.substring(0, s.indexOf("-webkit-transition")), s += "-webkit-transition: width " + o + " ease-in;                       -o-transition: width " + o + " ease-in;                       transition: width " + o + " ease-in;                       background-color: " + i + ";", t.attr("style", s).addClass("jq-toast-loaded") }, animate: function () { var t = this; if (this._toastEl.hide(), this._toastEl.trigger("beforeShow"), "fade" === this.options.showHideTransition.toLowerCase() ? this._toastEl.fadeIn(function () { t._toastEl.trigger("afterShown") }) : "slide" === this.options.showHideTransition.toLowerCase() ? this._toastEl.slideDown(function () { t._toastEl.trigger("afterShown") }) : this._toastEl.show(function () { t._toastEl.trigger("afterShown") }), this.canAutoHide()) { var t = this; o.setTimeout(function () { "fade" === t.options.showHideTransition.toLowerCase() ? (t._toastEl.trigger("beforeHide"), t._toastEl.fadeOut(function () { t._toastEl.trigger("afterHidden") })) : "slide" === t.options.showHideTransition.toLowerCase() ? (t._toastEl.trigger("beforeHide"), t._toastEl.slideUp(function () { t._toastEl.trigger("afterHidden") })) : (t._toastEl.trigger("beforeHide"), t._toastEl.hide(function () { t._toastEl.trigger("afterHidden") })) }, this.options.hideAfter) } }, reset: function (o) { "all" === o ? t(".jq-toast-wrap").remove() : this._toastEl.remove() }, update: function (t) { this.prepareOptions(t, this.options), this.setup(), this.bindToast() } }; t.toast = function (t) { var o = Object.create(i); return o.init(t, this), { reset: function (t) { o.reset(t) }, update: function (t) { o.update(t) } } }, t.toast.options = { text: "", heading: "", showHideTransition: "fade", allowToastClose: !0, hideAfter: 3e3, loader: !0, loaderBg: "#9EC600", stack: 5, position: "bottom-left", bgColor: !1, textColor: !1, textAlign: "left", icon: !1, beforeShow: function () { }, afterShown: function () { }, beforeHide: function () { }, afterHidden: function () { } } }(jQuery, window, document);


        /* Starts from here */

        function alertMsg(msg, type) {

            if (type == 'yes') {
                $.toast({
                    heading: 'İşlem Başarılı',
                    text: msg,
                    icon: 'success',
                    loader: true,
                    loaderBg: '#fff',
                    showHideTransition: 'fade',
                    hideAfter: 3000,
                    allowToastClose: false,
                    position: {
                        right: 100,
                        left: 0,
                        top: 50
                    },


                })
            } else {
                $.toast({
                    heading: 'Hata',
                    text: msg,
                    icon: 'error',
                    loader: true,
                    loaderBg: '#fff',
                    showHideTransition: 'plain',
                    hideAfter: 3000,
                    position: {
                        right: 100,
                        left: 0,
                        top: 50
                    }
                })
            }
        }
        $("#error").click(function () {
            $.toast({
                heading: 'Error',
                text: 'Try again!',
                icon: 'error',
                loader: true,
                loaderBg: '#fff',
                showHideTransition: 'plain',
                hideAfter: 3000,
                position: {
                    left: 100,
                    top: 30
                }
            })
        })

    </script>
</asp:Content>
