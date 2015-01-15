function WallViewModel(userId) {
    var
        self = this,
        userPostUri = '/api/UserPosts',
        userCommentUri = '/api/UserPostComment'
    ;

    self.hobbie = ko.observable('');
    self.media = ko.observable('');

    self.userPosts = ko.observableArray();
    self.getPosts = function () {
        var
            self = this
        ;

        $.ajax({
            type: "GET",
            url: userPostUri + "/" + userId,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("user"));
            },
            success: function (obj) {
                for (var i = 0; i < obj.length; i++) {
                    self.userPosts.push(
                        resultToUserPost(obj[i])
                    );
                }
            }
        });
    }
    self.post = function () {
        var
           self = this
        ;

        ajaxHelper(userPostUri, {
            data: JSON.stringify({
                Id: 0,
                PostContent: self.hobbie(),
                ContentType: 1
            }),
            success: function (obj) {
                self.userPosts.splice(0, 0, resultToUserPost(obj));
                self.hobbie('');
            }
        });
    }
    self.getPosts();

    self.logout = function () {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/api/Account/Logout',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("user"));
            },
            success: function () {
                window.location = '/';
            },
            error: function () {

            }
        });
    }

    function Post() {
        var
            self = this,
            likeUri = '/api/UserPostLike'
        ;

        self.id = 0;
        self.content = '';
        self.type = 1;

        self.userComment = ko.observable('');
        self.comments = ko.observableArray();
        self.hasComments = ko.computed(function () {
            return self.comments().length > 0;
        });
        self.comment = function () {
            var
                self = this,
                data = JSON.stringify({
                    Id: 0,
                    Comment: self.userComment(),
                    User_Id: 0,
                    UserPost_Id: self.id
                })
            ;

            self.userComment('');

            ajaxHelper(userCommentUri, {
                data: data,
                success: function (obj) {
                    self.comments.push(resultToComment(self.id, obj));
                }
            });
        }

        self.like = new Like("UserPost_Id", likeUri);
    }

    function Comment(postId) {
        var
            self = this,
            likeUri = '/api/UserPostCommentLike'
        ;

        self.id = 0;
        self.content = '';
        self.postId = postId;
        self.user = null;
        self.like = new Like("UserPostComment_Id", likeUri)
    }

    function User() {
        var
            self = this
        ;

        self.id = 0;
        self.name = '';
        self.portrait = '';
    }

    function Like(keyName, serviceUri) {
        var
            self = this
        ;

        self.keyValue = 0;
        self.likeId = 0;
        self.liked = ko.observable(false);
        self.count = ko.observable(0);
        self.showLike = ko.computed(function () {
            return self.count() > 0;
        });
        self.like = function () {
            var
                self = this,

                data = {
                    Id: 0,
                    User_Id: 0
                }
            ;

            data[keyName] = self.keyValue;

            ajaxHelper(serviceUri, {
                data: JSON.stringify(data),
                success: function (obj) {
                    self.likeId = obj.Id;
                    self.liked(true);
                    self.count(self.count() + 1);
                }
            });
        };
        self.unlike = function () {
            ajaxHelper(userPostUri, {
                type: "DELETE",
                success: function (obj) {
                    self.likeId = 0;
                    self.liked(false);
                    self.count(self.count() - 1);
                }
            });
        }
    }

    function resultToUserPost(result) {
        var
            post = new Post()
        ;

        post.id = result.Id;
        post.content = result.Post.replace(/\n/g, "<br />");
        post.type = result.Type;
        post.like.keyValue = post.id;
        post.like.count(result.LikeCount);

        if (result.Comments) {
            for (var i = 0; i < result.Comments.length; i++) {
                post.comments.push(
                    resultToComment(post.id, result.Comments[i])
                );
            }
        }

        return post;
    }

    function resultToComment(postId, result) {
        var
            user,
            comment
        ;

        user = new User();
        user.id = result.User.Id;
        user.name = result.User.Name;
        user.portrait = result.User.Portrait;

        comment = new Comment(postId);
        comment.id = result.Id;
        comment.content = result.Comment;
        comment.user = user;
        comment.like.keyValue = result.Id;
        comment.like.count(result.LikeCount);

        return comment;
    }

    function ajaxHelper(url, options) {
        var
            defaults = {
                type: "POST",
                contentType: "application/json",
                url: url,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("user"));
                },
                success: function () { },
                error: function () { }
            }
        ;

        options = $.extend({}, defaults, options);

        $.ajax(options);
    }
}