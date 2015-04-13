define(['../app'], function (app) {
    app.controller('BananaCreateController', function ($scope, $location, dataService) {
        $scope.submitted = false;
        $scope.submitting = false;

        $scope.blurred = {
            Price: false
        };
        
        $scope.data = {};

        $scope.priceHasError = function () {
            var price = parseFloat($scope.data.price);
            console.log(price);

            if (isNaN(price)) {
                return true;
            }

            if (price < 0) {
                return true;
            }

            return false;
        }

        $scope.submit = function () {
            $scope.submitted = true;

            if ($scope.BananaCreateRequestModel.$valid) {
                dataService.addBanana($scope.data).then(
                    function (response) {
                        console.log(response);
                        //$location.path('/index');
                    },
                    function (ex) {
                        console.log(ex);
                    },
                    function (notify) {
                        console.log(notify);
                    }
                );
            }
        }
    });
})