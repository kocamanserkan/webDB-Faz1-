<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TableDesigning.aspx.cs" Inherits="faz1.TableOperations.TableDesigning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <script src="../vendors/alert.js"></script>
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
