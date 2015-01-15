function LoginViewModel() {
    var
         self = this
    ;

    self.busy = ko.observable(false);

    self.userLogin = new LoginEntity();
    self.userPassword = new LoginEntity();
    self.loginHasErrors = ko.observable(false);
    self.loginErrorMessage = ko.observable('');
    self.login = function () {
        var
            self = this,
            login = self.userLogin,
            password = self.userPassword
        ;

        login.hasErrors(false);
        password.hasErrors(false);

        if (login.value().trim().length === 0) {
            login.errorMessage('Invalid user name.');
            login.hasErrors(true);
        }

        if (password.value().length === 0) {
            password.errorMessage('Invalid password.');
            password.hasErrors(true);
        }

        if (login.hasErrors() || password.hasErrors()) {
            return;
        }

        self.busy(true);

        internalLogin(login.value(), password.value(), {
            success: function (obj, status, jqXHR) {
                self.loginHasErrors(false);

                if (Modernizr.localstorage) {
                    localStorage.setItem("user", obj.access_token);
                    window.location = "/";
                }
            },
            error: function (jqXHR, status, error) {
                self.loginHasErrors(true);
                self.loginErrorMessage(jqXHR.responseJSON.error_description);
            },
            complete: function () {
                self.busy(false);
            }
        });
    };

    self.userNameSignup = new LoginEntity();
    self.userLastNameSignup = new LoginEntity();
    self.userEmailSignup = new LoginEntity();
    self.userPassordSignup = new LoginEntity();
    self.signupHasErrors = ko.observable(false);
    self.signupErrorMessage = ko.observable('');
    self.signup = function () {
        var
            self = this,
            userName = self.userNameSignup,
            userLastName = self.userLastNameSignup,
            userEmail = self.userEmailSignup,
            userPassword = self.userPassordSignup
        ;

        userName.hasErrors(false);
        userLastName.hasErrors(false);
        userEmail.hasErrors(false);
        userPassword.hasErrors(false);

        if (userName.value().trim().length === 0) {
            userName.errorMessage('Invalid name.');
            userName.hasErrors(true);
        }

        if (userLastName.value().length === 0) {
            userLastName.errorMessage('Invalid last name.');
            userLastName.hasErrors(true);
        }

        if (userEmail.value().length === 0) {
            userEmail.errorMessage('Invalid e-mail.');
            userEmail.hasErrors(true);
        }

        if (userPassword.value().length === 0) {
            userPassword.errorMessage('Invalid password.');
            userPassword.hasErrors(true);
        }

        if (userName.hasErrors() || userLastName.hasErrors() || userEmail.hasErrors() || userPassword.hasErrors()) {
            return;
        }

        self.busy(true);

        $.ajax({
            type: "POST",
            url: "/api/Account/Register",
            contentType: 'application/json',
            data: JSON.stringify({
                Id: 0,
                Name: userName.value(),
                LastName: userLastName.value(),
                Email: userEmail.value(),
                Password: userPassword.value(),
                About: ''
            }),
            success: function (obj, status, jqXHR) {
                self.signupHasErrors(false);

                internalLogin(obj.Email, obj.Password, {
                    success: function (obj, status, jqXHR) {
                        self.signupHasErrors(false);

                        if (Modernizr.localstorage) {
                            localStorage.setItem("user", obj.access_token);
                            window.location = "/";
                        }
                    },
                    error: function (jqXHR, status, error) {
                        self.signupHasErrors(true);
                        self.signupErrorMessage(jqXHR.responseJSON.error_description);
                    },
                    complete: function () {
                        self.busy(false);
                    }
                })
            },
            error: function (jqXHR, status, error) {
                var
                    error = "Unexpected error."
                ;
                self.signupHasErrors(true);

                if (jqXHR.status === 500)
                {
                    error = jqXHR.responseJSON.ExceptionMessage;
                } else if (jqXHR.status === 400) {
                    error = jqXHR.responseJSON.ModelState.user[0]
                }

                self.signupErrorMessage(error);
            },
            complete: function () {
                self.busy(false);
            }
        });
    };
    
    function LoginEntity() {
        this.value = ko.observable('');
        this.hasErrors = ko.observable(false);
        this.errorMessage = ko.observable('');
    }

    function internalLogin(user, pwd, options)
    {
        var
            defaults = {
                success: function () { },
                error: function () { },
                complete: function () { }
            }
        ;

        options = $.extend({}, defaults, options)

        $.ajax({
            type: "POST",
            url: "/token",
            contentType: 'application/x-www-form-urlencoded',
            accepts: 'application/json',
            data: {
                grant_type: 'password',
                username: user,
                password: pwd
            },
            success: options.success,
            error: options.error,
            complete: options.complete
        });
    }
}