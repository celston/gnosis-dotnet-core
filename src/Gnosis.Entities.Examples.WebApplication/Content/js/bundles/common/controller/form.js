define([], function () {
    function FormController($scope) {
        $scope.submitted = false;
        $scope.submitting = false;

        $scope.blurred = {};
        $scope.data = {};

        $scope.setFormName = function (formName) {
            this.formName = formName;
        }

        $scope.showError = function (name) {
            console.log(this.formName);
            return ($scope.submitted || $scope.blurred[name]) && $scope[this.formName][name].$invalid;
        }
    }

    return FormController;
})