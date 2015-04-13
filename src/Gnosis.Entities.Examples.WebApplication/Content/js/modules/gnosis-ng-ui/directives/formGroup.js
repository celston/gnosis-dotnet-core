define(['../app'], function (app) {
    app.directive('formGroup', function () {
        return {
            transclude: true,
            restrict: 'E',
            replace: true,
            scope: {
                'error': '='
            },
            templateUrl: '/Content/js/modules/gnosis-ng-ui/directives/formGroup.html'
        }
    });
})