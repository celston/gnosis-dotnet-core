define(['../app'], function (app) {
    function EntityChangeController($scope) {
        $scope.submitted = false;
        $scope.submitting = false;

        $scope.blurred = {};
        $scope.data = {};

        $scope.setFormName = function (formName) {
            this.formName = formName;
        }

        $scope.showError = function (name) {
            return ($scope.submitted || $scope.blurred[name]) && $scope[this.formName][name].$invalid;
        }
    }

    function WidgetCreateController($scope) {
        EntityChangeController.call(this, $scope);

        $scope.data.s2 = ['first', 'second', 'third'];
        $scope.blurred.s2 = [false, false, false];

        $scope.submit = function () {
            $scope.submitted = true;
        }
    }

    WidgetCreateController.prototype = Object.create(EntityChangeController.prototype);

    app.controller('WidgetCreateController', WidgetCreateController);
})