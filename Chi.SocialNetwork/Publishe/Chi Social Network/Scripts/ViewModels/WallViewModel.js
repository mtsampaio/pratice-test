function WallViewModel(userId) {
    var
        self = this,
        userPostUri = '/api/UserPosts',
        userCommentUri = '/api/UserPostComment',

        dropzone,
        uploadFileError = ''
    ;

    if ($('#form-comment').length) {
        dropzone = new Dropzone($('#form-comment')[0], {
            url: "/api/FileUpload",
            acceptedFiles: 'image/*, video/*',
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            parallelUploads: 20,
            previewsContainer: "#previews",
            addRemoveLinks: true,
            maxFilesize: 20,
            clickable: ".fileinput-button",
            autoProcessQueue: false,
            headers: {
                Authorization: "Bearer " + localStorage.getItem("user")
            },
            error: function (e, text, xhr) {
                uploadFileError = text;
            }
        });

        dropzone.on('removedfile', function () {
            uploadFileError = '';
        });
    }

    self.busy = ko.observable(true);

    self.hobbie = ko.observable('');

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

        if (uploadFileError.length) {
            alert(uploadFileError);
            return;
        }

        self.busy(true);

        ajaxHelper(userPostUri, {
            data: JSON.stringify({
                Id: 0,
                PostContent: self.hobbie()
            }),
            success: function (obj) {
                var
                    addedFiles = dropzone.getFilesWithStatus(Dropzone.QUEUED),
                    post = resultToUserPost(obj)
                ;

                if (addedFiles.length > 0) {
                    var
                        count = 0,
                        files = []
                    ;

                    dropzone.on('success', function (result) {

                        var
                            file = JSON.parse(result.xhr.response)[0]
                        ;

                        if (file) {
                            ajaxHelper('/api/UserPostFile', {
                                data: JSON.stringify({
                                    Id: 0,
                                    UserPostId: post.id,
                                    FileName: file.FileName,
                                    Type: file.Type
                                }),
                                success: function (obj) {
                                    var
                                        file = new File()
                                    ;

                                    file.id = obj.Id;
                                    file.file = obj.FileName;
                                    file.type = obj.Type;

                                    files.push(file);
                                },
                                error: function () {
                                    alert('Error!');
                                },
                                complete: function () {
                                    count++;
                                }
                            })
                        }
                    });

                    dropzone.processQueue();

                    var interval = setInterval(function () {
                        if (count == dropzone.getAcceptedFiles().length) {
                            clearInterval(interval);

                            dropzone.removeAllFiles(true);
                            self.hobbie('');

                            self.userPosts.splice(0, 0, post);
                            post.medias(files);
                        }
                    }, 100);
                }
                else {
                    self.userPosts.splice(0, 0, post);
                    self.hobbie('');
                }
            },
            complete: function () {
                self.busy(false);
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
        self.medias = ko.observableArray();

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
            ajaxHelper(serviceUri + "/" + self.keyValue, {
                type: "DELETE",
                data: JSON.stringify({
                    id: self.keyValue
                }),
                success: function (obj) {
                    self.likeId = 0;
                    self.liked(false);
                    self.count(self.count() - 1);
                }
            });
        }
    }

    function File() {
        var
           self = this
        ;

        self.id = 0;
        self.file = '';
        self.type = '';
    }

    function resultToUserPost(result) {
        var
            post = new Post()
        ;

        post.id = result.Id;
        post.content = result.Post.replace(/\n/g, "<br />");
        post.like.keyValue = post.id;
        post.like.count(result.LikeCount);
        post.like.liked(result.Liked);

        if (result.Files) {
            for (var i = 0; i < result.Files.length; i++) {
                var
                    media = result.Files[i],
                    file = new File()
                ;

                file.id = media.Id;
                file.file = media.FileName;
                file.type = media.Type;

                post.medias.push(file);
            }
        }

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
        comment.like.liked(result.Liked);

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