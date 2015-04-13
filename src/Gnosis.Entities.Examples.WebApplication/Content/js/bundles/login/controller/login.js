define(['../app', '../../common/controller/form'], function (app, FormController) {
    function LoginController($scope, $http) {
        FormController.call(this, $scope);

        $scope.data.s2 = ['first', 'second', 'third'];
        $scope.blurred.s2 = [false, false, false];

        $scope.submit = function () {
            $scope.submitted = true;

            if ($scope.loginRequestModel.$valid) {
                $scope.submitting = true;

                var data = angular.copy($scope.data);

                $http.post(
                    '/Login/Data/Login',
                    {
                        data: data
                    }
                ).then(
                    function (response) {
                        console.log(response);
                        $scope.submitting = false;
                    },
                    function (exception) {
                        console.log(exception);
                        $scope.submitting = false;
                    }
                )
            }
        }
    }

    LoginController.prototype = Object.create(FormController.prototype);

    app.controller('LoginController', LoginController);
})