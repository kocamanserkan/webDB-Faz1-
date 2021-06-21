<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TableViewing.aspx.cs" Inherits="faz1.TableOperations.TableViewing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <script src="../vendors/alert.js"></script>
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
    <div class="container cls-block offset2" style="width: 75%">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h2>Tablo Görüntüleme</h2>
            </div>
            <div class="panel-body">
                <div class="row-fluid">
                    <div class="span12">
                        <label>Görüntülenecek Tablo</label>
                        <asp:DropDownList ID="ddlSelectedTable" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSelectedTable_SelectedIndexChanged" runat="server">
                            <asp:ListItem Text="Tablo Seçiniz" Value="1" />
                        </asp:DropDownList>
                        <br />
                        <br />
                        <br />
                        <asp:Panel Visible="false" ID="pnlDocumantary" runat="server">
                            <asp:LinkButton ID="btnSaveExcel" runat="server" OnClick="btnSaveExcel_Click" CssClass="btn btn-success">Excel<i class="icon icon-download-alt"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnSavePdfs" runat="server" OnClick="btnSavePdf_Click" CssClass="btn btn-danger">Pdf<i class="icon icon-download-alt"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnGetScript" runat="server" OnClick="btnGetScript_Click" CssClass="btn btn-info">Script<i class="icon icon-download-alt"></i></asp:LinkButton>
                            <a href="#myModal" style="float: right" role="button" class="btn btn-info" data-toggle="modal">Tablo Bilgilerini Göster</a>

                        </asp:Panel>
                    </div>
                    <div class="span0">
                        <br />
                        <asp:Panel Visible="false" ID="pnlTableInfo" runat="server">
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-primary">
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class=" cls-block span12">
                    <asp:Panel Visible="false" ID="pnlGrd" runat="server">
                        <asp:GridView  OnRowDataBound="grdTable_RowDataBound" CssClass="table table-hover" ID="grdTable" runat="server">
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <script>

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
                    heading: 'Error',
                    text: msg,
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
            }


        }
    </script>
</asp:Content>

