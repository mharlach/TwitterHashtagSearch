<%@ Page Title="Twitter Hashtag Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HashtagSearch._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <div class="form-group">
            <label for="searchTextbox">Search Twitter for a hashtag...</label>
            <asp:TextBox ID="searchTextbox" CssClass="form-control" placeholder="#search" runat="server" />
            <asp:Button ID="searchButton" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="searchButton_click" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 col-md-offset-1">
            <asp:UpdatePanel ID="tweetsUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:ListView ID="tweetsListView" runat="server">
                        <LayoutTemplate>
                            <ul runat="server" id="tweetsTable" class="list-group">
                                <li runat="server" id="itemPlaceholder" class="list-group-item" />
                            </ul>
                            <asp:DataPager runat="server" ID="TweetDataPager" PageSize="5" PagedControlID="tweetsListView">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowLastPageButton="true" ButtonCssClass="btn btn-primary btn-sm" />
                                </Fields>
                            </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li runat="server" class="list-group-item">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Image ID="UserImage" runat="server" ImageUrl='<%#Eval("ImageUrl")%>' CssClass="img-circle" Width="50" Height="50" />
                                        <strong>
                                            <asp:Label ID="AuthorNameLabel" runat="server" Text='<%#Eval("AuthorName")%>' /></strong>
                                        <asp:Label ID="ScreenNameLabel" runat="server" Text='<%#Eval("ScreenName")%>' />
                                        <em>
                                            <asp:Label ID="TimestampLabel" runat="server" Text='<%#Eval("Timestamp")%>' /></em>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="TweetLabel" runat="server" Text='<%#Eval("TweetText")%>' />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Retweets <asp:Label ID="RetweetsLabel" runat="server" CssClass="badge" Text='<%#Eval("RetweetsCount")%>' /></label>
                                    <span>
                                        <asp:LinkButton ID="LikeButton" CssClass="btn btn-xs btn-info" runat="server" CommandName="Like" OnCommand="LikeButton_Click" CommandArgument='<%#Eval("Id")%>' Enabled='<%#Eval("LikeEnabled")%>'>Likes  <asp:Label CssClass="badge" runat="server" Text=<%#Eval("LikesCount")%> /></asp:LinkButton>
                                    </span>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
</asp:Content>
