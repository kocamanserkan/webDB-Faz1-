<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTables.aspx.cs" Inherits="faz1.TableOperations.MyTables" %>

<%@ Register Assembly="DevExpress.Web.v21.1, Version=21.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="../bootstrap/js/bootstrap.js"></script>
    <link href="../vendors/customCss.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 70%" class="cls-block offset2  ">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Oluşturduğum Son 10 Tablom</h3>
            </div>
            <div class="panel-body">
                <div class="row-fluid">
                    <div class="span3">
                        <img class="img-circle" src="../Images/table.png" />
                    </div>
                    <div class="span6">

                        <table class="table table-condensed table-responsive table-user-information">
                            <thead runat="server" visible="false" id="txtThead">
                                <tr>
                                    <th><strong>Tablo Adı</strong></th>
                                    <th><strong>Oluşturulma Tarihi</strong></th>
                                </tr>
                            </thead>
                            <tbody runat="Server" id="tableBody">
                                <tr>
                                    <th>asd</th>
                                </tr>
                                <tr>
                                    <th>asd</th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
           
            <div class="panel-footer">
                <br />
                <asp:Button Style="float: right" OnClick="btnOpenCreateField_Click" ID="btnOnayla" Text="Yeni Tablo Oluştur" CssClass="btn btn-success" runat="server" />
                <br />
                <br />
                <br />
            </div>
        </div>
       
    </div>



    <script>
        $(document).ready(function () {
            var panels = $('.user-infos');
            var panelsButton = $('.dropdown-user');
            panels.show();

            //Click dropdown
            panelsButton.click(function () {
                //get data-for attribute
                var dataFor = $(this).attr('data-for');
                var idFor = $(dataFor);

                //current button
                var currentButton = $(this);
                idFor.slideToggle(400, function () {
                    //Completed slidetoggle
                    if (idFor.is(':visible')) {
                        currentButton.html('<i class="icon-chevron-up text-muted"></i>');
                    }
                    else {
                        currentButton.html('<i class="icon-chevron-down text-muted"></i>');
                    }
                })
            });


            $('[data-toggle="tooltip"]').tooltip();

            $('button').click(function (e) {
                e.preventDefault();
                alert("This is a demo.\n :-)");
            });
        });
    </script>
    <div style="display: none" id="myTablesField" class="cls-block offset3">
        <div class="row-fluid">
            <div class="span12">
                <h1 style="text-align: center">Tablolarım</h1>
                <br />
            </div>
        </div>
        <div class="row-fluid">
            <div class="span4">
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th><strong>Tablo Adı</strong></th>
                            <th><strong>Oluşturulma Tarihi</strong></th>
                        </tr>
                    </thead>
                    <tbody runat="Server" id="tableBodyg">
                        <tr>
                            <th>asd</th>

                        </tr>
                        <tr>
                            <th>asd</th>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="span6">
                <asp:Button ID="btnOpenCreateField" Style="float: right" Text="Yeni Tablo Oluştur" class="btn btn-primary" runat="server" />
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
            </div>
        </div>
    </div>
</asp:Content>
